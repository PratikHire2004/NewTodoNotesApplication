using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoNotesApplication.DbEntities;
using TodoNotesApplication.Models;

namespace TodoNotesApplication
{
    public class TodoNotesController : Controller
    {
        private readonly TodoContext _context;

        public TodoNotesController(TodoContext context)
        {
            _context = context;
        }

        // GET: TodoNotes
        public async Task<IActionResult> Index()
        {
            TodoViewModel viewModel = new()
            {
                Todos = await _context.TodoList.
               Where(m => m.IsActive == true && m.CreatedBy == User.Identity.Name).ToListAsync(),
                TotalTodo = _context.TodoList.Where(m => m.CreatedBy == User.Identity.Name).Count(),
                ModifiedTodo = _context.TodoList.Where(m => m.CreatedBy == User.Identity.Name && m.ModifiedCount > 0).Count(),
                DeletedTodo = _context.TodoList.Where(m => m.CreatedBy == User.Identity.Name && m.IsActive == false).Count()
            };
            return View(viewModel);
        }

        // GET: TodoNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoList
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive == true && m.CreatedBy == User.Identity.Name);
            if (todoList == null)
            {
                return NotFound();
            }

            return Json(todoList);
        }

        [HttpPost, ActionName("Create")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create([FromBody] TodoSaveModel todo)
        {
            if (ModelState.IsValid && todo.Id == 0)
            {
                TodoNotes todoList = new()
                {
                    Title = todo.Title,
                    Desc = todo.Desc,
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                    ModifiedCount = 0
                };
                _context.Add(todoList);
                var result = await _context.SaveChangesAsync();
                return Json(result);
            }
            else
            {
                dynamic result;
                try
                {
                    var updateTodo = _context.TodoList.Where(x => x.Id == todo.Id).FirstOrDefault();
                    updateTodo.Title = todo.Title;
                    updateTodo.Desc = todo.Desc;
                    updateTodo.ModifiedOn = DateTime.UtcNow;
                    updateTodo.ModifiedCount = todo.ModifiedCount + 1;
                    _context.Update(updateTodo);
                    result = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoNotesExists((int)todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(result);
            }
        }

        // GET: TodoNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoNotes = await _context.TodoList.FindAsync(id);
            if (todoNotes == null)
            {
                return NotFound();
            }
            return View(todoNotes);
        }

        // GET: TodoNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoList
                .FirstOrDefaultAsync(m => m.Id == id);
            var deleteTodo = _context.TodoList.Where(x => x.Id == id).FirstOrDefault();
            deleteTodo.ModifiedOn = DateTime.UtcNow;
            deleteTodo.ModifiedCount = deleteTodo.ModifiedCount + 1;
            deleteTodo.IsActive = false;
            _context.Update(deleteTodo);
            var result = await _context.SaveChangesAsync();
            if (todoList == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        // POST: TodoNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoNotes = await _context.TodoList.FindAsync(id);
            _context.TodoList.Remove(todoNotes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoNotesExists(int id)
        {
            return _context.TodoList.Any(e => e.Id == id);
        }
    }
}

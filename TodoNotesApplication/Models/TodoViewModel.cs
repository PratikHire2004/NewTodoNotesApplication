using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoNotesApplication.DbEntities;

namespace TodoNotesApplication.Models
{
    public class TodoViewModel
    {
        public int TotalTodo { get; set; }
        public int DeletedTodo { get; set; }
        public int ModifiedTodo { get; set; }
        public List<TodoNotes> Todos { get; set; }
    }

    public class TodoSaveModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int ModifiedCount { get; set; }
    }
}

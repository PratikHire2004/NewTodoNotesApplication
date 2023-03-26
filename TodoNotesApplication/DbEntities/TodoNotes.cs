using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoNotesApplication.DbEntities
{
    public class TodoNotes
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedCount { get; set; }
        public bool IsActive { get; set; }
    }
}

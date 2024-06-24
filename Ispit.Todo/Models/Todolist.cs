using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ispit.Todo.Models
{
    public class Todolist
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? UserId { get; set; }
        public string Description { get; set; }

        [ForeignKey("TodolistId")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}

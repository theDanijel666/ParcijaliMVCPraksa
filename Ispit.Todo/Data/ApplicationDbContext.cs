using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Ispit.Todo.Models;

namespace Ispit.Todo.Data
{
    public class ApplicationUser:IdentityUser
    {
        [ForeignKey("UserId")]
        public virtual ICollection<Todolist> Todolists { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todolist> Todolist { get; set; }
        public DbSet<Models.Task> Task { get; set; }
    }
}

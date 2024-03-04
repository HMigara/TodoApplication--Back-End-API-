using Microsoft.EntityFrameworkCore;

namespace TodoApplication.Modles
{
    public class TodoDBcontex : DbContext
    {
        public TodoDBcontex(DbContextOptions options) : base (options)
        {
            
        }

        public DbSet<TodoModel> Todos { get; set; }
    }
}

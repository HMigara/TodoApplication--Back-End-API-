using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApplication.Modles;

namespace TodoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDBcontex _DbContex;

        public TodosController(TodoDBcontex DbContex)
        {
            _DbContex = DbContex;
        }

        //check the todo is avilble for update
        private bool CheckTodoAvailble(int id)
        {
            return (_DbContex.Todos?.Any(t => t.id == id)).GetValueOrDefault();
        }

        //API end poin for get all the Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoModel>>> GetTodos()
        {
            var outObj = _DbContex.Todos;

            if (outObj == null)
            {
                return NotFound();
            }

            return await outObj.ToListAsync();
        }

        //API end point for the get Todos by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoModel>> GetTodos(int id)
        {

            if (_DbContex.Todos == null)
            {
                return NotFound("There is no Todos Created");
            }

            var outObj = await _DbContex.Todos.FindAsync(id);
            if (outObj == null)
            {
                return NotFound(id);
            }

            return outObj;
        }

        //API end point for the save new Todo to the database
        [HttpPost]
        public async Task<ActionResult<TodoModel>> AddTodo(TodoModel todo)
        {
            _DbContex.Todos.Add(todo);
            await _DbContex.SaveChangesAsync();

            return CreatedAtAction(nameof(AddTodo), new { id = todo.id }, todo);
        }

        //API end point for the update Todo from the data base
        [HttpPut]
        public async Task<IActionResult> UpdateTodo(int id, TodoModel todo)
        {
            if (id != todo.id)
            {
                return BadRequest();
            }
            _DbContex.Entry(todo).State = EntityState.Modified;

            try
            {
                await _DbContex.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckTodoAvailble(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        //API end poin for the delete Todo from the data base
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            if (_DbContex.Todos == null)
            {
                return NotFound();
            }

            var todo = await _DbContex.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _DbContex.Todos.Remove(todo);
            await _DbContex.SaveChangesAsync();
            return Ok();
        }
    }
}

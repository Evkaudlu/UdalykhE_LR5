using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdalykhE_LR5.Models;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
    {
        return await _context.TodoItems.ToListAsync();
    }

    // GET: api/TodoItem/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
        var todoitem = await _context.TodoItems.FindAsync(id);

        if (todoitem == null)
        {
            return NotFound();
        }

        return todoitem;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(
    [FromQuery] long[] ids)
    {
        if (ids.Length < 2)
        {
            return BadRequest("Specify at least two IDs.");
        }

        var items = await _context.TodoItems
            .Where(item => ids.Contains(item.Id))
            .ToListAsync();

        return Ok(items);
    }

    // PUT: api/TodoItem/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long? id, TodoItem todoitem)
    {
        if (id != todoitem.Id)
        {
            return BadRequest();
        }

        _context.Entry(todoitem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/TodoItem
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoitem)
    {
        _context.TodoItems.Add(todoitem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodoItem", new { id = todoitem.Id }, todoitem);
    }

    [HttpPost("batch")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> PostTodoItems(
    List<TodoItem> todoItems)
    {
        if (todoItems == null || todoItems.Count == 0)
        {
            return BadRequest("The list cannot be empty.");
        }

        _context.TodoItems.AddRange(todoItems);

        await _context.SaveChangesAsync();

        return StatusCode(201, todoItems);
    }
    // DELETE: api/TodoItem/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long? id)
    {
        var todoitem = await _context.TodoItems.FindAsync(id);
        if (todoitem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoitem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long? id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}

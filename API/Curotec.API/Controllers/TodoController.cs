using Curotec.Application.DTOs;
using Curotec.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Curotec.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllTodos(CancellationToken cancellationToken)
        {
            var todos = await _todoService.GetAllTodosAsync(cancellationToken);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoById(Guid id, CancellationToken cancellationToken)
        {
            var todo = await _todoService.GetTodoByIdAsync(id, cancellationToken);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodoDto, CancellationToken cancellationToken)
        {
            var createdTodo = await _todoService.CreateTodoAsync(createTodoDto, cancellationToken);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }
    }
} 
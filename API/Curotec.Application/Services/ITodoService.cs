using Curotec.Application.DTOs;

namespace Curotec.Application.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetAllTodosAsync(Guid userId,CancellationToken cancellationToken = default);
        Task<TodoDto> GetTodoByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
        Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto, CancellationToken cancellationToken = default);
        Task<TodoDto> UpdateTodoAsync(Guid id, CreateTodoDto updateTodoDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken = default);
    }
} 
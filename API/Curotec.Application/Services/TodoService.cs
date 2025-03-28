using Curotec.Application.DTOs;
using Curotec.Application.Validators;
using Curotec.Domain.Entities;
using Curotec.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Curotec.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IBaseRepository<TodoList> _todoRepository;

        public TodoService(
            IBaseRepository<TodoList> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoDto>> GetAllTodosAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var todos = await _todoRepository.GetByFilterAsync(
                x => x.UserId == userId,
                q => q.Include(t => t.Items),
                cancellationToken);

            return todos.Select(MapToDto);
        }

        public async Task<TodoDto> GetTodoByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await _todoRepository.GetByIdAsync(id, cancellationToken);
            return todo != null ? MapToDto(todo) : null;
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto, CancellationToken cancellationToken = default)
        {
            var validationResult = await new CreateTodoDtoValidator().ValidateAsync(createTodoDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var todoList = new TodoList
            {
                Id = Guid.NewGuid(),
                Name = createTodoDto.Name,
                UserId = createTodoDto.UserId,
                Items = createTodoDto.Items.Select(item => new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = item.Description,
                    IsCompleted = item.IsCompleted
                }).ToList()
            };

            var createdTodo = await _todoRepository.AddAsync(todoList, cancellationToken);
            return MapToDto(createdTodo);
        }

        public async Task<TodoDto> UpdateTodoAsync(Guid id, CreateTodoDto updateTodoDto, CancellationToken cancellationToken = default)
        {
            var existingTodo = await _todoRepository.GetByIdAsync(id, cancellationToken);
            if (existingTodo == null)
                return null;

            var updatedTodoList = new TodoList
            {
                Id = existingTodo.Id,
                Name = updateTodoDto.Name,
                UserId = updateTodoDto.UserId,
                Items = updateTodoDto.Items.Select(item => new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = item.Description,
                    IsCompleted = item.IsCompleted
                }).ToList()
            };

            var updatedTodo = await _todoRepository.UpdateAsync(updatedTodoList, cancellationToken);
            return MapToDto(updatedTodo);
        }

        public async Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await _todoRepository.GetByIdAsync(id, cancellationToken);
            if (todo == null)
                return false;

            return await _todoRepository.RemoveAsync(todo, cancellationToken);
        }

        private static TodoDto MapToDto(TodoList todoList)
        {
            return new TodoDto
            {
                Id = todoList.Id,
                Name = todoList.Name,
                Items = todoList.Items.Select(item => new CreateTodoItemDto
                {
                    Description = item.Description,
                    IsCompleted = item.IsCompleted
                }).ToList()
            };
        }
    }
} 
using Curotec.Domain.Entities;

namespace Curotec.Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CreateTodoItemDto> Items { get; set; }
    }
} 
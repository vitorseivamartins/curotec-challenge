using Curotec.Domain.Entities;

namespace Curotec.Application.DTOs
{
    public class CreateTodoDto
    {
        public string Name { get; set; }
        public List<CreateTodoItemDto> Items { get; set; } = new();
    }
} 
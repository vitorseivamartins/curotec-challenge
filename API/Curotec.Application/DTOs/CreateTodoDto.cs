namespace Curotec.Application.DTOs
{
    public class CreateTodoDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<CreateTodoItemDto> Items { get; set; } = new();
    }
} 
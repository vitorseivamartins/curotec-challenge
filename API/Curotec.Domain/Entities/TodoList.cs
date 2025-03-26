namespace Curotec.Domain.Entities
{
    public record TodoList
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<TodoItem> Items { get; init; }

        public TodoList() { }

        public TodoList(Guid id, string name, List<TodoItem> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }
    }
}

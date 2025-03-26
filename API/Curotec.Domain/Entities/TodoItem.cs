namespace Curotec.Domain.Entities
{
    public record TodoItem
    {
        public Guid Id { get; init; }
        public Guid ParentListId { get; init; }
        public string Description { get; init; }
        public bool IsCompleted { get; init; }

        public TodoItem() { }

        public TodoItem(Guid id, Guid parentListId, string description, bool isCompleted)
        {
            Id = id;
            ParentListId = parentListId;
            Description = description;
            IsCompleted = isCompleted;
        }
    }
}

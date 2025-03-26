namespace Curotec.Domain.Entities
{
    public record User(Guid Id, string Name, string Email, string PasswordHash);
}

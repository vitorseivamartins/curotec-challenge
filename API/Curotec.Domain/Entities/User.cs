namespace Curotec.Domain.Entities
{
    public record User(Guid Id, string Email, string PasswordHash);
}

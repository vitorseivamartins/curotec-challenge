namespace Curotec.Application.DTOs
{
    public class UserDto
    {
        public  string Email { get; set; }
        public  string Password { get; set; }

        public UserDto(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
        
    }
}

using Curotec.Application.DTOs;
using Curotec.Application.Validations;
using Curotec.Domain.Entities;
using Curotec.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Curotec.Application.Services
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration { get; }
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IConfiguration configuration, IBaseRepository<User> userRepository)
            => (_configuration, _userRepository) = (configuration, userRepository);

        public async Task<ActionResult<User>> CreateUser(UserDto userToBeCreated, CancellationToken cancellationToken = default)
        {
            var validationResult = new UserValidator().Validate(userToBeCreated);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (await userToBeCreated.IsUsernameAlreadyTaken(_userRepository))
                return new BadRequestObjectResult("Username already taken!");

            if (await userToBeCreated.IsEmailAlreadyTaken(_userRepository))
                return new BadRequestObjectResult("Email already taken!");

            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(userToBeCreated.Password);

            var newUser = new User(
                Id: Guid.NewGuid(),
                Name: userToBeCreated.Name,
                Email: userToBeCreated.Email,
                PasswordHash: passwordHash);

            await _userRepository.AddAsync(newUser, cancellationToken);

            return new OkObjectResult(newUser);
        }
        
        public async Task<ActionResult<string>> LoginUser(UserDto userToBeLogIn, CancellationToken cancellationToken = default)
        {
            var userFromDatabase = (await _userRepository
                .GetByFilterAsync(userDb => userDb.Email.Equals(userToBeLogIn.Email), null, cancellationToken)).FirstOrDefault();

            if (userFromDatabase is null)
                return new BadRequestObjectResult("User do not exist!");

            if (!userToBeLogIn.IsUsernameOrEmailCorrect(userFromDatabase))
                return new BadRequestObjectResult("Incorrect username or password!");

            return new OkObjectResult(CreateToken(userFromDatabase));
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Jwt:SecretKey").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred,
                issuer: _configuration.GetSection("Jwt:Issuer").Value!,
                audience: _configuration.GetSection("Jwt:Audience").Value!
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
       
    }

    public static class UserExtensions
    {
        public static async Task<bool> IsUsernameAlreadyTaken(this UserDto userToBeCreated, IBaseRepository<User> userRepository, CancellationToken cancellationToken = default)
        {
            var existingUsername = await userRepository
               .GetByFilterAsync(userDb => userDb.Name.Equals(userToBeCreated.Name), null, cancellationToken);
            return existingUsername.Count() > 0;
        }

        public static async Task<bool> IsEmailAlreadyTaken(this UserDto userToBeCreated, IBaseRepository<User> userRepository, CancellationToken cancellationToken = default)
        {
            var existingEmail = await userRepository
                .GetByFilterAsync(userDb => userDb.Email.Equals(userToBeCreated.Email), null, cancellationToken);
            return existingEmail.Count() > 0;
        }

        public static bool IsUsernameOrEmailCorrect(this UserDto userToBeLogIn, User userFromDatabase)
        {
            var isUsernameCorrect = userFromDatabase.Name == userToBeLogIn.Name;
            var isPasswordCorrect =
                BCrypt.Net.BCrypt.Verify(userToBeLogIn.Password, userFromDatabase.PasswordHash);

            return isUsernameCorrect && isPasswordCorrect;
        } 
        

    }
}

using Curotec.Application.DTOs;
using Curotec.Application.Services;
using Curotec.Domain.Entities;
using Curotec.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using FluentValidation;
using System;

namespace Curotec.Tests.Services
{
    public class UserServiceTest
    {
        private readonly UserService _sut;
        private readonly Mock<IBaseRepository<User>> _userRepository
            = new Mock<IBaseRepository<User>>();
        private readonly IConfiguration _configuration;

        public UserServiceTest()
        {
            Dictionary<string, string> inMemoryConfiguration =
            new Dictionary<string, string> {
                {"Jwt:SecretKey", "xbZk8uHjiLfpcajw0lDuTv4OCYuBHGnTD2Td60KS"},
                {"Jwt:Issuer", "Curotec"},
                {"Jwt:Audience", "Curotec"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();

            _sut = new UserService(_configuration, _userRepository.Object);
        }

        [Fact]
        public async void CreateUser_WithUnexistingUser_ReturnsNewUser()
        {
            //Arrange
            var newUser = new UserDto(
                    Name: "New User",
                    Email: "newUserEmail@email.com",
                    Password: "Password");

            //Act
            var result = (await _sut.CreateUser(newUser)).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var resultUserObject = (result as OkObjectResult).Value as User;
            Assert.Equal(newUser.Name, resultUserObject.Name);
            Assert.Equal(newUser.Email, resultUserObject.Email);
            Assert.True(BCrypt.Net.BCrypt.Verify(newUser.Password, resultUserObject.PasswordHash));
        }

        [Fact]
        public async void CreateUser_WithExistingUsername_Returns400Error()
        {
            //Arrange
            var userToBeCreated = new UserDto(
                    Name: "Existing Username",
                    Email: "newUserEmail@email.com",
                    Password: "Password");

            _userRepository.Setup(repo => repo.GetByFilterAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetExistingUser());


            //Act
            var result = (await _sut.CreateUser(userToBeCreated)).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, (result as BadRequestObjectResult).StatusCode);
            Assert.Equal("Username already taken!", (result as BadRequestObjectResult).Value);
        }

        [Fact]
        public async void CreateUser_WithEmptyUsername_Returns400Error()
        {
            //Arrange
            var userToBeCreated = new UserDto(
                    Name: "",
                    Email: "existingEmail@email.com",
                    Password: "Password");

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(userToBeCreated));
            Assert.Equal("Username can't be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async void CreateUser_WithEmptyEmail_Returns400Error()
        {
            //Arrange
            var userToBeCreated = new UserDto(
                    Name: "New User",
                    Email: "",
                    Password: "Password");

            //Act-Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(userToBeCreated));
            Assert.Equal("Email can't be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async void CreateUser_WithInvalidEmail_Returns400Error()
        {
            //Arrange
            var userToBeCreated = new UserDto(
                    Name: "New User",
                    Email: "invalidEmail@",
                    Password: "Password");

            //Act-Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(userToBeCreated));
            Assert.Equal("Email is in invalid format", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async void CreateUser_WithEmptyPassword_Returns400Error()
        {
            //Arrange
            var userToBeCreated = new UserDto(
                    Name: "New User",
                    Email: "existingEmail@email.com",
                    Password: "");

            //Act-Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(userToBeCreated));
            Assert.Equal("Password can't be empty.", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async void LoginUser_WithUnexistingUser_Returns400Error()
        {
            //Arrange
            var userToBeLogIn = new UserDto(
                    Name: "New User",
                    Email: "newUserEmail@email.com",
                    Password: "Password");

            //Act
            var result = await _sut.LoginUser(userToBeLogIn);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void LoginUser_WithExistingUser_ReturnsToken()
        {
            //Arrange
            var userToBeLogIn = new UserDto(
                   Name: "Existing Username",
                   Email: "existingEmail@email.com",
                   Password: "Password");

            _userRepository.Setup(repo => repo.GetByFilterAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetExistingUser());

            //Act
            var result = await _sut.LoginUser(userToBeLogIn);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, (result.Result as OkObjectResult).StatusCode);

        }

        private List<User> GetExistingUser()
        {
            return new List<User>
            {
                new User(
                    Id: Guid.NewGuid(),
                    Name: "Existing Username",
                    Email: "existingEmail@email.com",
                    PasswordHash: "$2a$11$JPxzZzPTOOw8rsmO7Kfs5OTKwRnupxU5NVDDI0BpRyUaKH3dYsska")
            };
        }
    }
}

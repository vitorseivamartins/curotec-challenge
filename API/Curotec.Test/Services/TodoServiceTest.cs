using Curotec.Application.DTOs;
using Curotec.Application.Services;
using Curotec.Domain.Entities;
using Curotec.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Curotec.Tests.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<IBaseRepository<TodoList>> _mockTodoRepository;
        private readonly TodoService _todoService;

        public TodoServiceTests()
        {
            _mockTodoRepository = new Mock<IBaseRepository<TodoList>>();
            _todoService = new TodoService(_mockTodoRepository.Object);
        }

        [Fact]
        public async Task GetAllTodosAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var testTodos = new List<TodoList>
            {
                new TodoList
                {
                    Id = Guid.NewGuid(),
                    Name = "Test List 1",
                    Items = new List<TodoItem>
                    {
                        new TodoItem { Id = Guid.NewGuid(), Description = "Item 1", IsCompleted = false }
                    }
                },
                new TodoList
                {
                    Id = Guid.NewGuid(),
                    Name = "Test List 2",
                    Items = new List<TodoItem>
                    {
                        new TodoItem { Id = Guid.NewGuid(), Description = "Item 2", IsCompleted = true }
                    }
                }
            };

            _mockTodoRepository.Setup(repo => repo.GetByFilterAsync(
                   It.IsAny<Expression<Func<TodoList, bool>>>(),
                   It.IsAny<Func<IQueryable<TodoList>, IIncludableQueryable<TodoList, object>>>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(testTodos);


            // Act
            var result = await _todoService.GetAllTodosAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Test List 1");
            result.Last().Name.Should().Be("Test List 2");
            result.First().Items.Should().HaveCount(1);
            result.First().Items.First().Description.Should().Be("Item 1");
        }

        [Fact]
        public async Task GetTodoByIdAsync_WithValidId_ShouldReturnTodo()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var testTodo = new TodoList
            {
                Id = testId,
                Name = "Test List",
                Items = new List<TodoItem>
                {
                    new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false }
                }
            };

            _mockTodoRepository.Setup(repo => repo.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testTodo);

            // Act
            var result = await _todoService.GetTodoByIdAsync(testId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(testId);
            result.Name.Should().Be("Test List");
            result.Items.Should().HaveCount(1);
            result.Items.First().Description.Should().Be("Test Item");
        }

        [Fact]
        public async Task GetTodoByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockTodoRepository.Setup(repo => repo.GetByIdAsync(invalidId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoList)null);

            // Act
            var result = await _todoService.GetTodoByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateTodoAsync_WithValidData_ShouldCreateAndReturnTodo()
        {
            // Arrange
            var createDto = new CreateTodoDto
            {
                Name = "New List",
                Items = new List<CreateTodoItemDto>
                {
                    new CreateTodoItemDto { Description = "New Item", IsCompleted = false }
                }
            };

            var expectedTodo = new TodoList
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Items = createDto.Items.Select(item => new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = item.Description,
                    IsCompleted = item.IsCompleted
                }).ToList()
            };

            _mockTodoRepository.Setup(repo => repo.AddAsync(It.IsAny<TodoList>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTodo);

            // Act
            var result = await _todoService.CreateTodoAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(createDto.Name);
            result.Items.Should().HaveCount(1);
            result.Items.First().Description.Should().Be("New Item");
            _mockTodoRepository.Verify(repo => repo.AddAsync(It.IsAny<TodoList>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTodoAsync_WithInvalidData_ShouldThrowValidationException()
        {
            // Arrange
            var invalidCreateDto = new CreateTodoDto
            {
                Name = "", // Invalid - empty name
                Items = new List<CreateTodoItemDto>()
            };

            // Act & Assert
            await _todoService.Invoking(async x => await x.CreateTodoAsync(invalidCreateDto))
                .Should().ThrowAsync<ValidationException>();
        }
    }
}
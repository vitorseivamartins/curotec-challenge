using Curotec.Application.DTOs;
using Curotec.Domain.Entities;
using FluentValidation;

namespace Curotec.Application.Validators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
    public CreateTodoDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Description is required and must not exceed 500 characters");

        RuleFor(x => x.Items)
            .NotNull()
            .WithMessage("Items list cannot be null");

        RuleForEach(x => x.Items).SetValidator(new TodoItemValidator());
    }
}

public class TodoItemValidator : AbstractValidator<CreateTodoItemDto>
{
    public TodoItemValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Item description is required and must not exceed 200 characters");
    }
} 
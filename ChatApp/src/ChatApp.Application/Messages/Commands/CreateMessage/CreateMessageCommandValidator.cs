using FluentValidation;

namespace ChatApp.Application.Messages.Commands.CreateMessage;

/// <summary>
/// Validator for the CreateMessageCommand
/// </summary>
public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateMessageCommandValidator class
    /// </summary>
    public CreateMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Message content is required.")
            .MaximumLength(2000)
            .WithMessage("Message content cannot exceed 2000 characters.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}
using FluentValidation.Results;

namespace Blazor.Api.User.Validations;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this(new[] { error })
    {}
}
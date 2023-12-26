namespace Blazor.Api.Client.OutputModels;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public bool IsError { get; }

    public bool IsSucess => !IsError;

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> sucess,
        Func<TError, TResult> failure) =>
        !IsError ? sucess(_value!) : failure(_error!);
}


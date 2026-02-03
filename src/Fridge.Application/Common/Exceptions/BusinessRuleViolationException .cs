namespace Fridge.Application.Common.Exceptions;

public sealed class BusinessRuleViolationException(string message) : Exception(message)
{
}
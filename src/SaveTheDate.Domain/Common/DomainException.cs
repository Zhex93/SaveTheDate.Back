namespace SaveTheDate.Domain.Common;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, int id)
        : base($"{entityName} with Id {id} was not found.") { }

    public NotFoundException(string message) : base(message) { }
}

public class InvalidOperationDomainException : DomainException
{
    public InvalidOperationDomainException(string message) : base(message) { }
}

public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
}

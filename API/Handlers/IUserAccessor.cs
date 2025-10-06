namespace API.Handlers;

public interface IUserAccessor
{
    string? GetUserNameFromToken();
} 
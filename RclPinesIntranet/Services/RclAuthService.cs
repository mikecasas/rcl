using System;
using System.Threading.Tasks;

namespace AuthRCL.Services;

public class RclAuthService
{
    public event Action<bool>? AuthStateChanged;

    public bool IsAuthenticated { get; private set; }

    public Task LoginAsync(string username, string password)
    {
        IsAuthenticated = true;
        AuthStateChanged?.Invoke(IsAuthenticated);
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        IsAuthenticated = false;
        AuthStateChanged?.Invoke(IsAuthenticated);
        return Task.CompletedTask;
    }
}

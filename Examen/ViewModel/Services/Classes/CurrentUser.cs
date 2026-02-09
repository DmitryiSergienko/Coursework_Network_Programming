#nullable enable
using ViewModel.Models;

namespace ViewModel.Services.Classes;
public class CurrentUser
{
    public static CurrentUser Instance { get; } = new();
    public LoginResponse? User { get; private set; }

    public void SetUser(LoginResponse user)
    {
        User = user;
    }

    public void Clear()
    {
        User = null;
    }
}
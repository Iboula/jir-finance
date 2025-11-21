namespace JIR.BlazorApp.Services;

public class ConfirmDialogService
{
    public event Func<string, string, string, Task<bool>>? OnShow;

    public async Task<bool> ShowAsync(string title, string message, string confirmButtonText = "Supprimer")
    {
        if (OnShow != null)
        {
            return await OnShow.Invoke(title, message, confirmButtonText);
        }
        return false;
    }
}

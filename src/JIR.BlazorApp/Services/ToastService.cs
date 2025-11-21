namespace JIR.BlazorApp.Services;

public class ToastService
{
    public event Action<string, string, ToastType>? OnShow;
    
    public void ShowSuccess(string message, string title = "Succès")
    {
        OnShow?.Invoke(title, message, ToastType.Success);
    }
    
    public void ShowError(string message, string title = "Erreur")
    {
        OnShow?.Invoke(title, message, ToastType.Error);
    }
    
    public void ShowWarning(string message, string title = "Attention")
    {
        OnShow?.Invoke(title, message, ToastType.Warning);
    }
    
    public void ShowInfo(string message, string title = "Information")
    {
        OnShow?.Invoke(title, message, ToastType.Info);
    }
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}

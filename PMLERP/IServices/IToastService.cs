using Pml.Shared.ToastMessage;

namespace PMLERP.IServices
{
    public interface IToastService
    {
        event Action<ToastMessage> OnShow;
        Task ShowToastAsync(ToastMessage message);
        Task ShowSuccessAsync(string title, string content, int timeout = 5000);
        Task ShowErrorAsync(string title, string content, int timeout = 5000);
        Task ShowWarningAsync(string title, string content, int timeout = 5000);
        Task ShowInfoAsync(string title, string content, int timeout = 5000);
    }
}

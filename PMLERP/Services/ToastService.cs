
using Pml.Shared.ToastMessage;
using PMLERP.IServices;

namespace PMLERP.Services
{
    public class ToastService : IToastService
    {
        public event Action<ToastMessage> OnShow;

        public async Task ShowToastAsync(ToastMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            OnShow?.Invoke(message);
            await Task.CompletedTask;
        }

        public async Task ShowSuccessAsync(string title, string content, int timeout = 5000)
        {
            var message = new ToastMessage
            {
                Title = title,
                Content = content,
                CssClass = "e-toast-success",
                Timeout = timeout
            };
            await ShowToastAsync(message);
        }

        public async Task ShowErrorAsync(string title, string content, int timeout = 5000)
        {
            var message = new ToastMessage
            {
                Title = title,
                Content = content,
                CssClass = "e-toast-danger",
                Timeout = timeout
            };
            await ShowToastAsync(message);
        }

        public async Task ShowWarningAsync(string title, string content, int timeout = 5000)
        {
            var message = new ToastMessage
            {
                Title = title,
                Content = content,
                CssClass = "e-toast-warning",
                Timeout = timeout
            };
            await ShowToastAsync(message);
        }

        public async Task ShowInfoAsync(string title, string content, int timeout = 5000)
        {
            var message = new ToastMessage
            {
                Title = title,
                Content = content,
                CssClass = "e-toast-info",
                Timeout = timeout
            };
            await ShowToastAsync(message);
        }
    }
}

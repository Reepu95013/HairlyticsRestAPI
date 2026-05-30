namespace Hairlytics.WebApp.Services
{
    public class LoadingService
    {
        public event Action? OnChange;

        private int _loadingCount;

        public bool IsLoading => _loadingCount > 0;

        public void Show()
        {
            _loadingCount++;
            OnChange?.Invoke();
        }

        public void Hide()
        {
            if (_loadingCount > 0)
                _loadingCount--;
            OnChange?.Invoke();
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            Show();
            try
            {
                await action();
            }
            finally
            {
                Hide();
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            Show();
            try
            {
                return await action();
            }
            finally
            {
                Hide();
            }
        }
    }
}

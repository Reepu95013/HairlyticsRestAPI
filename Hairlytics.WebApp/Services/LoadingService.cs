namespace Hairlytics.WebApp.Services
{
    public class LoadingService
    {
        public event Action? OnChange;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnChange?.Invoke(); // notify UI
                }
            }
        }


    }

}

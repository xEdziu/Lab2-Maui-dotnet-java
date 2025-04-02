using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace MauiApp1
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private bool _isLoading;
        public ObservableCollection<UserEntity> Users { get; set; }

        public MainPageViewModel()
        {
            _dbContext = new AppDbContext();
            Users = new ObservableCollection<UserEntity>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            if (_isLoading) return;

            _isLoading = true;
            try
            {
                await AppDbContext.EnsureDataAsync(_dbContext);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                // Tutaj możesz dodać obsługę błędów
                System.Diagnostics.Debug.WriteLine($"Błąd podczas inicjalizacji: {ex.Message}");

                //inner error
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task LoadUsersAsync()
        {
            var usersFromDb = await _dbContext.Users.Include(u => u.Posts).ToListAsync();
            Users.Clear();
            foreach (var user in usersFromDb)
            {
                Users.Add(user);
            }
        }

        // Stara metoda LoadUsers pozostawiona dla kompatybilności wstecznej
        public async void LoadUsers()
        {
            await LoadUsersAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}

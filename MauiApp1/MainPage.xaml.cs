using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var email = EmailEntry.Text;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
            {
                using var db = new AppDbContext();

                var newUser = new UserEntity { Name = name };
                newUser.Email = email;
                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                // Odśwież listę użytkowników
                (BindingContext as MainPageViewModel)?.LoadUsers();
                NameEntry.Text = "";
                EmailEntry.Text = "";
            }
            else
            {
                // poinformuj użytkownika o błędzie
                await DisplayAlert("Błąd", "Wprowadź imię i email", "OK");
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var user = (sender as Button)?.BindingContext as UserEntity;
            if (user != null)
            {
                using var db = new AppDbContext();
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                // Odśwież listę użytkowników
                (BindingContext as MainPageViewModel)?.LoadUsers();
            }
        }
    }

}

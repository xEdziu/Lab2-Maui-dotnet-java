using dotnetLab2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnetLab2
{
    internal class Program
    {
        private static async Task EnsureDataAsync(AppDbContext dbContext)
        {
            var dataAlreadyDownloaded = dbContext.ApiStates.Any(a => a.DataDownloaded);

            if (!dataAlreadyDownloaded)
            {
                var client = new HttpClient();

                Console.WriteLine("[INFO] Pobieram dane z API...");

                var usersJson = await client.GetStringAsync("https://jsonplaceholder.typicode.com/users");
                var postsJson = await client.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
                
                Console.WriteLine("[INFO] Deserializuje dane...");

                var users = JsonSerializer.Deserialize<List<User>>(usersJson);
                var posts = JsonSerializer.Deserialize<List<Post>>(postsJson);

                if (users == null || posts == null)
                {
                    Console.WriteLine("[ERROR] Błąd pobierania danych.");
                    return;
                }

                Console.WriteLine("[INFO] Dane pobrane, zapisuje do bazy...");

                foreach (var user in users)
                {
                    var userEntity = new UserEntity
                    {
                        UserId = user.id,
                        Name = user.name!,
                        Username = user.username,
                        Email = user.email,
                        Phone = user.phone,
                        Website = user.website,
                    };

                    var userPosts = posts.Where(p => p.UserId == user.id);
                    foreach (var post in userPosts)
                    {
                        var postEntity = new PostEntity
                        {
                            PostId = post.Id,
                            Title = post.Title!,
                            Body = post.Body,
                            User = userEntity
                        };

                        userEntity.Posts.Add(postEntity);
                        dbContext.Posts.Add(postEntity);
                    }

                    dbContext.Users.Add(userEntity);
                }

                dbContext.ApiStates.Add(new ApiState
                {
                    DataDownloaded = true,
                    DownloadedAt = DateTime.UtcNow
                });

                dbContext.SaveChanges();
                Console.WriteLine("[INFO] Dane zapisane do bazy danych.");
            }
            else
            {
                Console.WriteLine("[INFO] Dane już istnieją w bazie, pomijam pobieranie.");
            }
        }

        static async Task Main(string[] args)
        {
            using var dbContext = new AppDbContext();

            await EnsureDataAsync(dbContext);

            bool exitRequested = false;

            while (!exitRequested)
            {
                //Console.Clear();
                Console.WriteLine("=== MENU ===");
                Console.WriteLine("1. Wyświetl użytkowników");
                Console.WriteLine("2. Dodaj użytkownika");
                Console.WriteLine("3. Aktualizuj użytkownika");
                Console.WriteLine("4. Usuń użytkownika");
                Console.WriteLine("5. Wyjście");
                Console.Write("Wybierz opcję: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AppDbContext.DisplayUsers(dbContext);
                        break;
                    case "2":
                        await AppDbContext.AddUserAsync(dbContext);
                        break;
                    case "3":
                        await AppDbContext.UpdateUserAsync(dbContext);
                        break;
                    case "4":
                        await AppDbContext.DeleteUserAsync(dbContext);
                        break;
                    case "5":
                        exitRequested = true;
                        break;
                    default:
                        Console.WriteLine("Nieznana opcja.");
                        break;
                }

                if (!exitRequested)
                {
                    Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                    Console.ReadKey();
                }

                Console.Clear();
            }

            Console.WriteLine("Czy chcesz usunąć wszystkie dane z bazy? (t/n)");
            var key = Console.ReadKey();

            if (key.KeyChar == 't')
            {
                dbContext.Database.EnsureDeleted();
                Console.WriteLine("\n[INFO] Dane usunięte z bazy.");
            }
        }
    }
}

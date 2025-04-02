using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetLab2.Entities;
using Microsoft.EntityFrameworkCore;

namespace dotnetLab2
{
    internal class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ApiState> ApiStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=JsonLab2.db");
        }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasMany(u => u.Posts)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<PostEntity>(entity =>
            {
                entity.HasKey(e => e.PostId);
            });
        }

        public static void DisplayUsers(AppDbContext dbContext)
        {
            try
            {
                var users = dbContext.Users.Include(u => u.Posts);
                foreach (var user in users)
                {
                    Console.WriteLine($"\n{user}\nLiczba postów: {user.Posts.Count}\n");
                }

                Console.WriteLine("Czy chcesz wyświetlić posty konkretnego użytkownika? (t/n)");

                if (Console.ReadLine() == "t")
                {
                    Console.Write("Podaj ID użytkownika: ");
                    if (int.TryParse(Console.ReadLine(), out int userId))
                    {
                        var user = dbContext.Users.Include(u => u.Posts).FirstOrDefault(u => u.UserId == userId);
                        if (user != null)
                        {
                            foreach (var post in user.Posts)
                            {
                                Console.WriteLine(post);
                            }
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] Użytkownik nie znaleziony.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Wystąpił błąd: {ex.Message}");
            }
        }

        public static async Task AddUserAsync(AppDbContext dbContext)
        {
            try
            {
                var user = new UserEntity();

                Console.WriteLine("Podaj imię i nazwisko: ");
                user.Name = Console.ReadLine()!;

                Console.WriteLine("Podaj username: ");
                user.Username = Console.ReadLine();

                // Opcjonalne pola
                Console.WriteLine("Podaj email (opcjonalnie): ");
                user.Email = Console.ReadLine();

                Console.WriteLine("Podaj telefon (opcjonalnie): ");
                user.Phone = Console.ReadLine();

                Console.WriteLine("Podaj stronę internetową (opcjonalnie): ");
                user.Website = Console.ReadLine();

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                Console.WriteLine("[INFO] Użytkownik dodany.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"[ERROR] Wystąpił błąd podczas zapisywania zmian: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"[INNER EXCEPTION] {dbEx.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Wystąpił błąd: {ex.Message}");
            }
        }

        public static async Task UpdateUserAsync(AppDbContext dbContext)
        {
            try
            {
                Console.Write("Podaj ID użytkownika do aktualizacji: ");
                if (int.TryParse(Console.ReadLine(), out int userId))
                {
                    var user = await dbContext.Users.FindAsync(userId);
                    if (user != null)
                    {
                        Console.Write("Podaj nowe imię i nazwisko: ");
                        user.Name = Console.ReadLine()!;

                        Console.Write("Podaj nowy username: ");
                        user.Username = Console.ReadLine();

                        await dbContext.SaveChangesAsync();
                        Console.WriteLine("[INFO] Użytkownik zaktualizowany.");
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] Użytkownik nie znaleziony.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Wystąpił błąd: {ex.Message}");
            }
        }

        public static async Task DeleteUserAsync(AppDbContext dbContext)
        {
            try
            {
                Console.Write("Podaj ID użytkownika do usunięcia: ");
                if (int.TryParse(Console.ReadLine(), out int userId))
                {
                    var user = await dbContext.Users.Include(u => u.Posts).FirstOrDefaultAsync(u => u.UserId == userId);
                    if (user != null)
                    {
                        dbContext.Posts.RemoveRange(user.Posts);
                        dbContext.Users.Remove(user);
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine("[INFO] Użytkownik usunięty.");
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] Użytkownik nie znaleziony.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Wystąpił błąd: {ex.Message}");
            }
        }
    }
}

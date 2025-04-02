using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MauiApp1
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

        public static async Task EnsureDataAsync(AppDbContext dbContext)
        {
            var dataAlreadyDownloaded = false;
            try
            {
                dataAlreadyDownloaded = dbContext.ApiStates.Any(a => a.DataDownloaded);
            }
            catch (Exception)
            {
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
                dataAlreadyDownloaded = false;
            }

            if (!dataAlreadyDownloaded)
            {
                var client = new HttpClient();
                var usersJson = await client.GetStringAsync("https://jsonplaceholder.typicode.com/users");
                var postsJson = await client.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
                var users = JsonSerializer.Deserialize<List<User>>(usersJson);
                var posts = JsonSerializer.Deserialize<List<Post>>(postsJson);
                if (users == null || posts == null)
                {
                    return;
                }
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
                        dbContext.Posts.Add(postEntity);
                        userEntity.Posts.Add(postEntity);
                    }
                    dbContext.Users.Add(userEntity);
                }
                dbContext.ApiStates.Add(new ApiState { DataDownloaded = true });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1
{
    public class PostEntity
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string? Body { get; set; }

        // Klucz obcy do UserEntity
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        public override string ToString()
        {
            return $"\nPostId: {PostId}\nTitle: {Title}\nBody: {Body}";
        }
    }
}



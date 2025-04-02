using dotnetLab2.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserEntity
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }

    // Relacja 1-N
    public List<PostEntity> Posts { get; set; } = new List<PostEntity>();

    public override string ToString()
    {
        return $"ID: {UserId}, Name: {Name}, Username: {Username}, Email: {Email}, Phone: {Phone}, Website: {Website}";
    }
}

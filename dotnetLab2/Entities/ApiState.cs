namespace dotnetLab2.Entities;

using System.ComponentModel.DataAnnotations;

public class ApiState
{
    [Key]
    public int Id { get; set; }

    public bool DataDownloaded { get; set; }

    public DateTime DownloadedAt { get; set; }
}

#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class Association
{
    [Key]
    public int AssociationId {get;set;}
    [Required]
    public int WeddingId {get;set;}
    [Required]
    public int UserId {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public Wedding? Wedding {get;set;}
    public User? User {get;set;}
}
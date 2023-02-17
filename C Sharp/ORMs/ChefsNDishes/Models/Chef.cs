#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ChefsNDishes.Models;

public class Chef
{
    [Key]
    public int ChefId {get;set;}
    [Required]
    [Display(Name = "First Name")]
    public string FirstName {get;set;}
    [Required]
    [Display(Name = "Last Name")]
    public string LastName {get;set;}
    [Required]
    [DataType(DataType.Date)]
    [DateInPast]
    [Display(Name = "Date of Birth")]
    public DateTime DoB {get;set;}
    // navigation property
    public List<Dish> DishesFromChef {get;set;} = new List<Dish>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class DateInPastAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime dateGiven = (DateTime)value;
        System.Console.WriteLine(dateGiven);
        if (dateGiven < DateTime.Now)
        {
            return ValidationResult.Success;
        } else {
            return new ValidationResult("Date must be in the past.");
        }
    }
}
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ChefsNDishes.Models;

public class Dish
{
    [Key]
    public int DishId {get;set;}
    [Required]
    public string Name {get;set;}
    [Required]
    [GreaterThanZero]
    public int Calories {get;set;}
    [Required]
    [Range(1,5)]
    public int Tastiness {get;set;}
    [Required]
    public int ChefId {get;set;}
    // navigation property
    public Chef? Chef {get;set;}
}

public class GreaterThanZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        int calorieCount = (int)value;
        if (calorieCount > 0)
        {
            return ValidationResult.Success;
        } else {
            return new ValidationResult("Must be more than 0 Calories.");
        }
    }
}
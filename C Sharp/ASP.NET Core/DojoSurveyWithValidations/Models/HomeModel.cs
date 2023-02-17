#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace DojoSurveyWithValidations.Models;

public class User
{
    [Required]
    [MinLength(2)]
    public string Name {get;set;}
    [Required]
    public string Location {get;set;}
    [Required]
    public string Language {get;set;}
    [MinLength(21)]
    public string? Comment {get;set;}
}
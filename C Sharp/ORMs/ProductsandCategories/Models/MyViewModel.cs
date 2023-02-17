#pragma warning disable CS8618
namespace ProductsandCategories.Models;
public class MyViewModel
{    
    public Product Product {get;set;}
    public List<Product> AllProducts {get;set;}
    public List<Product> NotProducts {get;set;}

    public Category Category {get;set;}
    public List<Category> AllCategories {get;set;}
    public List<Category> NotChosenCategories {get;set;}

    public Association Assoc {get;set;}
    public List<Association> AllAssociations {get;set;}
}
#pragma warning disable CS8618
namespace WeddingPlanner.Models;
public class MyViewModel
{    
    public User User {get;set;}
    public List<User> AllUsers {get;set;}

    public Wedding Wedding {get;set;}
    public List<Wedding> AllWeddings {get;set;}

    public Association Assoc {get;set;}
    public List<Association> AllAssociations {get;set;}
}
class Attack
{
    private string Name;
    public string _Name
    {
        get {return Name;}
    }
    private int Damage;

    public Attack(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}
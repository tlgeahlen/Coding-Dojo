class Enemy
{
    private string Name;
    private int Health;
    public int _Health
    {
        get { return Health; }
    }
    public List<Attack> Attacks;

    public Enemy(string name, int health = 300)
    {
        Name = name;
        Health = health;
        Attacks = new List<Attack>();
    }

    public void RandomAttack()
    {
        Random rand = new Random();
        int attackNumber = rand.Next(0, Attacks.Count);
        System.Console.WriteLine($"The {Name} hurls a {Attacks[attackNumber]._Name} spell at you!");
    }
}
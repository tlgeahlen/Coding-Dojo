class Enemy
{
    public string Name;
    public int Health;
    public List<Attack> Attacks;

    public Enemy(string n, int h = 100)
    {
        Name = n;
        Health = h;
        Attacks = new List<Attack>();
    }

    public virtual void Attack()
    {
        Random rand  = new Random();
        Attack randAttack = Attacks[rand.Next(0, Attacks.Count)];
        System.Console.WriteLine($"The {randAttack.Name} deals {randAttack.Damage} damage!");
    }
}

class Attack
{
    public string Name;
    public int Damage;

    public Attack(string n, int d)
    {
        Name = n;
        Damage = d;
    }
}

class Melee : Enemy
{
    public Melee(string n, int h = 120) : base(n, h)
    {
        Attacks = new List<Attack>();
        Attacks.Add(new Attack("punches", 20));
        Attacks.Add(new Attack("kicks", 15));
        Attacks.Add(new Attack("tackles", 25));
    }

    public override void Attack()
    {
        Random rand = new Random();
        Attack randAttack = Attacks[rand.Next(0, Attacks.Count)];
        System.Console.WriteLine($"The enemy {randAttack.Name} you for {randAttack.Damage} damage.");
    }

    public void Rage()
    {
        Random rand = new Random();
        Attack randAttack = Attacks[rand.Next(0, Attacks.Count)];
        randAttack.Damage += 10;
        System.Console.WriteLine($"The enemy rages and {randAttack.Name} you for {randAttack.Damage} damage.");
    }
}

class Ranged : Enemy
{
    public int Distance;

    public Ranged(string n) : base(n)
    {
        Attacks = new List<Attack>();
        Attacks.Add(new Attack("shoots an arrow", 20));
        Attacks.Add(new Attack("throws a knife", 15));
        Distance = 5;
    }

    public override void Attack()
    {   
        Random rand = new Random();
        Attack randAttack = Attacks[rand.Next(0, Attacks.Count)];
        if (Distance >= 10)
        {
            System.Console.WriteLine($"The enemy {randAttack.Name} at you dealing {randAttack.Damage} points of damage.");
        } else {
            System.Console.WriteLine("The enemy is too close to attack you!");
        }
    }

    public void Dash()
    {
        Distance = 20;
        System.Console.WriteLine("The enemy dashes away from you.");
    }
}

class MagicCaster : Enemy
{
    public MagicCaster(string n, int h = 80) : base(n, h)
    {
        Attacks = new List<Attack>();
        Attacks.Add(new Attack("casts fireball", 25));
        Attacks.Add(new Attack("casts a shield", 0));
        Attacks.Add(new Attack("bludgeons you with their staff", 15));
    }

    public override void Attack()
    {
        Random rand = new Random();
        Attack randAttack = Attacks[rand.Next(0, Attacks.Count)];
        System.Console.WriteLine($"The caster {randAttack.Name} dealing {randAttack.Damage} damage.");
    }

    public void Heal(Enemy target)
    {
        target.Health += 40;
        System.Console.WriteLine($"The caster heals the {target.Name}, who now has {target.Health} health.");
    }
}
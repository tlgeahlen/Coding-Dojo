//Print all values 1-255
for (int i=1; i<=255; i++)
{
    System.Console.WriteLine(i);
}

//Generate 5 random numbers between 10 and 20 and print the sum of the values
int sum = 0;
Random rand = new Random();
for (int i=0; i<5; i++)
{
    sum = sum + rand.Next(10,21);
}
System.Console.WriteLine(sum);

//Print all values from 1-100 that are divisible by 3 or 5 but not both
for (int i=1; i<=100; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        System.Console.WriteLine("FizzBuzz");
    }
    else if (i % 3 == 0)
    {
        System.Console.WriteLine("Fizz");
    }
    else if (i % 5 == 0)
    {
        System.Console.WriteLine("Buzz");
    }
    else
    {
        System.Console.WriteLine(i);
    }
}
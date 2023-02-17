//***Three Basic Arrays***

//integers 0 to 9 inside
int[] zeroToNine = {0,1,2,3,4,5,6,7,8,9};

//with some names
string[] names = {"Tim", "Martin", "Nikki", "Sara"};

//array length 10 with alternating true/false values
bool[] alternatingBools = new bool[10];
for (int i = 0; i<alternatingBools.Length; i++) {
    if (i % 2 == 0)
    {
        alternatingBools[i] = true;
    } else {
        alternatingBools[i] = false;
    }
}
foreach (bool boolean in alternatingBools)
{
    // Console.WriteLine(boolean);
}

//**List of Flavors***

//List of ice cream flavors that holds at least 5 flavors
List<string> flavorList = new List<string>(){"Chocolate", "Vanilla", "Butter Pecan", "Stawberry", "Cookie Dough"};
//Output the length of the list
System.Console.WriteLine(flavorList.Count);
//Output the third flavor in the list
System.Console.WriteLine(flavorList[2]);
//Remove the third flavor using its index location
flavorList.RemoveAt(2);
//Output the length of the list again
System.Console.WriteLine(flavorList[2]);
System.Console.WriteLine(flavorList.Count);

//***User Dictionary***

//Create a dict that stores string keys and string values
Random rand = new Random();
Dictionary<string,string> nameFlavors = new Dictionary<string, string>();

//For each person in names, assign them a random flavor of ice cream
foreach (string name in names)
{
    string randomFlavor = flavorList[rand.Next(0, flavorList.Count)];
    nameFlavors.Add(name, randomFlavor);
}

//Print out each person's name and their assigned flavor
foreach (KeyValuePair<string,string> entry in nameFlavors)
{
    System.Console.WriteLine($"{entry.Key}: {entry.Value}");
}
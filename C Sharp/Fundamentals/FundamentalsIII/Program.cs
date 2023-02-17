// Iterate and print values
static void PrintList(List<string> MyList)
{
    foreach (string item in MyList)
    {
        System.Console.WriteLine(item);
    }
}

List<string> list = new List<string>(){"One", "Two", "Three"};
PrintList(list);


// Print Sum
static void SumOfNums(List<int> IntList)
{
    int sum = 0;
    foreach (int num in IntList)
    {
        sum += num;
    }
    System.Console.WriteLine(sum);
}

List<int> numList = new List<int>(){1,-2,7,4,-5};
SumOfNums(numList);


// Find Max
static int FindMax(List<int> IntList)
{
    int max = IntList[0];
    foreach (int num in IntList)
    {
        if (num > max)
        {
            max = num;
        }
    }
    return max;
}
System.Console.WriteLine(FindMax(numList));


// Square the Values
static List<int> SquareValues(List<int> IntList)
{
    for (int i = 0; i<IntList.Count; i++)
    {
        IntList[i] = IntList[i]*IntList[i];
    }
    return IntList;
}

foreach (int square in SquareValues(numList))
{
    System.Console.WriteLine(square);
}


// Replace Negative Numbers with 0
int[] numArray = {1,-2,7,4,-5};

static int[] NonNegatives(int[] IntArray)
{
    for (int i = 0; i<IntArray.Length; i++)
    {
        if (IntArray[i] < 0)
        {
            IntArray[i] = 0;
        }
    }
    return IntArray;
}

foreach (int num in NonNegatives(numArray))
{
    System.Console.WriteLine(num);
}


// Print Dictionary
Dictionary<string,string> people = new Dictionary<string, string>();
people.Add("Name", "Travis");
people.Add("Location", "Phoenix");

static void PrintDictionary(Dictionary<string,string> MyDict)
{
    foreach (KeyValuePair<string,string> entry in MyDict)
    {
        System.Console.WriteLine($"{entry.Key}: {entry.Value}");
    }
}

PrintDictionary(people);


// Find Key
static bool FindKey(Dictionary<string,string> MyDictionary, string SearchTerm)
{
    if (MyDictionary.ContainsKey(SearchTerm))
    {
        return true;
    } else {
        return false;
    }
}

System.Console.WriteLine(FindKey(people, "Name"));
System.Console.WriteLine(FindKey(people, "Age"));


// Generate a Dictionary
List<string> nameList = new List<string>(){"Julie", "Harold", "James", "Monica"};
List<int> ageList = new List<int>(){6,12,7,10};

static Dictionary<string,int> GenerateDict(List<string> Names, List<int> Age)
{
    Dictionary<string,int> returnDict = new Dictionary<string,int>();
    for (int i = 0; i<Names.Count; i++)
    {
        returnDict.Add(Names[i], Age[i]);
    }
    return returnDict;
}

foreach (KeyValuePair<string,int> entry in GenerateDict(nameList, ageList))
{
    System.Console.WriteLine($"{entry.Key}: {entry.Value}");
}
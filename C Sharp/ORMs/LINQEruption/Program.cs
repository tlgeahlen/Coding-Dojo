List<Eruption> eruptions = new List<Eruption>()
{
    new Eruption("La Palma", 2021, "Canary Is", 2426, "Stratovolcano"),
    new Eruption("Villarrica", 1963, "Chile", 2847, "Stratovolcano"),
    new Eruption("Chaiten", 2008, "Chile", 1122, "Caldera"),
    new Eruption("Kilauea", 2018, "Hawaiian Is", 1122, "Shield Volcano"),
    new Eruption("Hekla", 1206, "Iceland", 1490, "Stratovolcano"),
    new Eruption("Taupo", 1910, "New Zealand", 760, "Caldera"),
    new Eruption("Lengai, Ol Doinyo", 1927, "Tanzania", 2962, "Stratovolcano"),
    new Eruption("Santorini", 46, "Greece", 367, "Shield Volcano"),
    new Eruption("Katla", 950, "Iceland", 1490, "Subglacial Volcano"),
    new Eruption("Aira", 766, "Japan", 1117, "Stratovolcano"),
    new Eruption("Ceboruco", 930, "Mexico", 2280, "Stratovolcano"),
    new Eruption("Etna", 1329, "Italy", 3320, "Stratovolcano"),
    new Eruption("Bardarbunga", 1477, "Iceland", 2000, "Stratovolcano")
};
// Example Query - Prints all Stratovolcano eruptions
IEnumerable<Eruption> stratovolcanoEruptions = eruptions.Where(c => c.Type == "Stratovolcano");
PrintEach(stratovolcanoEruptions, "Stratovolcano eruptions.");
// Execute Assignment Tasks here!
// Use LINQ to find the first eruption that is in Chile and print the result.

Eruption? firstChile = eruptions.FirstOrDefault(e => e.Location == "Chile");
System.Console.WriteLine(firstChile.ToString());

// Find the first eruption from the "Hawaiian Is" location and print it. If none is found, print "No Hawaiian Is Eruption found."

Eruption? firstHawaii = eruptions.FirstOrDefault(e => e.Location == "Hawaiian Is");
if (firstHawaii != null)
{
    System.Console.WriteLine(firstHawaii.ToString());
} else {
    System.Console.WriteLine("Did not find");
}

// Find the first eruption from the "Greenland" location and print it. If none is found, print "No Greenland Eruption found."

Eruption? firstGreenland = eruptions.FirstOrDefault(e => e.Location == "Greenland");
if (firstGreenland != null)
{
    System.Console.WriteLine(firstGreenland.ToString());
} else {
    System.Console.WriteLine("Did not find a Greenland eruption");
}

// Find the first eruption that is after the year 1900 AND in "New Zealand", then print it.

Eruption? first1900NZ = eruptions.Where(e => e.Year > 1900).FirstOrDefault(l => l.Location == "New Zealand");
System.Console.WriteLine(first1900NZ.ToString());

// Find all eruptions where the volcano's elevation is over 2000m and print them.

IEnumerable<Eruption> elevation2000Plus = eruptions.Where(e => e.ElevationInMeters > 2000);
PrintEach(elevation2000Plus, "Over 2000m");

// Find all eruptions where the volcano's name starts with "L" and print them. Also print the number of eruptions found.

IEnumerable<Eruption> lNames = eruptions.Where(e => e.Volcano.StartsWith("L"));
PrintEach(lNames, "Starts with L");
System.Console.WriteLine(lNames.Count());

// Find the highest elevation, and print only that integer (Hint: Look up how to use LINQ to find the max!)

int? highestElevation = eruptions.Max(e => e.ElevationInMeters);
System.Console.WriteLine(highestElevation);

// Use the highest elevation variable to find a print the name of the Volcano with that elevation.

IEnumerable<string> highestName = eruptions.Where(e => e.ElevationInMeters == highestElevation).Select(v => v.Volcano);
foreach (string v in highestName)
{
    System.Console.WriteLine(v);
}

// Print all Volcano names alphabetically.

IEnumerable<string> namesAlpha = eruptions.OrderBy(v => v.Volcano).Select(e => e.Volcano);
foreach (string v in namesAlpha)
{
    System.Console.WriteLine(v);
}

// Print the sum of all the elevations of the volcanoes combined.

int? sumElevations = eruptions.Sum(e => e.ElevationInMeters);
System.Console.WriteLine(sumElevations);

// Print whether any volcanoes erupted in the year 2000 (Hint: look up the Any query)

bool? any2000 = eruptions.Any(e => e.Year == 2000);
System.Console.WriteLine(any2000);

// Find all stratovolcanoes and print just the first three (Hint: look up Take)

IEnumerable<Eruption> firstThreeStrato = eruptions.Where(e => e.Type == "Stratovolcano").Take(3);
PrintEach(firstThreeStrato, "First 3 stratos");

// Print all the eruptions that happened before the year 1000 CE alphabetically according to Volcano name.

IEnumerable<Eruption> before1000Alphabetically = eruptions.Where(e => e.Year < 1000).OrderBy(v => v.Volcano);
PrintEach(before1000Alphabetically, "Before 1000 alphabetically");

// Redo the last query, but this time use LINQ to only select the volcano's name so that only the names are printed.

IEnumerable<string> before1000AlphaNameOnly = eruptions.Where(e => e.Year < 1000).OrderBy(v => v.Volcano).Select(n => n.Volcano);
foreach (string v in before1000AlphaNameOnly)
{
    System.Console.WriteLine(v);
}

// Helper method to print each item in a List or IEnumerable. This should remain at the bottom of your class!
static void PrintEach(IEnumerable<Eruption> items, string msg = "")
{
    Console.WriteLine("\n" + msg);
    foreach (Eruption item in items)
    {
        Console.WriteLine(item.ToString());
    }
}
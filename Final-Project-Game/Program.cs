//Tanner Mascaro, Final-Project-Game, 12/10/2023
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Security;

Console.Clear();
Console.Title = "The V's and the Double V";
Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.ForegroundColor = ConsoleColor.Black;

//A little intro and some instructions on how to play the game
//CONSOLE.WRITELINE(@) --CHANGE VERBATUM
Console.Beep();
Console.WriteLine("Hello?");
Console.WriteLine(" "); //these are just to break up walls of text
Console.WriteLine("You seem to have falled into the dungeon of the v's");
Console.WriteLine("How on Earth did you end up here? Tell me!");
Console.ReadLine();
Console.WriteLine("You're not very smart are you?");
Console.WriteLine("What is your name?");
var player = Console.ReadLine(); //used var because they could put anything in here and i want to read it
Console.WriteLine(player + ", no matter the reason, you are here now and need to defeat the v's and thier master the double v!!");
Thread.Sleep(4000); //wanted to use this for comedic affect
Console.WriteLine("Yes....... The double U or W. Not the double V");
Thread.Sleep(4000);
Console.Clear();
Console.WriteLine("As dumb as he is you need to kill him!");
Console.WriteLine(" ");
Console.WriteLine("Here are some tips to make it out alive: ");
Console.WriteLine("Collect @ for a big boost of health");
Console.WriteLine("Collect % for smaller amounts of health!");
Console.WriteLine("Collect ^, this sword will help you defeat the W");
Console.WriteLine("Stay away from the v's. These little guys will hurt you!");
Console.WriteLine("Get enough $ to raise your score to defeat W!");
Console.WriteLine("Good Luck " + player);
Console.WriteLine(" ");
Console.WriteLine("Also! Use WASD to move! Press any button to start! ");

Console.ReadKey(true);

Console.CursorVisible = false;


//some basic variables to set up the game
int score = 0;
int health = 100;
int bosshealth = 400;

int cursorLeft = 2;
int cursorTop = 2;

char[] items = new char[2] { ' ', ' ' };//my array for the items the player will have. empty now
//the two "maps"
string[] mapRow = File.ReadAllLines("map.txt");
string[] endBossAreana = File.ReadAllLines("endboss.txt");

//first time writing the map
WriteMap(mapRow, cursorTop, cursorLeft);
Console.ReadKey(true);


//switch statement w/ do while loop for movement
do
{
    ConsoleKey key = Console.ReadKey(true).Key;
    switch (key)
    {
        case ConsoleKey.W:
            DoMove(cursorTop - 1, cursorLeft, mapRow);
            break;
        case ConsoleKey.S:
            DoMove(cursorTop + 1, cursorLeft, mapRow);
            break;
        case ConsoleKey.A:
            DoMove(cursorTop, cursorLeft - 1, mapRow);
            break;
        case ConsoleKey.D:
            DoMove(cursorTop, cursorLeft + 1, mapRow);
            break;
    }
    //breaks out of the code if our health or the bosses goes to 0 or below
    if (health <= 0)
    {
        Console.Clear();
        Console.WriteLine(player + " you lost!");
        Console.WriteLine("Maybe....try harder next time?");
        Console.ReadKey(true);
        break;
    }
    else if (bosshealth <= 0)
    {
        Console.Clear();
        Console.WriteLine("You win!");
        Console.WriteLine("Great job " + player + "! Thank you!");
        Console.ReadKey(true);
        break;
    }
    MoveBadGuys();
} while (true);

//this method moves the guys after checking the movebadguy method. I used a similar idea using two
//the same idea i had earlier. one checks and one does it
void MoveBadGuys()
{
    for (int i = 0; i < mapRow.Length; i++) //check for the old top
    {
        for (int j = 0; j < mapRow[i].Length; j++) //check for the old row using our i we found
        {
            if (mapRow[i][j] == 'v') //if they equal v we can move on
            {
                int randomDirection = new Random().Next(4); //use randoms again. only to 4 this time
                switch (randomDirection)
                {
                    case 0:
                        MoveBadGuy(i, j, i - 1, j); // Move up
                        break;
                    case 1:
                        MoveBadGuy(i, j, i + 1, j); // Move down
                        break;
                    case 2:
                        MoveBadGuy(i, j, i, j - 1); // Move left
                        break;
                    case 3:
                        MoveBadGuy(i, j, i, j + 1); // Move right
                        break;
                }
            }
        }
    }
}


//made this method use the old and new becuase it was moving randomly. 
//i couldnt just delete one back like I had before in previous. I needed to check the old position and remove it
void MoveBadGuy(int oldTop, int oldLeft, int proposedTop, int proposedLeft)
{
    //if basically checks for everything so nothing is erased
    if (proposedTop >= 0 && proposedTop < mapRow.Length && proposedLeft >= 0 && proposedLeft < mapRow[proposedTop].Length && mapRow[proposedTop][proposedLeft] != '~'
    && mapRow[proposedTop][proposedLeft] != '|' && mapRow[proposedTop][proposedLeft] != '~' && mapRow[proposedTop][proposedLeft] != '=' && mapRow[proposedTop][proposedLeft] != '/'
    && mapRow[proposedTop][proposedLeft] != '$' && mapRow[proposedTop][proposedLeft] != '@' && mapRow[proposedTop][proposedLeft] != '^' && mapRow[proposedTop][proposedLeft] != '%'
    && mapRow[proposedTop][proposedLeft] != 'W' && mapRow[proposedTop][proposedLeft] != 'v')
    {
        // Remove 'v' from the old position
        mapRow[oldTop] = mapRow[oldTop].Remove(oldLeft, 1).Insert(oldLeft, " ");
        mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, "v");
    }
}


//Move after seeing if we can move using the try move method
void DoMove(int proposedTop, int proposedLeft, string[] map)
{
    if (trymove(proposedTop, proposedLeft, map))
    {
        //needed to make both the acutual move
        cursorLeft = proposedLeft;
        cursorTop = proposedTop;
        updatescore(score);
        WriteMap(map, proposedTop, proposedLeft);
    }
    //all my checks!
    if (map[proposedTop][proposedLeft] == 'W')
    {
        bossfight();
        return;
    }
    if (map[proposedTop][proposedLeft] == '%')
    {
        Console.Beep();
        health += 50;
        mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, " ");
    }
    if (map[proposedTop][proposedLeft] == 'v')
    {
        Console.Beep();
        health -= 50;
        mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, " ");
    }
    if (map[proposedTop][proposedLeft] == '^')
    {
        Console.Beep();
        whatItems();
        mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, " ");
    }
    if (map[proposedTop][proposedLeft] == '@')
    {
        Console.Beep();
        whatItems();
        mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, " ");
        health += 100;

    }
    if (mapRow[proposedTop][proposedLeft] == '$')
    {
        Console.Beep();
        coins(mapRow, proposedTop, proposedLeft);
    }
}

//test to see if move can move
bool trymove(int proposedTop, int proposedLeft, string[] mapRow)
{
    if (proposedTop >= 0 && proposedTop < mapRow.Length && proposedLeft >= 0 && proposedLeft < mapRow[proposedTop].Length && mapRow[proposedTop][proposedLeft] != '~'
    && mapRow[proposedTop][proposedLeft] != '|' && mapRow[proposedTop][proposedLeft] != '~' && mapRow[proposedTop][proposedLeft] != '=' && mapRow[proposedTop][proposedLeft] != '/')
    {
        return true;
    }
    return false;
}


//method to call the map. writes out score and health and displays the item we have
void WriteMap(string[] map, int cursortop, int cursorleft)
{
    Console.Clear();
    foreach (var line in map)
    {
        Console.WriteLine(line);
    }

    Console.SetCursorPosition(6, 25);
    Console.WriteLine($"{score}");
    Console.SetCursorPosition(33, 25);
    Console.WriteLine($"{health}");
    Console.SetCursorPosition(55, 25);
    whatItems();
    Console.SetCursorPosition(cursorleft, cursortop);
    Console.WriteLine("T");
}

//method to add coins. every coin equals 100 each time
void coins(string[] map, int proposedTop, int proposedLeft)
{
    score += 100;
    cursorLeft = proposedLeft;
    cursorTop = proposedTop;
    mapRow[proposedTop] = mapRow[proposedTop].Remove(proposedLeft, 1).Insert(proposedLeft, " "); //delete the last one and add a blank space
}


//this method displays the items on the map from the array.
//we check for the item on the array and if it isnt there we add to the array
void whatItems()
{
    if (mapRow[2][9] != '^')
    {
        items[0] = '^';
    }
    if (mapRow[12][29] != '@')
    {
        items[1] = '@';
    }


    foreach (char items in items)
    {
        Console.Write(items + " ");
    }

}


//
void updatescore(int score)
{
    //first score is for the first door
    if (score >= 200)
    {
        mapRow[1] = mapRow[1].Remove(15, 1).Insert(15, " ");
    }
    //second door
    if (score >= 400)
    {
        mapRow[16] = mapRow[16].Remove(41, 1).Insert(41, " ");
    }
    //opens the door for the final boss. wants it to be kind of hard to open
    if (score >= 1000)
    {
        //old way I removed the rows. I leared how to use loops to check if they were there
        // mapRow[19] = mapRow[19].Remove(52, 1).Insert(52, " ");
        // mapRow[19] = mapRow[19].Remove(53, 1).Insert(53, " ");
        // mapRow[19] = mapRow[19].Remove(54, 1).Insert(54, " ");
        // mapRow[19] = mapRow[19].Remove(55, 1).Insert(55, " ");
        // mapRow[19] = mapRow[19].Remove(56, 1).Insert(56, " ");
        // mapRow[19] = mapRow[19].Remove(57, 1).Insert(57, " ");
        // mapRow[19] = mapRow[19].Remove(58, 1).Insert(58, " ");
        // mapRow[19] = mapRow[19].Remove(59, 1).Insert(59, " ");
        // mapRow[19] = mapRow[19].Remove(60, 1).Insert(60, " ");
        // mapRow[19] = mapRow[19].Remove(61, 1).Insert(61, " ");
        // mapRow[19] = mapRow[19].Remove(62, 1).Insert(62, " ");
        // mapRow[19] = mapRow[19].Remove(63, 1).Insert(63, " ");
        // mapRow[19] = mapRow[19].Remove(64, 1).Insert(64, " ");
        // mapRow[19] = mapRow[19].Remove(65, 1).Insert(65, " ");
        // mapRow[19] = mapRow[19].Remove(51, 1).Insert(51, " ");

        //it was on line 19 and instead of doing it each time i knew it started at 51 and my length was 65
        for (int i = 51; i <= 65; i++)
        {
            mapRow[19] = mapRow[19].Remove(i, 1).Insert(i, " ");
        }

    }
}


//prints the boss text
void printBoss()
{
    Console.Clear();

    foreach (var line in endBossAreana)
    {
        Console.WriteLine(line);
    }
    Console.SetCursorPosition(0, endBossAreana.Length + 2);
    Console.WriteLine("W's Health: " + bosshealth + "     " + "Your Health: " + health);
    Console.WriteLine("Attack with Space!! Good Luck!");

}


//all the final fight stuffs
void bossfight()
{
    Console.Clear();
    printBoss();

    // Process all the inputs
    do
    {
        ConsoleKeyInfo bossKey;
        do
        {
            bossKey = Console.ReadKey(true); //needed a new key

            // check to make sure its the spacebar
            if (bossKey.Key != ConsoleKey.Spacebar)
            {
                Console.Clear();
                printBoss();
            }

        } while (bossKey.Key != ConsoleKey.Spacebar);

        //probally don't need a switch, but its how i know how to do inputs
        switch (bossKey.Key)
        {
            case ConsoleKey.Spacebar:
                Console.Clear();
                printBoss();
                attackPercentage();
                printBoss();
                break;
        }

    } while (bosshealth > 0 && health > 0);
    //checking statuses of the boss and player
    if (bosshealth <= 0)
    {
        Console.Clear();
        return;
    }
    else if (health <= 0)
    {
        Console.Clear();
        return;
    }
}

//This method gets a random number and attack
//pulls a random number. if its divisable by two the player does damage
// if it's not than the player gets hit
void attackPercentage()
{
    Random rnd = new Random();
    int attack = rnd.Next();

    if (attack % 2 == 0)
    {
        Console.Beep();
        bosshealth -= 25;
        Console.WriteLine("Attack successful! Good Hit!");
        Thread.Sleep(1000); // Tutor helped find this. Gives me one second to read the line
        if (items.Contains('^'))
        {
            //sword adds some damage increase
            bosshealth -= 15;
            Console.WriteLine("Sword gave a little extra attack!");
            Thread.Sleep(500);
        }
    }
    else
    {
        Console.Beep();
        health -= 25;
        Console.WriteLine("You missed, so the W hit you!");
        Thread.Sleep(1000);
    }
}


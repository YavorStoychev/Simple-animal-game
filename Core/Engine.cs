using OOPProject.Core.Contracts;
using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Movement;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.FieldUtilities;
using OOPProject.Utilities.NpcUtilities;
using OOPProject.Utilities.PlayerUtilities;
using OOPProject.Utilities.WaterUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Core
{
    public class Engine : IEngine
    {
        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            AnimalList animalList = new AnimalList();
            
            List<Animal> terrestrialAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Terrestrial");
            List<Animal> aquaticAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Aquatic");
            List<Animal> bothAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Both");

            char border = (char)0x25A0;
            bool isPlayerInWater = false;
            int circleFieldCount = 0;

            ConsoleKeyInfo keyInput = new ConsoleKeyInfo();

            Console.WriteLine("Insert field size (NxN)");
            int n = int.Parse(Console.ReadLine());
           
            Console.WriteLine("What animal do you want to be:");
            Console.WriteLine(PlayerCommands.AnimalChoice());
            int wantedAnimalId = int.Parse(Console.ReadLine());

            while (wantedAnimalId > animalList.AddedAnimalList.Count)
            {
                Console.WriteLine("Incorrect animal selection!");
                wantedAnimalId = int.Parse(Console.ReadLine());
            }

            int radius = n / 4;

            string[,] waterField = WaterCommands.GenerateWater(radius, ref circleFieldCount);

            Animal player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == wantedAnimalId);

            Console.WriteLine("Insert npc animals count:");
            int maxCountNpcAnimals = ((n - 2) * (n - 2)) - 1;

            int npcCount = int.Parse(Console.ReadLine());
            while (maxCountNpcAnimals < npcCount)
            {
                Console.WriteLine($"Too much animals! The max capacity animals is {maxCountNpcAnimals}");
                npcCount = int.Parse(Console.ReadLine());
            }

            string commandType = " ";
            int playerRowIndex = 0;
            int playerColIndex = 0;

            string[,] field
                = FieldCommands.GeneratingFieldWithPlayerAndNpcAnimals(player, n, n, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, circleFieldCount);

            Console.Clear();
            Console.WriteLine(FieldCommands.FieldOutput(field));
            Console.WriteLine("Tap 'Enter' to enter a command (Type 'Help' to see the available commands.)");

            bool isThereException = false;

            while (true)
            {
                keyInput = Console.ReadKey();

                HelpfulCommands.RemoveTheSecondRowAfterTheField(field);

                try
                {
                    switch (keyInput.Key)
                    {
                        case ConsoleKey.UpArrow:
                            field = PlayerMove.Up(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, border, isThereException, ref isPlayerInWater);
                            break;
                        case ConsoleKey.DownArrow:
                            field = PlayerMove.Down(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, border, isThereException, ref isPlayerInWater);
                            break;
                        case ConsoleKey.RightArrow:
                            field = PlayerMove.Right(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, border, isThereException, ref isPlayerInWater);
                            break;
                        case ConsoleKey.LeftArrow:
                            field = PlayerMove.Left(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, border, isThereException, ref isPlayerInWater);
                            break;
                    }
                }
                catch (Exception)
                {
                    isThereException = true;
                    Console.WriteLine("You cannot go past the border!");
                }

                if (keyInput.Key == ConsoleKey.Enter)
                {
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);

                    commandType = Console.ReadLine();

                    commandType = commandType.ToLower();

                    if (commandType == "help")
                    {
                        Console.SetCursorPosition(0, field.GetLength(0));
                        Console.Write(new string(' ', Console.WindowWidth));

                        Console.WriteLine("1 -> Change animal");
                        Console.WriteLine("2 -> Reset game");
                        Console.WriteLine("3 -> End game");

                        Console.SetCursorPosition(0, field.GetLength(0));

                        commandType = Console.ReadLine();
                    }

                    if (commandType == "1")
                    {
                        Console.WriteLine("What animal do you pick:");

                        Console.WriteLine(PlayerCommands.AnimalChoice());

                        HelpfulCommands.RemoveTheSecondRowAfterTheField(field);

                        int animalId = int.Parse(Console.ReadLine());

                        player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);

                        while (!(WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex)) && player.LandType == "Aquatic")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        while (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex) && player.LandType == "Terrestrial")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        field[playerRowIndex, playerColIndex] = player.Emoji;

                        Console.Clear();
                    }

                    else if (commandType == "2")
                    {
                        field = FieldCommands.GeneratingFieldWithPlayerAndNpcAnimals(player, n, n, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, circleFieldCount);
            
                        foreach (var currAnimal in animalList.AddedAnimalList)
                        {
                            currAnimal.KillCount = 0;
                        }

                        isPlayerInWater = false;
                        Console.Clear();
                    }
                    else if (commandType == "3")
                    {
                        Console.Clear();
                        foreach (var animal in animalList.AddedAnimalList)
                        {
                            Console.WriteLine($"{animal.Emoji}'s kill count is {animal.KillCount}");
                        }
                        return;
                    }
                }

                Console.SetCursorPosition(0, 0);

                Console.WriteLine(FieldCommands.FieldOutput(field));

                Console.WriteLine("Tap 'Enter' to enter a command (Type 'Help' to see the available commands.)");
            }
        }                                                
    }
}
    


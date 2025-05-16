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

            Random random = new Random();

            AnimalList animalList = new AnimalList();

            List<Animal> terrestrialAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Terrestrial");
            List<Animal> aquaticAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Aquatic");
            List<Animal> bothAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Both");

            char border = (char)0x25A0;
            bool isPlayerInWater = false;
            int circleFieldCount = 0;

            ConsoleKeyInfo keyInput = new ConsoleKeyInfo();

            Console.WriteLine("Insert row count:");
            int rowCount = int.Parse(Console.ReadLine());

            Console.WriteLine("Insert column count:");
            int columnCount = int.Parse(Console.ReadLine());

            Console.WriteLine("What animal do you want to be:");
            Console.WriteLine(PlayerCommands.AnimalChoice());
            int wantedAnimalId = int.Parse(Console.ReadLine());

            while (wantedAnimalId > animalList.AddedAnimalList.Count)
            {
                Console.WriteLine("Incorrect animal selection!");
                wantedAnimalId = int.Parse(Console.ReadLine());
            }

            int radius = rowCount / 4;
            string[,] waterField = WaterCommands.GenerateWater(radius, ref circleFieldCount);

            Animal player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == wantedAnimalId);

            Console.WriteLine("Insert npc animals count:");
            int maxCountNpcAnimals = ((rowCount - 2) * (columnCount - 2)) - 1;

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
                = FieldCommands.GeneratingFieldWithPlayerAndNpcAnimals(player, rowCount, columnCount, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, circleFieldCount);

            Console.Clear();
            Console.WriteLine(FieldCommands.FieldOutput(field));
            Console.WriteLine("Tap 'Enter' to enter a command (Type 'Help' to see the available commands.)");

            bool isThereException = false;

            while (true)
            {
                keyInput = Console.ReadKey();

                if (keyInput.Key == ConsoleKey.UpArrow)
                {
                    try
                    {
                        if (player.LandType == "Terrestrial")
                        {
                            if (!(field[playerRowIndex - 2, playerColIndex] == border.ToString()))
                            {
                                if ((field[playerRowIndex - 1, playerColIndex] == EmojiList.Water
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex])) && player.LandType == "Terrestrial")
                                {
                                    HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }

                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                }

                                field[playerRowIndex, playerColIndex] = "  ";
                                playerRowIndex--;
                                field[playerRowIndex, playerColIndex] = player.Emoji;
                            }

                            if (isThereException && (field[playerRowIndex - 2, playerColIndex] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                        else if (player.LandType == "Aquatic")
                        {
                            if ((field[playerRowIndex - 1, playerColIndex] == "  "
                                || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex])) && player.LandType == "Aquatic")
                            {
                                HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                            {
                                string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                {
                                    continue;
                                }

                            }
                            field[playerRowIndex, playerColIndex] = EmojiList.Water;
                            playerRowIndex--;
                            field[playerRowIndex, playerColIndex] = player.Emoji;

                        }
                        else if (player.LandType == "Both")
                        {
                            if (!(field[playerRowIndex - 2, playerColIndex] == border.ToString()))
                            {
                                if (field[playerRowIndex - 1, playerColIndex] == EmojiList.Water
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex))
                                    {
                                        isPlayerInWater = true;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }

                                    playerRowIndex--;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = true;
                                }
                                else if (field[playerRowIndex - 1, playerColIndex] == "  "
                                    || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerRowIndex--;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = false;
                                }

                            }
                            if (isThereException && (field[playerRowIndex - 2, playerColIndex] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isThereException = true;
                        Console.WriteLine("You cannot go that way!");
                    }
                }//DONE
                else if (keyInput.Key == ConsoleKey.DownArrow)
                {
                    try
                    {
                        if (player.LandType == "Terrestrial")
                        {
                            if (!(field[playerRowIndex + 2, playerColIndex] == border.ToString()))
                            {
                                if ((field[playerRowIndex + 1, playerColIndex] == EmojiList.Water
                                   || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])) && player.LandType == "Terrestrial")
                                {
                                    HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                }
                                field[playerRowIndex, playerColIndex] = "  ";
                                playerRowIndex++;
                                field[playerRowIndex, playerColIndex] = player.Emoji;
                            }
                            if (isThereException && (field[playerRowIndex + 2, playerColIndex] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }

                        else if (player.LandType == "Aquatic")
                        {
                            if ((field[playerRowIndex + 1, playerColIndex] == "  " 
                                || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])) && player.LandType == "Aquatic")
                            {
                                HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                            {
                                string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                {
                                    continue;
                                }

                            }
                            field[playerRowIndex, playerColIndex] = EmojiList.Water;
                            playerRowIndex++;
                            field[playerRowIndex, playerColIndex] = player.Emoji;

                        }
                        else if (player.LandType == "Both")
                        {
                            if (!(field[playerRowIndex + 2, playerColIndex] == border.ToString()))
                            {
                                if (field[playerRowIndex + 1, playerColIndex] == EmojiList.Water
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex))
                                    {
                                        isPlayerInWater = true;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerRowIndex++;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = true;
                                }

                                else if (field[playerRowIndex + 1, playerColIndex] == "  "
                                    || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerRowIndex++;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = false;
                                }

                            }
                            if (isThereException && (field[playerRowIndex + 2, playerColIndex] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isThereException = true;
                        Console.WriteLine("You cannot go that way!");
                    }
                }//DONE
                else if (keyInput.Key == ConsoleKey.RightArrow)
                {
                    try
                    {
                        if (player.LandType == "Terrestrial")
                        {
                            if (!(field[playerRowIndex, playerColIndex + 2] == border.ToString()))
                            {
                                if ((field[playerRowIndex, playerColIndex + 1] == EmojiList.Water 
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1])) && player.LandType == "Terrestrial")
                                {
                                    HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]) 
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                }
                                field[playerRowIndex, playerColIndex] = "  ";
                                playerColIndex++;
                                field[playerRowIndex, playerColIndex] = player.Emoji;
                            }
                            if (isThereException && (field[playerRowIndex, playerColIndex + 1] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                        else if (player.LandType == "Aquatic")
                        {
                            if ((field[playerRowIndex, playerColIndex + 1] == "  " 
                                || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1])) && player.LandType == "Aquatic")
                            {
                                HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                            {
                                string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                {
                                    continue;
                                }

                            }
                            field[playerRowIndex, playerColIndex] = EmojiList.Water;
                            playerColIndex++;
                            field[playerRowIndex, playerColIndex] = player.Emoji;

                        }
                        else if (player.LandType == "Both")
                        {           
                            if (!(field[playerRowIndex, playerColIndex + 2] == border.ToString()))
                            {                 
                                if (field[playerRowIndex, playerColIndex + 1] == EmojiList.Water 
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]) 
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                                {                    
                                    string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }                          
                                    if (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex))
                                    {
                                        isPlayerInWater = true;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerColIndex++;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = true;
                                }

                                else if (field[playerRowIndex, playerColIndex + 1] == "  " 
                                    || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]) 
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }               
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }                                 
                                    playerColIndex++;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = false;
                                }                             

                            }
                            if (isThereException && (field[playerRowIndex, playerColIndex + 1] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isThereException = true;
                        Console.WriteLine("You cannot go that way!");
                    }
                }//DONE
                else if (keyInput.Key == ConsoleKey.LeftArrow)
                {
                    try
                    {
                        if (player.LandType == "Terrestrial")
                        {
                            if (!(field[playerRowIndex, playerColIndex - 2] == border.ToString()))
                            {
                                if ((field[playerRowIndex, playerColIndex - 1] == EmojiList.Water
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])) && player.LandType == "Terrestrial")
                                {
                                    HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                }
                                field[playerRowIndex, playerColIndex] = "  ";
                                playerColIndex--;
                                field[playerRowIndex, playerColIndex] = player.Emoji;
                            }
                            if (isThereException && (field[playerRowIndex, playerColIndex - 1] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                        else if (player.LandType == "Aquatic")
                        {
                            if ((field[playerRowIndex, playerColIndex - 1] == "  " 
                                || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])) && player.LandType == "Aquatic")
                            {
                                HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                            {
                                string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                {
                                    continue;
                                }

                            }
                            field[playerRowIndex, playerColIndex] = EmojiList.Water;
                            playerColIndex--;
                            field[playerRowIndex, playerColIndex] = player.Emoji;

                        }
                        else if (player.LandType == "Both")
                        {
                            if (!(field[playerRowIndex, playerColIndex - 2] == border.ToString()))
                            {
                                if (field[playerRowIndex, playerColIndex - 1] == EmojiList.Water
                                    || aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex))
                                    {
                                        isPlayerInWater = true;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerColIndex--;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = true;
                                }

                                else if (field[playerRowIndex, playerColIndex - 1] == "  "
                                    || terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (isPlayerInWater)
                                    {
                                        field[playerRowIndex, playerColIndex] = EmojiList.Water;
                                    }
                                    else
                                    {
                                        field[playerRowIndex, playerColIndex] = "  ";
                                    }
                                    playerColIndex--;
                                    field[playerRowIndex, playerColIndex] = player.Emoji;
                                    isPlayerInWater = false;
                                }

                            }
                            if (isThereException && (field[playerRowIndex, playerColIndex - 1] != border.ToString()))
                            {
                                Console.Clear();
                                isThereException = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isThereException = true;
                        Console.WriteLine("You cannot go that way!");
                    }
                }//DONE

                else if (keyInput.Key == ConsoleKey.Enter || keyInput.Key != ConsoleKey.UpArrow || keyInput.Key != ConsoleKey.DownArrow || keyInput.Key != ConsoleKey.LeftArrow || keyInput.Key != ConsoleKey.RightArrow)
                {

                    HelpfulCommands.RemoveTheFirstRowAfterTheField(field);

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

                        HelpfulCommands.RemoveTheFirstRowAfterTheField(field);

                        int animalId = int.Parse(Console.ReadLine());

                        player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);

                        while (!(WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex)) && player.LandType == "Aquatic")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        while (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex) && player.LandType == "Terrestrial")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            HelpfulCommands.RemoveTheFirstRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        field[playerRowIndex, playerColIndex] = player.Emoji;

                        Console.Clear();
                    }

                    else if (commandType == "2")
                    {
                        field = FieldCommands.GeneratingFieldWithPlayerAndNpcAnimals(player, rowCount, columnCount, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, circleFieldCount);
            
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
    


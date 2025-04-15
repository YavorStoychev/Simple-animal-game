using OOPProject.Core.Contracts;
using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.EmojiImages;
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
            Console.WriteLine(AnimalChoice());
            int wantedAnimalId = int.Parse(Console.ReadLine());

            while (wantedAnimalId > animalList.AddedAnimalList.Count)
            {
                Console.WriteLine("Incorrect animal selection!");
                wantedAnimalId = int.Parse(Console.ReadLine());
            }

            int radius = rowCount / 4;

            string[,] circlefield = GenerateCircleMatrix(radius, ref circleFieldCount);

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
                = GeneratingFieldWithPlayerAndNpcAnimals(player, rowCount, columnCount, npcCount, ref playerRowIndex, ref playerColIndex, circlefield, animalList, circleFieldCount);

            Console.Clear();
            Console.WriteLine(FieldOutput(field));
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
                                    RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]) 
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex - 1, playerColIndex]))
                            {
                                string enemyEmoji = field[playerRowIndex - 1, playerColIndex];

                                if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }                          
                                    if (IsEntityInWater(field, playerRowIndex, playerColIndex))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                    RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                                {
                                    string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex + 1, playerColIndex]))
                            {
                                string enemyEmoji = field[playerRowIndex + 1, playerColIndex];

                                if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (IsEntityInWater(field, playerRowIndex, playerColIndex))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                    RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]) 
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex + 1]))
                            {
                                string enemyEmoji = field[playerRowIndex, playerColIndex + 1];

                                if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }                          
                                    if (IsEntityInWater(field, playerRowIndex, playerColIndex))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                    RemoveTheFirstRowAfterTheField(field);
                                    Console.WriteLine("You cannot go that way!");
                                    continue;
                                }
                                else if (terrestrialAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1])
                                    || bothAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                                {
                                    string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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
                                RemoveTheFirstRowAfterTheField(field);
                                Console.WriteLine("You cannot go that way!");
                                continue;
                            }
                            else if (aquaticAnimals.Any(x => x.Emoji == field[playerRowIndex, playerColIndex - 1]))
                            {
                                string enemyEmoji = field[playerRowIndex, playerColIndex - 1];

                                if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
                                    {
                                        continue;
                                    }
                                    if (IsEntityInWater(field, playerRowIndex, playerColIndex))
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

                                    if (!AnimalInteraction(field, animalList, player, enemyEmoji))
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

                    RemoveTheFirstRowAfterTheField(field);

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

                        Console.WriteLine(AnimalChoice());

                        RemoveTheFirstRowAfterTheField(field);

                        int animalId = int.Parse(Console.ReadLine());

                        player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);

                        while (!(IsEntityInWater(field, playerRowIndex, playerColIndex)) && player.LandType == "Aquatic")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            RemoveTheFirstRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        while (IsEntityInWater(field, playerRowIndex, playerColIndex) && player.LandType == "Terrestrial")
                        {
                            Console.WriteLine($"You cannot choose that animal! You are not in {player.LandType} land! Choose other animal");
                            RemoveTheFirstRowAfterTheField(field);
                            animalId = int.Parse(Console.ReadLine());
                            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);
                        }

                        field[playerRowIndex, playerColIndex] = player.Emoji;

                        Console.Clear();
                    }

                    else if (commandType == "2")
                    {
                        field = GeneratingFieldWithPlayerAndNpcAnimals(player, rowCount, columnCount, npcCount, ref playerRowIndex, ref playerColIndex, circlefield, animalList, circleFieldCount);
            
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

                Console.WriteLine(FieldOutput(field));

                Console.WriteLine("Tap 'Enter' to enter a command (Type 'Help' to see the available commands.)");
            }
        }

        static string[,] GeneratingFieldWithPlayerAndNpcAnimals(IAnimal player, int rowCount, int columnCount, int npcCount, ref int playerRowIndex, ref int playerColIndex, string[,] circlefield, AnimalList animalList, int circleFieldCount)
        {
            string[,] field = FieldGenerator(rowCount, columnCount);

            PlaceCircleInMatrix(field, circlefield);

            GenerateRandomPlayerLocationInsideTheField(field, player, ref playerRowIndex, ref playerColIndex, ref circleFieldCount);

            GenerateRandomTypeOfEntity(field, npcCount, animalList, circleFieldCount);


            return field;
        }
        static string[,] FieldGenerator(int rowCount, int columnCount)
        {
            char border = (char)0x25A0;

            bool isLeftBorder = false;

            string[,] field = new string[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                isLeftBorder = false;

                for (int col = 0; col < columnCount; col++)
                {

                    if (row == 0 || row == rowCount - 1)
                    {
                        field[row, col] = border.ToString() + " ";
                    }

                    else if (!isLeftBorder)
                    {
                        field[row, col] = border.ToString() + " ";
                        isLeftBorder = true;
                    }

                    else if (isLeftBorder)
                    {
                        field[row, col] = "  ";
                    }
                }

                field[row, columnCount - 1] = border.ToString() + " "; //Inserting right border
            }

            return field;
        }
        static string FieldOutput(string[,] field)
        {
            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    sb.Append($"{field[row, col]} ");
                }
                sb.AppendLine();
            }


            return sb.ToString().TrimEnd();
        }

        static string[,] GenerateRandomPlayerLocationInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int circleFieldCount)
        {
            Random random = new Random();

            playerRowIndex = random.Next(1, field.GetLength(0));
            playerColIndex = random.Next(1, field.GetLength(1));

            PutPlayerInsideTheField(field, player, ref playerRowIndex, ref playerColIndex, ref circleFieldCount);


            field[playerRowIndex, playerColIndex] = player.Emoji;

            return field;
        }

        static void PutPlayerInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int circleFieldCount)
        {
            Random random = new Random();
            char border = (char)0x25A0;


            bool isTerrestrial = IsEntityTerrestrial(player);
            bool isAquatic = IsEntityAquatic(player);

            if (isTerrestrial && isAquatic)
            {
                while (field[playerRowIndex, playerColIndex] == border.ToString() + " ")
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
            }
            else if (isAquatic)
            {
                while (field[playerRowIndex, playerColIndex] != EmojiList.Water)
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
                circleFieldCount--;
            }
            else if (isTerrestrial)
            {
                while (field[playerRowIndex, playerColIndex] != "  ")
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
            }
        }

        //Boolean methods
        static bool IsEntityInWater(string[,] field, int playerRowIndex, int playerColIndex)
        {
            int circleCountAroundThePlayer = 0;


            if (field[playerRowIndex + 1, playerColIndex] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex - 1, playerColIndex] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex, playerColIndex + 1] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex, playerColIndex - 1] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }

            if (circleCountAroundThePlayer > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static bool IsEntityTerrestrial(IAnimal player)
        {
            return player.LandType == "Terrestrial" || player.LandType == "Both" ? true : false;
        }
        static bool IsEntityAquatic(IAnimal player)
        {
            return player.LandType == "Aquatic" || player.LandType == "Both" ? true : false;
        }
        static bool AnimalInteraction(string[,] field, AnimalList animalList, Animal player, string enemyEmoji)
        {
            Animal enemy = animalList.AddedAnimalList.FirstOrDefault(x => x.Emoji == enemyEmoji);
            
            if (enemy == null)
            {
                return true;
            }

            if ((player.LandType == enemy.LandType) || player.LandType == "Both" || enemy.LandType == "Both")
            {
                if ((player.Hp + player.Defence) > (enemy.Hp + enemy.Defence))
                {
                    Animal animal = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == player.Id);                
                    animal.KillCount++;                  
                    return true;
                }
                else if ((player.Hp + player.Defence) <= (enemy.Hp + enemy.Defence))
                {
                    RemoveTheFirstRowAfterTheField(field);
                    Console.WriteLine("You cannot kill that animal!");
                    return false;
                }
            }
            else if (player.LandType != enemy.LandType)
            {
                RemoveTheFirstRowAfterTheField(field);
                Console.WriteLine("You cannot interfere with this animal!");
                return false;
            }

            return true;
        }


        static void GenerateRandomTypeOfEntity(string[,] field, int npcCount, AnimalList animalList, int circleFieldCount)
        {
            Random random = new Random();

            for (int i = 0; i < npcCount; i++)
            {
                int npcRowIndex = random.Next(1, field.GetLength(0));
                int npcColIndex = random.Next(1, field.GetLength(1));

                int randomAnimalId = random.Next(1, 13);

                IAnimal animal = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == randomAnimalId);

                PutCurrEntityInsideTheField(field, ref npcRowIndex, ref npcColIndex, ref animal, ref circleFieldCount, animalList);

                field[npcRowIndex, npcColIndex] = animal.Emoji;
            }
        }
        static void PutCurrEntityInsideTheField(string[,] field, ref int npcRowIndex, ref int npcColIndex, ref IAnimal animal, ref int circleFieldCount, AnimalList animalList)//За оправяне
        {
            Random random = new Random();

            char border = (char)0x25A0;

        rewritingTheindexes:

            while (field[npcRowIndex, npcColIndex] != "  ")
            {
                if (field[npcRowIndex, npcColIndex] == EmojiList.Water && (animal.LandType == "Aquatic" || animal.LandType == "Both") && field[npcRowIndex, npcColIndex] != border.ToString() + " ")
                {
                    circleFieldCount--;
                    break;
                }

                npcRowIndex = random.Next(1, field.GetLength(0) - 1);
                npcColIndex = random.Next(1, field.GetLength(1) - 1);
            }

            if (field[npcRowIndex, npcColIndex] == "  " && animal.LandType == "Aquatic")
            {
                if (circleFieldCount == 0)
                {
                    while (animal.LandType == "Aquatic")
                    {
                        int newAnimalId = random.Next(1, 13);
                        animal = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == newAnimalId);
                    }
                }

                npcRowIndex = random.Next(1, field.GetLength(0) - 1);
                npcColIndex = random.Next(1, field.GetLength(1) - 1);

                goto rewritingTheindexes;
            }
        }
        static void RemoveTheFirstRowAfterTheField(string[,] field)
        {
            Console.SetCursorPosition(0, field.GetLength(0));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, field.GetLength(0));
        }
        static string AnimalChoice()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1 - 🐅; 2 - 🐆; 3 - 🐊; 4 - 🦍");
            sb.AppendLine("5 - 🦏; 6 - 🦈; 7 - 🦑; 8 - 🐟");
            sb.AppendLine("9 - 🐂; 10 - 🐧; 11 - 🐘; 12 - 🐒");
            return sb.ToString().TrimEnd();
        }


        //CircleField     
        static string[,] GenerateCircleMatrix(int radius, ref int circleFieldCount)
        {
            int size = radius * 2 + 1;
            string[,] matrix = new string[size, size];

            // Initialize with empty spaces
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = " ";

            double margin = 0.5; // Tweak this to avoid extra side points base : 0.9

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    double value = x * x + y * y;
                    double outerBound = (radius - margin) * (radius - margin); // Stricter boundary

                    if (value <= outerBound) // Fills the entire circle while avoiding outer artifacts
                    {
                        circleFieldCount++;
                        matrix[y + radius, x + radius] = EmojiList.Water;
                    }
                }
            }

            return matrix;
        }
        static void PlaceCircleInMatrix(string[,] field, string[,] circleMatrix)
        {
            int outerSize = field.GetLength(0);
            int circleSize = circleMatrix.GetLength(0);

            int startY = (outerSize - circleSize) / 2;
            int startX = (outerSize - circleSize) / 2;

            for (int y = 0; y < circleSize; y++)
            {
                for (int x = 0; x < circleSize; x++)
                {
                    if (circleMatrix[y, x] == EmojiList.Water)
                    {
                        field[startY + y, startX + x] = EmojiList.Water;
                    }
                }
            }
        }

    }
}
    


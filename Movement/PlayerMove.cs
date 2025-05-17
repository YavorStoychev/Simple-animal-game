using OOPProject.Models;
using OOPProject.Movement;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.WaterUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Movement
{
    public class PlayerMove
    {       

        public static string[,] Up(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex,AnimalList animalList ,List<Animal> terrestrialAnimals, List<Animal> aquaticAnimals, List<Animal> bothAnimals ,char border,bool isThereException, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == "Terrestrial")
            {
                if (!(field[tempPlayerRowIndex - 2, tempPlayerColIndex] == border.ToString()))
                {
                    if ((field[tempPlayerRowIndex - 1, tempPlayerColIndex] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex])) && !player.CanSwim)
                    {                       
                        Console.WriteLine("You cannot go that way!");
                        return field;
                    }

                    else if (terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex - 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                    }

                    field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                    tempPlayerRowIndex--;
                    field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                }

                if (isThereException && (field[tempPlayerRowIndex - 1, tempPlayerColIndex] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }
            else if (player.LandType == "Aquatic")
            {
                if ((field[tempPlayerRowIndex - 1, tempPlayerColIndex] == "  "
                    || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex])) && player.LandType == "Aquatic")
                {
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot go that way!");
                    return field;
                }
                else if (aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex]))
                {
                    string enemyEmoji = field[tempPlayerRowIndex - 1, tempPlayerColIndex];

                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                    {
                        return field;
                    }

                }
                field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                tempPlayerRowIndex--;
                field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;

            }
            else if (player.LandType == "Both")
            {
                if (!(field[tempPlayerRowIndex - 2, tempPlayerColIndex] == border.ToString()))
                {
                    if (field[tempPlayerRowIndex - 1, tempPlayerColIndex] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex - 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (WaterCommands.IsPlayerInWater(field, tempPlayerRowIndex, tempPlayerColIndex))
                        {
                            tempIsPlayerInWater = true;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }

                        tempPlayerRowIndex--;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = true;
                    }
                    else if (field[tempPlayerRowIndex - 1, tempPlayerColIndex] == "  "
                        || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex - 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex - 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerRowIndex--;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = false;
                    }

                }
                if (isThereException && (field[tempPlayerRowIndex - 1, tempPlayerColIndex] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }

            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Down(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, List<Animal> terrestrialAnimals, List<Animal> aquaticAnimals, List<Animal> bothAnimals, char border, bool isThereException, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;


            if (player.LandType == "Terrestrial")
            {
                if (!(field[tempPlayerRowIndex + 2, tempPlayerColIndex] == border.ToString()))
                {
                    if ((field[tempPlayerRowIndex + 1, tempPlayerColIndex] == EmojiList.Water
                       || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex])) && player.LandType == "Terrestrial")
                    {
                        HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                        Console.WriteLine("You cannot go that way!");
                        return field;
                    }
                    else if (terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex + 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                    }
                    field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                    tempPlayerRowIndex++;
                    field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                }
                if (isThereException && (field[tempPlayerRowIndex + 1, tempPlayerColIndex] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }
            else if (player.LandType == "Aquatic")
            {
                if ((field[tempPlayerRowIndex + 1, tempPlayerColIndex] == "  "
                    || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex])) && player.LandType == "Aquatic")
                {
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot go that way!");
                    return field;
                }
                else if (aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex]))
                {
                    string enemyEmoji = field[tempPlayerRowIndex + 1, tempPlayerColIndex];

                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                    {
                        return field;
                    }

                }
                field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                tempPlayerRowIndex++;
                field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;

            }
            else if (player.LandType == "Both")
            {
                if (!(field[tempPlayerRowIndex + 2, tempPlayerColIndex] == border.ToString()))
                {
                    if (field[tempPlayerRowIndex + 1, tempPlayerColIndex] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex + 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (WaterCommands.IsPlayerInWater(field, tempPlayerRowIndex, tempPlayerColIndex))
                        {
                            tempIsPlayerInWater = true;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerRowIndex++;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = true;
                    }

                    else if (field[tempPlayerRowIndex + 1, tempPlayerColIndex] == "  "
                        || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex + 1, tempPlayerColIndex]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex + 1, tempPlayerColIndex];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerRowIndex++;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = false;
                    }

                }
                if (isThereException && (field[tempPlayerRowIndex + 1, tempPlayerColIndex] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Right(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, List<Animal> terrestrialAnimals, List<Animal> aquaticAnimals, List<Animal> bothAnimals, char border, bool isThereException, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == "Terrestrial")
            {
                if (!(field[tempPlayerRowIndex, tempPlayerColIndex + 2] == border.ToString()))
                {
                    if ((field[tempPlayerRowIndex, tempPlayerColIndex + 1] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1])) && player.LandType == "Terrestrial")
                    {
                        HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                        Console.WriteLine("You cannot go that way!");
                        return field;
                    }
                    else if (terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex + 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                    }
                    field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                    tempPlayerColIndex++;
                    field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                }
                if (isThereException && (field[tempPlayerRowIndex, tempPlayerColIndex + 1] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }
            else if (player.LandType == "Aquatic")
            {
                if ((field[tempPlayerRowIndex, tempPlayerColIndex + 1] == "  "
                    || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1])) && player.LandType == "Aquatic")
                {
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot go that way!");
                    return field;
                }
                else if (aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1]))
                {
                    string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex + 1];

                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                    {
                        return field;
                    }

                }
                field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                tempPlayerColIndex++;
                field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;

            }
            else if (player.LandType == "Both")
            {
                if (!(field[tempPlayerRowIndex, tempPlayerColIndex + 2] == border.ToString()))
                {
                    if (field[tempPlayerRowIndex, tempPlayerColIndex + 1] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex + 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (WaterCommands.IsPlayerInWater(field, tempPlayerRowIndex, tempPlayerColIndex))
                        {
                            tempIsPlayerInWater = true;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerColIndex++;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = true;
                    }

                    else if (field[tempPlayerRowIndex, tempPlayerColIndex + 1] == "  "
                        || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex + 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex + 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerColIndex++;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = false;
                    }

                }
                if (isThereException && (field[tempPlayerRowIndex, tempPlayerColIndex + 1] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Left(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, List<Animal> terrestrialAnimals, List<Animal> aquaticAnimals, List<Animal> bothAnimals, char border, bool isThereException, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == "Terrestrial")
            {
                if (!(field[tempPlayerRowIndex, tempPlayerColIndex - 2] == border.ToString()))
                {
                    if ((field[tempPlayerRowIndex, tempPlayerColIndex - 1] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1])) && player.LandType == "Terrestrial")
                    {
                        HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                        Console.WriteLine("You cannot go that way!");
                        return field;
                    }
                    else if (terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex - 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                    }
                    field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                    tempPlayerColIndex--;
                    field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                }
                if (isThereException && (field[tempPlayerRowIndex, tempPlayerColIndex - 1] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }
            else if (player.LandType == "Aquatic")
            {
                if ((field[tempPlayerRowIndex, tempPlayerColIndex - 1] == "  "
                    || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1])) && player.LandType == "Aquatic")
                {
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot go that way!");
                    return field;
                }
                else if (aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1]))
                {
                    string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex - 1];

                    if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                    {
                        return field;
                    }

                }
                field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                tempPlayerColIndex--;
                field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;

            }
            else if (player.LandType == "Both")
            {
                if (!(field[tempPlayerRowIndex, tempPlayerColIndex - 2] == border.ToString()))
                {
                    if (field[tempPlayerRowIndex, tempPlayerColIndex - 1] == EmojiList.Water
                        || aquaticAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex - 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (WaterCommands.IsPlayerInWater(field, tempPlayerRowIndex, tempPlayerColIndex))
                        {
                            tempIsPlayerInWater = true;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerColIndex--;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = true;
                    }

                    else if (field[tempPlayerRowIndex, tempPlayerColIndex - 1] == "  "
                        || terrestrialAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1])
                        || bothAnimals.Any(x => x.Emoji == field[tempPlayerRowIndex, tempPlayerColIndex - 1]))
                    {
                        string enemyEmoji = field[tempPlayerRowIndex, tempPlayerColIndex - 1];

                        if (!HelpfulCommands.BattleInteraction(field, animalList, player, enemyEmoji))
                        {
                            return field;
                        }
                        if (tempIsPlayerInWater)
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
                        }
                        else
                        {
                            field[tempPlayerRowIndex, tempPlayerColIndex] = "  ";
                        }
                        tempPlayerColIndex--;
                        field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
                        tempIsPlayerInWater = false;
                    }

                }
                if (isThereException && (field[tempPlayerRowIndex, tempPlayerColIndex - 1] != border.ToString()))
                {
                    Console.Clear();
                    isThereException = false;
                }
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }

    }
}

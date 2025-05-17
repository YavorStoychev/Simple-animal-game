using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.EmojiImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.PlayerUtilities
{
    public class PlayerCommands
    {
        public static string[,] GenerateRandomPlayerLocationInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int circleFieldCount)
        {
            Random random = new Random();

            playerRowIndex = random.Next(1, field.GetLength(0));
            playerColIndex = random.Next(1, field.GetLength(1));

            InsertPlayerInsideTheField(field, player, ref playerRowIndex, ref playerColIndex, ref circleFieldCount);


            field[playerRowIndex, playerColIndex] = player.Emoji;

            return field;
        }

        public static void InsertPlayerInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int circleFieldCount)
        {
            Random random = new Random();
            char border = (char)0x25A0;


            bool isTerrestrial = IsPlayerTerrestrial(player);
            bool isAquatic = IsPlayerAquatic(player);

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

        public static bool IsPlayerTerrestrial(IAnimal player)
        {
            return player.LandType == "Terrestrial" || player.LandType == "Both" ? true : false;
        }

       public static bool IsPlayerAquatic(IAnimal player)
        {
            return player.LandType == "Aquatic" || player.LandType == "Both" ? true : false;
        }

       public static string AnimalChoice()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"1 - {EmojiList.Tiger}; 2 - {EmojiList.Leopard}; 3 - {EmojiList.Crocodile}; 4 - {EmojiList.Gorilla}");
            sb.AppendLine($"5 - {EmojiList.Rhino}; 6 - {EmojiList.Shark}; 7 - {EmojiList.Squid}; 8 - {EmojiList.Fish}");
            sb.AppendLine($"9 - {EmojiList.Ox}; 10 - {EmojiList.Penguin}; 11 - {EmojiList.Elephant}; 12 - {EmojiList.Monkey}");
            return sb.ToString().TrimEnd();
        }

       public static bool BattleInteraction(string[,] field, AnimalList animalList, Animal player, string enemyEmoji)
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
                    HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot kill that animal!");
                    return false;
                }
            }
            else if (player.LandType != enemy.LandType)
            {
                HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
                Console.WriteLine("You cannot interfere with this animal!");
                return false;
            }

            return true;
        }


    }
}

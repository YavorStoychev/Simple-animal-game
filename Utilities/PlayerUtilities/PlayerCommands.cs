using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.PlayerUtilities
{
    public class PlayerCommands
    {
        public static string[,] GenerateRandomPlayerLocationInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int waterFieldCount)
        {
            Random random = new Random();

            playerRowIndex = random.Next(1, field.GetLength(0));
            playerColIndex = random.Next(1, field.GetLength(1));

            InsertPlayerInsideTheField(field, player, ref playerRowIndex, ref playerColIndex, ref waterFieldCount);


            field[playerRowIndex, playerColIndex] = player.Emoji;

            return field;
        }

        public static void InsertPlayerInsideTheField(string[,] field, IAnimal player, ref int playerRowIndex, ref int playerColIndex, ref int circleFieldCount)
        {
            Random random = new Random();
            char border = (char)0x25A0;          

            if (player.LandType == LandType.Terrestrial && player.CanSwim)
            {
                while (field[playerRowIndex, playerColIndex] == border.ToString() + " ")
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
            }
            if (player.LandType == LandType.Terrestrial && !player.CanSwim)
            {
                while (field[playerRowIndex, playerColIndex] != "  ")
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
            }
            else if (player.LandType == LandType.Aquatic)
            {
                while (field[playerRowIndex, playerColIndex] != EmojiList.Water)
                {
                    playerRowIndex = random.Next(1, field.GetLength(0));
                    playerColIndex = random.Next(1, field.GetLength(1));
                }
                circleFieldCount--;
            }
        }
     
       public static string AnimalChoice()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"1 - {EmojiList.Tiger}; 2 - {EmojiList.Leopard}; 3 - {EmojiList.Crocodile}; 4 - {EmojiList.Gorilla}");
            sb.AppendLine($"5 - {EmojiList.Rhino}; 6 - {EmojiList.Shark}; 7 - {EmojiList.Squid}; 8 - {EmojiList.Fish}");
            sb.AppendLine($"9 - {EmojiList.Ox}; 10 - {EmojiList.Penguin}; 11 - {EmojiList.Elephant}; 12 - {EmojiList.Monkey}");
            return sb.ToString().TrimEnd();
        }

       public static int GetPlayerAnimalId(AnimalList animalList)
        {
            int playerAnimalId = int.Parse(Console.ReadLine());

            while (playerAnimalId > animalList.AddedAnimalList.Count)
            {
                Console.WriteLine(OutputMessages.IncorrectAnimalSelection);
                playerAnimalId = int.Parse(Console.ReadLine());
            }

            return playerAnimalId;
        }

    }
}

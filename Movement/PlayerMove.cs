using OOPProject.Models;
using OOPProject.Movement;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.Messages;
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
        public static string[,] Up(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex,AnimalList animalList , ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            string nextElement = field[tempPlayerRowIndex - 1, tempPlayerColIndex];

            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerRowIndex - 1 > 0)
                {                   
                    TerrestrialMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, -1, 0, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }
                
            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field,player,animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, -1, 0);            
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Down(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList,  ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerRowIndex + 2 < field.GetLength(0))
                {
                    TerrestrialMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 1, 0, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }         
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 1, 0);
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Right(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerColIndex + 2 < field.GetLength(0))
                {
                    TerrestrialMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, 1, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, 1);
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Left(string[,] field, Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

           if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerColIndex - 1 > 0)
                {
                    TerrestrialMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, -1, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, -1);
            }

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
       
        private static void TerrestrialMove(string[,] field, Animal player, AnimalList animalList ,ref int playerRowIndex, ref int playerColIndex, int rowMove, int colMove, ref bool isPlayerInWater)
        {        

            int newRow = playerRowIndex + rowMove;
            int newCol = playerColIndex + colMove;

            string nextElement = field[newRow, newCol];

            if (WaterCommands.IsPlayerInWater(field,playerRowIndex,playerColIndex))//Предварително питане, защото може да се озове играча във вода с сухоземно животно
            {
                isPlayerInWater = true;
            }

            if (nextElement == EmojiList.Water && !player.CanSwim)
            {
                Console.WriteLine(OutputMessages.CannotGoInsideTheWater);
                return;
            }

            if (animalList.AddedAnimalList.Any(a => a.Emoji == nextElement))
            {
                if (!HelpfulCommands.BattleInteraction(field, animalList, player, nextElement))
                {
                    return;
                }
            }

            player.Energy -= 5;



            field[playerRowIndex, playerColIndex] = isPlayerInWater ? EmojiList.Water : "  ";
            playerRowIndex = newRow;
            playerColIndex = newCol;
            field[playerRowIndex, playerColIndex] = player.Emoji;

            isPlayerInWater = nextElement == EmojiList.Water || animalList.AddedAnimalList
                .Where(a => a.LandType == LandType.Aquatic)
                .Any(a => a.Emoji == nextElement);
        }

        private static void AquaticMove(string[,] field, Animal player, AnimalList animalList, ref int tempPlayerRowIndex, ref int tempPlayerColIndex, int rowMove, int colMove)
        {
            int newRow = tempPlayerRowIndex + rowMove;
            int newCol = tempPlayerColIndex + colMove;

            string nextElement = field[newRow, newCol];

            if (nextElement == "  ")
            {
                HelpfulCommands.RemoveTheNRowAfterTheField(field,2);
                Console.WriteLine(OutputMessages.CannotGoOutsideTheWater);
                return;
            }
            if (animalList.AddedAnimalList.Any(a => a.Emoji == nextElement))
            {
                if (!HelpfulCommands.BattleInteraction(field, animalList, player, nextElement))
                {
                    return;
                }
            }

            field[tempPlayerRowIndex, tempPlayerColIndex] = EmojiList.Water;
            tempPlayerRowIndex = newRow;
            tempPlayerColIndex = newCol;
            field[tempPlayerRowIndex, tempPlayerColIndex] = player.Emoji;
        }
    }
}

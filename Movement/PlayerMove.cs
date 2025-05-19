using OOPProject.Models;
using OOPProject.Movement;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.Messages;
using OOPProject.Utilities.PlayerUtilities;
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
        public static string[,] Up(string[,] field, ref Animal player, ref int playerRowIndex, ref int playerColIndex,AnimalList animalList , ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;


            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerRowIndex - 1 > 0)
                {                   
                    TerrestrialMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, -1, 0, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }
                
            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field,ref player,animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, -1, 0);            
            }           

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Down(string[,] field, ref Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList,  ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerRowIndex + 2 < field.GetLength(0))
                {
                    TerrestrialMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 1, 0, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }         
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 1, 0);
            }
          

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Right(string[,] field, ref Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

            if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerColIndex + 2 < field.GetLength(0))
                {
                    TerrestrialMove(field,ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, 1, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, 1);
            }

           

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
        public static string[,] Left(string[,] field, ref Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList, ref bool isPlayerInWater)
        {
            int tempPlayerRowIndex = playerRowIndex;
            int tempPlayerColIndex = playerColIndex;
            bool tempIsPlayerInWater = isPlayerInWater;

           if (player.LandType == LandType.Terrestrial)
            {

                if (tempPlayerColIndex - 1 > 0)
                {
                    TerrestrialMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, -1, ref tempIsPlayerInWater);
                }
                else
                {
                    Console.WriteLine(ExceptionMessages.BorderException);
                    return field;
                }

            }
            else if (player.LandType == LandType.Aquatic)
            {
                AquaticMove(field, ref player, animalList, ref tempPlayerRowIndex, ref tempPlayerColIndex, 0, -1);
            }            

            playerRowIndex = tempPlayerRowIndex;
            playerColIndex = tempPlayerColIndex;
            isPlayerInWater = tempIsPlayerInWater;
            return field;
        }
       
        private static void TerrestrialMove(string[,] field, ref Animal player, AnimalList animalList ,ref int playerRowIndex, ref int playerColIndex, int rowMove, int colMove, ref bool isPlayerInWater)
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
                if (!HelpfulCommands.BattleInteraction(field, animalList, ref player, ref playerRowIndex, ref playerColIndex, nextElement))
                {
                    return;
                }
            }
            else
            {
                player.ConsumeEnergy();

                if (player.Energy <= 0)
                {
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 2);
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 1);
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 0);

                    field = GameOptions.ChangePlayerAnimal(field, ref player, ref playerRowIndex, ref playerColIndex, animalList);
                    return;
                }

            }


            field[playerRowIndex, playerColIndex] = isPlayerInWater ? EmojiList.Water : "  ";
            playerRowIndex = newRow;
            playerColIndex = newCol;
            field[playerRowIndex, playerColIndex] = player.Emoji;

            isPlayerInWater = nextElement == EmojiList.Water || animalList.AddedAnimalList
                .Where(a => a.LandType == LandType.Aquatic)
                .Any(a => a.Emoji == nextElement);
        }

        private static void AquaticMove(string[,] field, ref Animal player, AnimalList animalList, ref int playerRowIndex, ref int playerColIndex, int rowMove, int colMove)
        {
            int newRow = playerRowIndex + rowMove;
            int newCol = playerColIndex + colMove;

            string nextElement = field[newRow, newCol];

            if (nextElement == "  ")
            {
                HelpfulCommands.RemoveTheNRowAfterTheField(field,2);
                Console.WriteLine(OutputMessages.CannotGoOutsideTheWater);
                return;
            }
            if (animalList.AddedAnimalList.Any(a => a.Emoji == nextElement))
            {
                if (!HelpfulCommands.BattleInteraction(field, animalList, ref player, ref playerRowIndex, ref playerColIndex, nextElement))
                {
                    return;
                }
            }
            else
            {
                player.ConsumeEnergy();

                if (player.Energy <= 0)
                {
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 2);
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 1);
                    HelpfulCommands.RemoveTheNRowAfterTheField(field, 0);

                    field = GameOptions.ChangePlayerAnimal(field, ref player, ref playerRowIndex, ref playerColIndex, animalList);
                    return;
                }

            }

            field[playerRowIndex, playerColIndex] = EmojiList.Water;
            playerRowIndex = newRow;
            playerColIndex = newCol;
            field[playerRowIndex, playerColIndex] = player.Emoji;
        }
    }
}

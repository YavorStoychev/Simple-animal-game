using OOPProject.Models;
using OOPProject.Utilities.FieldUtilities;
using OOPProject.Utilities.Messages;
using OOPProject.Utilities.PlayerUtilities;
using OOPProject.Utilities.WaterUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities
{
    public class GameOptions
    {
        public static string[,] ChangePlayerAnimal(string[,] field,ref Animal player, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList)
        {
            Console.WriteLine(InputMessages.AnimalPick);

            Console.WriteLine(PlayerCommands.AnimalChoice());

            HelpfulCommands.RemoveTheNRowAfterTheField(field,0);

            int animalId = PlayerCommands.GetPlayerAnimalId(animalList);


            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);

            while (player.Energy <= 0 || player.Hp <= 0)
            {
                HelpfulCommands.RemoveTheNRowAfterTheField(field, 4);
                HelpfulCommands.RemoveTheNRowAfterTheField(field, 3);
                HelpfulCommands.RemoveTheNRowAfterTheField(field, 2);
                HelpfulCommands.RemoveTheNRowAfterTheField(field, 1);
                HelpfulCommands.RemoveTheNRowAfterTheField(field, 0);

                Console.WriteLine(string.Format(OutputMessages.AlreadyDead,player.Emoji));
                Thread.Sleep(2000);
                HelpfulCommands.RemoveTheNRowAfterTheField(field,0);
                ChangePlayerAnimal(field,ref player,ref playerRowIndex,ref playerColIndex,animalList);
            }

            while (!WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex) && player.LandType == LandType.Aquatic)
            {
                player = ChangeCurrPlayerAnimalException(field, ref player, animalList);
            }

            while (WaterCommands.IsPlayerInWater(field, playerRowIndex, playerColIndex) && player.LandType == LandType.Terrestrial && !player.CanSwim)
            {
                player = ChangeCurrPlayerAnimalException(field, ref player, animalList);
            }

            field[playerRowIndex, playerColIndex] = player.Emoji;

            Console.Clear();

            return field;
        }

        private static Animal ChangeCurrPlayerAnimalException(string[,] field, ref Animal player, AnimalList animalList)
        {         
            Console.WriteLine(string.Format(OutputMessages.CannotPickCurrAnimal, player.LandType));

            HelpfulCommands.RemoveTheNRowAfterTheField(field,0);

            int animalId = int.Parse(Console.ReadLine());

            player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == animalId);

            return player;
        }

        public static string[,] ResetGame(string[,] field, Animal player, int n, int npcCount, ref int playerRowIndex, ref int playerColIndex, AnimalList animalList,ref bool isPlayerInWater)
        {
            animalList = new AnimalList();

            field = FieldCommands.GenerateFieldWithPlayerAndNpcAnimals(player, n, n, npcCount, ref playerRowIndex, ref playerColIndex, animalList);

            foreach (var currAnimal in animalList.AddedAnimalList)
            {
                currAnimal.KillCount = 0;
            }

            isPlayerInWater = false;
            Console.Clear();

            return field;
        }

        public static void EndGame(AnimalList animalList)
        {
            Console.Clear();

            foreach (var animal in animalList.AddedAnimalList)
            {
                Console.WriteLine(string.Format(OutputMessages.PlayerKillCount, animal.Emoji, animal.KillCount));              
            }
        }
        public static string GameMenu(string[,] field)
        {
            HelpfulCommands.RemoveTheNRowAfterTheField(field, 0);

            string commandType = Console.ReadLine();
            commandType = commandType.ToLower();

            if (commandType == "help")
            {
                Console.SetCursorPosition(0, field.GetLength(0));
                Console.Write(new string(' ', Console.WindowWidth));
                AvailableOptions();
                Console.SetCursorPosition(0, field.GetLength(0));

                commandType = Console.ReadLine();
            }

            return commandType;
        }

        private static void AvailableOptions()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1 -> Change animal");
            sb.AppendLine("2 -> Reset game");
            sb.AppendLine("3 -> End game");
            Console.WriteLine(string.Join(" ",sb.ToString().TrimEnd()));
        }
    }
}

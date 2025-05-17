using OOPProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities
{
    public class HelpfulCommands
    {
        public static void RemoveTheFirstRowAfterTheField(string[,] field)
        {
            Console.SetCursorPosition(0, field.GetLength(0));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, field.GetLength(0));
        }

        public static void RemoveTheSecondRowAfterTheField(string[,] field)
        {
            Console.SetCursorPosition(0, field.GetLength(0) + 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, field.GetLength(0) + 1);
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
                    RemoveTheSecondRowAfterTheField(field);
                    Console.WriteLine("You cannot kill that animal!");
                    return false;
                }
            }
            else if (player.LandType != enemy.LandType)
            {
                RemoveTheSecondRowAfterTheField(field);
                Console.WriteLine("You cannot interfere with this animal!");
                return false;
            }

            return true;
        }


    }
}

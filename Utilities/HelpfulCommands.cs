using OOPProject.Models;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities
{
    public class HelpfulCommands
    {
        public static void RemoveTheNRowAfterTheField(string[,] field, int n)
        {
            Console.SetCursorPosition(0, field.GetLength(0) + n);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, field.GetLength(0) + n);
        }
      

        public static bool BattleInteraction(string[,] field, AnimalList animalList, Animal player, string enemyEmoji)
        {
            Animal enemy = animalList.AddedAnimalList.FirstOrDefault(x => x.Emoji == enemyEmoji);

            if (enemy == null)
            {
                Console.WriteLine(ExceptionMessages.InvalidAnimalType);
                return false;
            }
            if ((player.LandType == enemy.LandType) || (player.LandType != enemy.LandType && player.CanSwim))
            {
                RemoveTheNRowAfterTheField(field, 0);
                Console.WriteLine(InputMessages.BeginOrCancelFight);
                Console.WriteLine(string.Format(InputMessages.PlayerInfo,EmojiList.Heart,player.Hp,EmojiList.Attack,player.Attack ,EmojiList.Energy,player.Energy));
                Console.WriteLine(string.Format(InputMessages.AreYouSureAboutTheFight,enemyEmoji));
                Console.WriteLine(string.Format(InputMessages.AnimalInfo, EmojiList.Heart, enemy.Hp, EmojiList.Attack, enemy.Attack));
                
                ConsoleKeyInfo keyInput = new ConsoleKeyInfo();

                keyInput = Console.ReadKey();              
                    
                if (keyInput.Key == ConsoleKey.Enter)
                {
                    RemoveTheNRowAfterTheField(field, 0);
                    RemoveTheNRowAfterTheField(field, 1);
                    RemoveTheNRowAfterTheField(field, 2);
                    RemoveTheNRowAfterTheField(field, 3);

                    for (int i = 3; i >= 1; i--)
                    {
                        RemoveTheNRowAfterTheField(field, 0);
                        Console.WriteLine($"Battle will begin in {i}...");
                        Thread.Sleep(1000);
                    }
                  

                    while (player.Hp > 0)
                    {
                        if (enemy.Hp <= 0)
                        {
                            player.KillCount++;

                            Console.WriteLine(string.Format(OutputMessages.SuccessfullyKilledAnAnimal, enemy.Emoji, player.Emoji, player.KillCount));

                            return true;
                        }

                        RemoveTheNRowAfterTheField(field, 1);
                        RemoveTheNRowAfterTheField(field, 0);

                        Console.WriteLine(string.Format(InputMessages.PlayerInfo, EmojiList.Heart, player.Hp, EmojiList.Attack, player.Attack, EmojiList.Energy, player.Energy));
                        Console.WriteLine(string.Format(InputMessages.AnimalInfo, EmojiList.Heart, enemy.Hp, EmojiList.Attack, enemy.Attack));
                                            
                        player.Hp -= enemy.Attack;
                        enemy.Hp -= player.Attack;

                        Thread.Sleep(1500);
                    }
                   
                }

                else if (player.Hp  <= enemy.Hp)
                {
                    RemoveTheNRowAfterTheField(field,2);
                    Console.WriteLine(OutputMessages.CannotKillAnimal);
                    return false;
                }
            }
            else if (player.LandType != enemy.LandType)
            {
                RemoveTheNRowAfterTheField(field,2);
                Console.WriteLine(string.Format(OutputMessages.CannotInterfere,enemy.Emoji));
                return false;
            }

            return true;
        }


    }
}

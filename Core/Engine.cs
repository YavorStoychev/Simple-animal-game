using OOPProject.Core.Contracts;
using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Movement;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.FieldUtilities;
using OOPProject.Utilities.Messages;
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

            AnimalList animalList = new AnimalList();
            
            ConsoleKeyInfo keyInput = new ConsoleKeyInfo();

            Console.WriteLine(InputMessages.FieldSize);
            int n = int.Parse(Console.ReadLine());
           
            Console.WriteLine(InputMessages.AnimalPick);
            Console.WriteLine(PlayerCommands.AnimalChoice());
            int playerAnimalId = PlayerCommands.GetPlayerAnimalId(animalList);

            Animal player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == playerAnimalId);

            Console.WriteLine(InputMessages.AnimalCountInput);
            int npcCount = NpcCommands.GenerateNpcCount(n);
           
            int playerRowIndex = 0;
            int playerColIndex = 0;
            bool isPlayerInWater = false;

            string[,] field
                = FieldCommands.GenerateFieldWithPlayerAndNpcAnimals(player, n, n, npcCount, ref playerRowIndex, ref playerColIndex, animalList);

            Console.Clear();
            Console.WriteLine(FieldCommands.FieldOutput(field));
            Console.WriteLine(InputMessages.EnterACommand);
            Console.WriteLine(string.Format(InputMessages.PlayerInfo,EmojiList.Heart,player.Hp,EmojiList.Attack,player.Attack ,EmojiList.Energy,player.Energy));

            while (true)
            {
                keyInput = Console.ReadKey();

                HelpfulCommands.RemoveTheNRowAfterTheField(field,2);
               
                switch (keyInput.Key)
                {
                    case ConsoleKey.UpArrow:
                        field = PlayerMove.Up(field,ref player, ref playerRowIndex, ref playerColIndex, animalList, ref isPlayerInWater);
                        break;
                    case ConsoleKey.DownArrow:
                        field = PlayerMove.Down(field,ref player, ref playerRowIndex, ref playerColIndex, animalList, ref isPlayerInWater);
                        break;
                    case ConsoleKey.RightArrow:
                        field = PlayerMove.Right(field,ref player, ref playerRowIndex, ref playerColIndex, animalList, ref isPlayerInWater);
                        break;
                    case ConsoleKey.LeftArrow:
                        field = PlayerMove.Left(field,ref player, ref playerRowIndex, ref playerColIndex, animalList, ref isPlayerInWater);
                        break;
                }
                if (keyInput.Key == ConsoleKey.Enter)
                {
                    HelpfulCommands.RemoveTheNRowAfterTheField(field,1);

                    string commandType = GameOptions.GameMenu(field);
              
                    switch (commandType)
                    {
                        case "1":
                            field = GameOptions.ChangePlayerAnimal(field, ref player, ref playerRowIndex, ref playerColIndex, animalList);
                            break;
                        case "2":
                            field = GameOptions.ResetGame(field, player, n, npcCount, ref playerRowIndex, ref playerColIndex, animalList,  ref isPlayerInWater);
                            break;
                        case "3":
                            GameOptions.EndGame(animalList);
                            return;
                    }
                }

                Console.SetCursorPosition(0, 0);

                Console.WriteLine(FieldCommands.FieldOutput(field));

                Console.WriteLine(InputMessages.EnterACommand);
                HelpfulCommands.RemoveTheNRowAfterTheField(field,1);
                Console.WriteLine(string.Format(InputMessages.PlayerInfo, EmojiList.Heart, player.Hp, EmojiList.Attack, player.Attack, EmojiList.Energy, player.Energy));
            }
        }        
    }
}
    


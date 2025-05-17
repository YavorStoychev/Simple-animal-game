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
            
            List<Animal> terrestrialAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == LandType.Terrestrial);
            List<Animal> aquaticAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == LandType.Aquatic);
            List<Animal> bothAnimals = animalList.AddedAnimalList.FindAll(x => x.LandType == "Both");

            bool isPlayerInWater = false;
            int waterFieldCount = 0;

            ConsoleKeyInfo keyInput = new ConsoleKeyInfo();

            Console.WriteLine(InputMessages.FieldSize);
            int n = int.Parse(Console.ReadLine());
           
            Console.WriteLine(InputMessages.AnimalPick);
            Console.WriteLine(PlayerCommands.AnimalChoice());

            int playerAnimalId = int.Parse(Console.ReadLine());

            while (playerAnimalId > animalList.AddedAnimalList.Count)
            {
                Console.WriteLine(OutputMessages.IncorrectAnimalSelection);
                playerAnimalId = int.Parse(Console.ReadLine());
            }

            int radius = n / 4;

            string[,] waterField = WaterCommands.GenerateWater(radius, ref waterFieldCount);

            Animal player = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == playerAnimalId);

            Console.WriteLine(InputMessages.AnimalCountInput);

            int npcCount = NpcCommands.GenerateNpcCount(n);

            
            int playerRowIndex = 0;
            int playerColIndex = 0;

            string[,] field
                = FieldCommands.GeneratingFieldWithPlayerAndNpcAnimals(player, n, n, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, waterFieldCount);

            Console.Clear();
            Console.WriteLine(FieldCommands.FieldOutput(field));
            Console.WriteLine(InputMessages.EnterACommand);

            while (true)
            {
                keyInput = Console.ReadKey();

                HelpfulCommands.RemoveTheSecondRowAfterTheField(field);
               
                    switch (keyInput.Key)
                    {
                        case ConsoleKey.UpArrow:
                            field = PlayerMove.Up(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, ref isPlayerInWater);
                            break;
                        case ConsoleKey.DownArrow:
                            field = PlayerMove.Down(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, ref isPlayerInWater);
                            break;
                        case ConsoleKey.RightArrow:
                            field = PlayerMove.Right(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, ref isPlayerInWater);
                            break;
                        case ConsoleKey.LeftArrow:
                            field = PlayerMove.Left(field, player, ref playerRowIndex, ref playerColIndex, animalList, terrestrialAnimals, aquaticAnimals, bothAnimals, ref isPlayerInWater);
                            break;
                    }

                if (keyInput.Key == ConsoleKey.Enter)
                {                
                    string commandType = GameOptions.GameMenu(field);
              
                    switch (commandType)
                    {
                        case "1":
                            field = GameOptions.ChangePlayerAnimal(field, ref player, ref playerRowIndex, ref playerColIndex, animalList);
                            break;
                        case "2":
                            field = GameOptions.ResetGame(field, player, n, npcCount, ref playerRowIndex, ref playerColIndex, waterField, animalList, waterFieldCount, ref isPlayerInWater);
                            break;
                        case "3":
                            GameOptions.EndGame(animalList);
                            return;
                    }
                }

                Console.SetCursorPosition(0, 0);

                Console.WriteLine(FieldCommands.FieldOutput(field));

                Console.WriteLine(InputMessages.EnterACommand);
            }
        }

        
    }
}
    


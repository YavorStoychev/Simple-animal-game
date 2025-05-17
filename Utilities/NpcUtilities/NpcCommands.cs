using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.EmojiImages;
using OOPProject.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.NpcUtilities
{
    public class NpcCommands
    {
        public static int GenerateNpcCount(int n)
        {
            int npcCount = int.Parse(Console.ReadLine());

            int maxCountNpcAnimals = ((n - 2) * (n - 2)) - 1;

            while (maxCountNpcAnimals < npcCount)
            {
                Console.WriteLine(string.Format(OutputMessages.TooMuchAnimalInput, maxCountNpcAnimals));
                npcCount = int.Parse(Console.ReadLine());
            }

            return npcCount;
        }

       public static void PutCurrEntityInsideTheField(string[,] field, ref int npcRowIndex, ref int npcColIndex, ref IAnimal animal, ref int circleFieldCount, AnimalList animalList)
        {
            Random random = new Random();

            char border = (char)0x25A0;

            rewritingTheindexes:

            while (field[npcRowIndex, npcColIndex] != "  ")
            {
                if (field[npcRowIndex, npcColIndex] == EmojiList.Water && (animal.LandType == "Aquatic" || animal.LandType == "Both") && field[npcRowIndex, npcColIndex] != border.ToString() + " ")
                {
                    circleFieldCount--;
                    break;
                }

                npcRowIndex = random.Next(1, field.GetLength(0) - 1);
                npcColIndex = random.Next(1, field.GetLength(1) - 1);
            }

            if (field[npcRowIndex, npcColIndex] == "  " && animal.LandType == "Aquatic")
            {
                if (circleFieldCount == 0)
                {
                    while (animal.LandType == "Aquatic")
                    {
                        int newAnimalId = random.Next(1, 13);
                        animal = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == newAnimalId);
                    }
                }

                npcRowIndex = random.Next(1, field.GetLength(0) - 1);
                npcColIndex = random.Next(1, field.GetLength(1) - 1);

                goto rewritingTheindexes;
            }
        }

       public static void GenerateRandomTypeOfEntity(string[,] field, int npcCount, AnimalList animalList, int circleFieldCount)
        {
            Random random = new Random();

            for (int i = 0; i < npcCount; i++)
            {
                int npcRowIndex = random.Next(1, field.GetLength(0));
                int npcColIndex = random.Next(1, field.GetLength(1));

                int randomAnimalId = random.Next(1, 13);

                IAnimal animal = animalList.AddedAnimalList.FirstOrDefault(x => x.Id == randomAnimalId);

                PutCurrEntityInsideTheField(field, ref npcRowIndex, ref npcColIndex, ref animal, ref circleFieldCount, animalList);

                field[npcRowIndex, npcColIndex] = animal.Emoji;
            }
        }

    }
}

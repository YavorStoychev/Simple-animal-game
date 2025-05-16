using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.NpcUtilities;
using OOPProject.Utilities.PlayerUtilities;
using OOPProject.Utilities.WaterUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.FieldUtilities
{
    public class FieldCommands
    {
       public static string[,] FieldGenerator(int rowCount, int columnCount)
        {
            char border = (char)0x25A0;

            bool isLeftBorder = false;

            string[,] field = new string[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                isLeftBorder = false;

                for (int col = 0; col < columnCount; col++)
                {

                    if (row == 0 || row == rowCount - 1)
                    {
                        field[row, col] = border.ToString() + " ";
                    }

                    else if (!isLeftBorder)
                    {
                        field[row, col] = border.ToString() + " ";
                        isLeftBorder = true;
                    }

                    else if (isLeftBorder)
                    {
                        field[row, col] = "  ";
                    }
                }

                field[row, columnCount - 1] = border.ToString() + " ";
            }

            return field;
        }

       public static string FieldOutput(string[,] field)
        {
            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    sb.Append($"{field[row, col]} ");
                }
                sb.AppendLine();
            }


            return sb.ToString().TrimEnd();
        }

       public static string[,] GeneratingFieldWithPlayerAndNpcAnimals(IAnimal player, int rowCount, int columnCount, int npcCount, ref int playerRowIndex, ref int playerColIndex, string[,] circlefield, AnimalList animalList, int circleFieldCount)
        {
            string[,] field = FieldCommands.FieldGenerator(rowCount, columnCount);

            WaterCommands.PlaceWaterInField(field, circlefield);

            PlayerCommands.GenerateRandomPlayerLocationInsideTheField(field, player, ref playerRowIndex, ref playerColIndex, ref circleFieldCount);

            NpcCommands.GenerateRandomTypeOfEntity(field, npcCount, animalList, circleFieldCount);


            return field;
        }

       
    }
}

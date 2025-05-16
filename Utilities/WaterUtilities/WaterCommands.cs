using OOPProject.Utilities.EmojiImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.WaterUtilities
{
    public class WaterCommands
    {     
        public static string[,] GenerateWater(int radius, ref int circleFieldCount)
        {

            int size = radius * 2 + 1;
            string[,] matrix = new string[size, size];

            // Initialize with empty spaces
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = " ";

            double margin = 0.5; // Tweak this to avoid extra side points base : 0.9

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    double value = x * x + y * y;
                    double outerBound = (radius - margin) * (radius - margin); // Stricter boundary

                    if (value <= outerBound) // Fills the entire circle while avoiding outer artifacts
                    {
                        circleFieldCount++;
                        matrix[y + radius, x + radius] = EmojiList.Water;
                    }
                }
            }

            return matrix;
        }

        public static void PlaceWaterInField(string[,] field, string[,] circleMatrix)
        {
            int outerSize = field.GetLength(0);
            int circleSize = circleMatrix.GetLength(0);

            int startY = (outerSize - circleSize) / 2;
            int startX = (outerSize - circleSize) / 2;

            for (int y = 0; y < circleSize; y++)
            {
                for (int x = 0; x < circleSize; x++)
                {
                    if (circleMatrix[y, x] == EmojiList.Water)
                    {
                        field[startY + y, startX + x] = EmojiList.Water;
                    }
                }
            }
        }

        public static bool IsPlayerInWater(string[,] field, int playerRowIndex, int playerColIndex)
        {
            int circleCountAroundThePlayer = 0;


            if (field[playerRowIndex + 1, playerColIndex] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex - 1, playerColIndex] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex, playerColIndex + 1] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }
            if (field[playerRowIndex, playerColIndex - 1] == EmojiList.Water)
            {
                circleCountAroundThePlayer++;
            }

            if (circleCountAroundThePlayer > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

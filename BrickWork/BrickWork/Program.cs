using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickWrok
{
    enum Orientation
    {
        HORIZONTAL,
        VERTICAL
    }

    class Program
    {
        static void Main(string[] args)
        {
            int numberOfRows = 0;
            int numberOfColumn = 0;

            readNumberOfRowsAndColumns(out numberOfRows, out numberOfColumn);

            //initializing list of lists that will contain orientation of bricks
            List<List<Orientation>> inputBrickWall = new List<List<Orientation>>();

            //initializing stack that will contain values of bricks
            ISet<int> values = new HashSet<int>();

            readBricksValuesAndOrientations(inputBrickWall, values, numberOfRows, numberOfColumn);

            int numberOfBricksOutput = 0;
            List<List<Orientation>> outputBrickWall = new List<List<Orientation>>();
            for (int i = 0; i < inputBrickWall.Count; i += 2)
            {
                List<Orientation> outputBrickLine = new List<Orientation>();
                int inputBrickIndex = 0;
                int outputBrickIndex = 0;
                int index = 0;
                do
                {
                    if (inputBrickIndex > outputBrickIndex)
                    {
                        if (numberOfColumn - outputBrickIndex > 1)
                        {
                            outputBrickLine.Add(Orientation.HORIZONTAL);
                            inputBrickIndex += inputBrickWall[i][index] == Orientation.HORIZONTAL ? 2 : 1;
                            outputBrickIndex += 2;
                        }
                        else
                        {
                            outputBrickLine.Add(Orientation.VERTICAL);
                            inputBrickIndex += 2;
                            outputBrickIndex += 1;
                        }
                    }
                    else if (inputBrickIndex == outputBrickIndex)
                    {
                        if (inputBrickWall[i][index] == Orientation.HORIZONTAL)
                        {
                            outputBrickLine.Add(Orientation.VERTICAL);
                            inputBrickIndex += 2;
                            outputBrickIndex += 1;
                        }
                        else
                        {
                            outputBrickLine.Add(Orientation.HORIZONTAL);
                            inputBrickIndex += 1;
                            outputBrickIndex += 2;
                        }
                    }
                    else if (inputBrickIndex < outputBrickIndex)
                    {
                        if (numberOfColumn - outputBrickIndex > 1)
                        {
                            outputBrickLine.Add(Orientation.HORIZONTAL);
                            inputBrickIndex += inputBrickWall[i][index] == Orientation.HORIZONTAL ? 2 : 1;
                            outputBrickIndex += 2;
                        }
                        else
                        {
                            outputBrickLine.Add(Orientation.VERTICAL);
                            inputBrickIndex += inputBrickWall[i][index] == Orientation.HORIZONTAL ? 2 : 1;
                            outputBrickIndex += 1;
                        }
                    }
                    index++;
                } while (outputBrickIndex < numberOfColumn);
                outputBrickWall.Add(outputBrickLine);
                numberOfBricksOutput += outputBrickIndex;
            }

            if (numberOfBricksOutput != values.Count)
            {
                Console.WriteLine("-1 Solution does not exist.");
            }

            PrintOutputBrickWall(values, outputBrickWall);
        }

        private static void readBricksValuesAndOrientations(List<List<Orientation>> inputBrickWall, ISet<int> values, int numberOfRows, int numberOfColumn)
        {
            //reading data: rows of elements and populating data 
            for (int row = 0; row < numberOfRows; row++)
            {
                List<int> lineOfBricks = readNextLineOfIntegers();

                //check: are elements equal to column number
                while (lineOfBricks.Count != numberOfColumn)
                {
                    Console.WriteLine("Number of elements must be equal to column number!");
                    lineOfBricks = readNextLineOfIntegers();
                }

                List<Orientation> lineOfBrickOrientations = new List<Orientation>();
                //populating line of bricks with their orientation and pushing their values into the stack of values
                foreach (IGrouping<int, int> brickValue in lineOfBricks.GroupBy(i => i))
                {
                    if (brickValue.Count() == 1)
                    {
                        lineOfBrickOrientations.Add(Orientation.VERTICAL);
                    }
                    else if (brickValue.Count() == 2)
                    {
                        lineOfBrickOrientations.Add(Orientation.HORIZONTAL);
                    }
                    else
                    {
                        throw new Exception("-1 invalid input");
                    }
                    values.Add(brickValue.Key);
                }
                inputBrickWall.Add(lineOfBrickOrientations);
            }
        }

        private static void readNumberOfRowsAndColumns(out int numberOfRows, out int numberOfColumn)
        {
            //reading data: rows and columns
            do
            {
                //transform data into array of integers
                int[] input = readNextLineOfIntegers().ToArray();
                numberOfRows = input[0];
                numberOfColumn = input[1];

                //validaton: checks if rows or columns are even
                if (numberOfRows % 2 != 0)
                {
                    Console.WriteLine($"N({numberOfRows}) must be even!");
                }
                else if (numberOfColumn % 2 != 0)
                {
                    Console.WriteLine($"M({numberOfColumn}) must be even!");
                }

                //validaton: checks if rows or columns are higher than 100 and less than 2
                if (numberOfRows > 100 || numberOfColumn > 100 || numberOfRows < 2 || numberOfColumn < 2)
                {
                    Console.WriteLine($"Number of rows/columns should be between to 2 and 100!");
                }

            } while (numberOfRows % 2 != 0 || numberOfColumn % 2 != 0 || numberOfRows > 100 || numberOfColumn > 100);
        }
        private static List<int> readNextLineOfIntegers()
        {
            return Console.ReadLine().Split(' ').Select(int.Parse).ToList();
        }

        private static void PrintOutputBrickWall(ISet<int> values, List<List<Orientation>> outputBrickWall)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbNewLine = new StringBuilder();
            Stack<int> uniqueValues = new Stack<int>(values);
            foreach (List<Orientation> outputOrientationLine in outputBrickWall)
            {
                sb.Clear();
                sbNewLine.Clear();
                foreach (Orientation outputOrientation in outputOrientationLine)
                {
                    if (outputOrientation == Orientation.HORIZONTAL)
                    {
                        Console.Write($"|{uniqueValues.Peek()} {uniqueValues.Pop()}| ");
                        sb.Append($"|{uniqueValues.Peek()} {uniqueValues.Pop()}| ");
                        sbNewLine.Append(" - -  ");
                    }
                    else
                    {
                        Console.Write($"{uniqueValues.Peek()} ");
                        sb.Append($"{uniqueValues.Pop()} ");
                        sbNewLine.Append("  ");
                    }
                }
                Console.WriteLine();
                Console.WriteLine(sbNewLine.ToString());
                Console.WriteLine(sb.ToString());
            }
        }
    }
}

using System;
using System.Threading;

namespace Life
{
    /// <summary>
    /// Contains methods used to run Conway's Game of Life
    /// </summary>
    /// <author>
    /// Ching Yin Lau (Ricky) : N10330895
    /// </author>
    public static class Conway
    {
        
        /// <summary>
        /// A random number generator used during creation
        /// </summary>
        private static Random rng = new Random();

        /// <summary>
        /// Runs the game of life according to its rules and continuously refreshes the result
        /// </summary>
        public static void Main()
        {
            // Control the size of the grid
            const int numRow = 20;
            const int numCol = 50;
            const int WaitingTime = 100;

            // Welcome message
            Console.WriteLine("Welcome Conway's Game of Life!");
            Console.WriteLine("Press ENTER to start the simulation.");
            Console.ReadLine();

            // Create the initial grid
            bool[,] grid = MakeGrid(numRow, numCol);

            // Loop forever to see the population
            while (true)
            {
                // Clear the console
                Console.Clear();

                // Writes the given game grid to standard output
                DrawGrid(grid);

                // Update the grid 
                grid = UpdateGrid(grid);

                // Wait a specific time
                System.Threading.Thread.Sleep(WaitingTime);
            }

        }

        /// <summary>
        /// Returns a new grid for Conway's Game of life using the given dimensions.
        /// Each cell has a 50% chance of initially being alive.
        /// </summary>
        /// <param name="rows">The desired number of rows</param>
        /// <param name="cols">The desired number of columns</param>
        /// <returns></returns>
        public static bool[,] MakeGrid(int rows, int cols)
        {
            // Create a grid with desired number of rows and columns
            bool[,] grid = new bool[rows, cols];

            // Loop over every row
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // Loop over every column
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    // Generate a random boolean value into each cell
                    grid[row, col] = rng.Next(0, 2) > 0;
                }
            }
            return grid;
        }

        /// <summary>
        /// Writes the given game grid to standard output
        /// </summary>
        /// <param name="grid">The grid to draw to standard output</param>
        public static void DrawGrid(bool[,] grid)
        {

            // Loop over every row
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // Loop over every column
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == true)
                    {
                        // Write '#' if the value of the cell is true
                        Console.Write("#");
                    }
                    else
                    {
                        // Write '.' if the value of the cell is false
                        Console.Write(".");
                    }
                }

                // Line break after every row is printed
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Returns the number of living neighbours adjacent to a given cell position
        /// </summary>
        /// <param name="grid">The game grid</param>
        /// <param name="row">The cell's row</param>
        /// <param name="col">The cell's column</param>
        /// <returns>The number of adjacent living neighbours</returns>
        public static int CountNeighbours(bool[,] grid, int row, int col)
        {
            // Store the number of neighbours
            int NeighboursCounter = 0;

            // Loop over from the row above to the row below of the cell which is being checked 
            for (int i = row - 1; i <= row + 1; i++)
            {
                // Loop over from the column on the left to
                // the row on the right of the cell which is being checked 
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col)
                    {
                        // if the cell being checked is the cell which is counting neighbours
                        continue;
                    }
                    else if (i < 0 || i == grid.GetLength(0))
                    {
                        // if the row being checked is out of the grid
                        continue;
                    }
                    else if (j < 0 || j == grid.GetLength(1))
                    {
                        // if the column being checked is our of the grid
                        continue;
                    }
                    else if (grid[i, j] == true)
                    {
                        // If the neighbour cell is alive
                        NeighboursCounter++;
                    }
                }

            }
            return NeighboursCounter;
        }

        /// <summary>
        /// Returns an updated grid after progressing the rules of the Game of Life.
        /// </summary>
        /// <param name="oldgrid">The original grid from which the new grid is derived</param>
        /// <returns>A new grid which has been updated by one time-step</returns>
        public static bool[,] UpdateGrid(bool[,] oldGrid)
        {
            // Create a new grid which has the same length of the old grid
            bool[,] newGrid = new bool[oldGrid.GetLength(0), oldGrid.GetLength(1)];

            // Loop over every row of the old grid
            for (int row = 0; row < oldGrid.GetLength(0); row++)
            {
                // Loop over every column of the old grid
                for (int col = 0; col < oldGrid.GetLength(1); col++)
                {
                    // Count the neighbour of the cell that is being checked
                    int NeighboursCounter = CountNeighbours(oldGrid, row, col);

                    if (oldGrid[row, col] == true && NeighboursCounter < 2)
                    {
                        // Underpopulation
                        newGrid[row, col] = false;
                    }
                    else if (oldGrid[row, col] == true && (NeighboursCounter == 2 || NeighboursCounter == 3))
                    {
                        // Cell that lives on
                        newGrid[row, col] = true;
                    }
                    else if (oldGrid[row, col] == true && NeighboursCounter > 3)
                    {
                        // Overpopulation
                        newGrid[row, col] = false;
                    }
                    else if (oldGrid[row, col] == false && NeighboursCounter == 3)
                    {
                        // Reproduction
                        newGrid[row, col] = true;
                    }
                    else
                    {
                        // Anything else is copied across
                        newGrid[row, col] = oldGrid[row, col];
                    }
                }
            }
            // return the new grid after updated every cell
            return newGrid;

        }
    }
}
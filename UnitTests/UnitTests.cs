using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Life;
using System.Diagnostics;
using System.IO;

namespace UnitTests
{
    public static class Utility
    {
        public static int NumAlive(bool[,] grid)
        {
            int numAlive = 0;
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col])
                    {
                        numAlive++;
                    }
                }
            }
            return numAlive;
        }
    }

    [TestClass]
    public class A_Test_MakeGrid
    {
        [TestMethod]
        public void MakeGrid_RunsWithoutErrors()
        {
            bool[,] grid = Conway.MakeGrid(10, 10);
            grid = Conway.MakeGrid(10, 20);
            grid = Conway.MakeGrid(20, 20);
        }

        [TestMethod]
        public void MakeGrid_DimensionsAreCorrect()
        {
            int[] rowAmounts = { 5, 10, 15, 30, 50, 100 };
            int[] colAmounts = { 5, 10, 15, 30, 50, 100 };
            bool[,] grid;

            foreach (int rowAmount in rowAmounts)
            {
                foreach (int colAmount in colAmounts)
                {
                    grid = Conway.MakeGrid(rowAmount, colAmount);
                    Assert.AreEqual(
                        expected: rowAmount,
                        actual: grid.GetLength(0),
                        message: string.Format(
                            format: "The generated grid has {0} rows instead of {1}!",
                            arg0: grid.GetLength(0),
                            arg1: rowAmount
                        )
                    );

                    Assert.AreEqual(
                        expected: colAmount,
                        actual: grid.GetLength(1),
                        message: string.Format(
                            format: "The generated grid has {0} cols instead of {1}!",
                            arg0: grid.GetLength(1),
                            arg1: colAmount
                        )
                    );
                }
            }
        }

        [TestMethod]
        public void MakeGrid_GridIsAboutHalfLiving()
        {
            const double ERROR_MARGIN = 0.1;
            const double FILL_PERCENT = 0.5;
            int rows = 100;
            int cols = 100;
            int numCells = rows * cols;

            bool[,] grid = Conway.MakeGrid(rows, cols);

            int numAlive = Utility.NumAlive(grid);

            Assert.IsTrue(Math.Abs(numCells * FILL_PERCENT - numAlive) < numCells * ERROR_MARGIN);
        }

        [TestMethod]
        public void MakeGrid_GridIsAlwaysDifferent()
        {
            int rows = 100;
            int cols = 100;

            bool[,] grid1 = Conway.MakeGrid(rows, cols);
            bool[,] grid2 = Conway.MakeGrid(rows, cols);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (grid1[row, col] != grid2[row, col])
                    {
                        return;
                    }
                }
            }

            Assert.Fail(
                "Two different 100x100 grids were generated, " +
                "but they were both exactly the same! Is your algorithm truly random?"
            );
        }
    }

    [TestClass]
    public class B_Test_DrawGrid
    {
        [TestMethod]
        public void DrawGrid_RunsWithoutErrors()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { true, true, false, false },
                { false, false, true, false },
                { false, true, false, true },
                { true, false, false, false }
            };

            Conway.DrawGrid(grid);
        }

        [TestMethod]
        public void DrawGrid_Test1()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { true, true, false, false },
                { false, false, true, false },
                { false, true, false, true },
                { true, false, false, false }
            };

            // get the standard output stream
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());

            // create a new output stream and redirect the console to it
            StringWriter newOut = new StringWriter();
            Console.SetOut(newOut);

            // draw the grid
            Conway.DrawGrid(grid);

            // return to normal standard output
            Console.SetOut(standardOut);
            
            // NOTE: the \r in the \r\n is automatically added during console output
            //       you do NOT need to add in your DrawGrid method
            Assert.AreEqual(
                expected: "##..\r\n..#.\r\n.#.#\r\n#...\r\n",
                actual: newOut.ToString()
            );
        }

        [TestMethod]
        public void DrawGrid_Test2()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { false, false, true,  false },
                { false, true,  false, false },
                { true,  false, true,  true },
                { false, false, false, true }
            };

            // get the standard output stream
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());

            // create a new output stream and redirect the console to it
            StringWriter newOut = new StringWriter();
            Console.SetOut(newOut);

            // draw the grid
            Conway.DrawGrid(grid);

            // return to normal standard output
            Console.SetOut(standardOut);

            // NOTE: the \r in the \r\n is automatically added during console output
            //       you do NOT need to add in your DrawGrid method
            Assert.AreEqual(
                expected: "..#.\r\n.#..\r\n#.##\r\n...#\r\n",
                actual: newOut.ToString()
            );
        }
    }

    [TestClass]
    public class C_Test_CountNeighbours
    {
        [TestMethod]
        public void CountNeighbours_InternalSpace_RunsWithoutErrors()
        {
            bool[,] grid = {
                { false, false, true, false },
                { true, true, false, false },
                { true, false, false, true },
                { false, false, true, true },
            };

            Conway.CountNeighbours(grid, 2, 1);
            Conway.CountNeighbours(grid, 1, 1);
        }

        [TestMethod]
        public void CountNeighbours_InternalSpace_ZeroNeighbours_CellIsDead()
        {
            bool[,] grid = {
                { true, true,  true,  true,  true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true,  true,  true,  true },
            };

            Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }

        [TestMethod]
        public void CountNeighbours_InternalSpace_ZeroNeighbours_CellIsAlive()
        {
            bool[,] grid = {
                { true, true,  true,  true,  true },
                { true, false, false, false, true },
                { true, false, true,  false, true },
                { true, false, false, false, true },
                { true, true,  true,  true,  true },
            };

            Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }

        [TestMethod]
        public void CountNeighbours_InternalSpace_AdjacentNeighbours()
        {
            bool[,] grid;
            
            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, true,  false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }

        [TestMethod]
        public void CountNeighbours_InternalSpace_DiagonalNeighbours()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true, false },
                { false, false, false, false, false },
                { false, true,  false, true, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }

        [TestMethod]
        public void CountNeighbours_InternalSpace_AllNeighbours()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  true,  true,  false },
                { false, true,  true,  true,  false },
                { false, true,  true,  true,  false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 8,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }

        [TestMethod]
        public void CountNeighbours_BoundaryAndCornerSpaces_RunsWithoutErrors()
        {
            bool[,] grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // first column
                Conway.CountNeighbours(grid, row, 0);

                // middle columns
                if (row != 0 && row != grid.GetLength(0) - 1) {
                    for (int col = 1; col < grid.GetLength(1) - 1; col++)
                    {
                        Conway.CountNeighbours(grid, row, col);
                    }
                }
                
                // last column
                Conway.CountNeighbours(grid, row, grid.GetLength(1) - 1);
            }
        }
        
        [TestMethod]
        public void CountNeighbours_CornerSpaces_Tests()
        {
            bool[,] grid;
            
            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 0, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, true  },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 0, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false,  true },
                { false, false, false, true,  false }
            };

            Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 4, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { false, true,  false, false, false }
            };

            Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 4, 0)
            );

            grid = new bool[,] {
                { true, true, false, false, false },
                { true, true, false, false, false },
                { false, false, false, false, false },
                { false,  false,  false, false, false },
                { false,  false,  false, false, false }
            };

            Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 0, 0)
            );
        }

        [TestMethod]
        public void CountNeighbours_EdgeSpaces_Tests()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 0, 1)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, true  },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, true,  true, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 4, 2)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 5,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { true,  true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            Assert.AreEqual(
                expected: 5,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );
        }
    }

    [TestClass]
    public class D_Test_UpdateGrid
    {
        [TestMethod]
        public void UpdateGrid_RunsWithoutErrors()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Conway.UpdateGrid(grid);

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            Conway.UpdateGrid(grid);
        }

        [TestMethod]
        public void UpdateGrid_Example1_Blinker1()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, true,  true,  true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example2_Blinker2()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, true,  true,  true,  false },
                { false, true,  true,  true,  false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, true,  false, false },
                { false, true,  false, false, true,  false },
                { false, true,  false, false, true,  false },
                { false, false, true,  false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example3_Blinker3()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, true,  true,  false },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { false, true,  false, false, false, false },
                { false, false, false, false, true,  false },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example4_StillLife1()
        {
            bool[,] original = new bool[,] {
                { true,  true, false, false, false, false },
                { true,  true, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { true,  true,  false, false, false, false },
                { true,  true,  false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example5_StillLife2()
        {
            bool[,] original = new bool[,] {
                { false, false, false, true,  true,  false },
                { false, false, true,  false, false, true },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, true,  true,  false },
                { false, false, true,  false, false, true },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example6_StillLife3()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { true,  false, false, true, false, false },
                { false, true,  false, true, false, false },
                { false, false, true,  false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { true,  false, false, true, false, false },
                { false, true,  false, true, false, false },
                { false, false, true,  false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGrid_Example7_Glider()
        {
            //..#
            //#.#
            //.##
            bool[,] original = new bool[,] {
                { false, false, true,  false, false, false },
                { true,  false, true,  false, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            //#..
            //.##
            //##.
            bool[,] expected1 = new bool[,] {
                { false, true,  false, false, false, false },
                { false, false, true,  true, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            //.#.
            //..#
            //###
            bool[,] expected2 = new bool[,] {
                { false, false, true, false, false, false },
                { false, false, false, true, false, false },
                { false, true,  true,  true, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            CollectionAssert.AreEqual(expected1, Conway.UpdateGrid(original));
            CollectionAssert.AreEqual(expected2, Conway.UpdateGrid(expected1));
        }
    }
}

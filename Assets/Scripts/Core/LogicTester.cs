using Sudoku.Core;
using System;
using UnityEngine;

public class LogicTester : MonoBehaviour
{
    void Start()
    {
        //TestSolverOnEmptyBoard();
        //TestSolverOnKnownPuzzle();
        //TestSolverOnUnsolvableBoard();
        TestCountSolutions();
    }

    private void TestSolverOnEmptyBoard()
    {
        Debug.Log("=== Test 1: Empty board ===");
        var board = new SudokuBoard();
        var solver = new SudokuSolver(board);

        bool solved = solver.Solve();
        Debug.Log($"Solved: {solved}");
        PrintBoard(board);
    }

    private void TestSolverOnKnownPuzzle()
    {
        Debug.Log("=== Test 2: Known puzzle ===");
        int[,] puzzle =
        {
            { 5, 3, 0,  0, 7, 0,  0, 0, 0 },
            { 6, 0, 0,  1, 9, 5,  0, 0, 0 },
            { 0, 9, 8,  0, 0, 0,  0, 6, 0 },

            { 8, 0, 0,  0, 6, 0,  0, 0, 3 },
            { 4, 0, 0,  8, 0, 3,  0, 0, 1 },
            { 7, 0, 0,  0, 2, 0,  0, 0, 6 },

            { 0, 6, 0,  0, 0, 0,  2, 8, 0 },
            { 0, 0, 0,  4, 1, 9,  0, 0, 5 },
            { 0, 0, 0,  0, 8, 0,  0, 7, 9 },
        };

        var board = new SudokuBoard();
        board.LoadFrom(puzzle);
        var solver = new SudokuSolver(board);

        bool solved = solver.Solve();
        Debug.Log($"Solved: {solved}");
        PrintBoard(board);
    }

    private void TestSolverOnUnsolvableBoard()
    {
        Debug.Log("=== Test 3: Unsolvable board ===");
        int[,] bad =
        {
        { 5, 5, 0, 0, 0, 0, 0, 0, 0 },  // two 5s in a row — invalid
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

        var board = new SudokuBoard();
        board.LoadFrom(bad);

        Debug.Log($"IsConsistent: {board.IsConsistent()} (expected: False)");

        var solver = new SudokuSolver(board);
        bool solved = solver.Solve();
        Debug.Log($"Solved: {solved} (expected: False)");
    }

    private void PrintBoard(SudokuBoard board)
    {
        var sb = new System.Text.StringBuilder();
        for (int row = 0; row < SudokuBoard.Size; row++)
        {
            if (row % 3 == 0 && row != 0)
                sb.AppendLine("------+-------+------");

            for (int col = 0; col < SudokuBoard.Size; col++)
            {
                if (col % 3 == 0 && col != 0)
                    sb.Append("| ");

                int val = board.GetCell(row, col);
                sb.Append(val == 0 ? ". " : val + " ");
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    private void TestCountSolutions()
    {
        Debug.Log("=== Test 4: CountSolutions ===");

        // Случай 1: пустая доска — решений МНОГО
        var emptyBoard = new SudokuBoard();
        var solver1 = new SudokuSolver(emptyBoard);
        int count1 = solver1.CountSolutions(2);
        Debug.Log($"Empty board: {count1} (expected: 2)");

        // Случай 2: реальная головоломка — решение ЕДИНСТВЕННОЕ
        int[,] puzzle =
        {
        { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
        { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
        { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
        { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
        { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
        { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
        { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
        { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
        { 0, 0, 0, 0, 8, 0, 0, 7, 9 },
    };
        var puzzleBoard = new SudokuBoard();
        puzzleBoard.LoadFrom(puzzle);
        var solver2 = new SudokuSolver(puzzleBoard);
        int count2 = solver2.CountSolutions(2);
        Debug.Log($"Known puzzle: {count2} (expected: 1)");

        // Случай 3: невалидная доска — решений НОЛЬ
        var badBoard = new SudokuBoard();
        badBoard.SetCell(0, 0, 5);
        badBoard.SetCell(0, 1, 5);  // конфликт
        var solver3 = new SudokuSolver(badBoard);
        int count3 = solver3.CountSolutions(2);
        Debug.Log($"Invalid board: {count3} (expected: 0)");
    }
}

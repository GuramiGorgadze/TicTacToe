using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Program
    {

        static char[] board = new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static int[,] winningCombinations = new int[8, 3]
           {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 },
                { 0, 4, 8 },
                { 2, 4, 6 },
                { 0, 3, 6 },
                { 1, 4, 7 },
                { 2, 5, 8 }
            };

        static bool circleTurn;
        static bool mode;
        static char currentPlayer = circleTurn ? 'o' : 'x';
        static int player1Wins = 0;
        static int player2Wins = 0;
        static int playerWins = 0;
        static int botWins = 0;
        static string toCopyStr;
        static bool @continue = true;
        static void Main(string[] args)
        {
            Console.Title = "Tic Tac Toe Game";

            Console.SetWindowSize(40, 20);
            Console.ForegroundColor = ConsoleColor.Green;


            while (@continue)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    board[i] = (char)('1' + i);
                }
                circleTurn = false;
                currentPlayer = 'x';
                Console.WriteLine("1. Play vs Computer");
                Console.WriteLine("2. Player vs Player");
                Console.WriteLine("3. Save and exit");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        PlayAgainstComputer();
                        break;
                    case 2:
                        PlayAgainstAPlayer();
                        break;
                    case 3:
                        SaveResult(toCopyStr);
                        @continue = false;
                        break;
                }
            }
        }

        #region Computer
        static void ComputerMove()
        {
           
            
            Random rand = new Random();
            int move;

            if (board[4] != 'x' && board[4] != 'o')
            {
                board[4] = 'o';
                return;
            }
            for (int i = 0; i < 8; i++)
            {
                // მოგების ფუნქცია
                #region winfunc
                if (board[winningCombinations[i, 0]] == 'o' && board[winningCombinations[i, 1]] == 'o' && board[winningCombinations[i, 2]] != 'x')
                {
                    board[winningCombinations[i, 2]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 1]] == 'o' && board[winningCombinations[i, 2]] == 'o' && board[winningCombinations[i, 0]] != 'x')
                {
                    board[winningCombinations[i, 0]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 0]] == 'o' && board[winningCombinations[i, 2]] == 'o' && board[winningCombinations[i, 1]] != 'x')
                {
                    board[winningCombinations[i, 1]] = 'o';
                    return;
                }


                #endregion 
            }
            for (int i = 0; i < 8; i++)
            {
                // დაცვის ფუნქცია
                #region defense
                if (board[winningCombinations[i, 0]] == 'x' && board[winningCombinations[i, 1]] == 'x' && board[winningCombinations[i, 2]] != 'o')
                {
                    board[winningCombinations[i, 2]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 1]] == 'x' && board[winningCombinations[i, 2]] == 'x' && board[winningCombinations[i, 0]] != 'o')
                {
                    board[winningCombinations[i, 0]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 0]] == 'x' && board[winningCombinations[i, 2]] == 'x' && board[winningCombinations[i, 1]] != 'o')
                {
                    board[winningCombinations[i, 1]] = 'o';
                    return;
                }
                #endregion
            }
            for (int i = 0; i < 8; i++)
            {
                // მომზადების ფუნქცია
                #region buildup
                if (board[winningCombinations[i, 0]] == 'o' && board[winningCombinations[i, 1]] != 'x' && board[winningCombinations[i, 2]] != 'x')
                {
                    board[winningCombinations[i, 1]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 1]] == 'o' && board[winningCombinations[i, 0]] != 'x' && board[winningCombinations[i, 2]] != 'x')
                {
                    board[winningCombinations[i, 0]] = 'o';
                    return;
                }
                else if (board[winningCombinations[i, 2]] == 'o' && board[winningCombinations[i, 0]] != 'x' && board[winningCombinations[i, 1]] != 'x')
                {
                    board[winningCombinations[i, 0]] = 'o';
                    return;
                }


                #endregion 
            }
            
            do
            {
                move = rand.Next(1, 10);
            } while (board[move - 1] == 'x' || board[move - 1] == 'o');


            board[move - 1] = 'o';

        }
        static void PlayAgainstComputer()
        {
            mode = true;
            while (!CheckForWin('x') && !CheckForWin('o') && !CheckForDraw())
            {

                if (circleTurn)
                {
                    ComputerMove();

                }
                else
                {
                    PlayerMove();

                }
                SwapTurns();

            }

            PrintResults();
        }
        #endregion

        #region Player
        static void PlayerMove()
        {

            DisplayBoard();
          //  Console.WriteLine($"Player {currentPlayer}, make your move: \n{board[0]} {board[1]} {board[2]} {board[3]} {board[4]} {board[5]} {board[6]} {board[7]} {board[8]}");
            MakeMove();
        }
        static void PlayAgainstAPlayer()
        {

            while (!CheckForWin('x') && !CheckForWin('o') && !CheckForDraw())
            {

                PlayerMove();
                SwapTurns();
            }
            mode = false;
            DisplayBoard();
            PrintResults();
            ResetBoard();
        }
        #endregion
        static void MakeMove()
        {

            int playerMove;
            playerMove = Convert.ToInt32(Console.ReadLine());
            if (playerMove < 1 || playerMove > 9 || board[playerMove - 1] == 'x' || board[playerMove - 1] == 'o')
            {
                Console.WriteLine("The move you tried to play is either invalid or has already been played. Try again");
            }
            else
            {
                board[playerMove - 1] = currentPlayer;

            }

        }
        static void DisplayBoard()
        {
            Console.WriteLine();
            Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
            Console.WriteLine("--+---+--");
            Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
            Console.WriteLine("--+---+--");
            Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
            Console.WriteLine();
        }
        static void SwapTurns()
        {

            circleTurn = !circleTurn;
            currentPlayer = circleTurn ? 'o' : 'x';
        }

        static void ResetBoard()
        {
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = (char)('1' + i);
            }
            circleTurn = false;
        }
        static void PrintResults()
        {
            if (CheckForWin('x') || CheckForWin('o'))
            {
                Console.WriteLine($"Player {(circleTurn ? 'x' : 'o')} wins!");

                if (mode) 
                {
                    if (circleTurn)
                    {
                        
                        playerWins++;
                    }
                    else
                    {
                        botWins++;
                    }
                    Console.WriteLine($"Player wins: {playerWins}");
                    Console.WriteLine($"Bot wins: {botWins}");
                }
                else
                {
                    if (circleTurn)
                    {
                        player1Wins++;
                    }
                    else
                    {
                        player2Wins++;
                        
                    }
                    Console.WriteLine($"Player 1 wins: {player1Wins}");
                    Console.WriteLine($"Player 2 wins: {player2Wins}");
                }
            }
            else if (CheckForDraw())
            {
                Console.WriteLine("It's a draw!");
            }
        }


        static void SaveResult(string toCopy)
        {
            string path = "C:\\Users\\ggorg\\Downloads\\TicTacToe\\TicTacToe\\NewFile";

            if (mode)
            {
                toCopyStr = $"Player Wins: {playerWins} Computer Wins: {botWins}";
            }
            else
            {
                toCopyStr = $"Player 1 Wins: {player1Wins} Player 2 Wins: {player2Wins}";
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(toCopyStr);
                fs.Write(buffer, 0, buffer.Length);
            }
        }




        #region win-draw
        static bool CheckForWin(char player)
        {
            for (int i = 0; i < 8; i++)
            {
                if (board[winningCombinations[i, 0]] == player &&
                    board[winningCombinations[i, 1]] == player &&
                    board[winningCombinations[i, 2]] == player)
                {
                    return true;
                }
            }
            return false;
        }
        static bool CheckForDraw()
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != 'x' && board[i] != 'o')
                {
                    return false;
                }
            }
            return true;
        }
        #endregion       
    }
}

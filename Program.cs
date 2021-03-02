using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new PlayerAI("x");
            Console.WriteLine("Player 1 pawn : '" + player1.Pawn + "' ");
            var player2 = new PlayerAI("o");
            Console.WriteLine("Player AI pawn : '" + player2.Pawn + "' ");

            var playBoard = new Board(player1, player2);
            Console.WriteLine("Iterations number");
            var epochs = int.Parse(Console.ReadLine());
            var count = 0;

            while (count <= epochs)
            {
                playBoard.InitBoard();

                while (!playBoard.IsFinished())
                {
                    playBoard.AddPlayer1Play(isRandom: true);
                    player1.CalculateCurrentStateQuality(playBoard, player2.Pawn, 0.5, 0.9);
                    player2.CalculateCurrentStateQuality(playBoard, player2.Pawn, 0.5, 0.9);

                    if (!playBoard.IsFinished()) 
                    {
                        playBoard.AddPlayer2Play(isRandom: true);
                        player1.CalculateCurrentStateQuality(playBoard, player1.Pawn, 0.5, 0.9);
                        player2.CalculateCurrentStateQuality(playBoard, player1.Pawn, 0.5, 0.9);
                    }
                }

                count++;
            }

            Console.WriteLine("\n--------- Training end ---------");

            while (true)
            {
                playBoard.InitBoard();

                Console.WriteLine("--------- Current Board ---------");
                Console.Write(playBoard);

                while (!playBoard.IsFinished())
                {
                    int play;
                    string playerPlay = " ";
                    while (!int.TryParse(playerPlay, out play) || !string.IsNullOrEmpty(playBoard.BoardValue[play]))
                    {
                        playerPlay = Console.ReadLine();
                        Console.WriteLine("Player 1, your play (0 - 8)");
                        if (playerPlay == "q1")
                        {
                            Console.WriteLine(player1.ToQualityString());
                            playerPlay = " ";
                        }
                        if (playerPlay == "q2")
                        {
                            Console.WriteLine(player2.ToQualityString());
                            playerPlay = " ";
                        }
                    }

                    playBoard.Add(player1.Pawn, play);
                    player2.CalculateCurrentStateQuality(playBoard, player2.Pawn, 0.5, 0.9);
                    Console.WriteLine("\n--------- Player 1 Play ---------");
                    Console.Write(playBoard);

                    if (playBoard.TryGetWinner(out var _)) 
                    {
                        Console.WriteLine("\n Player One win");
                        break;
                    }

                    if (playBoard.IsComplete()) 
                    {
                        Console.WriteLine("\n No winner");
                        break;
                    }

                    playBoard.AddPlayer2Play(isRandom: false);
                    player2.CalculateCurrentStateQuality(playBoard, player1.Pawn, 0.5, 0.9);
                    Console.WriteLine("\n--------- AI Play ---------");
                    Console.Write(playBoard);

                    if (playBoard.TryGetWinner(out var _)) 
                    {
                        Console.WriteLine("\n Player Two win");
                        break;
                    }

                    if (playBoard.IsComplete()) 
                    {
                        Console.WriteLine("\n No winner");
                        break;
                    }
                }
            }
        }
    }
}

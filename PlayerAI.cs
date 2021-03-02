using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeAI
{
    public class PlayerAI
    {
        public string Pawn { get; }

        public Dictionary<string, double> QualityBoard { get; } = new Dictionary<string, double>();

        public PlayerAI(string pawn)
        {
            Pawn = pawn;
        }

        public void CalculateCurrentStateQuality(Board board, string nextPawn, double lrnRate, double gamma)
        {
            var possibilities = board.GetPossibleCase().ToList();
            double nextQuality = 0;
            for (int j = 0; j < possibilities.Count(); j++)
            {
                var key = board.ToNextHash(possibilities[j], nextPawn);

                double q = QualityBoard.TryGetValue(key, out var value) ? value : 0;
                if (q > nextQuality)
                {
                    nextQuality = q;
                }
            }

            RegisterQuality(board, lrnRate, gamma, nextQuality);
        }

        public void RegisterQuality(Board board, double lrnRate, double gamma, double nextQuality)
        {
            var currentQuality = QualityBoard.ContainsKey(board.ToHash()) ? QualityBoard[board.ToHash()] : 0;
            var quality = ((1 - lrnRate) * currentQuality) + (lrnRate * (GetReward(board) + (gamma * nextQuality)));
            if (QualityBoard.ContainsKey(board.ToHash()))
            {
                QualityBoard[board.ToHash()] = quality;
            }
            else
            {
                QualityBoard.Add(board.ToHash(), quality);
            }
        }

        public double GetReward(Board board)
        {
            var multiplicator = (board.GetNumberOfTurn() / 2);
            if (board.TryGetWinner(out var winner))
                return (winner.Pawn == Pawn ? 10 : -10) / multiplicator;

            //if (board.IsComplete())
            //    return 0.1;

            return 0;
        }

        public string ToQualityString()
        {
            var result = "";

            foreach (var element in QualityBoard)
            {
                result = result + element.Key + " : " + element.Value + "\n";
            }

            return result;
        }
    }
}

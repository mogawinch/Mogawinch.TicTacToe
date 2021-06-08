using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeAI
{
    public class Board
    {
        public readonly static Random rnd = new Random();

        public int ColumnRow = 3;

        public string[] BoardValue { get; }

        public Player Player1 { get; }

        public Player Player2 { get; }
        
        public Board(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            BoardValue = new string[ColumnRow * ColumnRow];
            InitBoard();
        }

        public void InitBoard()
        {
            for (int i = 0; i < ColumnRow * ColumnRow; i++)
            {
                BoardValue[i] = string.Empty;
            }
        }

        public void Add(string pawn, int location)
        {
            BoardValue[location] = pawn;
        }

        public void AddPlayer1Play(bool isRandom)
        {
            Add(Player1.Pawn, isRandom ? GetRandomNextCase() : GetLogicalNextCaseForPlayer(Player1));
        }

        public void AddPlayer2Play(bool isRandom)
        {
            Add(Player2.Pawn, isRandom ? GetRandomNextCase() : GetLogicalNextCaseForPlayer(Player2));
        }

        public IEnumerable<int> GetPossibleCase()
        {
            for (int i = 0; i < ColumnRow * ColumnRow; i++)
            {
                if (string.IsNullOrEmpty(BoardValue[i]))
                    yield return i;
            }
        }

        public bool IsComplete()
        {
            var isEmpty = false;
            for(var i = 0; i < ColumnRow * ColumnRow; i++)
            {
                isEmpty = isEmpty || string.IsNullOrEmpty(BoardValue[i]);
            }
            return !isEmpty;
        }

        public bool IsFinished()
        {
            return TryGetWinner(out var _) || IsComplete();
        }

        public int GetNumberOfTurn()
        {
            var count = 0;
            for (var i = 0; i < ColumnRow * ColumnRow; i++)
            {
                if (!string.IsNullOrEmpty(BoardValue[i]))
                    count++;
            }
            return count;
        }

        public bool TryGetWinner(out Player winner)
        {
            if (HasPlayerPawnWin(Player1.Pawn))
            {
                winner = Player1;
                return true;
            }

            if (HasPlayerPawnWin(Player2.Pawn))
            {
                winner = Player2;
                return true;
            }

            winner = null;
            return false;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < ColumnRow; i++)
            {
                for (var j = 0; j < ColumnRow; j++)
                {
                    var index = j + (i * 3);
                    var value = string.IsNullOrEmpty(BoardValue[index]) ? " " : BoardValue[index];
                    result = result + "[" + value + "]";
                }
                result = result + "\n";
            }

            return result;
        }

        public string ToHash()
        {
            var result = "";
            for(var i = 0; i < ColumnRow * ColumnRow; i++)
            {
                if (!string.IsNullOrEmpty(BoardValue[i]))
                {
                    result = result + $"{i}" + BoardValue[i];
                }
            }

            if(string.IsNullOrEmpty(result))
            {
                return "EmptyBoard";
            }

            return result;
        }

        public string ToNextHash(int location, string value)
        {
            var result = "";
            for (var i = 0; i < ColumnRow * ColumnRow; i++)
            {
                if (!string.IsNullOrEmpty(BoardValue[i]))
                {
                    result = result + $"{i}" + BoardValue[i];
                }
                else if(string.IsNullOrEmpty(BoardValue[i]) && i == location)
                {
                    result = result + $"{i}" + value;
                }
            }

            return result;
        }

        private bool HasPlayerPawnWin(string currentPawn)
        {
            //horizontal
            for (var k = 0; k < ColumnRow; k++)
            {
                var consecutive = true;
                for (var i = (k*3); i < (k+1)*3; i++)
                {
                    consecutive = consecutive && (BoardValue[i] == currentPawn);
                }
                if (consecutive)
                    return true;
            }

            //vertical
            for (var k = 0; k < ColumnRow; k++)
            {
                var consecutive = true;
                for (var i = k; i < ((ColumnRow * (ColumnRow -1)) + k + 1); i = i+ ColumnRow)
                {
                    consecutive = consecutive && (BoardValue[i] == currentPawn);
                }
                if (consecutive)
                    return true;
            }

            //diagonal
            if (BoardValue[0] == BoardValue[4] && BoardValue[4] == BoardValue[8] && BoardValue[8] == currentPawn)
                return true;

            if (BoardValue[6] == BoardValue[4] && BoardValue[4] == BoardValue[2] && BoardValue[2] == currentPawn)
                return true;

            return false;
        }

        private int GetRandomNextCase()
        {
            List<int> possibleStates = GetPossibleCase().ToList();
            return possibleStates[rnd.Next(0, possibleStates.Count)];
        }

        private int GetLogicalNextCaseForPlayer(Player player)
        {
            double? bestQ = null;
            var bestLocations = new List<int>();
            var possibilities = GetPossibleCase().ToArray();
            for (var i = 0; i < possibilities.Count(); i++)
            {
                var currentQ = player.QualityBoard.TryGetValue(ToNextHash(possibilities[i], player.Pawn), out var value) ? value : 0;
                if (bestQ == null || currentQ > bestQ)
                {
                    bestQ = currentQ;
                    
                    bestLocations.Clear();
                    bestLocations.Add(possibilities[i]);
                }
                else if (currentQ == bestQ)
                {
                    bestLocations.Add(possibilities[i]);
                }
            }

            return bestLocations[rnd.Next(bestLocations.Count)];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWpf
{
    public class TicTacToeGame
    {
        private static List<TicTacToeGame> OldGames = new List<TicTacToeGame>();
        
        private int[,] gameMatrix;
        private int turn;
        private int steps;

        public int[,] GameMatrix
        {
            get { return gameMatrix; }
            private set { gameMatrix = value; }
        }
        public int Turn
        {
            get { return turn; }
            private set { turn = value; }
        }
        public int Steps
        {
            get { return steps; }
            private set { steps = value; }
        }
        Random rand;
        public TicTacToeGame()
        {
            Steps = 0;
            //create the board
            GameMatrix = new int[3, 3];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                    GameMatrix[i,j] = 0;
            
            // random the first turn
            rand = new Random();
            if (rand.Next(2) == 1)
                Turn = 1;
            else Turn = -1;
        }
        /// <summary>
        /// This method, change the turn in end of each turn.
        /// </summary>
        private void ChangeTurn()
        {
            Turn *= -1;
        }

        /// <summary>
        /// after you think where to put the X or O, use this method to play.
        /// </summary>
        /// <param name="player">your number</param>
        /// <param name="i">row</param>
        /// <param name="j">column</param>
        /// <returns>true if all are OK</returns>
        /// 
        public bool Play(int player, int i, int j)
        {
            if (player != Turn) throw new Exception("it's not your turn");
            if (i<0 || i>2 || j<0 ||j>2)
                throw new Exception($"parameters not in the board: {i},{j}");
            if (GameMatrix[i, j]!=0)
                throw new Exception($"this is played place, try another place...");
            GameMatrix[i, j] = player; //play
            steps++;

            ChangeTurn(); //change the turn
            return true;
        }
        /// <summary>
        /// start a new game and save the current.
        /// return new game.
        /// </summary>
        /// <returns>new game</returns>
        public TicTacToeGame GameOver()
        {
            OldGames.Add(this);
            return new TicTacToeGame();
        }


        public void DoTurnRobot()
        {
            //start with checking all options to win:
            int[] OptionToWin = TwoOfThisAndOneFree(Turn);
            if (OptionToWin != null)
            { Play(Turn, OptionToWin[0], OptionToWin[1]); return; }

            //then, check if my opponent can win and block him:
            int[] OptionToBlock = TwoOfThisAndOneFree(Turn * -1);
            if (OptionToBlock != null)
            { Play(Turn, OptionToBlock[0], OptionToBlock[1]); return; }

            //now, search where to put my xo:
            if (steps == 0) //if i'm the first
            {
                int a = 0, b = 0;
                if (rand.Next(2) == 1)
                    a = 2;
                if (rand.Next(2) == 1)
                    b = 2;
                Play(Turn, a, b);

                return;
            }

            //Default for checking only:
            Play(turn, rand.Next(2), rand.Next(2));
        }
        /// <summary>
        /// check if there are option to win and return the option. if don't have, return null;
        /// </summary>
        /// <param name="xo">the </param>
        /// <returns>return the option, or null</returns>
        private int[] TwoOfThisAndOneFree(int xo)
        {
            //row:
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                if (GameMatrix[i, 0] == xo && GameMatrix[i, 1] == xo && GameMatrix[i, 2] == 0)
                    return new int[] { i, 2 };
                else if (GameMatrix[i, 1] == xo && GameMatrix[i, 2] == xo && GameMatrix[i, 0] == 0)
                    return new int[] { i, 0 };
                else if (GameMatrix[i, 0] == xo && GameMatrix[i, 2] == xo && GameMatrix[i, 1] == 0)
                    return new int[] { i, 1 };
            }
            //column:
            for (int i = 0; i < GameMatrix.GetLength(1); i++)
            {
                if (GameMatrix[0,i] == xo && GameMatrix[1, i] == xo && GameMatrix[2, i] == 0)
                    return new int[] { 2, i };
                else if (GameMatrix[1, i] == xo && GameMatrix[2, i] == xo && GameMatrix[0,i] == 0)
                    return new int[] { 0, i };
                else if (GameMatrix[0, i] == xo && GameMatrix[2, i] == xo && GameMatrix[1, i] == 0)
                    return new int[] { 1, i };
            }
            //alachson \:
            if (GameMatrix[0, 0] == xo && GameMatrix[2, 2] == xo && GameMatrix[1, 1] == 0)
                return new int[] { 1, 1 };
            else if (GameMatrix[1, 1] == xo && GameMatrix[2, 2] == xo && GameMatrix[0, 0] == 0)
                return new int[] { 0, 0 };
            else if (GameMatrix[0, 0] == xo && GameMatrix[1,1] == xo && GameMatrix[2, 2] == 0)
                return new int[] { 2, 2 };
            //alachson /:
            if (GameMatrix[2, 0] == xo && GameMatrix[0, 2] == xo && GameMatrix[1, 1] == 0)
                return new int[] { 1, 1 };
            else if (GameMatrix[1, 1] == xo && GameMatrix[0, 2] == xo && GameMatrix[2, 0] == 0)
                return new int[] { 2, 0 };
            else if (GameMatrix[2, 0] == xo && GameMatrix[1, 1] == xo && GameMatrix[0, 2] == 0)
                return new int[] { 0, 2 };

            return null;
        }
        
        //private int[] 
    }
}

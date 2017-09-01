using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToeWpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TicTacToeGame BoardData; //the game object
        Button[,] BoardView;
        string PlayerName = "Player 1";
        public MainWindow()
        {
            InitializeComponent();
            BoardData = new TicTacToeGame();
            CreateBoardViewMatrix();
            UpdateBoardView();

            if (BoardData.Turn == 1)
                RobotPlay();
        }
        /// <summary>
        /// save in matrix all the buttons
        /// </summary>
        private void CreateBoardViewMatrix()
        {
            BoardView = new Button[3, 3];
            BoardView[0, 0] = Place00;
            BoardView[0, 1] = Place01;
            BoardView[0, 2] = Place02;
            BoardView[1, 0] = Place10;
            BoardView[1, 1] = Place11;
            BoardView[1, 2] = Place12;
            BoardView[2, 0] = Place20;
            BoardView[2, 1] = Place21;
            BoardView[2, 2] = Place22;
        }
        /// <summary>
        /// update the boardview from the boarddata
        /// </summary>
        private void UpdateBoardView()
        {
            // Robot is 1
            if (BoardData.Turn == 1)
                WhoIsNow.Text = "Robot - O";
            else WhoIsNow.Text = PlayerName+" - X";
            for (int i = 0; i < BoardView.GetLength(0); i++)
                for (int j = 0; j < BoardView.GetLength(1); j++)
                {
                    if (BoardData.GameMatrix[i, j] == 1)
                    {
                        BoardView[i, j].Content = "O"; //Robot is O
                        BoardView[i, j].IsEnabled = false;
                    }
                    else if (BoardData.GameMatrix[i, j] == -1)
                    {
                        BoardView[i, j].Content = "X";
                        BoardView[i, j].IsEnabled = false;
                    }
                    else
                    {
                        BoardView[i, j].Content = "";
                        BoardView[i, j].IsEnabled = true;
                    }
                }
        }

        private void One_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button Selected = sender as Button;
                int i=-1, j=-1;
                bool found = false;
                for (i = 0; i < BoardView.GetLength(0) && !found; i++)
                    for (j = 0; j < BoardView.GetLength(1) && !found; j++)
                        if (Selected.Name == BoardView[i, j].Name) found=true;
                //if (i == -1 || j == -1)
                //    StatusAndErrors.Text = "לא נמצא הכפתור";

                i--;j--; //because after finish the loop, it add them 1.
                BoardData.Play(BoardData.Turn, i, j);
                UpdateBoardView();

                RobotPlay();
            }
            catch (Exception ex)
            {
                if (ex.Message == "winner")
                    SomeoneWon();
                StatusAndErrors.Text = ex.Message;
            }
        }

        private void RobotPlay()
        {
            try
            {
                //Thread.Sleep(5000);
                BoardData.DoTurnRobot();
                UpdateBoardView();
            }
            catch (Exception ex)
            {

                StatusAndErrors.Text = ex.Message;
                if (ex.Message == "winner")
                { SomeoneWon(); return; }

                if (BoardData.Steps<9)
                    RobotPlay();

                
            }
            
        }
        private void SomeoneWon()
        {
            UpdateBoardView();
            WinnerLineView.X1 = BoardData.WinLine.X1;
            WinnerLineView.X2 = BoardData.WinLine.X2;
            WinnerLineView.Y1 = BoardData.WinLine.Y1;
            WinnerLineView.Y2 = BoardData.WinLine.Y2;
            WinnerLineView.Visibility = Visibility.Visible;

            //disable all the buttons:
            for (int i = 0; i < BoardView.GetLength(0); i++)
                for (int j = 0; j < BoardView.GetLength(1); j++)
                    BoardView[i, j].IsEnabled = false;
        }

        private void GameOverButton_Click(object sender, RoutedEventArgs e)
        {
            this.BoardData= BoardData.GameOver();
            WinnerLineView.Visibility = Visibility.Collapsed;
            UpdateBoardView();
        }
    }
}

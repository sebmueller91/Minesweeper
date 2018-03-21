using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper
{

    /// <summary>
    /// Interaction logic for MinesweeperWindow.xaml
    /// </summary>
    public partial class MinesweeperWindow : Window
    {
        private Field game;
        private Button[,] buttons;
        private bool[,] flagged;
        private bool gameEnd = false, timerRunning = false;
        private const int CELL_SIZE = 30;
        private int secCounter = 0;
        private DispatcherTimer timer;

        private System.Windows.Media.Brush[] colors = new System.Windows.Media.Brush[]
        {
      System.Windows.Media.Brushes.White,
      System.Windows.Media.Brushes.Blue,
      System.Windows.Media.Brushes.Green,
      System.Windows.Media.Brushes.Red,
      System.Windows.Media.Brushes.Purple,
      System.Windows.Media.Brushes.Brown,
      System.Windows.Media.Brushes.Turquoise,
      System.Windows.Media.Brushes.Violet
        };


        public MinesweeperWindow(string mode, MainWindow main_window)
        {
            main_window.Close();
            InitializeComponent();
            game = new Field();
            game.StartSession(mode);

            buttons = new Button[game.GetRows(), game.GetCols()];
            flagged = new bool[game.GetRows(), game.GetCols()];

            this.Height = game.GetRows() * CELL_SIZE;
            this.Width = game.GetCols() * CELL_SIZE;

            GRID_VAR.Rows = game.GetRows();
            GRID_VAR.Columns = game.GetCols();

            GRID_VAR.Height = game.GetRows() * CELL_SIZE;
            GRID_VAR.Width = game.GetCols() * CELL_SIZE;

            for (int i = 0; i < game.GetRows(); i++)
            {
                for (int j = 0; j < game.GetCols(); j++)
                {
                    System.Windows.Controls.Button newBtn = new Button();

                    newBtn.Content = "";
                    newBtn.Name = ("B" + (i * game.GetCols() + j)).ToString();
                    newBtn.Click += Button_Click;
                    newBtn.MouseRightButtonUp += RightClick;
                    newBtn.MouseDoubleClick += DoubleClick;
                    //newBtn.Width = CELL_SIZE;
                    //newBtn.Height = CELL_SIZE;
                    newBtn.BorderBrush = Brushes.Black;
                    newBtn.BorderThickness = new Thickness(0.5);

                    GRID_VAR.Children.Add(newBtn);
                    buttons[i, j] = newBtn;


                    flagged[i, j] = false;
                }
            }

            timer = new DispatcherTimer();
            timer.Tick += UpdateTime;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            //this.MaxWidth = this.MinWidth = this.Width;
            //this.MaxHeight = this.MinHeight = this.Height;

            UpdateWindow();

            TIME.Text = secCounter.ToString("000");
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            secCounter++;
            TIME.Text = secCounter.ToString("000");
        }

        private void UpdateWindow()
        {
            int n_flagged = 0;
            for (int i = 0; i < flagged.GetLength(0); i++)
            {
                for (int j = 0; j < flagged.GetLength(1); j++)
                {
                    if (flagged[i, j] == true)
                    {
                        n_flagged++;
                    }
                }
            }
            NUMBER_MINES.Text = Convert.ToString(game.GetNumberMines() - n_flagged);

            for (int i = 0; i < game.GetRows(); i++)
            {
                for (int j = 0; j < game.GetCols(); j++)
                {
                    if (game.GetField()[i, j].IsOpen() == false)
                    {
                        if (flagged[i, j] == true)
                        {
                            buttons[i, j].Content = "\uD83C\uDFF3";

                            //buttons[i, j].Background = System.Windows.Media.Brushes.Green;
                            buttons[i, j].FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            buttons[i, j].Content = "";
                            buttons[i, j].Background = System.Windows.Media.Brushes.DarkGray;
                        }
                    }
                    else if (game.GetField()[i, j].IsMine() == true)
                    {
                        buttons[i, j].Content = "\uD83D\uDCA3";
                        buttons[i, j].FontSize = 20.0;
                        buttons[i, j].Background = System.Windows.Media.Brushes.Red;
                        buttons[i, j].FontWeight = FontWeights.Bold;

                    }
                    else
                    {
                        int nM = game.GetField()[i, j].GetNNeighbouringMines();
                        buttons[i, j].Content = (nM > 0) ? Convert.ToString(nM) : "";
                        buttons[i, j].Foreground = colors[nM];
                        buttons[i, j].Background = System.Windows.Media.Brushes.LightGray;
                        buttons[i, j].FontSize = 20.0;
                        buttons[i, j].FontWeight = FontWeights.Bold;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (timerRunning == false)
            {
                timer.Start();
                timerRunning = true;
            }
            if (gameEnd == true)
            {
                return;
            }

            bool lost = false;
            int b_n = Convert.ToInt32((sender as Button).Name.Replace("B", ""));
            if (flagged[game.GetRowFromPos(b_n), game.GetColFromPos(b_n)] == true)
            {
                return;
            }
            else
            {
                lost = game.OpenCell(b_n);
            }

            UpdateWindow();

            if (lost == false)
            {
                timer.Stop();
                gameEnd = true;

            }
            else if (game.Finished() == true)
            {
                timer.Stop();
                gameEnd = true;
            }
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            if (gameEnd == true)
            {
                return;
            }
            int b_n = Convert.ToInt32((sender as Button).Name.Replace("B", ""));
            int r = game.GetRowFromPos(b_n), c = game.GetColFromPos(b_n);
            if (game.GetField()[r, c].IsOpen() == false)
            {
                flagged[game.GetRowFromPos(b_n), game.GetColFromPos(b_n)] =
                  (flagged[r, c] == false) ? true : false;
                UpdateWindow();
            }
        }

        private void DoubleClick(object sender, RoutedEventArgs e)
        {
            if (gameEnd == true)
            {
                return;
            }

            int b_n = Convert.ToInt32((sender as Button).Name.Replace("B", ""));
            int r = game.GetRowFromPos(b_n), c = game.GetColFromPos(b_n);

            if (game.GetField()[r, c].IsOpen() == true)
            {
                int nMines = 0;
                for (int i = r - 1; i <= r + 1; i++)
                {
                    for (int j = c - 1; j <= c + 1; j++)
                    {
                        if (i >= 0 && i < game.GetRows() && j >= 0 && j < game.GetCols() && flagged[i, j] == true)
                        {
                            nMines++;
                        }
                    }
                }

                if (nMines == game.GetField()[r, c].GetNNeighbouringMines())
                {
                    for (int i = r - 1; i <= r + 1; i++)
                    {
                        for (int j = c - 1; j <= c + 1; j++)
                        {
                            if (i < 0 || i >= flagged.GetLength(0) || j < 0 || j >= flagged.GetLength(1))
                            {
                                continue;
                            }
                            if (flagged[i, j] == false && game.GetField()[i, j].IsOpen() == false)
                            {
                                bool lost = game.OpenCell(i, j);

                                UpdateWindow();

                                if (lost == false)
                                {
                                    timer.Stop();
                                    gameEnd = true;

                                }
                                else if (game.Finished() == true)
                                {
                                    timer.Stop();
                                    gameEnd = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

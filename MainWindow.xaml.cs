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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2048
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Square[,] massSquare = new Square[4, 4];
        DelegateVoid newMoveEvent;
        int score = 0;
        WindowRezult formEndGame;

        public MainWindow()
        {
            InitializeComponent();
            Test();

            CreateFone();
            this.KeyDown += new KeyEventHandler(EventKeyDown);
            CreateButton();
        }

        void Test()
        {
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = Colors.Orange;
            animation.To = Colors.Gray;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void EventKeyDown(object sender, KeyEventArgs e)
        {
            IMove moveObj = null;
            switch (e.Key.ToString())
            {
                case "Right":
                    {
                        moveObj = new MoveRigth();
                        break;
                    }
                case "Left":
                    {
                        moveObj = new MoveLeft();
                        break;
                    }
                case "Up":
                    {
                        moveObj = new MoveUp();
                        break;
                    }
                case "Down":
                    {
                        moveObj = new MoveDown();
                        break;
                    }
                default:
                    break;
            }
            if (moveObj != null) NewStep(moveObj);
        }

        void NewStep(IMove moveObj)
        {
            bool move = moveObj.Move(massSquare,SetNewPos);
            if (IsEndGame(move)) return;
            newMoveEvent();
            if (move) CreateButton();
        }

        void NewScore(int value)
        {
            score += value;
            textScore.Text = "Score: " + score.ToString();
        }

        void ResetScore()
        {
            score = 0;
            textScore.Text = "Score: " + score.ToString();
        }

        void EndGame()
        {
            formEndGame = new WindowRezult(score, NewGame);
            if(!formEndGame.IsActive) formEndGame.Show();

        }

        void NewGame()
        {
            //Удаляем компоненты
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (massSquare[x, y] != null)
                    {
                        grid.Children.Remove(massSquare[x, y].button);
                        massSquare[x, y] = null;
                    }
                }
            }

            ResetScore();
            CreateButton();
        }

        bool IsEndGame(bool move)
        {
            if (!move)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (massSquare[x, y] == null)
                            return false;
                    }
                }
                EndGame();
                return true;
            }
            else
            {
                return false;
            }
        }
        

        void SetNewPos(Square square, Position pos)
        {
            massSquare[pos.X, pos.Y] = square;
            square.position = pos;
            Grid.SetColumn(square.button, pos.X * 2 + 2);
            Grid.SetRow(square.button, pos.Y * 2 + 4);
        }

        void CreateButton()
        {
            int x = 0;
            int y = 0;
            do
            {
                x = (MyClass.random.Next(0, 4));
                y = (MyClass.random.Next(0, 4));
            } while (massSquare[x, y] != null);

            Button button = new Button();
            button.FontFamily = new FontFamily("Arial Black");
            button.FontSize = 50;
            button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //button.FontFamily = new FontFamily("TimesNewRoman");
            grid.Children.Add(button);
            Grid.SetColumn(button, x * 2 + 2);
            Grid.SetRow(button, y * 2 + 4);
            Square square = new Square(x, y, button);
            massSquare[x, y] = square;
            square.destroy += DeleteButton;
            newMoveEvent += square.ResetAddStatus;
        }

        void CreateFone()
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Button button = new Button();
                    button.Background = new SolidColorBrush(Colors.Green);
                    grid.Children.Add(button);
                    Grid.SetColumn(button, x * 2 + 2);
                    Grid.SetRow(button, y * 2 + 4);
                }
            }
        }

        void DeleteButton(Square square)
        {
            NewScore(square.Value * 2);
            grid.Children.Remove(square.button);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (square.Equals(massSquare[x, y]))
                    {
                        massSquare[x, y] = null;
                    }
                }
            }
        }
    }


    public class MoveRigth : IMove
    {
        public bool Move(Square[,] massSquare, DelegateMove SetNewPos)
        {
            bool move = false;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 2; x >= 0; x--)
                {
                    if (massSquare[x, y] != null)
                    {
                        int pos = x;
                        while (pos < 2 && massSquare[pos + 1, y] == null)
                        {
                            pos++;
                        }

                        if (massSquare[pos + 1, y] != null) //Если есть по соседству квадрат
                        {
                            SetNewPos(massSquare[x, y], new Position(pos, y));
                            if (pos != x)
                            {
                                massSquare[x, y] = null;
                                move = true;
                            }

                            if (massSquare[pos + 1, y].AddNow) continue;
                            if (massSquare[pos + 1, y].AddSquare(massSquare[pos, y])) move = true;
                            continue;
                        }
                        else
                        {
                            SetNewPos(massSquare[x, y], new Position(pos + 1, y));
                            massSquare[x, y] = null;
                            move = true;
                        }
                    }
                }
            }
            return move;
        }
    }

    public class MoveLeft : IMove
    {
        public bool Move(Square[,] massSquare, DelegateMove SetNewPos)
        {
            bool move = false;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 1; x < 4; x++)
                {
                    if (massSquare[x, y] != null)
                    {
                        int pos = x;
                        while (pos > 1 && massSquare[pos - 1, y] == null)
                        {
                            pos--;
                        }

                        if (massSquare[pos - 1, y] != null) //Если есть по соседству квадрат
                        {
                            SetNewPos(massSquare[x, y], new Position(pos, y));
                            if (pos != x)
                            {
                                massSquare[x, y] = null;
                                move = true;
                            }


                            if (massSquare[pos - 1, y].AddNow) continue;
                            if (massSquare[pos - 1, y].AddSquare(massSquare[pos, y])) move = true;
                            continue;
                        }
                        else
                        {
                            SetNewPos(massSquare[x, y], new Position(pos - 1, y));
                            massSquare[x, y] = null;
                            move = true;
                        }
                    }
                }
            }
            return move;
        }
    }

    public class MoveUp : IMove
    {
        public bool Move(Square[,] massSquare, DelegateMove SetNewPos)
        {
            bool move = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 1; y < 4; y++)
                {
                    if (massSquare[x, y] != null)
                    {
                        int pos = y;
                        while (pos > 1 && massSquare[x, pos - 1] == null)
                        {
                            pos--;
                        }

                        if (massSquare[x, pos - 1] != null) //Если есть по соседству квадрат
                        {
                            SetNewPos(massSquare[x, y], new Position(x, pos));
                            if (pos != y)
                            {
                                massSquare[x, y] = null;
                                move = true;
                            }
                            if (massSquare[x, pos - 1].AddNow) continue;
                            if (massSquare[x, pos - 1].AddSquare(massSquare[x, pos])) move = true;
                            continue;
                        }
                        else
                        {
                            SetNewPos(massSquare[x, y], new Position(x, pos - 1));
                            massSquare[x, y] = null;
                            move = true;
                        }

                    }
                }
            }
            return move;
        }
    }

    public class MoveDown : IMove
    {
        public bool Move(Square[,] massSquare, DelegateMove SetNewPos)
        {
            bool move = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 2; y >= 0; y--)
                {
                    if (massSquare[x, y] != null)
                    {
                        int pos = y;
                        while (pos < 2 && massSquare[x, pos + 1] == null)
                        {
                            pos++;
                        }

                        if (massSquare[x, pos + 1] != null) //Если есть по соседству квадрат
                        {
                            SetNewPos(massSquare[x, y], new Position(x, pos));
                            if (pos != y)
                            {
                                massSquare[x, y] = null;
                                move = true;
                            }
                            if (massSquare[x, pos + 1].AddNow) continue;
                            if (massSquare[x, pos + 1].AddSquare(massSquare[x, pos])) move = true;
                            continue;
                        }
                        else
                        {
                            SetNewPos(massSquare[x, y], new Position(x, pos + 1));
                            massSquare[x, y] = null;
                            move = true;
                        }

                    }
                }
            }
            return move;
        }
    }
}

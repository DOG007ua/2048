using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2048
{
    public class Square
    {
        public Button button;
        public Position position;
        int value = 1;
        Color[] massColor = {
                Color.FromArgb(255, 219, 240, 240), //1
                Color.FromArgb(255, 219, 219, 219), //2
                Color.FromArgb(255, 240, 200, 150), //4
                Color.FromArgb(255, 255, 180, 100), //8
                Color.FromArgb(255, 255, 160, 80), //16 
                Color.FromArgb(255, 219, 128, 50), //32
                Color.FromArgb(255, 210, 16, 0),   //64
                Color.FromArgb(255, 255, 250, 147), //128
                Color.FromArgb(255, 255, 255, 110), //256
                Color.FromArgb(255, 255, 255, 80), //512
                Color.FromArgb(255, 255, 255, 40), //1024
                Color.FromArgb(255, 255, 255, 0), //2048
            };
        bool addStatus = false;
        public event DelegateSquare destroy;
        public bool AddNow { get { return addStatus; } }


        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                SetColor();
                button.Content = Value;
            }
        }

        void Destroy()
        {
            destroy(this);
        }

        void Click(object sender, RoutedEventArgs e)
        {
            
        }

        public bool AddSquare(Square squareAdd)
        {
            if (squareAdd.Value == Value)
            {
                Value *= 2;
                addStatus = true;
                squareAdd.Destroy();
                return true;
            }
            else return false;
        }

        public Square(int x, int y, Button button)
        {
            this.position = new Position(x, y);
            this.button = button;
            //Value = (int)Math.Pow(2, MyClass.random.Next(0, 1));
            Value = 2;
            button.Click += Click;
        }

        void SetColor()
        {
            Color color = new Color();
            int pos = (int)Math.Log(Value, 2);
            if (pos >= massColor.Length)    color = Colors.Red;
            else                            color = massColor[pos];

            button.Background = new SolidColorBrush(color);
            if (pos == 1)   button.Foreground = new SolidColorBrush(Color.FromArgb(255, 40, 10, 0));
            else            button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        public void ResetAddStatus()
        {
            addStatus = false;
        }
    }

    

    public class Position
    {
        int x;
        int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        
        public Position() { }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

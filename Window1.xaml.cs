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

namespace _2048
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindowRezult : Window
    {
        DelegateVoid FunNewGame;
        public WindowRezult(int rezult, DelegateVoid FunNewGame)
        {
            InitializeComponent();
            this.FunNewGame = FunNewGame;
            textScore.Text = "Result: " + rezult.ToString();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            FunNewGame();
            this.Close();
        }
    }
}

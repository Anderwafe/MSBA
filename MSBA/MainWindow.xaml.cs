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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSBA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnGameStart.Click += (s, e) =>
            {
                try
                {
                    if (PlaygroundSettings.isInputCorrect(Convert.ToInt32(CellCount.Text), Convert.ToInt32(BombCount.Text), BombCountType.SelectedIndex == 0 ? PlaygroundSettings.BombCountType.Count : PlaygroundSettings.BombCountType.Percent))
                    {
                        if (PlaygroundValues.CreatePlaygroundTable(Convert.ToInt32(CellCount.Text), BombCountType.SelectedIndex == 0 ? Convert.ToInt32(BombCount.Text) : ((Convert.ToInt32(BombCount.Text)/100) * Convert.ToInt32(CellCount.Text))))
                        {
                            this.Visibility = Visibility.Collapsed;
                            GameWindow gameWindow = new GameWindow();
                            gameWindow.ShowInTaskbar = true;
                            gameWindow.WindowState = WindowState.Maximized;
                            gameWindow.ShowDialog();
                            this.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Не получилось создать игровое поле. Повторите попытку, или посмотрите подсказку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введены неправильные параметры. Повторите попытку, или посмотрите подсказку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch(Exception a)
                {
                    MessageBox.Show("Ошибка чтения параметров. \nПовторите ввод, или посмотрите подказку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }
    }
}

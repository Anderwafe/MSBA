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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSBA
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public GameWindow()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(TimerCallbackHandler);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Stop();

            Button[,] Buttons = new Button[PlaygroundValues.PlaygroundTable.GetLength(0), PlaygroundValues.PlaygroundTable.GetLength(1)];
            StackPanel[] Panels = new StackPanel[PlaygroundValues.PlaygroundTable.GetLength(0)];
            for(int i = 0; i < PlaygroundValues.PlaygroundTable.GetLength(0); i++)
            {
                Panels[i] = new StackPanel();
                Panels[i].Orientation = Orientation.Horizontal;
                Playground.Children.Add(Panels[i]);
            }

            lblPresetName.Content += " Пользовательское";
            lblBombs1.Content += $" {PlaygroundValues.BombsCount} /";
            lblBombs2.Content = "0";

            for(int i = 0; i < PlaygroundValues.PlaygroundTable.GetLength(0); i++)
            {
                for(int j = 0; j < PlaygroundValues.PlaygroundTable.GetLength(1); j++)
                {
                    Buttons[i, j] = new Button();
                    Buttons[i, j].Name = $"btn{i}x{j}";
                    Buttons[i, j].Background = new SolidColorBrush(Colors.Gray);
                    Buttons[i, j].BorderBrush = new SolidColorBrush(Colors.Black);
                    Buttons[i, j].BorderThickness = new Thickness(1);
                    Buttons[i, j].Foreground = new SolidColorBrush(Colors.Black);
                    Buttons[i, j].Content = "";
                    Buttons[i, j].Height = 40;
                    Buttons[i, j].Width = 40;
                    Buttons[i, j].FontSize = 14;
                    Buttons[i, j].PreviewMouseDown += (s, e) =>
                    {
                        if(dispatcherTimer.IsEnabled == false)
                        {
                            dispatcherTimer.Start();
                            
                        }

                        Button but = (Button)s;
                        int i = Convert.ToInt32(but.Name.Split('x').First()[3..]);
                        int j = Convert.ToInt32(but.Name.Split('x').Last());
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {

                            if ((Buttons[i, j].Background as SolidColorBrush).Color == Colors.Gray)
                            {
                                if (Buttons[i, j].Content.ToString() != "F")
                                {
                                    if (PlaygroundValues.PlaygroundTable[i, j] == 'B')
                                    {
                                        Buttons[i, j].Background = new SolidColorBrush(Colors.OrangeRed);
                                        Image bomb = new Image();
                                        bomb.Source = new BitmapImage(new Uri("Resources/mine.png", UriKind.Relative));
                                        Buttons[i, j].Content = bomb;
                                        dispatcherTimer.IsEnabled = false;
                                        dispatcherTimer.Stop();
                                        MessageBox.Show("Упс... Как говорится: \"Сапёр совершает лишь две ошибки за жизнь, и, боюсь, это была вторая...\"", "Ложись, мина!", MessageBoxButton.OK);
                                        this.Close();
                                    }
                                    if (char.IsDigit(PlaygroundValues.PlaygroundTable[i, j]) && PlaygroundValues.PlaygroundTable[i, j] != '0')
                                    {
                                        Buttons[i, j].Background = new SolidColorBrush(Colors.White);
                                        Buttons[i, j].Content = PlaygroundValues.PlaygroundTable[i, j];
                                    }
                                    if (PlaygroundValues.PlaygroundTable[i, j] == '0')
                                    {
                                        Buttons[i, j].Background = new SolidColorBrush(Colors.White);
                                        for (int i1 = i - 1; i1 <= i + 1; i1++)
                                        {
                                            for (int j1 = j - 1; j1 <= j + 1; j1++)
                                            {
                                                if (i1 < 0 || j1 < 0 || i1 > Buttons.GetLength(0) - 1 || j1 > Buttons.GetLength(1) - 1)
                                                {
                                                    continue;
                                                }
                                                if (i == i1 && j == j1)
                                                {
                                                    continue;
                                                }
                                                Buttons[i1, j1].RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = Button.PreviewMouseDownEvent });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (e.RightButton == MouseButtonState.Pressed)
                            {
                                if ((Buttons[i, j].Background as SolidColorBrush).Color == Colors.Gray)
                                {
                                    if (Buttons[i, j].Content == "F")
                                    {
                                        Buttons[i, j].Content = "";
                                        lblBombs2.Content = (Convert.ToInt32(lblBombs2.Content) - 1).ToString();
                                    }
                                    else
                                    {
                                        Buttons[i, j].Content = "F";
                                        lblBombs2.Content = (Convert.ToInt32(lblBombs2.Content) + 1).ToString();
                                    }
                                }
                            }
                        }
                        if(Convert.ToInt32(lblBombs2.Content) == PlaygroundValues.BombsCount)
                        {
                            for(int a = 0; a < Buttons.GetLength(0); a++)
                            {
                                for(int b = 0; b < Buttons.GetLength(1); b++)
                                {
                                    if(Buttons[a, b].Content == "" && (Buttons[a, b].Background as SolidColorBrush).Color == Colors.Gray)
                                    {
                                        return;
                                    }
                                }
                            }

                            dispatcherTimer.IsEnabled = false;
                            dispatcherTimer.Stop();

                            MessageBox.Show("Вау. Возможно, это и называется победой? \nПоздравляю!", "Неожиданно", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                    };

                    Panels[i].Children.Add(Buttons[i, j]);
                }
            }
        }


        private void TimerCallbackHandler(object s, EventArgs e)
        {
            (lblTime as Label).Content = Convert.ToInt32((lblTime as Label).Content) + 1;
        }
    }
}

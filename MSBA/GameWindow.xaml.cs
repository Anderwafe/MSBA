using System;
using System.IO;
using System.Text.Json;
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

            System.ComponentModel.CancelEventHandler closingEvent = (s,e) =>
            {
                if (!Directory.Exists(System.IO.Path.Join(Environment.CurrentDirectory, "Saves")))
                {
                    Directory.CreateDirectory(System.IO.Path.Join(Environment.CurrentDirectory, "Saves"));
                }
                File.WriteAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", PlaygroundValues.Instance.GameName), JsonSerializer.Serialize<SerializablePlaygroundValues>(new()));
            };
            this.Closing += closingEvent;

            dispatcherTimer.Tick += new EventHandler(TimerCallbackHandler);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Stop();

            this.Closing += (s, e) => { dispatcherTimer.Stop(); };

            Button[,] Buttons = new Button[PlaygroundValues.Instance.PlaygroundTable.Length, PlaygroundValues.Instance.PlaygroundTable[0].Length];
            StackPanel[] Panels = new StackPanel[PlaygroundValues.Instance.PlaygroundTable.Length];
            for(int i = 0; i < PlaygroundValues.Instance.PlaygroundTable.Length; i++)
            {
                Panels[i] = new StackPanel();
                Panels[i].Orientation = Orientation.Horizontal;
                Playground.Children.Add(Panels[i]);
            }

            lblPresetName.Content += $" {PlaygroundValues.Instance.PresetName}";
            lblBombs1.Content += $" {PlaygroundValues.Instance.BombsCount} /";
            lblTime.Content = PlaygroundValues.Instance.ResultTime == "" ? "0" : PlaygroundValues.Instance.ResultTime;

            int buf = 0;
            for(int i = 0; i < PlaygroundValues.Instance.PlaygroundTable.Length; i++)
            {
                for(int j = 0; j < PlaygroundValues.Instance.PlaygroundTable[i].Length; j++)
                {
                    Buttons[i, j] = CreateButton(i,j);
                    /*Buttons[i, j].Name = $"btn{i}x{j}";
                    Buttons[i, j].Background = new SolidColorBrush(Colors.LightGray);
                    Buttons[i, j].BorderBrush = new SolidColorBrush(Colors.Black);
                    Buttons[i, j].BorderThickness = new Thickness(1);
                    Buttons[i, j].Foreground = new SolidColorBrush(Colors.Black);
                    Buttons[i, j].Content = "";
                    Buttons[i, j].Height = 40;
                    Buttons[i, j].Width = 40;
                    Buttons[i, j].FontSize = 14;*/
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

                            if ((Buttons[i, j].Background as SolidColorBrush).Color == Colors.LightGray)
                            {
                                if (Buttons[i, j].Content.ToString() != "F")
                                {
                                    if (PlaygroundValues.Instance.PlaygroundTable[i][j] == 'B')
                                    {
                                        Buttons[i, j].Background = new SolidColorBrush(Colors.OrangeRed);
                                        Image bomb = new Image();
                                        bomb.Source = new BitmapImage(new Uri("Resources/mine.png", UriKind.Relative));
                                        Buttons[i, j].Content = bomb;
                                        dispatcherTimer.IsEnabled = false;
                                        dispatcherTimer.Stop();
                                        MessageBox.Show("Боюсь, времени уже не осталось.", "Фатальная ошибка...", MessageBoxButton.OK);
                                        this.Closing -= closingEvent;
                                        if(File.Exists(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", PlaygroundValues.Instance.GameName)))
                                        {
                                            File.Delete(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", PlaygroundValues.Instance.GameName));
                                        }
                                        this.Close();
                                    }
                                    PlaygroundValues.Instance.CellsStatus[i][j] = 'O';
                                    if (char.IsDigit(PlaygroundValues.Instance.PlaygroundTable[i][j]) && PlaygroundValues.Instance.PlaygroundTable[i][j] != '0')
                                    {
                                        Buttons[i, j].Background = new SolidColorBrush(Colors.White);
                                        Buttons[i, j].Content = PlaygroundValues.Instance.PlaygroundTable[i][j];
                                    }
                                    if (PlaygroundValues.Instance.PlaygroundTable[i][j] == '0')
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
                                if ((Buttons[i, j].Background as SolidColorBrush).Color == Colors.LightGray)
                                {
                                    if (Buttons[i, j].Content == "F")
                                    {
                                        Buttons[i, j].Content = "";
                                        PlaygroundValues.Instance.CellsStatus[i][j] = 'C';
                                        lblBombs2.Content = (Convert.ToInt32(lblBombs2.Content) - 1).ToString();
                                    }
                                    else
                                    {
                                        Buttons[i, j].Content = "F";
                                        PlaygroundValues.Instance.CellsStatus[i][j] = 'M';
                                        lblBombs2.Content = (Convert.ToInt32(lblBombs2.Content) + 1).ToString();
                                    }
                                }
                            }
                        }
                        if(Convert.ToInt32(lblBombs2.Content) == PlaygroundValues.Instance.BombsCount)
                        {
                            for(int a = 0; a < Buttons.GetLength(0); a++)
                            {
                                for(int b = 0; b < Buttons.GetLength(1); b++)
                                {
                                    if(Buttons[a, b].Content == "" && (Buttons[a, b].Background as SolidColorBrush).Color == Colors.LightGray)
                                    {
                                        return;
                                    }
                                }
                            }

                            dispatcherTimer.IsEnabled = false;
                            dispatcherTimer.Stop();

                            //PlaygroundValues.Instance.ResultTime = lblTime.Content.ToString();
                            WinWindow WW = new();
                            WW.ShowDialog();
                            //MessageBox.Show("Вау. Возможно, это и называется победой? \nПоздравляю!", "Неожиданно", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Closing -= closingEvent;
                            if (File.Exists(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", PlaygroundValues.Instance.GameName)))
                            {
                                File.Delete(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", PlaygroundValues.Instance.GameName));
                            }
                            this.Close();
                        }
                    };

                    Panels[i].Children.Add(Buttons[i, j]);

                    if(PlaygroundValues.Instance.CellsStatus[i][j] == 'M')
                    {
                        buf++;
                    }
                }
            }
            lblBombs2.Content = $"{buf}";
        }

        private Button CreateButton(int i, int j)
        {
            Button button = new Button();


            switch(PlaygroundValues.Instance.CellsStatus[i][j])
            {
                case 'C':
                    button.Name = $"btn{i}x{j}";
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    button.BorderBrush = new SolidColorBrush(Colors.Black);
                    button.BorderThickness = new Thickness(1);
                    button.Foreground = new SolidColorBrush(Colors.Black);
                    button.Content = "";
                    button.Height = 40;
                    button.Width = 40;
                    button.FontSize = 14;
                    break;
                case 'M':
                    button.Name = $"btn{i}x{j}";
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    button.BorderBrush = new SolidColorBrush(Colors.Black);
                    button.BorderThickness = new Thickness(1);
                    button.Foreground = new SolidColorBrush(Colors.Black);
                    button.Content = "F";
                    button.Height = 40;
                    button.Width = 40;
                    button.FontSize = 14;
                    break;
                case 'O':
                    button.Name = $"btn{i}x{j}";
                    button.Background = new SolidColorBrush(Colors.White);
                    button.BorderBrush = new SolidColorBrush(Colors.Black);
                    button.BorderThickness = new Thickness(1);
                    button.Foreground = new SolidColorBrush(Colors.Black);
                    button.Content = PlaygroundValues.Instance.PlaygroundTable[i][j] != '0' ? PlaygroundValues.Instance.PlaygroundTable[i][j] : "";
                    button.Height = 40;
                    button.Width = 40;
                    button.FontSize = 14;
                    break;
            }
            
            return button;
        }

        private void TimerCallbackHandler(object s, EventArgs e)
        {
            (lblTime as Label).Content = Convert.ToInt32((lblTime as Label).Content) + 1;
            PlaygroundValues.Instance.ResultTime = lblTime.Content.ToString();
        }
    }
}

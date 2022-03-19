using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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

            

            if(!File.Exists(System.IO.Path.Join(Environment.CurrentDirectory, "Presets")) || File.ReadAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Presets")) == "")
            {
                File.WriteAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Presets"), JsonSerializer.Serialize<List<GamePreset>>(new List<GamePreset> { new GamePreset("Легко", "81", "10", PlaygroundSettings.BombCountType.Count), new GamePreset("Нормально", "256", "40", PlaygroundSettings.BombCountType.Count), new GamePreset("Сложно", "480", "99", PlaygroundSettings.BombCountType.Count), new GamePreset("Нереально", "720", "180", PlaygroundSettings.BombCountType.Count) }));
            }

            List<GamePreset> gamePresets = JsonSerializer.Deserialize<List<GamePreset>>(File.ReadAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Presets")));
            PresetChoose.Items.Add("Новый");
            foreach (var i in gamePresets)
            {
                PresetChoose.Items.Add(i.Name);
            }
            PresetChoose.SelectionChanged += (s, e) =>
            {
                try
                {
                    ComboBox CB = (ComboBox)s;
                    switch (CB.SelectedIndex)
                    {
                        case 0:
                            {
                                WindowForUserInput WFUI = new();
                                WFUI.ShowDialog();
                                if (WindowForUserInput.isOk)
                                {
                                    gamePresets.Add(new GamePreset(WindowForUserInput.PresetName, CellCount.Text, BombCount.Text, BombCountType.SelectedIndex == 0 ? PlaygroundSettings.BombCountType.Count : PlaygroundSettings.BombCountType.Percent));
                                    CB.Items.Add(WindowForUserInput.PresetName);
                                    CB.SelectedIndex = CB.Items.Count - 1;
                                }
                                else
                                {
                                    CB.SelectedIndex = 1;
                                }
                                break;
                            }
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            {
                                btnDeletePreset.IsEnabled = false;
                                CB.IsEditable = false;
                                CellCount.IsEnabled = false;
                                BombCount.IsEnabled = false;
                                BombCountType.IsEnabled = false;
                                CellCount.Text = gamePresets[CB.SelectedIndex - 1].CellsCount;
                                BombCount.Text = gamePresets[CB.SelectedIndex - 1].BombsCount;
                                BombCountType.SelectedIndex = gamePresets[CB.SelectedIndex - 1].BombsType == PlaygroundSettings.BombCountType.Count ? 0 : 1;
                                break;
                            }
                        default:
                            {
                                btnDeletePreset.IsEnabled = true;
                                CellCount.IsEnabled = true;
                                BombCount.IsEnabled = true;
                                BombCountType.IsEnabled = true;
                                CellCount.Text = gamePresets[CB.SelectedIndex - 1].CellsCount;
                                BombCount.Text = gamePresets[CB.SelectedIndex - 1].BombsCount;
                                BombCountType.SelectedIndex = gamePresets[CB.SelectedIndex - 1].BombsType == PlaygroundSettings.BombCountType.Count ? 0 : 1;
                                break;
                            }
                    }
                }
                catch
                {
                    PresetChoose.SelectedIndex = 1;
                }
            };
            PresetChoose.SelectedIndex = 1;

            this.Closed += (s, e) =>
            {
                File.WriteAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Presets"), JsonSerializer.Serialize<List<GamePreset>>(gamePresets));
            };

            btnDeletePreset.Click += (s, e) =>
            {
                gamePresets.RemoveAt(PresetChoose.SelectedIndex - 1);
                PresetChoose.Items.RemoveAt(PresetChoose.SelectedIndex);
                PresetChoose.SelectedIndex = 1;
            };

            btnGameStart.Click += (s, e) =>
            {
                try
                {
                    if (PlaygroundSettings.isInputCorrect(Convert.ToInt32(CellCount.Text), Convert.ToInt32(BombCount.Text), BombCountType.SelectedIndex == 0 ? PlaygroundSettings.BombCountType.Count : PlaygroundSettings.BombCountType.Percent))
                    {
                        if (PlaygroundValues.CreatePlaygroundTable(Convert.ToInt32(CellCount.Text), BombCountType.SelectedIndex == 0 ? Convert.ToInt32(BombCount.Text) : Convert.ToInt32(MathF.Ceiling((Convert.ToSingle(BombCount.Text)/100f) * Convert.ToSingle(CellCount.Text)))))
                        {
                            gamePresets[PresetChoose.SelectedIndex - 1].CellsCount = CellCount.Text;
                            gamePresets[PresetChoose.SelectedIndex - 1].BombsCount = BombCount.Text;
                            gamePresets[PresetChoose.SelectedIndex - 1].BombsType = BombCountType.SelectedIndex == 0 ? PlaygroundSettings.BombCountType.Count : PlaygroundSettings.BombCountType.Percent;
                            PlaygroundValues.PresetName = gamePresets.ElementAt(PresetChoose.SelectedIndex - 1).Name;
                            PlaygroundValues.GameName = DateTime.Now.ToString() + "_" + PlaygroundValues.PresetName;

                            this.Visibility = Visibility.Collapsed;
                            GameWindow gameWindow = new GameWindow();
                            gameWindow.ShowInTaskbar = true;
                            gameWindow.WindowState = WindowState.Maximized;
                            File.WriteAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Presets"), JsonSerializer.Serialize<List<GamePreset>>(gamePresets));
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

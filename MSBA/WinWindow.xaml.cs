﻿using System;
using System.IO;
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

namespace MSBA
{
    /// <summary>
    /// Логика взаимодействия для WinWindow.xaml
    /// </summary>
    public partial class WinWindow : Window
    {
        public WinWindow()
        {
            InitializeComponent();

            lblPresetName.Content = PlaygroundValues.PresetName;
            lblTime.Content = PlaygroundValues.ResultTime;
            lblCellsCount.Content = PlaygroundValues.PlaygroundTable.GetLength(0) * PlaygroundValues.PlaygroundTable.GetLength(1);
            lblMinesCount.Content = PlaygroundValues.BombsCount;

            btnExit.Click += (s, e) => this.Close();

            btnSave.Click += (s, e) =>
            {
                if (!Directory.Exists(System.IO.Path.Join(Environment.CurrentDirectory, "Saves")))
                    Directory.CreateDirectory(System.IO.Path.Join(Environment.CurrentDirectory, "Saves"));
                File.WriteAllText(System.IO.Path.Join(Environment.CurrentDirectory, "Saves", DateTime.Now.ToString("G").Replace(":", "-").Replace(".", "-").Replace(" ", "_")), MatchInfo.GetMatchInfo(new MatchInfo(lblPresetName.Content.ToString(), lblTime.Content.ToString(), lblMinesCount.Content.ToString(), lblCellsCount.Content.ToString())));
            };
        }
    }
}
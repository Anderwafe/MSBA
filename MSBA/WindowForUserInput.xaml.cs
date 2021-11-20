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

namespace MSBA
{
    /// <summary>
    /// Логика взаимодействия для WindowForUserInput.xaml
    /// </summary>
    public partial class WindowForUserInput : Window
    {
        public static bool isOk { get; set; }
        public static string? PresetName { get; set; }

        public WindowForUserInput()
        {
            InitializeComponent();
            isOk = false;
            PresetName = null;
            btnCancel.Click += (s, e) =>
            {
                isOk = false;
                PresetName = null;
                this.Close();
            };
            btnOK.Click += (s, e) =>
            {
                if (tbPresetName.Text.Replace(" ", "").Length != 0)
                {
                    isOk = true;
                    PresetName = tbPresetName.Text.Trim();
                    this.Close();
                }
            };
        }
    }
}

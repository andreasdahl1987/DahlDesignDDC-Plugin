using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace DahlDesign.Plugin
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public DDC Plugin { get; }

        public List<string> itemsList = new List<string>
        {
            "Item 1",
            "Item 2",
            "Item 3"
        };


        public SettingsControl()
        {
            InitializeComponent();
        }


        public SettingsControl(DDC plugin) : this()
        {
            this.Plugin = plugin;
            myComboBox.ItemsSource = plugin.controllerNames;
            myComboBox.SelectionChanged += MyComboBox_SelectionChanged;
        }


        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Plugin.selectedItem = myComboBox.SelectedIndex;
        }       
    }
}

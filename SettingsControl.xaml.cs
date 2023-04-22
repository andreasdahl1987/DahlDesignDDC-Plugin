using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace DahlDesign.Plugin
{
    public partial class SettingsControl : UserControl
    {
        public DDC Plugin { get; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public SettingsControl(DDC plugin) : this()
        {
            this.Plugin = plugin;
            myComboBox.ItemsSource = plugin.controllerNames;
            myComboBox.SelectionChanged += MyComboBox_SelectionChanged;
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Plugin.selectedItem = myComboBox.SelectedIndex;
            Plugin.Settings.DDCselector = Plugin.controllerNames[myComboBox.SelectedIndex];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Plugin.firstSelect)
            {
                myComboBox.SelectedIndex = Plugin.controllerNames.IndexOf(Plugin.Settings.DDCselector);
                Plugin.firstSelect = false;
            }
                
        }
    }
}

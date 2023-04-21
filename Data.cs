using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;

namespace DahlDesign.Plugin
{
    [PluginDescription("Detects DDC controller to build properties")]
    [PluginAuthor("Andreas Dahl")]
    [PluginName("DDC")]
    public class DDC : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public DataSettings Settings;
        public PluginManager PluginManager { get; set; }
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.Dahl_icon);
        public string LeftMenuTitle => "DDC";


        public ObservableCollection<string> controllerNames { get; set; } = new ObservableCollection<string> { "dummy"};
        public int selectedItem { get; set; } = -1;
        public int clutchValue { get; set; } = 0;
        public int throttleValue { get; set; } = 0;
        public int brakeValue { get; set; } = 0;
        public int bitePointValue { get; set; } = 0;
        public int rotaryFieldValue { get; set; } = 0;
        public int buttonField { get; set; } = 0;


        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            List<ACToolsUtilities.JoystickDevice> gameControllers = new List<ACToolsUtilities.JoystickDevice> { };
            gameControllers = (List<ACToolsUtilities.JoystickDevice>) pluginManager.GetConnectedGameControllers();
            controllerNames.Clear();

            for(int i = 0; i < gameControllers.Count; i++)
            {
                controllerNames.Add(gameControllers[i].Name.Replace("_"," "));
            }

            if (selectedItem >= 0)
            {
                clutchValue = gameControllers[selectedItem].LastState.X;
                clutchValue = gameControllers[selectedItem].LastState.X;
            }
        }

        public void grabControllerInfo (int selector)
        {


        }
        
        public void End(PluginManager pluginManager)
        {
            this.SaveCommonSettings("GeneralSettings", Settings);
        }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControl(this);

        }
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");

            Settings = this.ReadCommonSettings<DataSettings>("GeneralSettings", () => new DataSettings());

            this.AttachDelegate("CurrentDateTime", () => DateTime.Now);
        }
    }
}
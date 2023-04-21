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
using System.Windows;


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

        //Public globals
        public ObservableCollection<string> controllerNames { get; set; } = new ObservableCollection<string> { "dummy"};
        public int selectedItem { get; set; } = -1;
        public bool selectInit { get; set; } = false;

        //Private globals
        double clutchValue = 0;
        double throttleValue = 0;
        double brakeValue = 0;
        double bitePointValue = 0;
        int rotaryField = 0;
        int buttonField = 0;

        int controllerCount = 0;
        bool countUpdate = false;
        bool firstInit = true;

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            List<ACToolsUtilities.JoystickDevice> gameControllers = new List<ACToolsUtilities.JoystickDevice> { };
            gameControllers = (List<ACToolsUtilities.JoystickDevice>) pluginManager.GetConnectedGameControllers();

            if (controllerCount != gameControllers.Count)
            {
                controllerCount = gameControllers.Count;
                countUpdate = true;
            }

            if (countUpdate)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    controllerNames.Clear();
                    for (int i = 0; i < gameControllers.Count; i++)
                    {
                        controllerNames.Add(gameControllers[i].Name.Replace("_", " "));
                    }
                    countUpdate = false;
                });
            }

            if (controllerCount >= Settings.DDCselector && firstInit)
            {
                firstInit = false;
                selectInit = true;
            }


            if (selectedItem >= 0)
            {
                clutchValue = Math.Round(gameControllers[selectedItem].LastState.X / 655.35, 1);
                brakeValue = Math.Round(gameControllers[selectedItem].LastState.RotationZ / 655.35,1);
                throttleValue = Math.Round(gameControllers[selectedItem].LastState.AccelerationX / 655.35, 1);
                bitePointValue = Math.Round(gameControllers[selectedItem].LastState.RotationY / 655.35,1);

                buttonField = Convert.ToInt32(gameControllers[selectedItem].LastState.Y);
                rotaryField = Convert.ToInt32(gameControllers[selectedItem].LastState.Z);
            }
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

            addProps();
        }

        public void addProps()
        {
            //Proper axis
            this.AttachDelegate("Clutch", () => clutchValue);
            this.AttachDelegate("Brake", () => brakeValue);
            this.AttachDelegate("Throtle", () => throttleValue);
            this.AttachDelegate("BitePoint", () => bitePointValue);

            //Button field
            this.AttachDelegate("B1", () => buttonField & 0x01);
            this.AttachDelegate("B2", () => buttonField >> 1 & 0x01);
            this.AttachDelegate("B3", () => buttonField >> 2 & 0x01);
            this.AttachDelegate("B4", () => buttonField >> 3 & 0x01);
            this.AttachDelegate("NeutralActive", () => buttonField >> 4 & 0x01);
            this.AttachDelegate("ThrottleHoldActive", () => buttonField >> 5 & 0x01);
            this.AttachDelegate("BrakeMagicActive", () => buttonField >> 6 & 0x01);
            this.AttachDelegate("QuickSwitchMode", () => buttonField >> 7 & 0x01);
            this.AttachDelegate("QuickSwitchActive", () => buttonField >> 8 & 0x01);
            this.AttachDelegate("HandbrakeActive", () => buttonField >> 9 & 0x01);
            this.AttachDelegate("Preset", () => (buttonField >> 10 & 0x0f) + 1);
            this.AttachDelegate("NeutralMode", () => buttonField >> 14 & 0x01);

            //Rotary field
            this.AttachDelegate("R1", () => rotaryField & 0x01);
            this.AttachDelegate("R2", () => rotaryField >> 1 & 0x01);
            this.AttachDelegate("R3", () => rotaryField >> 2 & 0x01);
            this.AttachDelegate("R4", () => rotaryField >> 3 & 0x01);
            this.AttachDelegate("R5", () => rotaryField >> 4 & 0x01);
            this.AttachDelegate("R6", () => rotaryField >> 5 & 0x01);
            this.AttachDelegate("R7", () => rotaryField >> 6 & 0x01);
            this.AttachDelegate("R8", () => rotaryField >> 7 & 0x01);
            this.AttachDelegate("DDSMode", () => rotaryField >> 8 & 0x03);
            this.AttachDelegate("BiteSetting", () => rotaryField >> 10 & 0x03);
            this.AttachDelegate("ClutchMode", () => rotaryField >> 12 & 0x03);
            this.AttachDelegate("R15", () => rotaryField >> 14 & 0x01);
        }
    }
}
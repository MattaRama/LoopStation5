using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopStation5._1
{
    public partial class frmLoopstation : Form
    {
        public readonly static string loopPath = @"C:\loopInit\1\";
        //Loopstation Variables
        public static int currentLoop = 0;
        public static int loopStandard = 0;
        public static bool loopRunning = false;

        private static LoopStation currentSettingDevice;
        private static Button currentSettingButton;

        //Initializes loopstations
        private static LoopStation ls1 = new LoopStation(1);
        private static LoopStation ls2 = new LoopStation(2);
        private static LoopStation ls3 = new LoopStation(3);
        private static LoopStation ls4 = new LoopStation(4);
        private static LoopStation ls5 = new LoopStation(5);
        private static LoopStation loopStationConfig = new LoopStation(-1, true); //CONFIG HOLDER ONLY

        private static LoopStation[] lsArray;

        //Updates Button States
        private static Button[] playPauseButtons;
        private static Button[] editButtons;
        public static int[] buttonState = { 0, 0, 0, 0, 0 };
        public static bool[] isHoldingAction = { false, false, false, false, false };

        public static void UpdateRecButton(int button, int state)
        {
            //Gets color to change button to
            Color color = new Color();
            if (state == 0)
            {
                color = SystemColors.ControlDark;
                buttonState[button - 1] = 0;
            }
            else if (state == 1)
            {
                color = SystemColors.ControlDarkDark;
                buttonState[button - 1] = 1;
            }
            else if (state == 2)
            {
                color = Color.LawnGreen;
                buttonState[button - 1] = 2;
            }
            else if (state == 3)
            {
                color = Color.Yellow;
                buttonState[button - 1] = 3;
            }
            else if (state == 4)
            {
                color = Color.Red;
                buttonState[button - 1] = 4;
            }

            //Sets color of button
            playPauseButtons[button - 1].BackColor = color;
        }

        //Form initialization
        public frmLoopstation()
        {
            InitializeComponent();
            //Initializes Arrays
            playPauseButtons = new Button[] { cmdPlayPause1, cmdPlayPause2, cmdPlayPause3, cmdPlayPause4, cmdPlayPause5 };
            editButtons = new Button[] { cmdEdit1, cmdEdit2, cmdEdit3, cmdEdit4, cmdEdit5, cmdEditLoopstation };
            lsArray = new LoopStation[] { ls1, ls2, ls3, ls4, ls5 };

            //  CONFIGURATION CONTROLLER    //

            /* Boolean = 0
             * Integer = 1
             * String = 2 (God forbid you have to use a string)
            */

            //Initializes Configuration
            loopStationConfig.configuration.Add(new Object[] { "Stop Rec After Track Init.", false, 0 });
        }
        private void FrmLoopstation_Load(object sender, EventArgs e)
        {
            
        }

        //Primary Timer
        private void TmrTicks_Tick(object sender, EventArgs e)
        {
            //Handles loop increase
            if (loopRunning == true)
            {
                currentLoop += 1;
                if (currentLoop >= loopStandard)
                {
                    //Loop Reset
                    currentLoop = 0;

                    //If a stop action was queued, it is executed here
                    for (int i = 0; i < lsArray.Count(); i++)
                    {
                        if (isHoldingAction[i] == true)
                        {
                            if (Boolean.Parse(loopStationConfig.configuration[0][1].ToString()) == false)
                            {
                                lsArray[i].EventListener(0, new object[] { 0});
                            }
                        }
                    }
                }
            } else
            {
                currentLoop = 0;
            }

            //Passes tick to loopstations
            ls1.EventListener(2);
            ls2.EventListener(2);
            ls3.EventListener(2);
            ls4.EventListener(2);
            ls5.EventListener(2);

            //Gets loop information
            bool[] loopsRunning = new bool[] { ls1.loopPlaying, ls2.loopPlaying, ls3.loopPlaying, ls4.loopPlaying, ls5.loopPlaying};
            int numberOfLoopsPlaying = 0;
            foreach (bool isLoopRunning in loopsRunning)
            {
                if (isLoopRunning == true)
                {
                    numberOfLoopsPlaying += 1;
                }
            }
            if (numberOfLoopsPlaying > 0)
            {
                loopRunning = true;
            }
        }

        //Event Listener
        public static void EventListener(int loopEvent)
        {
            if (loopEvent == 0)
            {
                //Pushes loop standard to all loopstations
                ls1.loopLength = loopStandard;
                ls2.loopLength = loopStandard;
                ls3.loopLength = loopStandard;
                ls4.loopLength = loopStandard;
                ls5.loopLength = loopStandard;
            } else if (loopEvent == 1)
            {
                //Makes settings buttons look pretty
                foreach (Button button in editButtons)
                {
                    button.BackColor = Color.LawnGreen;
                }
            }
        }




        //Play & Record Button
        private void CmdPlayPause1_Click(object sender, EventArgs e)
        {
            if (buttonState[0] == 4)
            {
                isHoldingAction[0] = true;
            }
        }
        private void CmdPlayPause2_Click(object sender, EventArgs e)
        {
            if (buttonState[0] == 4)
            {
                isHoldingAction[1] = true;
            }
        }
        private void CmdPlayPause3_Click(object sender, EventArgs e)
        {
            if (buttonState[0] == 4)
            {
                isHoldingAction[2] = true;
            }
        }
        private void CmdPlayPause4_Click(object sender, EventArgs e)
        {
            if (buttonState[0] == 4)
            {
                isHoldingAction[3] = true;
            }
        }
        private void CmdPlayPause5_Click(object sender, EventArgs e)
        {
            if (buttonState[0] == 4)
            {
                isHoldingAction[4] = true;
            }
        }

        //Pause Button
        private void CmdPause1_Click(object sender, EventArgs e)
        {

        }
        private void CmdPause2_Click(object sender, EventArgs e)
        {

        }
        private void CmdPause3_Click(object sender, EventArgs e)
        {

        }
        private void CmdPause4_Click(object sender, EventArgs e)
        {

        }
        private void CmdPause5_Click(object sender, EventArgs e)
        {

        }
        private void CmdStartStopGroup_Click(object sender, EventArgs e)
        {

        }
        private void CmdStartStopAll_Click(object sender, EventArgs e)
        {

        }

        //Edit Buttons
        private void CmdEdit1_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = ls1;
            LoadSettings(ls1.configuration);
            currentSettingButton = cmdEdit1;

            EventListener(1);
        }
        private void CmdEdit2_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = ls2;
            LoadSettings(ls2.configuration);
            currentSettingButton = cmdEdit2;

            EventListener(1);
        }
        private void CmdEdit3_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = ls3;
            LoadSettings(ls3.configuration);
            currentSettingButton = cmdEdit3;

            EventListener(1);
        }
        private void CmdEdit4_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = ls4;
            LoadSettings(ls4.configuration);
            currentSettingButton = cmdEdit4;

            EventListener(1);
        }
        private void CmdEdit5_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = ls5;
            LoadSettings(ls5.configuration);
            currentSettingButton = cmdEdit5;

            EventListener(1);
        }
        private void CmdEditLoopstation_Click(object sender, EventArgs e)
        {
            //Sets settings devices
            currentSettingDevice = loopStationConfig;
            LoadSettings(loopStationConfig.configuration);
            currentSettingButton = cmdEditLoopstation;

            EventListener(1);
        }

        //Loads settings
        private void LoadSettings(List<Object[]> configuration)
        {
            txtSetting.Text = "";
            lstSettings.Items.Clear();
            foreach (Object[] confPair in configuration) {
                lstSettings.Items.Add(confPair[0]);
            }
        }

        //Settings controller
        private void LstSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Loads textbox with setting value
            txtSetting.Text = currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString();
        }
        private void CmdSettingLeft_Click(object sender, EventArgs e)
        {
            //Moves ints down or switches boolean
            if (currentSettingDevice.configuration[lstSettings.SelectedIndex][1] is int )
            {
                //Cannot go below 0
                if (int.Parse(currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString()) > 1)
                {
                    currentSettingDevice.configuration[lstSettings.SelectedIndex][1] = int.Parse(currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString()) - 1;
                }
            } else if (currentSettingDevice.configuration[lstSettings.SelectedIndex][1] is bool)
            {
                currentSettingDevice.configuration[lstSettings.SelectedIndex][1] = Lib.SwitchBoolean(Boolean.Parse(currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString()));
            }

            //Updates textbox
            txtSetting.Text = currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString();
        }
        private void CmdSettingRight_Click(object sender, EventArgs e)
        {
            //Moves ints up or switches boolean
            if (currentSettingDevice.configuration[lstSettings.SelectedIndex][1] is int)
            {
                currentSettingDevice.configuration[lstSettings.SelectedIndex][1] = int.Parse(currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString()) + 1;
            }
            else if (currentSettingDevice.configuration[lstSettings.SelectedIndex][1] is bool)
            {
                currentSettingDevice.configuration[lstSettings.SelectedIndex][1] = Lib.SwitchBoolean(Boolean.Parse(currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString()));
            }

            //Updates textbox
            txtSetting.Text = currentSettingDevice.configuration[lstSettings.SelectedIndex][1].ToString();
        }

        //Signifies active selection for settings. Really just to make things pretty.
        private void TmrButtonFlash_Tick(object sender, EventArgs e)
        {
            if (currentSettingButton != null)
            {
                if (currentSettingButton.BackColor == Color.Red)
                {
                    currentSettingButton.BackColor = Color.LawnGreen;
                } else
                {
                    currentSettingButton.BackColor = Color.Red;
                }
            }
        }
    }
}

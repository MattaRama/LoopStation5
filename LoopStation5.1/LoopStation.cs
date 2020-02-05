using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Windows.Forms;

namespace LoopStation5._1
{
    class LoopStation
    {
        //Loopstation Variables
        public bool loopPlaying = false;
        public bool isRecording = false;
        bool hasRecorded = false;
        bool isLoopStandard = false;
        int recordingTicks = 0;
        public int loopLength = 0;
        int loopStationID;
        List<object[]> recs = new List<object[]>();

        public List<object[]> configuration = new List<object[]>();

        //Record variables
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;

        public List<WaveOutEvent> playerList = new List<WaveOutEvent>();
        public List<AudioFileReader> audioFileList = new List<AudioFileReader>();

        //Constructor
        public LoopStation(int loopstationID, bool isEmpty = false)
        {
            loopStationID = loopstationID;

            //  CONFIGURATION CONTROLLER    //

            /* Boolean = 0
             * Integer = 1
             * String = 2 (God forbid you have to use a string)
            */

            //Initializes Configuration
            if (isEmpty == false)
            {
                configuration.Add(new object[] { "Length Multiplier", 1, 1 });
                configuration.Add(new object[] { "Is in Stop Group", false, 0 });
            }
        }

        //Handles button presses
        public void EventListener(int loopEvent, Object[] args = null)
        {
            if (loopEvent == 0)
            {
                if (int.Parse(args[0].ToString()) == 0) //Attempt to stop after first record
                {
                    if (bool.Parse(args[1].ToString()))
                    {
                        StopRecord(true);
                    } else
                    {
                        StopRecord(false);
                    }
                } else if (int.Parse(args[0].ToString()) == 1) //Attempt to start first record
                {
                    InitialRecord();

                } else if(int.Parse(args[0].ToString()) == 2) //Play
                {
                    PlayHandeler();
                } else if (int.Parse(args[0].ToString()) == 3) //Starts record while loop is active
                {
                    Record(false);
                } 
                


            } else if (loopEvent == 1)
            {
                //Handles Stop button

            } else if (loopEvent == 2)
            {
                //Handles timer tick
                if (isRecording == true)
                {
                    recordingTicks += 1;
                }
                PlayerController();

            } else if(loopEvent == 3)
            {
                //Pushes a complete loop cycle

            }
        }

        //Basic control functions
        private void InitialRecord()
        {
            //For first record
            recs.Add(new object[] { recs.Count, 0}); //Creates the looprec as the ID of the rec (Latest looprec + 1) and 0 (Init. time)
            frmLoopstation.UpdateRecButton(loopStationID, 4);
            if (frmLoopstation.loopStandard == 0)
            {
                isLoopStandard = true;
            }
            Record(true);
        }

        private void Record(bool isFirst)
        {
            //Performs actions to prep if InitialRecord has been established
            if (isFirst == false)
            {
                recs.Add(new object[] { recs.Count, frmLoopstation.currentLoop });
                frmLoopstation.UpdateRecButton(loopStationID, 3);
            }

            //Prep for Record
            if (Directory.Exists(frmLoopstation.loopPath + loopStationID) == false)
            {
                Directory.CreateDirectory(frmLoopstation.loopPath + loopStationID);
            }

            playerList.Add(new WaveOutEvent());

            recordingTicks = 0;
            isRecording = true;

            //Start Record
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveFile = new WaveFileWriter(frmLoopstation.loopPath + loopStationID + @"\" + recs[recs.Count - 1][0], waveSource.WaveFormat);

            waveSource.StartRecording();
        }

        private void StopRecord(bool shouldContinue)
        {
            //Pushes initial loop continue
            if (isLoopStandard == true)
            {
                frmLoopstation.loopStandard = recordingTicks;
                isLoopStandard = false;
                frmLoopstation.EventListener(0);
            }

            if (loopLength == 0)
            {
                loopLength = recordingTicks;
            }

            //Stops recording
            waveSource.StopRecording();
            audioFileList.Add(new AudioFileReader(frmLoopstation.loopPath + loopStationID + @"\" + recs[recs.Count - 1][0]));
            playerList[playerList.Count - 1].Init(audioFileList[playerList.Count - 1]);

            //Continues to record if set to
            if (shouldContinue == true)
            {
                Record(false);

            } else
            {
                isRecording = false;
                frmLoopstation.UpdateRecButton(loopStationID, 2);
            }
        }

        private void PlayHandeler()
        {

        }

        private void PlayerController()
        {
            for (int i = 0; i < recs.Count; i++)
            {
                if (int.Parse(recs[i][1].ToString()) == frmLoopstation.currentLoop)
                {
                    if (isRecording == true && recs.Count - 1 == i)
                    {
                        
                    } else
                    {
                        playerList[i].Play();
                    }
                }
            }
        }

        private void Pause()
        {

        }

        //Recording Voids
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }
        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }
    }
}

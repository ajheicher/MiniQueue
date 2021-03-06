﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Web;
using System.Web.Script.Serialization;

namespace MiniQueue
{
    public partial class MiniQueueWindow : Form
    {

        //TODO: create default setting constants

        //This entire section is for the weird signal magic we do below
        //It prevents flicker when forcing the aspect ratio static
        const double widthRatio = 113;
        const double heightRatio = 53;

        const int WM_SIZING = 0x214;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;
        const int WMSZ_BOTTOM = 6;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        

        //eventually this will be configurable in settings
        readonly int defaultRetryValue = Properties.Settings.Default.UpdateInterval * 1000;

        //maximum value containers
        int maxCalls;
        int maxWaiting;

        //Error handling
        bool isErrorState = false;
        int retryValueMs = Properties.Settings.Default.UpdateInterval * 1000;



        public MiniQueueWindow()
        {
            InitializeComponent();
            maxCalls = 0;
            maxWaiting = 0;

            label1.Text = "Contacts Waiting:";
            label2.Text = "Longest Waitiing:";


        }
        /*
        /// <summary>
        /// Basically this capitures the WndProc message for window resize
        /// and ensures that the window maintains the same expect ratio
        /// It does this before the Resize event actually fires, so we
        /// don't have to deal with flicker
        /// License and Credit:http://www.vcskicks.com/license.php
        /// </summary>
        /// <param name="m">Message</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SIZING)
            {
                RECT rc = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                int res = m.WParam.ToInt32();
                if (res == WMSZ_LEFT || res == WMSZ_RIGHT)
                {
                    //Left or right resize -> adjust height (bottom)
                    rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
                }
                else if (res == WMSZ_TOP || res == WMSZ_BOTTOM)
                {
                    //Up or down resize -> adjust width (right)
                    rc.Right = rc.Left + (int)(widthRatio * this.Height / heightRatio);
                }
                else if (res == WMSZ_RIGHT + WMSZ_BOTTOM)
                {
                    //Lower-right corner resize -> adjust height (could have been width)
                    rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
                }
                else if (res == WMSZ_LEFT + WMSZ_TOP)
                {
                    //Upper-left corner -> adjust width (could have been height)
                    rc.Left = rc.Right - (int)(widthRatio * this.Height / heightRatio);
                }
                Marshal.StructureToPtr(rc, m.LParam, true);
            }

            base.WndProc(ref m);
        }*/

        private void MiniQueueWindow_Load(object sender, EventArgs e)
        {
            
            //creates a new thread for the queue update operation
            //allows the desktop window to continue running
            Thread cThread = new Thread(updateQueue);

            //start thread
            cThread.Start();

        }

        private async void updateQueue()
        {
           
            //because scope matters

            WebClient wc = null;

            while (true)
            {
                if (isErrorState)
                {
                    if (retryValueMs < 60000) { retryValueMs += 5000; }
                }
                try
                {
                    wc = new WebClient();

                    //update the queue
                   
                    //TODO: your data structures are bad and you should feel bad
                    int[] theAnswer;

                    //Releases the current thread back to windows form
                    Application.DoEvents();

                    //TODO: move to app.config
                    theAnswer = parseJSONData(wc.DownloadString("http://uccxpub01.philorch.org:9080/realtime/VoiceIAQStats"));

                    //reset out of error state - we're recovered here
                    if (isErrorState)
                    {
                        isErrorState = false;
                        retryValueMs = defaultRetryValue;
                        contactWaitingValue.BackColor = Properties.Settings.Default.BackColor;
                        longestWaitingValue.BackColor = Properties.Settings.Default.BackColor;
                    }

                    //passes control to UI thread to update text boxes
                    updateQueueDisplay(theAnswer[0], theAnswer[1]);

                    await Task.Delay(retryValueMs);
                    
                }
                //TODO: Add specific exception handling
                //This should handle any network or transport layer errors, although not very cleanly
                catch (WebException ex)
                {
                    //user's gotta know what happened
                    //we don't exactly tell them - just make shit red
                    contactWaitingValue.BackColor = Properties.Settings.Default.ErrorColor;
                    longestWaitingValue.BackColor = Properties.Settings.Default.ErrorColor;

                    // get the error state handling in place 
                    isErrorState = true;
                }

                catch (OperationCanceledException)
                {
                    //Just in case
                    MessageBox.Show("Interrupt!\n");
                    
                    Thread.CurrentThread.Interrupt();
                    return;
                }

                finally
                {
                    //clean up
                    wc?.Dispose();
                }
            }
        }

        /// <summary>
        /// Format milliseconds as human readable
        /// </summary>
        /// <param name="ms">Number of Milliseonds</param>
        /// <returns>MM:SS of the max wait time</returns>
        private string humanReadableMinutesSeconds(int ms)
        {
            if (ms > 3600000) { return TimeSpan.FromMilliseconds(ms).ToString(@"h\:mm\:ss"); }
            else { return TimeSpan.FromMilliseconds(ms).ToString(@"mm\:ss"); }
        }

        /// <summary>
        /// The meat - parses JSON data to get the stuff we care about
        /// </summary>
        /// <param name="raw">String containing the raw JSON  data</param>
        /// <returns>Number of calls and the longest waiting call</returns>
        private int[] parseJSONData(string raw)
        {
            //call count
            int count = 0;

            //longest waiting call in ms
            //TODO: Longest call of the day
            int longestWaiting = 0;
            int longestWaitingTemp;
            

            //serialize the raw JSON data and shove it in a dynamic for now
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(raw);

            //Iterate through the queues and pull out the data we want
            for (int x = 0; x < item.Length; x++)
            {
                //you'll need to read the schema 
                //i'm not happy about it either
                count += item[x]["VoiceIAQStats"]["nWaitingContacts"];
                longestWaitingTemp = item[x]["VoiceIAQStats"]["longestCurrentlyWaitingDuration"];
               

                //We only care about this if it's longer than the others
                if (longestWaitingTemp > longestWaiting)
                {
                    longestWaiting = longestWaitingTemp;
                }

            }

            //return data
            return new int[] { count, longestWaiting };
        }

        /// <summary>
        /// Necessary because threading
        /// </summary>
        /// <param name="contacts">Number of calls in q</param>
        /// <param name="longest">Older call in q wait time</param>
        public void updateQueueDisplay(int contacts, int longest)
        {
            //Checks if we're in the right thread (we arent)
            if (InvokeRequired)
            {
                //Runs us up the call chain - we need a window
                this.Invoke(new Action<int, int>(updateQueueDisplay), new object[] { contacts, longest });
                return;
            }

            //Update text when we're in the right thread
            contactWaitingValue.Text = contacts.ToString();
            longestWaitingValue.Text = humanReadableMinutesSeconds(longest);

           

            if (maxCalls < contacts)
                
            {
                maxCalls = contacts;
                label1.Text = label1.Text.Substring(0, label1.Text.IndexOf(':')+1) + "(" + contacts + ")";
              
               
            }

            //"oh, this won't be so bad"
            if (maxWaiting < longest)
            {
                maxWaiting = longest;
               
               label2.Text = label2.Text.Substring(0, label2.Text.IndexOf(':')+1) + "(" + humanReadableMinutesSeconds(longest) + ")";
               
                    

            }
        }

        private void MiniQueueWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            //REMOVE THIS BEFORE DEPLOYMENT
            Properties.Settings.Default.Reset();
            Application.Exit();

        }

        private void MiniQueueWindow_Resize(object sender, EventArgs e)
        {/*
            if (this.Size.Height < 159)
            {
                contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, this.Size.Height / 5, contactWaitingValue.Font.Style);
                longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, this.Size.Height / 5, longestWaitingValue.Font.Style);

                label1.Hide();
                label2.Hide();
            }
            else
            {
                if (!label1.Visible && !label2.Visible) { label1.Show(); label2.Show(); }
                contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, this.Size.Height / 3, contactWaitingValue.Font.Style);
                longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, this.Size.Height / 3, longestWaitingValue.Font.Style);
            }
            
            */
            //Size s = TextRenderer.MeasureText(this.contactWaitingValue.Text, this.contactWaitingValue.Font);
            //contactWaitingValue.Size = s;

            //Console.WriteLine(this.Size + "; Font: " + contactWaitingValue.Font.Size);
            
        }

        private void contactWaitingValue_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SettingsDialog s = new SettingsDialog(this);
            s.ShowDialog();

        }

        public void updateAndPropegateSettings()
        {

            //For some reason, you need to "tickle" the background color of
            //read only text boxes before you can actually changes the forecolor (text color)
            //jesus.
            //Thanks stack overflow
            contactWaitingValue.BackColor = Properties.Settings.Default.BackColor;
            longestWaitingValue.BackColor = Properties.Settings.Default.BackColor;
            contactWaitingValue.ForeColor = Properties.Settings.Default.TextColor;
            longestWaitingValue.ForeColor = Properties.Settings.Default.TextColor;

            retryValueMs = Properties.Settings.Default.UpdateInterval * 1000;

            resizeWindowToPreset();
        }

        public void resizeWindowToPreset()
        {
            maxCalls = 0;
            maxWaiting = 0;
            switch (Properties.Settings.Default.SizeMode)
            {
                case "Very Small":
                    this.Size = this.MaximumSize = this.MaximumSize = new Size(216, 107);
                    this.contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, 24);
                    this.longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, 24);
                    this.contactWaitingValue.Location = new Point(10, 25);
                    this.longestWaitingValue.Location = new Point(45, 25);
                    this.label1.Text = "C.W.:";
                    this.label2.Text = "L.W.:";
                    this.label2.Location = new Point(100, 9);
                    
                    //label1.Hide();
                    //label2.Hide();
                    break;
                case "Small":
                    this.Size = this.MaximumSize = this.MaximumSize = new Size(280, 140);
                    this.contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, 48);
                    this.longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, 48);
                    this.label2.Location = new Point(100, 9);
                    break;
                case "Medium":
                    this.Size = this.MaximumSize = this.MaximumSize = new Size(339, 159);
                    this.contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, 48);
                    this.longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, 48);
                    this.contactWaitingValue.Location = new Point(15,33);
                    this.longestWaitingValue.Location = new Point(106,33);
                    this.label1.Text = "Contacts Waiting:";
                    this.label2.Text = "Longest Waiting:";
                    this.label2.Location = new Point(165,9);
                    if (!label1.Visible) { label1.Show(); }
                    if (!label2.Visible) { label2.Show(); }
                    break;
                case "Large":
                    this.Size = new Size(339, 159);
                    this.contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, 48);
                    this.longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, 48);
                    break;
                    
                case "Very Large":
                    this.Size = new Size(1356, 636);
                    this.contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, 192);
                    this.longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, 192);
                    break;
            }

            if (Properties.Settings.Default.HideTitleBar)
            { this.FormBorderStyle = FormBorderStyle.None;}
            else { this.FormBorderStyle = FormBorderStyle.Sizable; }
        }

        private void MiniQueueWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (Properties.Settings.Default.HideTitleBar)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            
        }

        
    }
}


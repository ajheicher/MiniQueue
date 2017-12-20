using System;
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
        const double widthRatio = 113;
        const double heightRatio = 53;

        const int WM_SIZING = 0x214;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;
        const int WMSZ_BOTTOM = 6;

        string maxCalls;
        string maxWaiting;

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public string credentials;
        public MiniQueueWindow()
        {
            InitializeComponent();
            maxCalls = "0";
            maxWaiting = "00:00"; 
        }

        //this is some scandalous shit

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
        }

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
                try
                {
                    wc = new WebClient();

                    //Authentication may not actually be necesary
                    //wc.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);

                    //update the queue
                    

                    //TODO: your data structures are bad and you should feel bad
                    int[] theAnswer;

                    //Releases the current thread back to windows form
                    Application.DoEvents();

                    //TODO: move to app.config
                    theAnswer = parseJSONData(wc.DownloadString("http://uccxpub01.philorch.org:9080/realtime/VoiceIAQStats"));

                    //obtains and formats miniqueue data
                    string quantity = theAnswer[0].ToString();
                    string time = humanReadableMinutesSeconds(theAnswer[1]);

                    //passes control to UI thread to update text boxes
                    updateQueueDisplay(quantity, time);

                    //5 second update interval
                    await Task.Delay(5000);
                    
                }
                //TODO: Add specific exception handling
                //This should handle any network or transport layer errors, although not very cleanly
                catch (WebException ex)
                {
                    //user's gotta know what happened
                    MessageBox.Show(ex.Message + "\nRetrying...");
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
        /// We're breaking this out into a separate method because reasons
        /// TODO: Build in handling for edge cases
        /// >What if we have wait times over an hour
        /// </summary>
        /// <param name="ms">Number of Milliseonds</param>
        /// <returns>MM:SS of the max wait time</returns>
        private string humanReadableMinutesSeconds(int ms)
        {
            //Creates a timespan object, formats as a string, returns, and dumbs the object
            return TimeSpan.FromMilliseconds(ms).ToString(@"mm\:ss");
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
        public void updateQueueDisplay(string contacts, string longest)
        {
            //Checks if we're in the right thread (we arent)
            if (InvokeRequired)
            {
                //Runs us up the call chain - we need a window
                this.Invoke(new Action<string, string>(updateQueueDisplay), new object[] { contacts, longest });
                return;
            }

            //Update text when we're in the right thread
            contactWaitingValue.Text = contacts;
            longestWaitingValue.Text = longest;

            //"oh, this won't be so bad"
            if (Convert.ToInt32(maxCalls) < Convert.ToInt32(contacts)) { label1.Text = "Contacts Waiting (" + contacts + ")"; maxCalls = contacts; }

            //oh no
            if (Convert.ToInt32(maxWaiting.Substring(0, maxWaiting.IndexOf(":"))) <
                Convert.ToInt32(longest.Substring(0,longest.IndexOf(":")) ))
            {
                maxWaiting = longest;
                label2.Text = "Longest Waiting (" + longest + ")";
                //maxLongestWaiting.Text = longest;
            }

            //OH NO
            else if (Convert.ToInt32(maxWaiting.Substring(0, maxWaiting.IndexOf(":"))) ==
                Convert.ToInt32(longest.Substring(0, longest.IndexOf(":"))) && 
                (Convert.ToInt32(maxWaiting.Substring(maxWaiting.IndexOf(":") + 1)) <
                Convert.ToInt32(longest.Substring(longest.IndexOf(":") + 1))))
                
            {
                label2.Text = "Longest Waiting (" + longest + ")";
                maxWaiting = longest;
                //maxLongestWaiting.Text = longest;
            }
           

        }

        private void MiniQueueWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }

        private void MiniQueueWindow_Resize(object sender, EventArgs e)
        {
            contactWaitingValue.Font = new Font(contactWaitingValue.Font.FontFamily, this.Size.Height / 3, contactWaitingValue.Font.Style);
            longestWaitingValue.Font = new Font(longestWaitingValue.Font.FontFamily, this.Size.Height / 3, longestWaitingValue.Font.Style);

            Size s = TextRenderer.MeasureText(this.contactWaitingValue.Text, this.contactWaitingValue.Font);
            contactWaitingValue.Size = s;

            
        }

        private void contactWaitingValue_TextChanged(object sender, EventArgs e)
        {
            

        }

       
    }
}


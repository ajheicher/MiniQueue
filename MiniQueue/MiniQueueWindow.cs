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
        public string credentials;
        public MiniQueueWindow(string uc)
        {
            InitializeComponent();
            credentials = uc;
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
            //TODO: wrap this nonsense in a try-catch block
            //do this stuff first so we don't repeat ourselves

            WebClient wc = null;

            try
            {
                wc = new WebClient();

                //Authentication may not actually be necesary
                wc.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);

                //update the queue
                while (true)
                {

                    int[] theAnswer;

                    //Releases the current thread back to windows form
                    Application.DoEvents();

                    theAnswer = parseJSONData(wc.DownloadString("http://uccxpub01.philorch.org:9080/realtime/VoiceIAQStats"));

                    //obtains and formats miniqueue data
                    string quantity = theAnswer[0].ToString();
                    string time = humanReadableMinutesSeconds(theAnswer[1]);

                    Console.WriteLine("Lets mkae sure we're only doing this once per 5 seconds");

                    //passes control to UI thread to update text boxes
                    updateQueue(quantity, time);



                    //5 second update interval
                    await Task.Delay(5000);
                }
            }
            //TODO: Add specific exception handling
            //This should handle any protocol errors, although not very cleanly
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (OperationCanceledException)
            {
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
        /// <param name="value">Value</param>
        public void updateQueue(string contacts, string longest)
        {
            //Checks if we're in the right thread (we arent)
            if (InvokeRequired)
            {
                //Runs us up the call chain - we need a window
                this.Invoke(new Action<string, string>(updateQueue), new object[] { contacts, longest });
                return;
            }

            //Update text
            contactWaitingValue.Text = contacts;
            longestWaitingValue.Text = longest;

        }
    }
}


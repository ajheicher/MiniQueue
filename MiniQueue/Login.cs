using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniQueue
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Again, fix this, you heathan
            //TODO: get this in app.config or something
            string loginUsername = usernameTextBox.Text;
            string loginPassword = passwordTextBox.Text;

            //Create cred string 
            string ucCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(loginUsername + ":" + loginPassword));

            //Open main window
            this.Hide();
            MiniQueueWindow main = new MiniQueueWindow(ucCredentials);

            main.Show();


        }
    }
}

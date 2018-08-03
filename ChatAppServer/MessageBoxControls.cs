using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ChatAppServer
{
    public partial class MessageBoxControls : DevExpress.XtraEditors.XtraUserControl
    {
        /*MessageBoxButtons messageBoxButtons;
        MessageBoxIcon messageBoxIcon;*/
        int messageBoxButtons;
        int messageBoxIcon;
        public MessageBoxControls()
        {
            InitializeComponent();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (radioGroup1.SelectedIndex == 0)
                messageBoxButtons = MessageBoxButtons.OK;
            else if (radioGroup1.SelectedIndex == 1)
                messageBoxButtons = MessageBoxButtons.OKCancel;
            else if (radioGroup1.SelectedIndex == 5)
                messageBoxButtons = MessageBoxButtons.AbortRetryIgnore;
            else if (radioGroup1.SelectedIndex == 3)
                messageBoxButtons = MessageBoxButtons.YesNoCancel;
            else if (radioGroup1.SelectedIndex == 2)
                messageBoxButtons = MessageBoxButtons.YesNo;
            else if (radioGroup1.SelectedIndex == 4)
                messageBoxButtons = MessageBoxButtons.RetryCancel;*/

            if (radioGroup1.SelectedIndex == 0)
                messageBoxButtons = 0;
            else if (radioGroup1.SelectedIndex == 1)
                messageBoxButtons = 1;
            else if (radioGroup1.SelectedIndex == 5)
                messageBoxButtons = 5;
            else if (radioGroup1.SelectedIndex == 3)
                messageBoxButtons = 3;
            else if (radioGroup1.SelectedIndex == 2)
                messageBoxButtons = 2;
            else if (radioGroup1.SelectedIndex == 4)
                messageBoxButtons = 4;
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (radioGroup2.SelectedIndex == 3)
                messageBoxIcon = MessageBoxIcon.Asterisk;
            else if (radioGroup2.SelectedIndex == 6)
                messageBoxIcon = MessageBoxIcon.Error;
            else if (radioGroup2.SelectedIndex == 2)
                messageBoxIcon = MessageBoxIcon.Exclamation;
            else if (radioGroup2.SelectedIndex == 4)
                messageBoxIcon = MessageBoxIcon.Hand;
            else if (radioGroup2.SelectedIndex == 0)
                messageBoxIcon = MessageBoxIcon.Information;
            else if (radioGroup2.SelectedIndex == 1)
                messageBoxIcon = MessageBoxIcon.Question;
            else if (radioGroup2.SelectedIndex == 7)
                messageBoxIcon = MessageBoxIcon.Stop;
            else
                messageBoxIcon = MessageBoxIcon.Warning;*/

            if (radioGroup2.SelectedIndex == 3)
                messageBoxIcon = 3;
            else if (radioGroup2.SelectedIndex == 6)
                messageBoxIcon = 6;
            else if (radioGroup2.SelectedIndex == 2)
                messageBoxIcon = 2;
            else if (radioGroup2.SelectedIndex == 4)
                messageBoxIcon = 4;
            else if (radioGroup2.SelectedIndex == 0)
                messageBoxIcon = 0;
            else if (radioGroup2.SelectedIndex == 1)
                messageBoxIcon = 1;
            else if (radioGroup2.SelectedIndex == 7)
                messageBoxIcon = 7;
            else
                messageBoxIcon = 5;
        }

        public int MessageBoxButtons
        {
            get { return messageBoxButtons; }
        }

        public int MessageBoxIcon
        {
            get { return messageBoxIcon; }
        }

        public string MemoEdit1
        {
            get { return memoEdit1.Text; }
        }

        public string TitleTextEdit
        {
            get { return titleTextEdit.Text; }
        }
        
    }
}

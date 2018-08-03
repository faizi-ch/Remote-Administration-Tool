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
    public partial class XtraUserControl1 : DevExpress.XtraEditors.XtraUserControl
    {
        public XtraUserControl1()
        {
            InitializeComponent();
        }
        public string GetIP()
        {
            return ipTextEdit.Text;
        }
        public string GetPort()
        {
            return portTextEdit.Text;
        }
        
        public string GetName()
        {
            return nameTextEdit.Text;
        }
    }
}

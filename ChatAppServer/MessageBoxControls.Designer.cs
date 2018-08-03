namespace ChatAppServer
{
    partial class MessageBoxControls
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            this.titleTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.titleLabel = new DevExpress.XtraEditors.LabelControl();
            this.btnTypeLabel = new DevExpress.XtraEditors.LabelControl();
            this.iconTypeLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleTextEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(35, 80);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(46, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Message:";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(35, 99);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(163, 155);
            this.memoEdit1.TabIndex = 1;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(218, 43);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "OK"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "OK-Cancel"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Yes-No"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Yes-No-Cancel"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Retry-Cancel"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Abort-Retry-Ignore")});
            this.radioGroup1.Size = new System.Drawing.Size(244, 89);
            this.radioGroup1.TabIndex = 2;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // radioGroup2
            // 
            this.radioGroup2.Location = new System.Drawing.Point(218, 158);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Information"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Question"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Exclamation"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Asterisk"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Hand"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Warning"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Error"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Stop")});
            this.radioGroup2.Size = new System.Drawing.Size(244, 96);
            this.radioGroup2.TabIndex = 3;
            this.radioGroup2.SelectedIndexChanged += new System.EventHandler(this.radioGroup2_SelectedIndexChanged);
            // 
            // titleTextEdit
            // 
            this.titleTextEdit.Location = new System.Drawing.Point(35, 43);
            this.titleTextEdit.Name = "titleTextEdit";
            this.titleTextEdit.Size = new System.Drawing.Size(163, 20);
            this.titleTextEdit.TabIndex = 4;
            // 
            // titleLabel
            // 
            this.titleLabel.Location = new System.Drawing.Point(35, 24);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(87, 13);
            this.titleLabel.TabIndex = 5;
            this.titleLabel.Text = "MessageBox Title:";
            // 
            // btnTypeLabel
            // 
            this.btnTypeLabel.Location = new System.Drawing.Point(218, 24);
            this.btnTypeLabel.Name = "btnTypeLabel";
            this.btnTypeLabel.Size = new System.Drawing.Size(68, 13);
            this.btnTypeLabel.TabIndex = 6;
            this.btnTypeLabel.Text = "Buttons Type:";
            // 
            // iconTypeLabel
            // 
            this.iconTypeLabel.Location = new System.Drawing.Point(218, 139);
            this.iconTypeLabel.Name = "iconTypeLabel";
            this.iconTypeLabel.Size = new System.Drawing.Size(52, 13);
            this.iconTypeLabel.TabIndex = 7;
            this.iconTypeLabel.Text = "Icon Type:";
            // 
            // MessageBoxControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iconTypeLabel);
            this.Controls.Add(this.btnTypeLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleTextEdit);
            this.Controls.Add(this.radioGroup2);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.labelControl1);
            this.Name = "MessageBoxControls";
            this.Size = new System.Drawing.Size(476, 271);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleTextEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private DevExpress.XtraEditors.TextEdit titleTextEdit;
        private DevExpress.XtraEditors.LabelControl titleLabel;
        private DevExpress.XtraEditors.LabelControl btnTypeLabel;
        private DevExpress.XtraEditors.LabelControl iconTypeLabel;
    }
}

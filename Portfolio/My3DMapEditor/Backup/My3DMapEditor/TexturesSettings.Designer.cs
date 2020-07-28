namespace My3DMapEditor
{
    partial class TexturesSettings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonNone = new System.Windows.Forms.RadioButton();
			this.radioButtonDownSide = new System.Windows.Forms.RadioButton();
			this.radioButtonUpSide = new System.Windows.Forms.RadioButton();
			this.radioButtonLeftSide = new System.Windows.Forms.RadioButton();
			this.radioButtonBackSide = new System.Windows.Forms.RadioButton();
			this.radioButtonRightSide = new System.Windows.Forms.RadioButton();
			this.radioButtonFrontSide = new System.Windows.Forms.RadioButton();
			this.radioButtonAllSide = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.radioButtonNoOwn = new System.Windows.Forms.RadioButton();
			this.radioButtonScale = new System.Windows.Forms.RadioButton();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.buttonLeftDownYPlus = new System.Windows.Forms.Button();
			this.buttonLeftDownYMinus = new System.Windows.Forms.Button();
			this.textBoxScaleY = new System.Windows.Forms.TextBox();
			this.groupBoxY = new System.Windows.Forms.GroupBox();
			this.buttonYPlus = new System.Windows.Forms.Button();
			this.buttonYMinus = new System.Windows.Forms.Button();
			this.textBoxY = new System.Windows.Forms.TextBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.buttonRightDownXPlus = new System.Windows.Forms.Button();
			this.buttonRightDownXMinus = new System.Windows.Forms.Button();
			this.textBoxScaleX = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.comboBoxModeIncrement = new System.Windows.Forms.ComboBox();
			this.groupBoxX = new System.Windows.Forms.GroupBox();
			this.buttonXPlus = new System.Windows.Forms.Button();
			this.buttonXMinus = new System.Windows.Forms.Button();
			this.textBoxX = new System.Windows.Forms.TextBox();
			this.buttonApply = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.buttonMarks = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.extendedImageList1 = new My3DMapEditor.ExtendedImageList();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBoxY.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBoxX.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButtonNone);
			this.groupBox1.Controls.Add(this.radioButtonDownSide);
			this.groupBox1.Controls.Add(this.radioButtonUpSide);
			this.groupBox1.Controls.Add(this.radioButtonLeftSide);
			this.groupBox1.Controls.Add(this.radioButtonBackSide);
			this.groupBox1.Controls.Add(this.radioButtonRightSide);
			this.groupBox1.Controls.Add(this.radioButtonFrontSide);
			this.groupBox1.Controls.Add(this.radioButtonAllSide);
			this.groupBox1.Location = new System.Drawing.Point(9, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(112, 209);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sides of primitive";
			// 
			// radioButtonNone
			// 
			this.radioButtonNone.AutoSize = true;
			this.radioButtonNone.Checked = true;
			this.radioButtonNone.Location = new System.Drawing.Point(6, 19);
			this.radioButtonNone.Name = "radioButtonNone";
			this.radioButtonNone.Size = new System.Drawing.Size(51, 17);
			this.radioButtonNone.TabIndex = 4;
			this.radioButtonNone.TabStop = true;
			this.radioButtonNone.Text = "None";
			this.radioButtonNone.UseVisualStyleBackColor = true;
			this.radioButtonNone.CheckedChanged += new System.EventHandler(this.radioButtonNone_CheckedChanged);
			// 
			// radioButtonDownSide
			// 
			this.radioButtonDownSide.AutoSize = true;
			this.radioButtonDownSide.Location = new System.Drawing.Point(6, 180);
			this.radioButtonDownSide.Name = "radioButtonDownSide";
			this.radioButtonDownSide.Size = new System.Drawing.Size(74, 17);
			this.radioButtonDownSide.TabIndex = 7;
			this.radioButtonDownSide.Text = "DownSide";
			this.radioButtonDownSide.UseVisualStyleBackColor = true;
			this.radioButtonDownSide.CheckedChanged += new System.EventHandler(this.radioButtonDownSide_CheckedChanged);
			// 
			// radioButtonUpSide
			// 
			this.radioButtonUpSide.AutoSize = true;
			this.radioButtonUpSide.Location = new System.Drawing.Point(6, 157);
			this.radioButtonUpSide.Name = "radioButtonUpSide";
			this.radioButtonUpSide.Size = new System.Drawing.Size(60, 17);
			this.radioButtonUpSide.TabIndex = 6;
			this.radioButtonUpSide.Text = "UpSide";
			this.radioButtonUpSide.UseVisualStyleBackColor = true;
			this.radioButtonUpSide.CheckedChanged += new System.EventHandler(this.radioButtonUpSide_CheckedChanged);
			// 
			// radioButtonLeftSide
			// 
			this.radioButtonLeftSide.AutoSize = true;
			this.radioButtonLeftSide.Location = new System.Drawing.Point(6, 134);
			this.radioButtonLeftSide.Name = "radioButtonLeftSide";
			this.radioButtonLeftSide.Size = new System.Drawing.Size(64, 17);
			this.radioButtonLeftSide.TabIndex = 5;
			this.radioButtonLeftSide.Text = "LeftSide";
			this.radioButtonLeftSide.UseVisualStyleBackColor = true;
			this.radioButtonLeftSide.CheckedChanged += new System.EventHandler(this.radioButtonLeftSide_CheckedChanged);
			// 
			// radioButtonBackSide
			// 
			this.radioButtonBackSide.AutoSize = true;
			this.radioButtonBackSide.Location = new System.Drawing.Point(6, 111);
			this.radioButtonBackSide.Name = "radioButtonBackSide";
			this.radioButtonBackSide.Size = new System.Drawing.Size(71, 17);
			this.radioButtonBackSide.TabIndex = 4;
			this.radioButtonBackSide.Text = "BackSide";
			this.radioButtonBackSide.UseVisualStyleBackColor = true;
			this.radioButtonBackSide.CheckedChanged += new System.EventHandler(this.radioButtonBackSide_CheckedChanged);
			// 
			// radioButtonRightSide
			// 
			this.radioButtonRightSide.AutoSize = true;
			this.radioButtonRightSide.Location = new System.Drawing.Point(6, 88);
			this.radioButtonRightSide.Name = "radioButtonRightSide";
			this.radioButtonRightSide.Size = new System.Drawing.Size(71, 17);
			this.radioButtonRightSide.TabIndex = 3;
			this.radioButtonRightSide.Text = "RightSide";
			this.radioButtonRightSide.UseVisualStyleBackColor = true;
			this.radioButtonRightSide.CheckedChanged += new System.EventHandler(this.radioButtonRightSide_CheckedChanged);
			// 
			// radioButtonFrontSide
			// 
			this.radioButtonFrontSide.AutoSize = true;
			this.radioButtonFrontSide.Location = new System.Drawing.Point(6, 65);
			this.radioButtonFrontSide.Name = "radioButtonFrontSide";
			this.radioButtonFrontSide.Size = new System.Drawing.Size(70, 17);
			this.radioButtonFrontSide.TabIndex = 2;
			this.radioButtonFrontSide.Text = "FrontSide";
			this.radioButtonFrontSide.UseVisualStyleBackColor = true;
			this.radioButtonFrontSide.CheckedChanged += new System.EventHandler(this.radioButtonFrontSide_CheckedChanged);
			// 
			// radioButtonAllSide
			// 
			this.radioButtonAllSide.AutoSize = true;
			this.radioButtonAllSide.Location = new System.Drawing.Point(6, 42);
			this.radioButtonAllSide.Name = "radioButtonAllSide";
			this.radioButtonAllSide.Size = new System.Drawing.Size(62, 17);
			this.radioButtonAllSide.TabIndex = 1;
			this.radioButtonAllSide.Text = "AllSides";
			this.radioButtonAllSide.UseVisualStyleBackColor = true;
			this.radioButtonAllSide.CheckedChanged += new System.EventHandler(this.radioButtonAllSide_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.groupBox6);
			this.groupBox2.Controls.Add(this.groupBox4);
			this.groupBox2.Controls.Add(this.groupBoxY);
			this.groupBox2.Controls.Add(this.groupBox5);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this.groupBoxX);
			this.groupBox2.Location = new System.Drawing.Point(9, 227);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(153, 375);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Parameters";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.radioButtonNoOwn);
			this.groupBox6.Controls.Add(this.radioButtonScale);
			this.groupBox6.Location = new System.Drawing.Point(6, 19);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(139, 64);
			this.groupBox6.TabIndex = 2;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "NameOfAction";
			// 
			// radioButtonNoOwn
			// 
			this.radioButtonNoOwn.AutoSize = true;
			this.radioButtonNoOwn.Location = new System.Drawing.Point(6, 41);
			this.radioButtonNoOwn.Name = "radioButtonNoOwn";
			this.radioButtonNoOwn.Size = new System.Drawing.Size(89, 17);
			this.radioButtonNoOwn.TabIndex = 1;
			this.radioButtonNoOwn.Text = "Размножить";
			this.radioButtonNoOwn.UseVisualStyleBackColor = true;
			this.radioButtonNoOwn.CheckedChanged += new System.EventHandler(this.radioButtonNoOwn_CheckedChanged);
			// 
			// radioButtonScale
			// 
			this.radioButtonScale.AutoSize = true;
			this.radioButtonScale.Checked = true;
			this.radioButtonScale.Location = new System.Drawing.Point(6, 18);
			this.radioButtonScale.Name = "radioButtonScale";
			this.radioButtonScale.Size = new System.Drawing.Size(77, 17);
			this.radioButtonScale.TabIndex = 0;
			this.radioButtonScale.TabStop = true;
			this.radioButtonScale.Text = "Растянуть";
			this.radioButtonScale.UseVisualStyleBackColor = true;
			this.radioButtonScale.CheckedChanged += new System.EventHandler(this.radioButtonScale_CheckedChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.buttonLeftDownYPlus);
			this.groupBox4.Controls.Add(this.buttonLeftDownYMinus);
			this.groupBox4.Controls.Add(this.textBoxScaleY);
			this.groupBox4.Location = new System.Drawing.Point(6, 315);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(137, 54);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Scale_Y";
			// 
			// buttonLeftDownYPlus
			// 
			this.buttonLeftDownYPlus.Location = new System.Drawing.Point(105, 19);
			this.buttonLeftDownYPlus.Name = "buttonLeftDownYPlus";
			this.buttonLeftDownYPlus.Size = new System.Drawing.Size(25, 20);
			this.buttonLeftDownYPlus.TabIndex = 6;
			this.buttonLeftDownYPlus.Text = "+";
			this.buttonLeftDownYPlus.UseVisualStyleBackColor = true;
			this.buttonLeftDownYPlus.Click += new System.EventHandler(this.buttonLeftDownYPlus_Click);
			// 
			// buttonLeftDownYMinus
			// 
			this.buttonLeftDownYMinus.Location = new System.Drawing.Point(6, 19);
			this.buttonLeftDownYMinus.Name = "buttonLeftDownYMinus";
			this.buttonLeftDownYMinus.Size = new System.Drawing.Size(25, 20);
			this.buttonLeftDownYMinus.TabIndex = 0;
			this.buttonLeftDownYMinus.Text = "-";
			this.buttonLeftDownYMinus.UseVisualStyleBackColor = true;
			this.buttonLeftDownYMinus.Click += new System.EventHandler(this.buttonLeftDownYMinus_Click);
			// 
			// textBoxScaleY
			// 
			this.textBoxScaleY.Location = new System.Drawing.Point(30, 19);
			this.textBoxScaleY.Name = "textBoxScaleY";
			this.textBoxScaleY.ReadOnly = true;
			this.textBoxScaleY.Size = new System.Drawing.Size(73, 20);
			this.textBoxScaleY.TabIndex = 5;
			this.textBoxScaleY.Text = "1,0";
			// 
			// groupBoxY
			// 
			this.groupBoxY.Controls.Add(this.buttonYPlus);
			this.groupBoxY.Controls.Add(this.buttonYMinus);
			this.groupBoxY.Controls.Add(this.textBoxY);
			this.groupBoxY.Location = new System.Drawing.Point(6, 195);
			this.groupBoxY.Name = "groupBoxY";
			this.groupBoxY.Size = new System.Drawing.Size(137, 54);
			this.groupBoxY.TabIndex = 5;
			this.groupBoxY.TabStop = false;
			this.groupBoxY.Text = "Pos_Y";
			// 
			// buttonYPlus
			// 
			this.buttonYPlus.Location = new System.Drawing.Point(105, 19);
			this.buttonYPlus.Name = "buttonYPlus";
			this.buttonYPlus.Size = new System.Drawing.Size(25, 20);
			this.buttonYPlus.TabIndex = 6;
			this.buttonYPlus.Text = "+";
			this.buttonYPlus.UseVisualStyleBackColor = true;
			this.buttonYPlus.Click += new System.EventHandler(this.buttonYPlus_Click);
			// 
			// buttonYMinus
			// 
			this.buttonYMinus.Location = new System.Drawing.Point(6, 19);
			this.buttonYMinus.Name = "buttonYMinus";
			this.buttonYMinus.Size = new System.Drawing.Size(25, 20);
			this.buttonYMinus.TabIndex = 0;
			this.buttonYMinus.Text = "-";
			this.buttonYMinus.UseVisualStyleBackColor = true;
			this.buttonYMinus.Click += new System.EventHandler(this.buttonYMinus_Click);
			// 
			// textBoxY
			// 
			this.textBoxY.Location = new System.Drawing.Point(30, 19);
			this.textBoxY.Name = "textBoxY";
			this.textBoxY.ReadOnly = true;
			this.textBoxY.Size = new System.Drawing.Size(73, 20);
			this.textBoxY.TabIndex = 5;
			this.textBoxY.Text = "0";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.buttonRightDownXPlus);
			this.groupBox5.Controls.Add(this.buttonRightDownXMinus);
			this.groupBox5.Controls.Add(this.textBoxScaleX);
			this.groupBox5.Location = new System.Drawing.Point(6, 255);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(137, 54);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Scale_X";
			// 
			// buttonRightDownXPlus
			// 
			this.buttonRightDownXPlus.Location = new System.Drawing.Point(105, 19);
			this.buttonRightDownXPlus.Name = "buttonRightDownXPlus";
			this.buttonRightDownXPlus.Size = new System.Drawing.Size(25, 20);
			this.buttonRightDownXPlus.TabIndex = 6;
			this.buttonRightDownXPlus.Text = "+";
			this.buttonRightDownXPlus.UseVisualStyleBackColor = true;
			this.buttonRightDownXPlus.Click += new System.EventHandler(this.buttonRightDownXPlus_Click);
			// 
			// buttonRightDownXMinus
			// 
			this.buttonRightDownXMinus.Location = new System.Drawing.Point(6, 19);
			this.buttonRightDownXMinus.Name = "buttonRightDownXMinus";
			this.buttonRightDownXMinus.Size = new System.Drawing.Size(25, 20);
			this.buttonRightDownXMinus.TabIndex = 0;
			this.buttonRightDownXMinus.Text = "-";
			this.buttonRightDownXMinus.UseVisualStyleBackColor = true;
			this.buttonRightDownXMinus.Click += new System.EventHandler(this.buttonRightDownXMinus_Click);
			// 
			// textBoxScaleX
			// 
			this.textBoxScaleX.Location = new System.Drawing.Point(30, 19);
			this.textBoxScaleX.Name = "textBoxScaleX";
			this.textBoxScaleX.ReadOnly = true;
			this.textBoxScaleX.Size = new System.Drawing.Size(73, 20);
			this.textBoxScaleX.TabIndex = 5;
			this.textBoxScaleX.Text = "1,0";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.comboBoxModeIncrement);
			this.groupBox3.Location = new System.Drawing.Point(6, 86);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(124, 43);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Mode Increment";
			// 
			// comboBoxModeIncrement
			// 
			this.comboBoxModeIncrement.FormattingEnabled = true;
			this.comboBoxModeIncrement.Items.AddRange(new object[] {
            "0,001",
            "0,01",
            "0,1",
            "1,0"});
			this.comboBoxModeIncrement.Location = new System.Drawing.Point(6, 16);
			this.comboBoxModeIncrement.Name = "comboBoxModeIncrement";
			this.comboBoxModeIncrement.Size = new System.Drawing.Size(112, 21);
			this.comboBoxModeIncrement.TabIndex = 0;
			this.comboBoxModeIncrement.TextChanged += new System.EventHandler(this.comboBoxModeIncrement_TextChanged);
			// 
			// groupBoxX
			// 
			this.groupBoxX.Controls.Add(this.buttonXPlus);
			this.groupBoxX.Controls.Add(this.buttonXMinus);
			this.groupBoxX.Controls.Add(this.textBoxX);
			this.groupBoxX.Location = new System.Drawing.Point(6, 135);
			this.groupBoxX.Name = "groupBoxX";
			this.groupBoxX.Size = new System.Drawing.Size(137, 54);
			this.groupBoxX.TabIndex = 4;
			this.groupBoxX.TabStop = false;
			this.groupBoxX.Text = "Pos_X";
			// 
			// buttonXPlus
			// 
			this.buttonXPlus.Location = new System.Drawing.Point(105, 19);
			this.buttonXPlus.Name = "buttonXPlus";
			this.buttonXPlus.Size = new System.Drawing.Size(25, 20);
			this.buttonXPlus.TabIndex = 6;
			this.buttonXPlus.Text = "+";
			this.buttonXPlus.UseVisualStyleBackColor = true;
			this.buttonXPlus.Click += new System.EventHandler(this.buttonXPlus_Click);
			// 
			// buttonXMinus
			// 
			this.buttonXMinus.Location = new System.Drawing.Point(6, 19);
			this.buttonXMinus.Name = "buttonXMinus";
			this.buttonXMinus.Size = new System.Drawing.Size(25, 20);
			this.buttonXMinus.TabIndex = 0;
			this.buttonXMinus.Text = "-";
			this.buttonXMinus.UseVisualStyleBackColor = true;
			this.buttonXMinus.Click += new System.EventHandler(this.buttonXMinus_Click);
			// 
			// textBoxX
			// 
			this.textBoxX.Location = new System.Drawing.Point(30, 19);
			this.textBoxX.Name = "textBoxX";
			this.textBoxX.ReadOnly = true;
			this.textBoxX.Size = new System.Drawing.Size(73, 20);
			this.textBoxX.TabIndex = 5;
			this.textBoxX.Text = "0";
			// 
			// buttonApply
			// 
			this.buttonApply.Location = new System.Drawing.Point(59, 607);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(52, 24);
			this.buttonApply.TabIndex = 3;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBox1.Location = new System.Drawing.Point(6, 607);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(52, 24);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "HideSel";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// buttonMarks
			// 
			this.buttonMarks.BackColor = System.Drawing.Color.LightBlue;
			this.buttonMarks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonMarks.Location = new System.Drawing.Point(725, 12);
			this.buttonMarks.Name = "buttonMarks";
			this.buttonMarks.Size = new System.Drawing.Size(10, 628);
			this.buttonMarks.TabIndex = 5;
			this.buttonMarks.UseVisualStyleBackColor = false;
			this.buttonMarks.Click += new System.EventHandler(this.buttonMarks_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(736, 12);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(242, 627);
			this.richTextBox1.TabIndex = 6;
			this.richTextBox1.Text = "";
			// 
			// extendedImageList1
			// 
			this.extendedImageList1.AutoScroll = true;
			this.extendedImageList1.Location = new System.Drawing.Point(168, 12);
			this.extendedImageList1.Name = "extendedImageList1";
			this.extendedImageList1.Size = new System.Drawing.Size(555, 627);
			this.extendedImageList1.TabIndex = 2;
			// 
			// TexturesSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(980, 643);
			this.ControlBox = false;
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.buttonMarks);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.extendedImageList1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.KeyPreview = true;
			this.Name = "TexturesSettings";
			this.Text = "TexturesSettings";
			this.Deactivate += new System.EventHandler(this.TexturesSettings_Deactivate);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TexturesSettings_KeyDown);
			this.Load += new System.EventHandler(this.TexturesSettings_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBoxY.ResumeLayout(false);
			this.groupBoxY.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBoxX.ResumeLayout(false);
			this.groupBoxX.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDownSide;
        private System.Windows.Forms.RadioButton radioButtonUpSide;
        private System.Windows.Forms.RadioButton radioButtonLeftSide;
        private System.Windows.Forms.RadioButton radioButtonBackSide;
        private System.Windows.Forms.RadioButton radioButtonRightSide;
        private System.Windows.Forms.RadioButton radioButtonFrontSide;
        private System.Windows.Forms.RadioButton radioButtonAllSide;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxModeIncrement;
        private System.Windows.Forms.RadioButton radioButtonNoOwn;
        public System.Windows.Forms.RadioButton radioButtonScale;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonLeftDownYPlus;
        private System.Windows.Forms.Button buttonLeftDownYMinus;
        private System.Windows.Forms.TextBox textBoxScaleY;
        private System.Windows.Forms.GroupBox groupBoxY;
        private System.Windows.Forms.Button buttonYPlus;
        private System.Windows.Forms.Button buttonYMinus;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonRightDownXPlus;
        private System.Windows.Forms.Button buttonRightDownXMinus;
        private System.Windows.Forms.TextBox textBoxScaleX;
        private System.Windows.Forms.GroupBox groupBoxX;
        private System.Windows.Forms.Button buttonXPlus;
        private System.Windows.Forms.Button buttonXMinus;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.GroupBox groupBox6;
        private ExtendedImageList extendedImageList1;
        private System.Windows.Forms.Button buttonApply;
        public System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonMarks;
        private System.Windows.Forms.RichTextBox richTextBox1;

    }
}
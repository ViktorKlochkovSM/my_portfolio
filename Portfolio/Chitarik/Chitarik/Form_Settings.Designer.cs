namespace Chitarik
{
    partial class Form_Settings
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
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Apply_BTN = new System.Windows.Forms.Button();
            this.Cancel_BTN = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.accent_UC1 = new Chitarik.Accent_UC();
            this.ShowAccent_CB = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CountSlov_LBL = new System.Windows.Forms.Label();
            this.Slova_LB = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CountCategories_LBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Categories_LB = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Apply_BTN
            // 
            this.Apply_BTN.Location = new System.Drawing.Point(19, 8);
            this.Apply_BTN.Name = "Apply_BTN";
            this.Apply_BTN.Size = new System.Drawing.Size(75, 23);
            this.Apply_BTN.TabIndex = 1;
            this.Apply_BTN.Text = "Применить";
            this.Apply_BTN.UseVisualStyleBackColor = true;
            this.Apply_BTN.Click += new System.EventHandler(this.Apply_BTN_Click);
            // 
            // Cancel_BTN
            // 
            this.Cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_BTN.Location = new System.Drawing.Point(100, 8);
            this.Cancel_BTN.Name = "Cancel_BTN";
            this.Cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BTN.TabIndex = 2;
            this.Cancel_BTN.Text = "Отмена";
            this.Cancel_BTN.UseVisualStyleBackColor = true;
            this.Cancel_BTN.Click += new System.EventHandler(this.Cancel_BTN_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1395, 634);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1387, 608);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Библиотека слов";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.accent_UC1);
            this.groupBox3.Controls.Add(this.ShowAccent_CB);
            this.groupBox3.Location = new System.Drawing.Point(455, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(916, 564);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройка ударения в слове";
            // 
            // accent_UC1
            // 
            this.accent_UC1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.accent_UC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accent_UC1.Location = new System.Drawing.Point(3, 16);
            this.accent_UC1.Name = "accent_UC1";
            this.accent_UC1.Size = new System.Drawing.Size(910, 545);
            this.accent_UC1.TabIndex = 6;
            // 
            // ShowAccent_CB
            // 
            this.ShowAccent_CB.AutoSize = true;
            this.ShowAccent_CB.Location = new System.Drawing.Point(192, 17);
            this.ShowAccent_CB.Name = "ShowAccent_CB";
            this.ShowAccent_CB.Size = new System.Drawing.Size(128, 17);
            this.ShowAccent_CB.TabIndex = 5;
            this.ShowAccent_CB.Text = "Ударение включено";
            this.ShowAccent_CB.UseVisualStyleBackColor = true;
            this.ShowAccent_CB.CheckedChanged += new System.EventHandler(this.ShowAccent_CB_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.CountSlov_LBL);
            this.groupBox2.Controls.Add(this.Slova_LB);
            this.groupBox2.Location = new System.Drawing.Point(213, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 564);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Слова из категории";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 532);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Всего слов в категории:";
            // 
            // CountSlov_LBL
            // 
            this.CountSlov_LBL.AutoSize = true;
            this.CountSlov_LBL.Location = new System.Drawing.Point(144, 532);
            this.CountSlov_LBL.Name = "CountSlov_LBL";
            this.CountSlov_LBL.Size = new System.Drawing.Size(29, 13);
            this.CountSlov_LBL.TabIndex = 1;
            this.CountSlov_LBL.Text = "label";
            // 
            // Slova_LB
            // 
            this.Slova_LB.FormattingEnabled = true;
            this.Slova_LB.Location = new System.Drawing.Point(6, 31);
            this.Slova_LB.Name = "Slova_LB";
            this.Slova_LB.Size = new System.Drawing.Size(218, 498);
            this.Slova_LB.TabIndex = 0;
            this.Slova_LB.SelectedIndexChanged += new System.EventHandler(this.Slova_LB_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CountCategories_LBL);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Categories_LB);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 564);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Категории";
            // 
            // CountCategories_LBL
            // 
            this.CountCategories_LBL.AutoSize = true;
            this.CountCategories_LBL.Location = new System.Drawing.Point(103, 532);
            this.CountCategories_LBL.Name = "CountCategories_LBL";
            this.CountCategories_LBL.Size = new System.Drawing.Size(35, 13);
            this.CountCategories_LBL.TabIndex = 2;
            this.CountCategories_LBL.Text = "label3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 532);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Всего категорий:";
            // 
            // Categories_LB
            // 
            this.Categories_LB.FormattingEnabled = true;
            this.Categories_LB.Location = new System.Drawing.Point(6, 31);
            this.Categories_LB.Name = "Categories_LB";
            this.Categories_LB.Size = new System.Drawing.Size(183, 498);
            this.Categories_LB.TabIndex = 0;
            this.Categories_LB.SelectedIndexChanged += new System.EventHandler(this.Categories_LB_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Apply_BTN);
            this.panel1.Controls.Add(this.Cancel_BTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 634);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1395, 41);
            this.panel1.TabIndex = 7;
            // 
            // Form_Settings
            // 
            this.AcceptButton = this.Apply_BTN;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_BTN;
            this.ClientSize = new System.Drawing.Size(1395, 675);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.IsMdiContainer = true;
            this.Name = "Form_Settings";
            this.Text = "Настройки ";
            this.Load += new System.EventHandler(this.Form_Settings_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Settings_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Apply_BTN;
        private System.Windows.Forms.Button Cancel_BTN;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox Categories_LB;
        private System.Windows.Forms.ListBox Slova_LB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CountSlov_LBL;
        private System.Windows.Forms.Label CountCategories_LBL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ShowAccent_CB;
        private System.Windows.Forms.Panel panel1;
        private Accent_UC accent_UC1;
    }
}
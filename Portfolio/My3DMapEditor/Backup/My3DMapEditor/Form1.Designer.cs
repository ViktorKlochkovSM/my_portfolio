namespace My3DMapEditor
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxSnap = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonGridMinus = new System.Windows.Forms.Button();
            this.buttonGridPlus = new System.Windows.Forms.Button();
            this.labelGridSize = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labelLength = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBoxPlayerBlue = new System.Windows.Forms.CheckBox();
            this.checkBoxPlayerRed = new System.Windows.Forms.CheckBox();
            this.checkBoxTextures = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectionMode = new System.Windows.Forms.CheckBox();
            this.checkBoxPointer = new System.Windows.Forms.CheckBox();
            this.checkBoxRect = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3D = new System.Windows.Forms.Panel();
            this.pictureBox3D = new System.Windows.Forms.PictureBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBoxTop = new System.Windows.Forms.PictureBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.pictureBoxRight = new System.Windows.Forms.PictureBox();
            this.panelFront = new System.Windows.Forms.Panel();
            this.pictureBoxFront = new System.Windows.Forms.PictureBox();
            this.timerTop = new System.Windows.Forms.Timer(this.components);
            this.timerRight = new System.Windows.Forms.Timer(this.components);
            this.timerFront = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer3D = new System.Windows.Forms.Timer(this.components);
            this.objectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3D.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3D)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).BeginInit();
            this.panelFront.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFront)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.checkBoxSnap);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 29);
            this.panel1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.panel1, "Hollow object [H]");
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(885, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(12, 13);
            this.panel11.TabIndex = 105;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(138, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 23);
            this.button2.TabIndex = 104;
            this.button2.Text = "Cerve";
            this.toolTip1.SetToolTip(this.button2, "Curve objectx\r\n[E]");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(72, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 103;
            this.button1.Text = "Hollow";
            this.toolTip1.SetToolTip(this.button1, "Hollow object to room\r\n[H]");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBoxSnap
            // 
            this.checkBoxSnap.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSnap.Checked = true;
            this.checkBoxSnap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSnap.Location = new System.Drawing.Point(5, 3);
            this.checkBoxSnap.Name = "checkBoxSnap";
            this.checkBoxSnap.Size = new System.Drawing.Size(61, 23);
            this.checkBoxSnap.TabIndex = 102;
            this.checkBoxSnap.TabStop = false;
            this.checkBoxSnap.Text = "Snap";
            this.checkBoxSnap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxSnap, "Snap\r\nEnabled or disabled snap\r\n[Ctrl+S]");
            this.checkBoxSnap.UseVisualStyleBackColor = true;
            this.checkBoxSnap.CheckedChanged += new System.EventHandler(this.checkBoxSnap_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(58, 664);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(842, 34);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.buttonGridMinus);
            this.panel3.Controls.Add(this.buttonGridPlus);
            this.panel3.Controls.Add(this.labelGridSize);
            this.panel3.Location = new System.Drawing.Point(6, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(135, 28);
            this.panel3.TabIndex = 105;
            // 
            // buttonGridMinus
            // 
            this.buttonGridMinus.Location = new System.Drawing.Point(3, 2);
            this.buttonGridMinus.Name = "buttonGridMinus";
            this.buttonGridMinus.Size = new System.Drawing.Size(30, 23);
            this.buttonGridMinus.TabIndex = 101;
            this.buttonGridMinus.TabStop = false;
            this.buttonGridMinus.Text = "#-";
            this.toolTip1.SetToolTip(this.buttonGridMinus, "Minus grid size -[-");
            this.buttonGridMinus.UseVisualStyleBackColor = true;
            this.buttonGridMinus.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGridPlus
            // 
            this.buttonGridPlus.Location = new System.Drawing.Point(34, 2);
            this.buttonGridPlus.Name = "buttonGridPlus";
            this.buttonGridPlus.Size = new System.Drawing.Size(30, 23);
            this.buttonGridPlus.TabIndex = 100;
            this.buttonGridPlus.TabStop = false;
            this.buttonGridPlus.Text = "#+";
            this.toolTip1.SetToolTip(this.buttonGridPlus, "Plus grid size -]-");
            this.buttonGridPlus.UseVisualStyleBackColor = true;
            this.buttonGridPlus.Click += new System.EventHandler(this.buttonGridPrus_Click);
            // 
            // labelGridSize
            // 
            this.labelGridSize.AutoSize = true;
            this.labelGridSize.Location = new System.Drawing.Point(70, 7);
            this.labelGridSize.Name = "labelGridSize";
            this.labelGridSize.Size = new System.Drawing.Size(26, 13);
            this.labelGridSize.TabIndex = 2;
            this.labelGridSize.Text = "# = ";
            this.toolTip1.SetToolTip(this.labelGridSize, "Current size of grid");
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.labelLength);
            this.panel5.Controls.Add(this.labelWidth);
            this.panel5.Controls.Add(this.labelHeight);
            this.panel5.Location = new System.Drawing.Point(636, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(203, 28);
            this.panel5.TabIndex = 104;
            this.toolTip1.SetToolTip(this.panel5, "Size of object");
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(147, 6);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(21, 13);
            this.labelLength.TabIndex = 104;
            this.labelLength.Text = "z : ";
            this.toolTip1.SetToolTip(this.labelLength, "Length of object");
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(3, 6);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(24, 13);
            this.labelWidth.TabIndex = 102;
            this.labelWidth.Text = "w : ";
            this.toolTip1.SetToolTip(this.labelWidth, "Height of object");
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(74, 6);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(22, 13);
            this.labelHeight.TabIndex = 103;
            this.labelHeight.Text = "h : ";
            this.toolTip1.SetToolTip(this.labelHeight, "Width of object");
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBoxPlayerBlue);
            this.panel4.Controls.Add(this.checkBoxPlayerRed);
            this.panel4.Controls.Add(this.checkBoxTextures);
            this.panel4.Controls.Add(this.checkBoxSelectionMode);
            this.panel4.Controls.Add(this.checkBoxPointer);
            this.panel4.Controls.Add(this.checkBoxRect);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 53);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(58, 645);
            this.panel4.TabIndex = 3;
            this.toolTip1.SetToolTip(this.panel4, "Textures");
            // 
            // checkBoxPlayerBlue
            // 
            this.checkBoxPlayerBlue.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPlayerBlue.BackgroundImage = global::My3DMapEditor.Properties.Resources.BluePlayer;
            this.checkBoxPlayerBlue.Location = new System.Drawing.Point(4, 271);
            this.checkBoxPlayerBlue.Name = "checkBoxPlayerBlue";
            this.checkBoxPlayerBlue.Size = new System.Drawing.Size(50, 50);
            this.checkBoxPlayerBlue.TabIndex = 106;
            this.toolTip1.SetToolTip(this.checkBoxPlayerBlue, "Blue Player position\r\n[I]");
            this.checkBoxPlayerBlue.UseVisualStyleBackColor = true;
            this.checkBoxPlayerBlue.CheckedChanged += new System.EventHandler(this.checkBoxPlayerBlue_CheckedChanged);
            // 
            // checkBoxPlayerRed
            // 
            this.checkBoxPlayerRed.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPlayerRed.BackgroundImage = global::My3DMapEditor.Properties.Resources.RedPlayer;
            this.checkBoxPlayerRed.Location = new System.Drawing.Point(4, 217);
            this.checkBoxPlayerRed.Name = "checkBoxPlayerRed";
            this.checkBoxPlayerRed.Size = new System.Drawing.Size(50, 50);
            this.checkBoxPlayerRed.TabIndex = 105;
            this.toolTip1.SetToolTip(this.checkBoxPlayerRed, "Red Player position\r\n[U]");
            this.checkBoxPlayerRed.UseVisualStyleBackColor = true;
            this.checkBoxPlayerRed.CheckedChanged += new System.EventHandler(this.checkBoxPlayer_CheckedChanged);
            // 
            // checkBoxTextures
            // 
            this.checkBoxTextures.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTextures.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBoxTextures.BackgroundImage")));
            this.checkBoxTextures.Location = new System.Drawing.Point(4, 164);
            this.checkBoxTextures.Name = "checkBoxTextures";
            this.checkBoxTextures.Size = new System.Drawing.Size(50, 50);
            this.checkBoxTextures.TabIndex = 105;
            this.checkBoxTextures.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxTextures, "Textures\r\nTexturing object\r\n[N]");
            this.checkBoxTextures.UseVisualStyleBackColor = true;
            this.checkBoxTextures.CheckedChanged += new System.EventHandler(this.checkBoxTextures_CheckedChanged);
            // 
            // checkBoxSelectionMode
            // 
            this.checkBoxSelectionMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSelectionMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBoxSelectionMode.BackgroundImage")));
            this.checkBoxSelectionMode.Location = new System.Drawing.Point(4, 111);
            this.checkBoxSelectionMode.Name = "checkBoxSelectionMode";
            this.checkBoxSelectionMode.Size = new System.Drawing.Size(50, 50);
            this.checkBoxSelectionMode.TabIndex = 104;
            this.checkBoxSelectionMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxSelectionMode, "Vertex\r\nTransformation verteces\r\n[T]");
            this.checkBoxSelectionMode.UseVisualStyleBackColor = true;
            this.checkBoxSelectionMode.CheckedChanged += new System.EventHandler(this.checkBoxSelectionMode_CheckedChanged);
            // 
            // checkBoxPointer
            // 
            this.checkBoxPointer.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPointer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBoxPointer.BackgroundImage")));
            this.checkBoxPointer.Location = new System.Drawing.Point(4, 5);
            this.checkBoxPointer.Name = "checkBoxPointer";
            this.checkBoxPointer.Size = new System.Drawing.Size(50, 50);
            this.checkBoxPointer.TabIndex = 103;
            this.checkBoxPointer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxPointer, "Pointer\r\nSelect eny primitive\r\n[P]");
            this.checkBoxPointer.UseVisualStyleBackColor = true;
            this.checkBoxPointer.CheckedChanged += new System.EventHandler(this.checkBoxPointer_CheckedChanged);
            // 
            // checkBoxRect
            // 
            this.checkBoxRect.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBoxRect.BackgroundImage")));
            this.checkBoxRect.Location = new System.Drawing.Point(4, 58);
            this.checkBoxRect.Name = "checkBoxRect";
            this.checkBoxRect.Size = new System.Drawing.Size(50, 50);
            this.checkBoxRect.TabIndex = 103;
            this.checkBoxRect.TabStop = false;
            this.checkBoxRect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxRect, "Rect\r\nCreate Box\r\n[B]");
            this.checkBoxRect.UseVisualStyleBackColor = true;
            this.checkBoxRect.CheckedChanged += new System.EventHandler(this.checkBoxRect_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel3D, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelTop, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelRight, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelFront, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(58, 53);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(842, 611);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel3D
            // 
            this.panel3D.Controls.Add(this.pictureBox3D);
            this.panel3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3D.Location = new System.Drawing.Point(3, 3);
            this.panel3D.Name = "panel3D";
            this.panel3D.Size = new System.Drawing.Size(415, 299);
            this.panel3D.TabIndex = 0;
            // 
            // pictureBox3D
            // 
            this.pictureBox3D.BackColor = System.Drawing.Color.Black;
            this.pictureBox3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3D.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3D.Name = "pictureBox3D";
            this.pictureBox3D.Size = new System.Drawing.Size(415, 299);
            this.pictureBox3D.TabIndex = 0;
            this.pictureBox3D.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox3D, "For view 3D world press\r\n[ Q]");
            this.pictureBox3D.Resize += new System.EventHandler(this.pictureBox3D_Resize);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.pictureBoxTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(424, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(415, 299);
            this.panelTop.TabIndex = 1;
            this.panelTop.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelTop_Scroll);
            this.panelTop.Resize += new System.EventHandler(this.panelTop_Resize);
            // 
            // pictureBoxTop
            // 
            this.pictureBoxTop.BackColor = System.Drawing.Color.Black;
            this.pictureBoxTop.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTop.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTop.Name = "pictureBoxTop";
            this.pictureBoxTop.Size = new System.Drawing.Size(415, 299);
            this.pictureBoxTop.TabIndex = 0;
            this.pictureBoxTop.TabStop = false;
            this.pictureBoxTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTop_MouseDown);
            this.pictureBoxTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTop_MouseMove);
            this.pictureBoxTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTop_MouseUp);
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.pictureBoxRight);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(424, 308);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(415, 300);
            this.panelRight.TabIndex = 2;
            this.panelRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelRight_Scroll);
            this.panelRight.Resize += new System.EventHandler(this.panelRight_Resize);
            // 
            // pictureBoxRight
            // 
            this.pictureBoxRight.BackColor = System.Drawing.Color.Black;
            this.pictureBoxRight.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBoxRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRight.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxRight.Name = "pictureBoxRight";
            this.pictureBoxRight.Size = new System.Drawing.Size(415, 300);
            this.pictureBoxRight.TabIndex = 0;
            this.pictureBoxRight.TabStop = false;
            this.pictureBoxRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRight_MouseDown);
            this.pictureBoxRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRight_MouseMove);
            this.pictureBoxRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRight_MouseUp);
            // 
            // panelFront
            // 
            this.panelFront.Controls.Add(this.pictureBoxFront);
            this.panelFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFront.Location = new System.Drawing.Point(3, 308);
            this.panelFront.Name = "panelFront";
            this.panelFront.Size = new System.Drawing.Size(415, 300);
            this.panelFront.TabIndex = 3;
            this.panelFront.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelFront_Scroll);
            this.panelFront.Resize += new System.EventHandler(this.panelFront_Resize);
            // 
            // pictureBoxFront
            // 
            this.pictureBoxFront.BackColor = System.Drawing.Color.Black;
            this.pictureBoxFront.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBoxFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFront.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFront.Name = "pictureBoxFront";
            this.pictureBoxFront.Size = new System.Drawing.Size(415, 300);
            this.pictureBoxFront.TabIndex = 0;
            this.pictureBoxFront.TabStop = false;
            this.pictureBoxFront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFront_MouseDown);
            this.pictureBoxFront.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFront_MouseMove);
            this.pictureBoxFront.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFront_MouseUp);
            // 
            // timerTop
            // 
            this.timerTop.Enabled = true;
            this.timerTop.Interval = 5;
            this.timerTop.Tick += new System.EventHandler(this.timerTop_Tick);
            // 
            // timerRight
            // 
            this.timerRight.Enabled = true;
            this.timerRight.Interval = 5;
            this.timerRight.Tick += new System.EventHandler(this.timerRight_Tick);
            // 
            // timerFront
            // 
            this.timerFront.Enabled = true;
            this.timerFront.Interval = 5;
            this.timerFront.Tick += new System.EventHandler(this.timerFront_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.objectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.openMapToolStripMenuItem,
            this.saveMapToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // openMapToolStripMenuItem
            // 
            this.openMapToolStripMenuItem.Name = "openMapToolStripMenuItem";
            this.openMapToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMapToolStripMenuItem.Text = "Open map";
            this.openMapToolStripMenuItem.Click += new System.EventHandler(this.openMapToolStripMenuItem_Click);
            // 
            // saveMapToolStripMenuItem
            // 
            this.saveMapToolStripMenuItem.Name = "saveMapToolStripMenuItem";
            this.saveMapToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveMapToolStripMenuItem.Text = "Save map";
            this.saveMapToolStripMenuItem.Click += new System.EventHandler(this.saveMapToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Exit";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.Tag = "";
            // 
            // timer3D
            // 
            this.timer3D.Enabled = true;
            this.timer3D.Interval = 1;
            this.timer3D.Tick += new System.EventHandler(this.timer3D_Tick);
            // 
            // objectToolStripMenuItem
            // 
            this.objectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pastToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.objectToolStripMenuItem.Name = "objectToolStripMenuItem";
            this.objectToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.objectToolStripMenuItem.Text = "Object";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pastToolStripMenuItem
            // 
            this.pastToolStripMenuItem.Name = "pastToolStripMenuItem";
            this.pastToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pastToolStripMenuItem.Text = "Paste";
            this.pastToolStripMenuItem.Click += new System.EventHandler(this.pastToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 698);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3D.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3D)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).EndInit();
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).EndInit();
            this.panelFront.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFront)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3D;
        private System.Windows.Forms.PictureBox pictureBox3D;
        public System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pictureBoxTop;
        public System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.PictureBox pictureBoxRight;
        public System.Windows.Forms.Panel panelFront;
        private System.Windows.Forms.PictureBox pictureBoxFront;
        private System.Windows.Forms.Timer timerTop;
        private System.Windows.Forms.Timer timerRight;
        private System.Windows.Forms.Timer timerFront;
        private System.Windows.Forms.CheckBox checkBoxRect;
        private System.Windows.Forms.Button buttonGridPlus;
        private System.Windows.Forms.Button buttonGridMinus;
        private System.Windows.Forms.Label labelGridSize;
        private System.Windows.Forms.CheckBox checkBoxSnap;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxPointer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxSelectionMode;
        private System.Windows.Forms.CheckBox checkBoxTextures;
        private System.Windows.Forms.Timer timer3D;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.ToolStripMenuItem openMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxPlayerRed;
        private System.Windows.Forms.CheckBox checkBoxPlayerBlue;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripMenuItem objectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}


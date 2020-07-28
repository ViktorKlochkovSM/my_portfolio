namespace StoDescktopClient
{
    partial class StoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgwStations = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.stationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSaveStation = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRestStationEdit = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnResetEditService = new System.Windows.Forms.Button();
            this.btnSaveService = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNewServiceDescription = new System.Windows.Forms.TextBox();
            this.tbNewServicePrice = new System.Windows.Forms.TextBox();
            this.tbNewServiceName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbStations = new System.Windows.Forms.ComboBox();
            this.dgwServices = new System.Windows.Forms.DataGridView();
            this.ServiceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoveService = new System.Windows.Forms.DataGridViewButtonColumn();
            this.servicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgwServedCars = new System.Windows.Forms.DataGridView();
            this.ServedCarId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarBrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceCompleteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sc_ServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sc_Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.summaryByAllStoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dtpCarYear = new System.Windows.Forms.DateTimePicker();
            this.dtpSrviceCompleteDate = new System.Windows.Forms.DateTimePicker();
            this.btnResedServedCar = new System.Windows.Forms.Button();
            this.btnSaveServedCar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCarBrand = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbServices_WorksTab = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbStations_WorksTab = new System.Windows.Forms.ComboBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cbStationsSummaries = new System.Windows.Forms.ComboBox();
            this.dgwServedCars_SumSto = new System.Windows.Forms.DataGridView();
            this.CarBrand_SummSto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarYear_SumSto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceName_SumSto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceCompleteDate_SumSto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwSumAllSto = new System.Windows.Forms.DataGridView();
            this.STOName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCompletedServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorksTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnFilterQueryDatesSum = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.dtPikEnd = new System.Windows.Forms.DateTimePicker();
            this.dtPikStart = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnLoadFirstData = new System.Windows.Forms.Button();
            this.servedCarsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.summaryBySto = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgwStations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stationBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwServices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.servicesBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwServedCars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryByAllStoBindingSource)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwServedCars_SumSto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgwSumAllSto)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.servedCarsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryBySto)).BeginInit();
            this.SuspendLayout();
            // 
            // dgwStations
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgwStations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgwStations.AutoGenerateColumns = false;
            this.dgwStations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwStations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.Remove});
            this.dgwStations.DataSource = this.stationBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwStations.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgwStations.Location = new System.Drawing.Point(337, 6);
            this.dgwStations.MultiSelect = false;
            this.dgwStations.Name = "dgwStations";
            this.dgwStations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwStations.Size = new System.Drawing.Size(900, 370);
            this.dgwStations.TabIndex = 0;
            this.dgwStations.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwStations_CellClick);
            this.dgwStations.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwStations_CellMouseDoubleClick);
            this.dgwStations.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgwStations_DataBindingComplete);
            this.dgwStations.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwStations_RowHeaderMouseDoubleClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            this.idDataGridViewTextBoxColumn.Width = 70;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Имя";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 250;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.descriptionDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Описание";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 500;
            // 
            // Remove
            // 
            this.Remove.HeaderText = "";
            this.Remove.Name = "Remove";
            // 
            // stationBindingSource
            // 
            this.stationBindingSource.DataSource = typeof(StoDescktopClient.ServiceReference1.Station);
            // 
            // btnSaveStation
            // 
            this.btnSaveStation.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveStation.Location = new System.Drawing.Point(85, 149);
            this.btnSaveStation.Name = "btnSaveStation";
            this.btnSaveStation.Size = new System.Drawing.Size(106, 27);
            this.btnSaveStation.TabIndex = 3;
            this.btnSaveStation.Text = "Сохранить";
            this.btnSaveStation.UseVisualStyleBackColor = true;
            this.btnSaveStation.Click += new System.EventHandler(this.btnSaveStation_Click);
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(85, 48);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(220, 22);
            this.tbName.TabIndex = 1;
            this.tbName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbName_KeyUp);
            // 
            // tbDescription
            // 
            this.tbDescription.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDescription.Location = new System.Drawing.Point(85, 85);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(220, 50);
            this.tbDescription.TabIndex = 2;
            this.tbDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbDescription_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Название";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Описание";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox2.Controls.Add(this.btnRestStationEdit);
            this.groupBox2.Controls.Add(this.btnSaveStation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbDescription);
            this.groupBox2.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 191);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "СТО";
            // 
            // btnRestStationEdit
            // 
            this.btnRestStationEdit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRestStationEdit.Location = new System.Drawing.Point(241, 149);
            this.btnRestStationEdit.Name = "btnRestStationEdit";
            this.btnRestStationEdit.Size = new System.Drawing.Size(64, 27);
            this.btnRestStationEdit.TabIndex = 4;
            this.btnRestStationEdit.Text = "Сброс";
            this.btnRestStationEdit.UseVisualStyleBackColor = true;
            this.btnRestStationEdit.Click += new System.EventHandler(this.btnRestStationEdit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1251, 410);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgwStations);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1243, 384);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "СТО";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.dgwServices);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1243, 384);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Услуги";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox3.Controls.Add(this.btnResetEditService);
            this.groupBox3.Controls.Add(this.btnSaveService);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tbNewServiceDescription);
            this.groupBox3.Controls.Add(this.tbNewServicePrice);
            this.groupBox3.Controls.Add(this.tbNewServiceName);
            this.groupBox3.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox3.Location = new System.Drawing.Point(3, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(328, 232);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Услуга";
            // 
            // btnResetEditService
            // 
            this.btnResetEditService.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnResetEditService.Location = new System.Drawing.Point(259, 190);
            this.btnResetEditService.Name = "btnResetEditService";
            this.btnResetEditService.Size = new System.Drawing.Size(63, 28);
            this.btnResetEditService.TabIndex = 6;
            this.btnResetEditService.Text = "Сброс";
            this.btnResetEditService.UseVisualStyleBackColor = true;
            this.btnResetEditService.Click += new System.EventHandler(this.btnResetEditService_Click);
            // 
            // btnSaveService
            // 
            this.btnSaveService.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveService.Location = new System.Drawing.Point(95, 190);
            this.btnSaveService.Name = "btnSaveService";
            this.btnSaveService.Size = new System.Drawing.Size(98, 28);
            this.btnSaveService.TabIndex = 5;
            this.btnSaveService.Text = "Сохранить";
            this.btnSaveService.UseVisualStyleBackColor = true;
            this.btnSaveService.Click += new System.EventHandler(this.btnSaveService_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Описание";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label5.Location = new System.Drawing.Point(44, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Цена";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label3.Location = new System.Drawing.Point(12, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Название";
            // 
            // tbNewServiceDescription
            // 
            this.tbNewServiceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNewServiceDescription.Location = new System.Drawing.Point(95, 75);
            this.tbNewServiceDescription.Multiline = true;
            this.tbNewServiceDescription.Name = "tbNewServiceDescription";
            this.tbNewServiceDescription.Size = new System.Drawing.Size(227, 69);
            this.tbNewServiceDescription.TabIndex = 3;
            this.tbNewServiceDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbNewServiceDescription_KeyUp);
            // 
            // tbNewServicePrice
            // 
            this.tbNewServicePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNewServicePrice.Location = new System.Drawing.Point(95, 157);
            this.tbNewServicePrice.Name = "tbNewServicePrice";
            this.tbNewServicePrice.Size = new System.Drawing.Size(80, 22);
            this.tbNewServicePrice.TabIndex = 4;
            this.tbNewServicePrice.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbNewServicePrice_KeyUp);
            // 
            // tbNewServiceName
            // 
            this.tbNewServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNewServiceName.Location = new System.Drawing.Point(95, 39);
            this.tbNewServiceName.Name = "tbNewServiceName";
            this.tbNewServiceName.Size = new System.Drawing.Size(227, 22);
            this.tbNewServiceName.TabIndex = 2;
            this.tbNewServiceName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbNewServiceName_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox1.Controls.Add(this.cbStations);
            this.groupBox1.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 68);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "СТО";
            // 
            // cbStations
            // 
            this.cbStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbStations.FormattingEnabled = true;
            this.cbStations.Location = new System.Drawing.Point(6, 27);
            this.cbStations.Name = "cbStations";
            this.cbStations.Size = new System.Drawing.Size(235, 24);
            this.cbStations.TabIndex = 0;
            this.cbStations.SelectedIndexChanged += new System.EventHandler(this.cbStations_SelectedIndexChanged);
            // 
            // dgwServices
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgwServices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgwServices.AutoGenerateColumns = false;
            this.dgwServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceID,
            this.ServiceName,
            this.ServiceDescription,
            this.Price,
            this.StationId,
            this.RemoveService});
            this.dgwServices.DataSource = this.servicesBindingSource;
            this.dgwServices.Location = new System.Drawing.Point(337, 6);
            this.dgwServices.Name = "dgwServices";
            this.dgwServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwServices.Size = new System.Drawing.Size(900, 370);
            this.dgwServices.TabIndex = 1;
            this.dgwServices.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwServices_CellClick);
            this.dgwServices.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwServices_CellMouseDoubleClick);
            this.dgwServices.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgwServices_DataBindingComplete);
            this.dgwServices.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwServices_RowHeaderMouseDoubleClick);
            // 
            // ServiceID
            // 
            this.ServiceID.DataPropertyName = "Id";
            this.ServiceID.HeaderText = "ID_";
            this.ServiceID.Name = "ServiceID";
            this.ServiceID.Visible = false;
            // 
            // ServiceName
            // 
            this.ServiceName.DataPropertyName = "Name";
            this.ServiceName.HeaderText = "Имя";
            this.ServiceName.Name = "ServiceName";
            // 
            // ServiceDescription
            // 
            this.ServiceDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServiceDescription.DataPropertyName = "Description";
            this.ServiceDescription.HeaderText = "Описание";
            this.ServiceDescription.Name = "ServiceDescription";
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Цена";
            this.Price.Name = "Price";
            // 
            // StationId
            // 
            this.StationId.DataPropertyName = "StationId";
            this.StationId.HeaderText = "StationID";
            this.StationId.Name = "StationId";
            this.StationId.Visible = false;
            // 
            // RemoveService
            // 
            this.RemoveService.HeaderText = "";
            this.RemoveService.Name = "RemoveService";
            this.RemoveService.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RemoveService.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgwServedCars);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1243, 384);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Работы";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgwServedCars
            // 
            this.dgwServedCars.AllowUserToAddRows = false;
            this.dgwServedCars.AutoGenerateColumns = false;
            this.dgwServedCars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwServedCars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServedCarId,
            this.CarBrand,
            this.CarYear,
            this.ServiceCompleteDate,
            this.Sc_ServiceId,
            this.Sc_Remove});
            this.dgwServedCars.DataSource = this.summaryByAllStoBindingSource;
            this.dgwServedCars.Location = new System.Drawing.Point(337, 6);
            this.dgwServedCars.Name = "dgwServedCars";
            this.dgwServedCars.Size = new System.Drawing.Size(900, 370);
            this.dgwServedCars.TabIndex = 3;
            this.dgwServedCars.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwServedCars_CellClick);
            this.dgwServedCars.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwServedCars_CellMouseDoubleClick);
            this.dgwServedCars.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgwServedCars_DataBindingComplete);
            this.dgwServedCars.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgwServedCars_RowHeaderMouseDoubleClick);
            // 
            // ServedCarId
            // 
            this.ServedCarId.DataPropertyName = "Id";
            this.ServedCarId.HeaderText = "ID";
            this.ServedCarId.Name = "ServedCarId";
            this.ServedCarId.ReadOnly = true;
            this.ServedCarId.Visible = false;
            // 
            // CarBrand
            // 
            this.CarBrand.DataPropertyName = "CarBrand";
            this.CarBrand.HeaderText = "Car Brand";
            this.CarBrand.Name = "CarBrand";
            this.CarBrand.ReadOnly = true;
            this.CarBrand.Width = 350;
            // 
            // CarYear
            // 
            this.CarYear.DataPropertyName = "CarYear";
            this.CarYear.HeaderText = "Car Year";
            this.CarYear.Name = "CarYear";
            this.CarYear.ReadOnly = true;
            this.CarYear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ServiceCompleteDate
            // 
            this.ServiceCompleteDate.DataPropertyName = "ServiceCompletDate";
            this.ServiceCompleteDate.HeaderText = "Complete Date";
            this.ServiceCompleteDate.Name = "ServiceCompleteDate";
            this.ServiceCompleteDate.ReadOnly = true;
            this.ServiceCompleteDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ServiceCompleteDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Sc_ServiceId
            // 
            this.Sc_ServiceId.DataPropertyName = "ServiceId";
            this.Sc_ServiceId.HeaderText = "ServiceID";
            this.Sc_ServiceId.Name = "Sc_ServiceId";
            this.Sc_ServiceId.ReadOnly = true;
            this.Sc_ServiceId.Visible = false;
            // 
            // Sc_Remove
            // 
            this.Sc_Remove.HeaderText = "";
            this.Sc_Remove.Name = "Sc_Remove";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox6.Controls.Add(this.dtpCarYear);
            this.groupBox6.Controls.Add(this.dtpSrviceCompleteDate);
            this.groupBox6.Controls.Add(this.btnResedServedCar);
            this.groupBox6.Controls.Add(this.btnSaveServedCar);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.tbCarBrand);
            this.groupBox6.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox6.Location = new System.Drawing.Point(7, 157);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(324, 220);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Обслуживание";
            // 
            // dtpCarYear
            // 
            this.dtpCarYear.CustomFormat = "MM-yyyy";
            this.dtpCarYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.dtpCarYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCarYear.Location = new System.Drawing.Point(168, 69);
            this.dtpCarYear.MinDate = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            this.dtpCarYear.Name = "dtpCarYear";
            this.dtpCarYear.Size = new System.Drawing.Size(150, 22);
            this.dtpCarYear.TabIndex = 8;
            this.dtpCarYear.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpCarYear_KeyUp);
            // 
            // dtpSrviceCompleteDate
            // 
            this.dtpSrviceCompleteDate.CustomFormat = "dd-MM-yyyy HH:mm";
            this.dtpSrviceCompleteDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpSrviceCompleteDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.dtpSrviceCompleteDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSrviceCompleteDate.Location = new System.Drawing.Point(168, 108);
            this.dtpSrviceCompleteDate.MinDate = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            this.dtpSrviceCompleteDate.Name = "dtpSrviceCompleteDate";
            this.dtpSrviceCompleteDate.ShowUpDown = true;
            this.dtpSrviceCompleteDate.Size = new System.Drawing.Size(150, 22);
            this.dtpSrviceCompleteDate.TabIndex = 7;
            // 
            // btnResedServedCar
            // 
            this.btnResedServedCar.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnResedServedCar.Location = new System.Drawing.Point(249, 147);
            this.btnResedServedCar.Name = "btnResedServedCar";
            this.btnResedServedCar.Size = new System.Drawing.Size(69, 28);
            this.btnResedServedCar.TabIndex = 5;
            this.btnResedServedCar.Text = "Сброс";
            this.btnResedServedCar.UseVisualStyleBackColor = true;
            this.btnResedServedCar.Click += new System.EventHandler(this.btnResedServedCar_Click);
            // 
            // btnSaveServedCar
            // 
            this.btnSaveServedCar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSaveServedCar.Location = new System.Drawing.Point(75, 147);
            this.btnSaveServedCar.Name = "btnSaveServedCar";
            this.btnSaveServedCar.Size = new System.Drawing.Size(109, 28);
            this.btnSaveServedCar.TabIndex = 5;
            this.btnSaveServedCar.Text = "Сохранить";
            this.btnSaveServedCar.UseVisualStyleBackColor = true;
            this.btnSaveServedCar.Click += new System.EventHandler(this.btnSaveServedCar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label8.Location = new System.Drawing.Point(35, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 20);
            this.label8.TabIndex = 4;
            this.label8.Text = "Дата обслуживания";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label7.Location = new System.Drawing.Point(6, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Год выпуска автомобиля";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label6.Location = new System.Drawing.Point(35, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Марка автомобиля";
            // 
            // tbCarBrand
            // 
            this.tbCarBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.tbCarBrand.Location = new System.Drawing.Point(168, 30);
            this.tbCarBrand.Name = "tbCarBrand";
            this.tbCarBrand.Size = new System.Drawing.Size(150, 22);
            this.tbCarBrand.TabIndex = 0;
            this.tbCarBrand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbCarBrand_KeyUp);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox5.Controls.Add(this.cbServices_WorksTab);
            this.groupBox5.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox5.Location = new System.Drawing.Point(6, 80);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(260, 71);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Услуга";
            // 
            // cbServices_WorksTab
            // 
            this.cbServices_WorksTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbServices_WorksTab.FormattingEnabled = true;
            this.cbServices_WorksTab.Location = new System.Drawing.Point(7, 29);
            this.cbServices_WorksTab.Name = "cbServices_WorksTab";
            this.cbServices_WorksTab.Size = new System.Drawing.Size(232, 24);
            this.cbServices_WorksTab.TabIndex = 0;
            this.cbServices_WorksTab.SelectedIndexChanged += new System.EventHandler(this.cbServices_WorksTab_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox4.Controls.Add(this.cbStations_WorksTab);
            this.groupBox4.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox4.Location = new System.Drawing.Point(3, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(263, 68);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "СТО";
            // 
            // cbStations_WorksTab
            // 
            this.cbStations_WorksTab.DataSource = this.stationBindingSource;
            this.cbStations_WorksTab.DisplayMember = "Name";
            this.cbStations_WorksTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbStations_WorksTab.FormattingEnabled = true;
            this.cbStations_WorksTab.Location = new System.Drawing.Point(7, 27);
            this.cbStations_WorksTab.Name = "cbStations_WorksTab";
            this.cbStations_WorksTab.Size = new System.Drawing.Size(235, 24);
            this.cbStations_WorksTab.TabIndex = 0;
            this.cbStations_WorksTab.ValueMember = "Id";
            this.cbStations_WorksTab.SelectedIndexChanged += new System.EventHandler(this.cbStations_WorksTab_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox9);
            this.tabPage4.Controls.Add(this.dgwServedCars_SumSto);
            this.tabPage4.Controls.Add(this.dgwSumAllSto);
            this.tabPage4.Controls.Add(this.groupBox8);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1243, 384);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Сводка";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox9.Controls.Add(this.cbStationsSummaries);
            this.groupBox9.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox9.Location = new System.Drawing.Point(5, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(666, 45);
            this.groupBox9.TabIndex = 8;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Выбор СТО";
            // 
            // cbStationsSummaries
            // 
            this.cbStationsSummaries.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbStationsSummaries.FormattingEnabled = true;
            this.cbStationsSummaries.Location = new System.Drawing.Point(160, 14);
            this.cbStationsSummaries.Name = "cbStationsSummaries";
            this.cbStationsSummaries.Size = new System.Drawing.Size(224, 24);
            this.cbStationsSummaries.TabIndex = 0;
            this.cbStationsSummaries.SelectedIndexChanged += new System.EventHandler(this.cbStationsSummaries_SelectedIndexChanged);
            // 
            // dgwServedCars_SumSto
            // 
            this.dgwServedCars_SumSto.AllowUserToAddRows = false;
            this.dgwServedCars_SumSto.AllowUserToDeleteRows = false;
            this.dgwServedCars_SumSto.AllowUserToOrderColumns = true;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgwServedCars_SumSto.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgwServedCars_SumSto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwServedCars_SumSto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CarBrand_SummSto,
            this.CarYear_SumSto,
            this.ServiceName_SumSto,
            this.ServiceCompleteDate_SumSto});
            this.dgwServedCars_SumSto.Location = new System.Drawing.Point(5, 51);
            this.dgwServedCars_SumSto.Name = "dgwServedCars_SumSto";
            this.dgwServedCars_SumSto.ReadOnly = true;
            this.dgwServedCars_SumSto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwServedCars_SumSto.Size = new System.Drawing.Size(666, 326);
            this.dgwServedCars_SumSto.TabIndex = 1;
            // 
            // CarBrand_SummSto
            // 
            this.CarBrand_SummSto.DataPropertyName = "CarBrand";
            this.CarBrand_SummSto.HeaderText = "Марка авто";
            this.CarBrand_SummSto.Name = "CarBrand_SummSto";
            this.CarBrand_SummSto.ReadOnly = true;
            this.CarBrand_SummSto.Width = 210;
            // 
            // CarYear_SumSto
            // 
            this.CarYear_SumSto.DataPropertyName = "CarYear";
            this.CarYear_SumSto.HeaderText = "Год выпуска авто";
            this.CarYear_SumSto.Name = "CarYear_SumSto";
            this.CarYear_SumSto.ReadOnly = true;
            // 
            // ServiceName_SumSto
            // 
            this.ServiceName_SumSto.DataPropertyName = "ServiceName";
            this.ServiceName_SumSto.HeaderText = "Услуга";
            this.ServiceName_SumSto.Name = "ServiceName_SumSto";
            this.ServiceName_SumSto.ReadOnly = true;
            this.ServiceName_SumSto.Width = 200;
            // 
            // ServiceCompleteDate_SumSto
            // 
            this.ServiceCompleteDate_SumSto.DataPropertyName = "ServiceCompleteDate";
            this.ServiceCompleteDate_SumSto.HeaderText = "Дата обслуживания";
            this.ServiceCompleteDate_SumSto.Name = "ServiceCompleteDate_SumSto";
            this.ServiceCompleteDate_SumSto.ReadOnly = true;
            // 
            // dgwSumAllSto
            // 
            this.dgwSumAllSto.AllowUserToAddRows = false;
            this.dgwSumAllSto.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgwSumAllSto.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgwSumAllSto.AutoGenerateColumns = false;
            this.dgwSumAllSto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwSumAllSto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STOName,
            this.CountCompletedServices,
            this.WorksTotalPrice});
            this.dgwSumAllSto.DataSource = this.summaryByAllStoBindingSource;
            this.dgwSumAllSto.Location = new System.Drawing.Point(677, 51);
            this.dgwSumAllSto.MultiSelect = false;
            this.dgwSumAllSto.Name = "dgwSumAllSto";
            this.dgwSumAllSto.ReadOnly = true;
            this.dgwSumAllSto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwSumAllSto.ShowEditingIcon = false;
            this.dgwSumAllSto.Size = new System.Drawing.Size(563, 326);
            this.dgwSumAllSto.TabIndex = 1;
            this.dgwSumAllSto.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgwSumAllSto_DataBindingComplete);
            // 
            // STOName
            // 
            this.STOName.DataPropertyName = "StationName";
            this.STOName.HeaderText = "СТО";
            this.STOName.Name = "STOName";
            this.STOName.ReadOnly = true;
            this.STOName.Width = 300;
            // 
            // CountCompletedServices
            // 
            this.CountCompletedServices.DataPropertyName = "CountCompletedServices";
            this.CountCompletedServices.HeaderText = "Кол-во услуг";
            this.CountCompletedServices.Name = "CountCompletedServices";
            this.CountCompletedServices.ReadOnly = true;
            // 
            // WorksTotalPrice
            // 
            this.WorksTotalPrice.DataPropertyName = "TotalPrice";
            this.WorksTotalPrice.HeaderText = "Сумма";
            this.WorksTotalPrice.Name = "WorksTotalPrice";
            this.WorksTotalPrice.ReadOnly = true;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox8.Controls.Add(this.btnFilterQueryDatesSum);
            this.groupBox8.Controls.Add(this.label9);
            this.groupBox8.Controls.Add(this.dtPikEnd);
            this.groupBox8.Controls.Add(this.dtPikStart);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Italic);
            this.groupBox8.Location = new System.Drawing.Point(677, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(563, 45);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Все СТО";
            // 
            // btnFilterQueryDatesSum
            // 
            this.btnFilterQueryDatesSum.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnFilterQueryDatesSum.Location = new System.Drawing.Point(443, 16);
            this.btnFilterQueryDatesSum.Name = "btnFilterQueryDatesSum";
            this.btnFilterQueryDatesSum.Size = new System.Drawing.Size(75, 23);
            this.btnFilterQueryDatesSum.TabIndex = 6;
            this.btnFilterQueryDatesSum.Text = "Запрос";
            this.btnFilterQueryDatesSum.UseVisualStyleBackColor = true;
            this.btnFilterQueryDatesSum.Click += new System.EventHandler(this.btnFilterQueryDatesSum_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label9.Location = new System.Drawing.Point(86, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 20);
            this.label9.TabIndex = 5;
            this.label9.Text = "C";
            // 
            // dtPikEnd
            // 
            this.dtPikEnd.CustomFormat = "dd-MM-yyyy HH:mm";
            this.dtPikEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.dtPikEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPikEnd.Location = new System.Drawing.Point(291, 16);
            this.dtPikEnd.Name = "dtPikEnd";
            this.dtPikEnd.Size = new System.Drawing.Size(146, 22);
            this.dtPikEnd.TabIndex = 4;
            // 
            // dtPikStart
            // 
            this.dtPikStart.CustomFormat = "dd-MM-yyyy HH:mm";
            this.dtPikStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.dtPikStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPikStart.Location = new System.Drawing.Point(107, 16);
            this.dtPikStart.Name = "dtPikStart";
            this.dtPikStart.Size = new System.Drawing.Size(152, 22);
            this.dtPikStart.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Italic);
            this.label10.Location = new System.Drawing.Point(265, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 20);
            this.label10.TabIndex = 5;
            this.label10.Text = "По";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnLoadFirstData);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1243, 384);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Администрирование";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnLoadFirstData
            // 
            this.btnLoadFirstData.Location = new System.Drawing.Point(38, 36);
            this.btnLoadFirstData.Name = "btnLoadFirstData";
            this.btnLoadFirstData.Size = new System.Drawing.Size(202, 23);
            this.btnLoadFirstData.TabIndex = 0;
            this.btnLoadFirstData.Text = "Загрузка начальных данных";
            this.btnLoadFirstData.UseVisualStyleBackColor = true;
            this.btnLoadFirstData.Click += new System.EventHandler(this.btnLoadFirstData_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Description";
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 500;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn5.HeaderText = "Название";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 150;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn6.HeaderText = "Описание";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Price";
            this.dataGridViewTextBoxColumn7.HeaderText = "Цена";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "StationID";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // StoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 414);
            this.Controls.Add(this.tabControl1);
            this.Name = "StoForm";
            this.Text = "Sto Manager";
            ((System.ComponentModel.ISupportInitialize)(this.dgwStations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stationBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwServices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.servicesBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwServedCars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryByAllStoBindingSource)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwServedCars_SumSto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgwSumAllSto)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.servedCarsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryBySto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSaveStation;
        private System.Windows.Forms.DataGridView dgwStations;
        private System.Windows.Forms.BindingSource stationBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Remove;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgwServices;
        private System.Windows.Forms.ComboBox cbStations;
        private System.Windows.Forms.BindingSource servicesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSaveService;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNewServiceDescription;
        private System.Windows.Forms.TextBox tbNewServicePrice;
        private System.Windows.Forms.TextBox tbNewServiceName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationId;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveService;
        private System.Windows.Forms.Button btnRestStationEdit;
        private System.Windows.Forms.Button btnResetEditService;
        private System.Windows.Forms.DataGridView dgwServedCars;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbServices_WorksTab;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbStations_WorksTab;
        private System.Windows.Forms.TextBox tbCarBrand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnResedServedCar;
        private System.Windows.Forms.Button btnSaveServedCar;
        private System.Windows.Forms.DateTimePicker dtpSrviceCompleteDate;
        private System.Windows.Forms.DateTimePicker dtpCarYear;
        private System.Windows.Forms.BindingSource servedCarsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServedCarId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceCompleteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sc_ServiceId;
        private System.Windows.Forms.DataGridViewButtonColumn Sc_Remove;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dgwSumAllSto;
        private System.Windows.Forms.BindingSource summaryByAllStoBindingSource;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtPikEnd;
        private System.Windows.Forms.DateTimePicker dtPikStart;
        private System.Windows.Forms.Button btnFilterQueryDatesSum;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ComboBox cbStationsSummaries;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridViewTextBoxColumn STOName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCompletedServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorksTotalPrice;
        private System.Windows.Forms.DataGridView dgwServedCars_SumSto;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarBrand_SummSto;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarYear_SumSto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName_SumSto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceCompleteDate_SumSto;
        private System.Windows.Forms.BindingSource summaryBySto;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnLoadFirstData;
    }
}


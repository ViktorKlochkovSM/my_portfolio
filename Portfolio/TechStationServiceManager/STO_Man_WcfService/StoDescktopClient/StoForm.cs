using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StoDescktopClient.ServiceReference1;
using StoDescktopClient.Utils;
using StoDescktopClient.Forms;

namespace StoDescktopClient
{
    public partial class StoForm : Form
    {
        StoServiceClient serviceClient;

        CurrentStationContext currentStationContext;//Контекст редактора СТО
        CurrentServiceContext currentServiceContext;//Контекст редактора Услуги
        CurrentServedCarContext currentServedCarContext;//Контекст редактора Обслуживаемого Авто

        public StoForm()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            serviceClient = new StoServiceClient();//клиент для коммуникации со службой WCF

            //Контексты редакторов объектов
            currentStationContext = new CurrentStationContext();
            currentServiceContext = new CurrentServiceContext();
            currentServedCarContext = new CurrentServedCarContext();

            InitStations();
        }

        //TABS
        #region CTO TAB
        /// <summary>
        /// Инициализация источников данных таблиц и комбиков СТО
        /// </summary>
        void InitStations()
        {
            try
            {
                stationBindingSource.DataSource = serviceClient.SelectStations();
                dgwStations.DataSource = stationBindingSource;
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();

                dgwStations.DataSource = null;
            }

            cbStationsSummaries.DisplayMember =  cbStations.DisplayMember = "Name";
            cbStationsSummaries.ValueMember = cbStations.ValueMember = "Id";
            cbStationsSummaries.DataSource = cbStations.DataSource = stationBindingSource;
            if(cbStations.Items.Count > 0)
            {
                cbStations.SelectedIndex = 0;
                cbStationsSummaries.SelectedIndex = 0;
            }

            int selectedStation = 0;
            if (cbStations.SelectedValue != null)
            {
                selectedStation = (int)cbStations.SelectedValue;
            }
            InitServicesDataSource(selectedStation);
        }

        private void dgwStations_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgwStations.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            foreach (DataGridViewRow dgr in dgwStations.Rows)
            {
                dgr.Cells[0].OwningColumn.Visible = false;
                dgr.Cells[1].ReadOnly = true;
                dgr.Cells[2].ReadOnly = true;
                DataGridViewButtonCell removeCell = (DataGridViewButtonCell)dgr.Cells[3];
                removeCell.Value = "Удалить";
                removeCell.Style = new DataGridViewCellStyle();
                removeCell.Style.Font = new Font("Arial", 8, FontStyle.Underline);
            }
        }
        //Сохранение изменений в редакторе СТО
        private void btnSaveStation_Click(object sender, EventArgs e)
        {
            CreatOrUpdateStation();
        }

        void CreatOrUpdateStation()
        {
            if (!String.IsNullOrEmpty(tbName.Text) && !String.IsNullOrEmpty(tbDescription.Text))
            {
                if (currentStationContext.CurrentStation != null)
                {
                    //Редактирование 
                    currentStationContext.CurrentStation.Name = tbName.Text;
                    currentStationContext.CurrentStation.Description = tbDescription.Text;
                }
                else
                {
                    //Создание новой СТО
                    currentStationContext.CurrentStation = new Station
                    {
                        Name = tbName.Text,
                        Description = tbDescription.Text
                    };
                }

                try
                {
                    serviceClient.CreateOrUpdateStation(currentStationContext.CurrentStation);
                    currentStationContext.Reset();//Сброс изменений для объекта в редакторе
                    stationBindingSource.DataSource = serviceClient.SelectStations();
                    tbName.Text = tbDescription.Text = "";
                }
                catch (FaultException fe)
                {
                    ErrorForm erFrm = new ErrorForm(fe);
                    if (erFrm.ShowDialog() == DialogResult.Abort)
                        this.Close();
                }
            }
            else
            {
                MessageBox.Show("Одно или несколько полей остались не заполненными.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgwStations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (MessageBox.Show("Действительно хотите удалить?", "Удалить СТО?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = (int)dgwStations.Rows[e.RowIndex].Cells[0].Value;
                    
                    try
                    {
                        //выполняем удаление СТО
                        serviceClient.DeleteStation(new Station
                        {
                            Id = id,
                            Name = dgwStations.Rows[e.RowIndex].Cells[1].Value.ToString(),
                            Description = dgwStations.Rows[e.RowIndex].Cells[2].Value.ToString()
                        });
                        stationBindingSource.DataSource = serviceClient.SelectStations();

                        //проверить не редактируется ли удаляемый объект
                        if (currentStationContext.CurrentStation != null)
                        {
                            //объект есть в редакторе
                            //является ли он удаляемым объектом
                            if (id == currentStationContext.CurrentStation.Id)
                            {
                                //в редакторе именно удаляемый объект - делаем сброс редактора
                                ResetEditStation();
                            }
                        }
                        if(cbStations.SelectedValue != null)
                        {
                            InitServicesDataSource((int)cbStations.SelectedValue);
                        }
                    }
                    catch (FaultException fe)
                    {
                        ErrorForm erFrm = new ErrorForm(fe);
                        if (erFrm.ShowDialog() == DialogResult.Abort)
                            this.Close();
                    }
                }
            }
        }

        private void cbStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStations.SelectedValue != null)
            {
                //проверим на наличие изменений в объекте
                if (currentServiceContext.HasAnyChangedData)
                {
                    //Изменения выполнялись
                    if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        CreateOrUpdateService();
                    }
                    else
                    {
                        //сброс
                        ResetEditService();
                    }
                }
                else
                {
                    //Изменения не выполнялись
                    //сброс
                    ResetEditService();
                }

                int selectedStation = (int)cbStations.SelectedValue;
                InitServicesDataSource(selectedStation);
            }
        }

        private void dgwStations_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedStationRow(e.RowIndex);//Рдактировать выбранную СТО
        }

        private void dgwStations_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedStationRow(e.RowIndex);//Рдактировать выбранную СТО
        }

        /// <summary>
        /// Редактирование выбранной в DataGridView СТО
        /// </summary>
        /// <param name="selectedRowIndex"></param>
        void EditSelectedStationRow(int selectedRowIndex)
        {
            //проверим на наличие изменений в объекте
            if (currentStationContext.HasAnyChangedData)
            {
                //Изменения выполнялись
                if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CreatOrUpdateStation();
                }
                else
                {
                    //сброс
                    ResetEditStation();
                }
            }
            else
            {
                //Изменения не выполнялись
                //сброс
                ResetEditStation();
            }
            int statId = (int)dgwStations.Rows[selectedRowIndex].Cells[0].Value;
            currentStationContext.CurrentStation = new Station
            {
                Name = dgwStations.Rows[selectedRowIndex].Cells[1].Value.ToString(),
                Description = dgwStations.Rows[selectedRowIndex].Cells[2].Value.ToString(),
                Id = statId
            };

            tbName.Text = currentStationContext.CurrentStation.Name;
            tbDescription.Text = currentStationContext.CurrentStation.Description;
        }

        private void btnRestStationEdit_Click(object sender, EventArgs e)
        {
            ResetEditStation();
        }

        /// <summary>
        /// Сброс изменений в редакторе СТО
        /// </summary>
        void ResetEditStation()
        {
            tbDescription.Text = tbName.Text = "";
            currentStationContext.Reset();
        }
        
        private void tbName_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любую попытку внесения изменений в редакторе СТО
            currentStationContext.HasAnyChangedData = true;
        }

        private void tbDescription_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любую попытку внесения изменений в редакторе СТО
            currentStationContext.HasAnyChangedData = true;
        }

        #endregion

        #region Услуги TAB
        /// <summary>
        /// Инициализация источников данных дл таблиц и комбиков с Услугами
        /// </summary>
        /// <param name="selectedStationId"></param>
        void InitServicesDataSource(int selectedStationId)
        {
            dgwServices.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            cbServices_WorksTab.DisplayMember = "Name";
            cbServices_WorksTab.ValueMember = "Id";
            try
            {
                servicesBindingSource.DataSource = serviceClient.SelectServices(selectedStationId);
                dgwServices.DataSource = servicesBindingSource;
                cbServices_WorksTab.DataSource = servicesBindingSource;
                if (cbServices_WorksTab.Items.Count > 0)
                    cbServices_WorksTab.SelectedIndex = 0;
                if(cbServices_WorksTab.SelectedValue != null)
                {
                    int selectedService = (int)cbServices_WorksTab.SelectedValue;
                    InitServedCarsDataGridView(selectedService);
                }

                InitSummaryAllStoDataSource();
                InitSummaryByStoDataSource();
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();

                dgwServices.DataSource = null;
                cbServices_WorksTab.DataSource = null;
                cbServices_WorksTab.Items.Clear();
            }
        }

        private void dgwServices_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgr in dgwServices.Rows)
            {
                dgr.Cells[0].OwningColumn.Visible = false;//скрываем колонку Id
                dgr.Cells[1].ReadOnly = true;
                dgr.Cells[2].ReadOnly = true;
                dgr.Cells[3].ReadOnly = true;
                dgr.Cells[4].ReadOnly = true;

                DataGridViewButtonCell removeCell = (DataGridViewButtonCell)dgr.Cells[5];
                removeCell.Value = "Удалить";
                removeCell.Style = new DataGridViewCellStyle();
                removeCell.Style.Font = new Font("Arial", 8, FontStyle.Underline);
            }
        }

        private void btnSaveService_Click(object sender, EventArgs e)
        {
            CreateOrUpdateService();
        }

        /// <summary>
        /// Создание новой или обновление редактируемой Услуги
        /// </summary>
        void CreateOrUpdateService()
        {
            //Проверка заполнения полей редактора
            if (!string.IsNullOrEmpty(tbNewServiceName.Text)
                && !String.IsNullOrEmpty(tbNewServiceDescription.Text)
                && !String.IsNullOrEmpty(tbNewServicePrice.Text))
            {
                decimal price = -1;
                if (decimal.TryParse(tbNewServicePrice.Text, out price) && price > 0)
                {
                    if (currentServiceContext.CurrentService != null)
                    {
                        //Обновление Услуги 
                        currentServiceContext.CurrentService.Name = tbNewServiceName.Text;
                        currentServiceContext.CurrentService.Description = tbNewServiceDescription.Text;
                        currentServiceContext.CurrentService.Price = price;
                    }
                    else
                    {
                        //Создание новой Услуги
                        currentServiceContext.CurrentService = new Service
                        {
                            Name = tbNewServiceName.Text,
                            Description = tbNewServiceDescription.Text,
                            Price = price,
                            StationId = (int)cbStations.SelectedValue
                        };
                    }

                    try
                    {
                        serviceClient.CreateOrUpdateService(currentServiceContext.CurrentService);
                        currentServiceContext.Reset();
                        servicesBindingSource.DataSource = serviceClient.SelectServices((int)cbStations.SelectedValue);

                        tbNewServicePrice.Text = tbNewServiceName.Text = tbNewServiceDescription.Text = "";
                    }
                    catch (FaultException fe)
                    {
                        ErrorForm erFrm = new ErrorForm(fe);
                        if (erFrm.ShowDialog() == DialogResult.Abort)
                            this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Не корректное значение в поле [Цена]", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Одно или несколько полей остались не заполненными.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Обработка нажатия на кнопку удаления Услуги
        private void dgwServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (MessageBox.Show("Действительно хотите удалить?", "Удалить Услугу?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int servId = (int)dgwServices.Rows[e.RowIndex].Cells[0].Value;
                    int statId = (int)dgwServices.Rows[e.RowIndex].Cells[4].Value;

                    try
                    {
                        //выполняем удаление Услуги
                        serviceClient.DeleteService(new Service
                        {
                            Id = servId,
                            Name = dgwServices.Rows[e.RowIndex].Cells[1].Value.ToString(),
                            Description = dgwServices.Rows[e.RowIndex].Cells[2].Value.ToString(),
                            Price = decimal.Parse(dgwServices.Rows[e.RowIndex].Cells[3].Value.ToString()),
                            StationId = statId
                        });
                        InitServicesDataSource((int)cbStations.SelectedValue);

                        //проверить не редактируется ли удаляемый объект
                        if (currentServiceContext.CurrentService != null)
                        {
                            //объект есть в редакторе
                            //является ли он удаляемым объектом
                            if (servId == currentServiceContext.CurrentService.Id)
                            {
                                //в редакторе именно удаляемый объект - делаем сброс редактора
                                ResetEditService();
                            }
                        }
                    }
                    catch (FaultException fe)
                    {
                        ErrorForm erFrm = new ErrorForm(fe);
                        if (erFrm.ShowDialog() == DialogResult.Abort)
                            this.Close();
                    }
                }
            }
        }

        private void dgwServices_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedServiceRow(e.RowIndex);//редактирование Услуги
        }

        private void dgwServices_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedServiceRow(e.RowIndex);//редактирование Услуги
        }

        /// <summary>
        /// Редактирование выбранной в DataGridView Услуги
        /// </summary>
        /// <param name="selectedRowIndex"></param>
        void EditSelectedServiceRow(int selectedRowIndex)
        {
            //проверим на наличие изменений в объекте
            if (currentServiceContext.HasAnyChangedData)
            {
                //Изменения выполнялись
                if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CreateOrUpdateService();//Сохраняем существующие изменения в редаторе
                }
                else
                {
                    //сброс
                    ResetEditService();
                }
            }
            else
            {
                //Изменения не выполнялись
                //сброс
                ResetEditService();
            }
            //Инициализация объекта контекста редактора Услуги
            int servId = (int)dgwServices.Rows[selectedRowIndex].Cells[0].Value;
            int statId = (int)dgwServices.Rows[selectedRowIndex].Cells[4].Value;
            currentServiceContext.CurrentService = new Service
            {
                Id = servId,
                Name = dgwServices.Rows[selectedRowIndex].Cells[1].Value.ToString(),
                Description = dgwServices.Rows[selectedRowIndex].Cells[2].Value.ToString(),
                Price = decimal.Parse(dgwServices.Rows[selectedRowIndex].Cells[3].Value.ToString()),
                StationId = statId
            };

            tbNewServiceName.Text = currentServiceContext.CurrentService.Name;
            tbNewServiceDescription.Text = currentServiceContext.CurrentService.Description;
            tbNewServicePrice.Text = currentServiceContext.CurrentService.Price.ToString();
        }

        private void btnResetEditService_Click(object sender, EventArgs e)
        {
            ResetEditService();//Сброс редактора и контекста Услуги 
        }

        /// <summary>
        /// Сброс редактора и контекста Услуги 
        /// </summary>
        void ResetEditService()
        {
            tbNewServiceName.Text = tbNewServiceDescription.Text = tbNewServicePrice.Text = "";
            currentServiceContext.Reset();
        }

        private void tbNewServiceName_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в поля формы редактора Услуги
            currentServiceContext.HasAnyChangedData = true;
        }

        private void tbNewServiceDescription_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в поля формы редактора Услуги
            currentServiceContext.HasAnyChangedData = true;
        }

        private void tbNewServicePrice_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в поля формы редактора Услуги
            currentServiceContext.HasAnyChangedData = true;
        }

        #endregion

        #region Работы TAB
        //Обработчик смены выбранной Услуги
        private void cbStations_WorksTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStations_WorksTab.SelectedValue != null)
            {
                //проверим на наличие изменений в объекте
                if (currentServedCarContext.HasAnyChangedData)
                {
                    //Изменения выполнялись
                    if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        CreateOrUpdateServedCar();//Сохраняем существующие изменения в редакторе Обслуживаемого Авто
                    }
                    else
                    {
                        //сброс
                        ResetEditServedCar();
                    }
                }
                else
                {
                    //Изменения не выполнялись
                    //сброс
                    ResetEditServedCar();
                }

                int selectedStation = (int)cbStations_WorksTab.SelectedValue;
                InitServicesDataSource(selectedStation);//Обновление источников данных Услуг

                if (cbServices_WorksTab.SelectedValue != null)
                {
                    int selectedService = (int)cbServices_WorksTab.SelectedValue;
                    InitServedCarsDataGridView(selectedService);//Обновление источников данных Обслуживаемых Авто
                }
                else
                {
                    cbServices_WorksTab.Text = "";
                    InitServedCarsDataGridView(-1);//Пустой набор
                }
            }
            else
            {
                cbStations_WorksTab.Text = "";
                InitServedCarsDataGridView(-1);//Пустой набор
            }
        }
        /// <summary>
        /// Инициализация источников данных для DataGridView Обслуживаемых Авто
        /// </summary>
        /// <param name="serviceId"></param>
        void InitServedCarsDataGridView(int serviceId)
        {
            dgwServedCars.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            try
            {
                if (serviceId > 0)
                {
                    servedCarsBindingSource.DataSource = serviceClient.SelectServedCars(serviceId);
                }
                else
                {
                    //сброс в пустой набор
                    servedCarsBindingSource.DataSource = serviceClient.SelectServedCars(0);
                }
                dgwServedCars.DataSource = servedCarsBindingSource;
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();
            }
        }

        /// <summary>
        /// Сброс редактора Обслуживаемого Авто
        /// </summary>
        void ResetEditServedCar()
        {
            tbCarBrand.Text = "";
            dtpSrviceCompleteDate.Value = dtpCarYear.Value = dtpCarYear.MinDate;
            currentServedCarContext.Reset();
        }
        /// <summary>
        /// Создание или обновление объекта Обслуживаемого Авто
        /// </summary>
        void CreateOrUpdateServedCar()
        {
            if (!string.IsNullOrEmpty(tbCarBrand.Text)
                && dtpCarYear.Value > DateTime.MinValue && dtpCarYear.Value < DateTime.Now
                && dtpSrviceCompleteDate.Value > DateTime.MinValue)
            {
                int selectedServiceId = (int)cbServices_WorksTab.SelectedValue;
                if (currentServedCarContext.CurrentServedCar != null)
                {
                    //Обновление Услуги
                    currentServedCarContext.CurrentServedCar.CarBrand = tbCarBrand.Text;
                    currentServedCarContext.CurrentServedCar.CarYear = dtpCarYear.Value;
                    currentServedCarContext.CurrentServedCar.ServiceCompletDate = dtpSrviceCompleteDate.Value;
                }
                else
                {
                    //Создание новой Услуги
                    currentServedCarContext.CurrentServedCar = new ServedCar
                    {
                        CarBrand = tbCarBrand.Text,
                        CarYear = dtpCarYear.Value,
                        ServiceCompletDate = dtpSrviceCompleteDate.Value,
                        ServiceId = selectedServiceId
                    };
                }

                try
                {
                    serviceClient.CreateOrUpdateServedCar(currentServedCarContext.CurrentServedCar);
                    ResetEditServedCar();//Сброс редактора Обслуживаемого Авто
                    //Обновление источника данных для таблицы Обслуживаемых Авто
                    servedCarsBindingSource.DataSource = serviceClient.SelectServedCars(selectedServiceId);
                    dgwServedCars.DataSource = servedCarsBindingSource;
                    //Обновление источников данных для Сводных таблиц
                    InitSummaryAllStoDataSource();
                    InitSummaryByStoDataSource();
                }
                catch (FaultException fe)
                {
                    ErrorForm erFrm = new ErrorForm(fe);
                    if (erFrm.ShowDialog() == DialogResult.Abort)
                        this.Close();
                }
            }
            else
            {
                MessageBox.Show("Одно или несколько полей остались не заполненными.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbServices_WorksTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStations_WorksTab.SelectedValue != null)
            {
                //проверим на наличие изменений в объекте
                if (currentServedCarContext.HasAnyChangedData)
                {
                    //Изменения выполнялись
                    if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        CreateOrUpdateServedCar();//Создаем или обновляем объект Обслуживаемого Авто
                    }
                    else
                    {
                        //сброс
                        ResetEditServedCar();
                    }
                }
                else
                {
                    //Изменения не выполнялись
                    //сброс
                    ResetEditServedCar();
                }

                if(cbServices_WorksTab.SelectedValue != null)
                {
                    int selectedService = (int)cbServices_WorksTab.SelectedValue;
                    InitServedCarsDataGridView(selectedService);//Набор Обслуживаемых Авто по выбранной Услуге
                }
            }
            else
            {
                cbServices_WorksTab.Text = "";
                InitServedCarsDataGridView(-1);//Пустой набор
            }
        }

        private void dgwServedCars_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgr in dgwServedCars.Rows)
            {
                dgr.Cells[0].OwningColumn.Visible = false;//скрываем колонку Id
                dgr.Cells[1].ReadOnly = true;
                dgr.Cells[2].ReadOnly = true;
                dgr.Cells[3].ReadOnly = true;
                dgr.Cells[4].ReadOnly = true;

                DataGridViewButtonCell removeCell = (DataGridViewButtonCell)dgr.Cells[5];
                removeCell.Value = "Удалить";
                removeCell.Style = new DataGridViewCellStyle();
                removeCell.Style.Font = new Font("Arial", 8, FontStyle.Underline);
            }
        }
        //Обработка нажатия кнопки Удалить в DataGridView Обслуживаемых Авто
        private void dgwServedCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (MessageBox.Show("Действительно хотите удалить?", "Удалить Авто?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int servedCarId = (int)dgwServedCars.Rows[e.RowIndex].Cells[0].Value;
                    int serviceId = (int)dgwServedCars.Rows[e.RowIndex].Cells[4].Value;

                    //проверить не редактируется ли удаляемый объект
                    if (currentServedCarContext.HasAnyChangedData)
                    {
                        //объект есть в редакторе
                        //является ли он удаляемым объектом
                        if (servedCarId == currentServedCarContext.CurrentServedCar.Id
                            && serviceId == currentServedCarContext.CurrentServedCar.ServiceId)
                        {
                            //в редакторе именно удаляемый объект - делаем сброс редактора
                            ResetEditServedCar();
                        }
                    }

                    try
                    {
                        //выполняем удаление Авто
                        serviceClient.DeleteServedCar(new ServedCar
                        {
                            Id = servedCarId,
                            CarBrand = dgwServedCars.Rows[e.RowIndex].Cells[1].Value.ToString(),
                            CarYear = (DateTime)dgwServedCars.Rows[e.RowIndex].Cells[2].Value,
                            ServiceCompletDate = (DateTime)dgwServedCars.Rows[e.RowIndex].Cells[3].Value,
                            ServiceId = serviceId
                        });
                        //Обновление источника дапнных для таблицы Обслуживаемых Авто
                        servedCarsBindingSource.DataSource = serviceClient.SelectServedCars((int)cbServices_WorksTab.SelectedValue);
                        dgwServedCars.DataSource = servedCarsBindingSource;
                        //Обновление источников данных для Сводных таблиц
                        InitSummaryAllStoDataSource();
                        InitSummaryByStoDataSource();
                    }
                    catch (FaultException fe)
                    {
                        ErrorForm erFrm = new ErrorForm(fe);
                        if (erFrm.ShowDialog() == DialogResult.Abort)
                            this.Close();
                    }
                }
            }
        }
        //Сохранение изменений в редакторе Обслуживаемого Авто
        private void btnSaveServedCar_Click(object sender, EventArgs e)
        {
            CreateOrUpdateServedCar();
        }
        //Сброс изменений в редакторе Обслуживаемого Авто
        private void btnResedServedCar_Click(object sender, EventArgs e)
        {
            ResetEditServedCar();
        }
        //Двойные клики по ячейкам или строке в DataGridView для отправки объекта на редактирование Обслуживаемого Авто
        private void dgwServedCars_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedServedCar(e.RowIndex);
        }

        private void dgwServedCars_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditSelectedServedCar(e.RowIndex);
        }
        /// <summary>
        /// Редактирование выбранного Обслуживаемого Авто
        /// </summary>
        /// <param name="selectedRowIndex"></param>
        public void EditSelectedServedCar(int selectedRowIndex)
        {
            //проверим на наличие изменений в редакторе
            if (currentServedCarContext.HasAnyChangedData)
            {
                //Изменения выполнялись
                if (MessageBox.Show("Хотите сохранить изменения?", "Обнаружены не сохраненный изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CreateOrUpdateServedCar();
                }
            }

            //сброс редактора Обслуживаемого Авто
            ResetEditServedCar();
            //Создаем объект Обслуживаемого Авто в контексте редактора
            int servedCarId = (int)dgwServedCars.Rows[selectedRowIndex].Cells[0].Value;
            int serviceId = (int)dgwServedCars.Rows[selectedRowIndex].Cells[4].Value;
            currentServedCarContext.CurrentServedCar = new ServedCar 
            { 
                Id = servedCarId, 
                CarBrand = dgwServedCars.Rows[selectedRowIndex].Cells[1].Value.ToString(),
                CarYear = (DateTime)dgwServedCars.Rows[selectedRowIndex].Cells[2].Value,
                ServiceCompletDate = (DateTime)dgwServedCars.Rows[selectedRowIndex].Cells[3].Value,
                ServiceId = serviceId
            };
            //Заполняем поля редактора данными из объекта контекста
            tbCarBrand.Text = currentServedCarContext.CurrentServedCar.CarBrand;
            dtpCarYear.Value = currentServedCarContext.CurrentServedCar.CarYear;
            dtpSrviceCompleteDate.Value = currentServedCarContext.CurrentServedCar.ServiceCompletDate;
        }

        private void tbCarBrand_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в редактор
            currentServedCarContext.HasAnyChangedData = true;
        }

        private void dtpCarYear_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в редактор
            currentServedCarContext.HasAnyChangedData = true;
        }

        private void dtpSrviceCompleteDate_KeyUp(object sender, KeyEventArgs e)
        {
            //Фиксируем любые попытки внесения изменений в редактор
            currentServedCarContext.HasAnyChangedData = true;
        }

        #endregion

        #region Сводка TAB
        /// <summary>
        /// Инициализация источника данных для Сводки по всем СТО
        /// </summary>
        void InitSummaryAllStoDataSource()
        {
            try
            {
                summaryByAllStoBindingSource.DataSource = serviceClient.SelectServedCarsByAllStation(dtPikStart.Value, dtPikEnd.Value);
                dgwSumAllSto.DataSource = summaryByAllStoBindingSource;
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();
            }
        }
        /// <summary>
        /// Инициализация источников данных для Сводки по конкретному СТО
        /// </summary>
        void InitSummaryByStoDataSource()
        {
            try
            {
                if (cbStationsSummaries.SelectedValue != null)
                {
                    int selectedStation = (int)cbStationsSummaries.SelectedValue;
                    summaryBySto.DataSource = serviceClient.SelectServedCarsByStation(selectedStation);
                    dgwServedCars_SumSto.DataSource = summaryBySto;
                }
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();
            }
        }

        private void dgwSumAllSto_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgr in dgwSumAllSto.Rows)
            {
                dgr.Cells[0].ReadOnly = true;
                dgr.Cells[1].ReadOnly = true;
                dgr.Cells[2].ReadOnly = true;
            }
        }
        //Зпросить Сводку с текущими настройками фильтра по датам
        private void btnFilterQueryDatesSum_Click(object sender, EventArgs e)
        {
            InitSummaryAllStoDataSource();
        }
        //Обновить Сводку по выбранному СТО
        private void cbStationsSummaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitSummaryByStoDataSource();
        }

        #endregion

        /// <summary>
        /// Загрузка начальных данных в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadFirstData_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClient.LoadFirstData();
                Init();
            }
            catch (FaultException fe)
            {
                ErrorForm erFrm = new ErrorForm(fe);
                if (erFrm.ShowDialog() == DialogResult.Abort)
                    this.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;

namespace StoDescktopClient.Forms
{
    /// <summary>
    /// Форма вывода сообщений об ошибках серверной стороны
    /// </summary>
    public partial class ErrorForm : Form
    {
        bool isWindowViewNormal = true;
        FaultException exception;
        public ErrorForm(FaultException ex)
        {
            InitializeComponent();
            
            exception = ex;
            tbErrorMessage.Text = $"{exception.Code}\r\n\tв {exception.Source} : {exception.Message}";
            tbErrorDetails.Text = $"{exception.InnerException}\r\n**********Текст ошибки**********\r\n{exception.StackTrace}";
            ApplyWindowViewStyle();
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            ApplyWindowViewStyle();
        }

        void ApplyWindowViewStyle()
        {
            if (isWindowViewNormal)
            {
                this.Size = new Size(this.Size.Width, this.Size.Height - 200);
                splitContainer1.Panel2.Hide();//Скрыть блок с деталями сообщения
                isWindowViewNormal = false;
            }
            else
            {
                this.Size = new Size(this.Size.Width, this.Size.Height + 200);
                splitContainer1.Panel2.Show();//Раскрыть блок с деталями сообщения
                isWindowViewNormal = true;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;//Продолжить работу
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;//Прервать работу приложения
            this.Close();
        }

        private void ErrorForm_Shown(object sender, EventArgs e)
        {
            tbErrorSource.SelectionLength = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace STO_Man_WcfService
{
    /// <summary>
    /// Класс перехвата исключений и переопределения в понятном для клиента формате
    /// </summary>
    public class CustomErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            //HandleError() не оказывает никакого воздействия на клиентское приложение.
            //вызывается после передачи управления клиенту и выполняется в отдельном потоке, нежели клиентский запрос и преобразование исключений 
            //В связи с этим, обработка ошибок может осуществляться даже после передачи управления клиенту
            try
            {
                //выполняем логирование ошибки
            }
            catch
            {

            }
            return false;//не останавливать вызов расширенной обработки исключений
        }

        //анализ входного исключения и формирование альтернативного сообщения об ошибке
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //вызывается сразу после того, как происходит исключение
            //клиент будет находиться в состоянии ожидания до тех пор, пока этот метод не закончит свою работу
            //следует остерегаться длительного выполнения ProvideFault()

            //fault = null;//подавление любых ошибок в контракте
            //т.е. клиент не будет знать о неизвестной ошибке на сервисе
            //но данная ошибка вызовет разрушение коммуникационного канала

            //необходимость определять контракт ошибок отсутствует
            ExceptionDetail detail = new ExceptionDetail(error);
            throw new FaultException<ExceptionDetail>(detail, error.Message);

            /* Обработка ошибок сервиса на клиенте
               try
               { ... }
             * catch(FaultException<ExceptionDetail> ex)
                {
                  MessageBox.Show(
                    null, 
                    string.Format("{0}\n{1}\n{2}", ex.Detail.Type, ex.Detail.Message,
                 ex.Detail.StackTrace),
                    Application.ProductName, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                }
             */
        }
    }
}
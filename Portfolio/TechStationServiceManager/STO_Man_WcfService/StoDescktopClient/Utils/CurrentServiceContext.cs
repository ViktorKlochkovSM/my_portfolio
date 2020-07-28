using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoDescktopClient.ServiceReference1;

namespace StoDescktopClient.Utils
{
    /// <summary>
    /// Класс контекста редактора объекта Услуга
    /// </summary>
    public class CurrentServiceContext : CurrentModelContext
    {
        /// <summary>
        /// Текущий объект Услуга
        /// </summary>
        public Service CurrentService { get; set; } = null;

        /// <summary>
        /// Выполняет отмену изменений и сброс текущего объекта
        /// </summary>
        public void Reset()
        {
            CancelChanges();
            CurrentService = null;
        }
    }
}

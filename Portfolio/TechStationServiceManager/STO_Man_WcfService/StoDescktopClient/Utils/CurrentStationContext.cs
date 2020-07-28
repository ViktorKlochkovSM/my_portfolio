using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoDescktopClient.ServiceReference1;

namespace StoDescktopClient.Utils
{
    /// <summary>
    /// Класс контекста редактора объекта СТО
    /// </summary>
    public class CurrentStationContext : CurrentModelContext
    {
        /// <summary>
        /// Текущий объект СТО
        /// </summary>
        public Station CurrentStation { get; set; } = null;

        /// <summary>
        /// Выполняет отмену изменений и сброс текущего объекта
        /// </summary>
        public void Reset()
        {
            CancelChanges();
            CurrentStation = null;
        }
    }
}

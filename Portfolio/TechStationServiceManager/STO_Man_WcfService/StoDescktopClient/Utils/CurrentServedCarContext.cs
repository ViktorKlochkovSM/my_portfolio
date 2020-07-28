using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoDescktopClient.ServiceReference1;

namespace StoDescktopClient.Utils
{
    /// <summary>
    /// Класс контекста редактора объекта Обслуживаемого Авто
    /// </summary>
    public class CurrentServedCarContext : CurrentModelContext
    {
        /// <summary>
        /// Текущий объект Обслуживаемый автомобиль
        /// </summary>
        public ServedCar CurrentServedCar { get; set; } = null;

        /// <summary>
        /// Выполняет отмену изменений и сброс текущего объекта
        /// </summary>
        public void Reset()
        {
            CancelChanges();
            CurrentServedCar = null;
        }
    }
}

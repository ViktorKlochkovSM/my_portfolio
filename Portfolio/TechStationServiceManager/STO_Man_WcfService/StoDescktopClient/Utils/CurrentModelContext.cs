using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoDescktopClient.Utils
{
    /// <summary>
    /// Базовый класс контекста редактора объекта
    /// </summary>
    public class CurrentModelContext
    {
        /// <summary>
        /// Фиксирует любую попытку внесения изменений в данные (текстовые поля редактирования/создания объекта)
        /// </summary>
        public bool HasAnyChangedData { get; set; }
        /// <summary>
        /// Сброс флага наличия изменений в объекте
        /// </summary>
        public void CancelChanges()
        {
            HasAnyChangedData = false;
        }
    }
}

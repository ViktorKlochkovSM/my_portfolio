using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace STO_Man_WcfService.Model
{
    /// <summary>
    /// Класс объекта Сводки по конкретным СТО
    /// </summary>
    [DataContract]
    public class SummaryByStation
    {
        /// <summary>
        /// Марка Авто
        /// </summary>
        [DataMember]
        public string CarBrand { get; set; }
        /// <summary>
        /// Год выпуска Авто
        /// </summary>
        [DataMember]
        public DateTime CarYear { get; set; }
        /// <summary>
        /// Наименование Услуги
        /// </summary>
        [DataMember]
        public string ServiceName { get; set; }
        /// <summary>
        /// Дата выполнения Услуги
        /// </summary>
        [DataMember]
        public DateTime ServiceCompleteDate { get; set; }
    }
}
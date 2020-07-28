using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace STO_Man_WcfService.Model
{
    /// <summary>
    /// Клас объекта Сводки по всем СТО
    /// </summary>
    [DataContract]
    public class SummaryByAllStations
    {
        /// <summary>
        /// Название СТО
        /// </summary>
        [DataMember]
        public string StationName { get; set; }
        /// <summary>
        /// Кол-во выполненных услуг
        /// </summary>
        [DataMember]
        public int CountCompletedServices { get; set; }
        /// <summary>
        /// Общая стоимость выполненных услуг
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }
    }
}
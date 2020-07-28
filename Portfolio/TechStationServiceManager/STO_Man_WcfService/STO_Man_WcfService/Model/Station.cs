using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace STO_Man_WcfService.Model
{
    /// <summary>
    /// Класс объекта СТО
    /// </summary>
    [DataContract]
    public class Station
    {
        [Key]
        [DataMember]
        public int Id { get; protected set; }
        /// <summary>
        /// Название СТО
        /// </summary>
        [Required]
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Описание СТО
        /// </summary>
        [Required]
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Список зависимых от СТО Услуг
        /// </summary>
        public virtual List<Service> Services { get; set; }
    }
}
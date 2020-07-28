using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STO_Man_WcfService.Model
{
    /// <summary>
    /// Класс объекта Услуга
    /// </summary>
    [DataContract]
    public class Service
    {
        [Key]
        [DataMember]
        public int Id { get; protected set; }
        /// <summary>
        /// Наименование Услуги
        /// </summary>
        [Required]
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Описание Услуги
        /// </summary>
        [Required]
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Цена услуги
        /// </summary>
        [Required]
        [DataMember]
        public decimal Price { get; set; }
        /// <summary>
        /// ID СТО
        /// </summary>
        [Required]
        [DataMember]
        public int StationId { get; set; }//внешний ключ

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }
        /// <summary>
        /// Список зависимых от Услуги автомобилей
        /// </summary>
        public virtual List<ServedCar> ServedCars { get; set; }

        public Service(int stationId)
        {
            StationId = stationId;
        }
    }
}
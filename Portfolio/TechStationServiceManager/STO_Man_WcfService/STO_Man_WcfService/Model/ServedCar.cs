using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STO_Man_WcfService.Model
{
    /// <summary>
    /// Класс объекта Обслуживаемый автмобиль
    /// </summary>
    [DataContract]
    public class ServedCar
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Марка Авто
        /// </summary>
        [Required]
        [DataMember]
        public string CarBrand { get; set; }
        /// <summary>
        /// Год выпуска Авто
        /// </summary>
        [Required]
        [DataMember]
        [DataType(DataType.Date)]
        public DateTime CarYear { get; set; }
        /// <summary>
        /// Дата обслуживания авто
        /// </summary>
        [Required]
        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime ServiceCompletDate { get; set; }
        /// <summary>
        /// ID Услуги
        /// </summary>
        [Required]
        [DataMember]
        public int ServiceId { get; set; }//внешний ключ

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        public ServedCar(int serviceId)
        {
            ServiceId = serviceId;
        }
    }
}
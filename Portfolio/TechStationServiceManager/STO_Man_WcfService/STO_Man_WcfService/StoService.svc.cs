using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Npgsql;
using STO_Man_WcfService.Model;
using Microsoft.EntityFrameworkCore;

namespace STO_Man_WcfService
{
    //Атрибут ServiceBehavior позволяет автоматически сделать так, чтобы все исключения, возникающие на сервисе и не определенные контрактом ошибок, 
    //передавались клиентской стороне в виде исключения типа FaultException<ExceptionDetail>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StoService : IStoService
    {
        #region Station

        public List<Station> SelectStations()
        {
            List<Station> stations = null;
            using (StoDbContext db = new StoDbContext())
            {
                stations = db.Stations.ToList<Station>();
            }
            return stations;
        }

        public Station SelectStation(int stationId)
        {
            Station station = null;
            using (StoDbContext db = new StoDbContext())
            {
                station = db.Stations.First<Station>(s => s.Id == stationId);
            }
            return station;
        }

        public Station CreateStation(Station station)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return station;
            }
        }

        public Station UpdateStation(Station station)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Stations.Update(station);
                db.SaveChanges();
                return station;
            }
        }

        public Station CreateOrUpdateStation(Station station)
        {
            if(station.Id > 0)
            {
                //Update
                return UpdateStation(station);
            }
            else
            {
                //Create
                return CreateStation(station);
            }
        }

        public void DeleteStation(Station station)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Stations.Remove(station);
                db.SaveChanges();
            }
        }

        #endregion

        #region Services

        public List<Service> SelectServices(int stationId)
        {
            List<Service> services = null;
            using (StoDbContext db = new StoDbContext())
            {
                services = db.Services.Where<Service>(s => s.StationId == stationId).ToList<Service>();
            }
            return services;
        }

        public Service SelectService(int serviceId)
        {
            Service service = null;
            using (StoDbContext db = new StoDbContext())
            {
                service = db.Services.First<Service>(s => s.Id == serviceId);
            }
            return service;
        }

        public Service CreateService(Service service)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Services.Add(service);
                db.SaveChanges();
                return service;
            }
        }

        public Service UpdateService(Service service)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Services.Update(service);
                db.SaveChanges();
                return service;
            }
        }

        public Service CreateOrUpdateService(Service service)
        {
            if(service.Id > 0)
            {
                //Update
                return UpdateService(service);
            }
            else
            {
                //Create
                return CreateService(service);
            }
        }

        public void DeleteService(Service service)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Services.Remove(service);
                db.SaveChanges();
            }
        }

        #endregion

        #region ServedCars

        //выбор обслуженных авто по конкретной услуге
        public List<ServedCar> SelectServedCars(int serviceId)
        {
            List<ServedCar> servedCars = null;
            using (StoDbContext db = new StoDbContext())
            {
                servedCars = db.ServedCars.Where<ServedCar>(s => s.ServiceId == serviceId).ToList<ServedCar>();
            }
            return servedCars;
        }

        public ServedCar SelectServedCar(int servedCarId)
        {
            ServedCar servedCar = null;
            using (StoDbContext db = new StoDbContext())
            {
                servedCar = db.ServedCars.First<ServedCar>(s => s.Id == servedCarId);
            }
            return servedCar;
        }

        public ServedCar CreateServedCar(ServedCar servedCar)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.ServedCars.Add(servedCar);
                db.SaveChanges();
                return servedCar;
            }
        }

        public ServedCar UpdateServedCar(ServedCar servedCar)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.ServedCars.Update(servedCar);
                db.SaveChanges();
                return servedCar;
            }
        }

        public ServedCar CreateOrUpdateServedCar(ServedCar servedCar)
        {
            if(servedCar.Id > 0)
            {
                //Update
                return UpdateServedCar(servedCar);
            }
            else
            {
                //Create
                return CreateServedCar(servedCar);
            }
        }

        public void DeleteServedCar(ServedCar servedCar)
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.ServedCars.Remove(servedCar);
                db.SaveChanges();
            }
        }

        #endregion

        #region Summaries

        public List<SummaryByStation> SelectServedCarsByStation(int stationId)
        {
            List<SummaryByStation> servedCars = null;
            using (StoDbContext db = new StoDbContext())
            {
                servedCars = db.ServedCars
                    .Include(s => s.Service)
                    .Include(st => st.Service.Station)
                    .Where(s => s.Service.StationId == stationId)
                    .Select(s => new SummaryByStation
                    { 
                        CarBrand = s.CarBrand,
                        CarYear = s.CarYear,
                        ServiceName = s.Service.Name,
                        ServiceCompleteDate = s.ServiceCompletDate
                    })
                    .ToList();
            }
            
            return servedCars;
        }

        public List<SummaryByAllStations> SelectServedCarsByAllStation(DateTime dateBegin, DateTime dateEnd)
        {
            List<SummaryByAllStations> servedCars = new List<SummaryByAllStations>();
            using (StoDbContext db = new StoDbContext())
            {
                var testCars =
                    db.ServedCars
                    .Where(s => s.ServiceCompletDate >= dateBegin && s.ServiceCompletDate <= dateEnd)
                    .Include(s => s.Service)
                    .Include(s => s.Service.Station)
                    .ToList()
                    .GroupBy(g => g.Service.Station.Name);

                foreach(var car in testCars)
                {
                    decimal totalPrice = 0;
                    int serviceCout = 0;

                    string stationName = car.Key;
                    foreach(var l in car.ToList())
                    {
                        totalPrice += l.Service.Price;
                        serviceCout++;
                    }

                    SummaryByAllStations summaryTbl = new SummaryByAllStations
                    {
                        StationName = stationName,
                        CountCompletedServices = serviceCout,
                        TotalPrice = totalPrice
                    };
                    servedCars.Add(summaryTbl);
                }
            }
            //Проверка проброса исключений
            //throw new Exception("My UNKNOWN Ex");

            return servedCars;
        }

        #endregion

        #region Загрузка первоначальных данных в БД

        public void LoadFirstData()
        {
            using (StoDbContext db = new StoDbContext())
            {
                db.Stations.Add(new Station { Name = "BusTed STO", Description = "СТО Теда - обслуживание и ремонт автобусов" });
                db.SaveChanges();

                db.Services.Add(new Service(1) { Name = "Замена подшипника рулевой рейки", Description = "Замена подшипника рулевой рейки. Подшипник предоставляется автовладельцем.", Price = 3000 });
                db.Services.Add(new Service(1) { Name = "Проверка тех-состояния подвески", Description = "Проверка технического состояния подвески (рессоры и аммортизаторы)", Price = 6000 });
                db.Services.Add(new Service(1) { Name = "Замена неисправных аммортизаторов", Description = "Замена неисправных аммортизаторов подвески. Новые аммотризаторы предоставляет автовладелец", Price = 4500 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(1) { CarBrand = "Hyundai", CarYear = new DateTime(2011, 2, 15), ServiceCompletDate = new DateTime(2014, 05, 21, 12, 40, 0) });
                db.ServedCars.Add(new ServedCar(2) { CarBrand = "ISUZU", CarYear = new DateTime(2012, 10, 25), ServiceCompletDate = new DateTime(2015, 03, 11, 9, 15, 0) });
                db.ServedCars.Add(new ServedCar(3) { CarBrand = "Solaris", CarYear = new DateTime(2014, 4, 23), ServiceCompletDate = new DateTime(2017, 11, 7, 8, 0, 0) });
                db.ServedCars.Add(new ServedCar(1) { CarBrand = "VOLVO", CarYear = new DateTime(2016, 1, 5), ServiceCompletDate = new DateTime(2017, 4, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(2) { CarBrand = "KIA", CarYear = new DateTime(2013, 8, 15), ServiceCompletDate = new DateTime(2018, 6, 17, 14, 40, 0) });
                db.ServedCars.Add(new ServedCar(3) { CarBrand = "IVECO", CarYear = new DateTime(2010, 2, 12), ServiceCompletDate = new DateTime(2015, 9, 12, 16, 25, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "BMW Service", Description = "Сервис автомобилей марки BMW" });
                db.SaveChanges();

                db.Services.Add(new Service(2) { Name = "Регулировака фар", Description = "Регулировка светового пятна в головных фарах", Price = 2000 });
                db.Services.Add(new Service(2) { Name = "Полировка фар", Description = "Полировка защитного стекла фар головного освещения с использованием спец. поолироли и ветоши", Price = 5000 });
                db.Services.Add(new Service(2) { Name = "Замена неисправного предохранителя", Description = "Замена неисправного предохранителя в электросистеме", Price = 200 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(4) { CarBrand = "BMW S-3 GT", CarYear = new DateTime(2013, 6, 11), ServiceCompletDate = new DateTime(2019, 3, 4, 12, 0, 0) });
                db.ServedCars.Add(new ServedCar(5) { CarBrand = "BMW M2", CarYear = new DateTime(2018, 3, 8), ServiceCompletDate = new DateTime(2020, 3, 1, 17, 10, 0) });
                db.ServedCars.Add(new ServedCar(6) { CarBrand = "BMW X5", CarYear = new DateTime(2003, 7, 18), ServiceCompletDate = new DateTime(2005, 9, 15, 13, 50, 0) });
                db.ServedCars.Add(new ServedCar(4) { CarBrand = "BMW X6", CarYear = new DateTime(2008, 8, 8), ServiceCompletDate = new DateTime(2012, 5, 23, 10, 50, 0) });
                db.ServedCars.Add(new ServedCar(5) { CarBrand = "BMW X7", CarYear = new DateTime(2019, 11, 18), ServiceCompletDate = new DateTime(2019, 9, 21, 16, 20, 0) });
                db.ServedCars.Add(new ServedCar(6) { CarBrand = "BMW M6 Gran Coupe", CarYear = new DateTime(2013, 2, 6), ServiceCompletDate = new DateTime(2017, 3, 11, 15, 30, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "Mitsubishi Trans", Description = "Обслуживание автомобилей марки Mitsubishi" });
                db.SaveChanges();

                db.Services.Add(new Service(3) { Name = "Полная покраска автомобиля", Description = "Нанесение лакокрасочного покрытия на авто по технологии покраски. 1 слой грнта, 2 слоя краски, 1 слой лака, сущка после каждого шага, финальная полировка ", Price = 90000 });
                db.Services.Add(new Service(3) { Name = "Реставрационный ремонт бампера", Description = "Восстановление целостности бампера(пайка трещин), восстановление лакокрасочного покрытия", Price = 15000 });
                db.Services.Add(new Service(3) { Name = "Регулировка угла схождения передних колес", Description = "Регулировка угла схождения передних колес, с использованием лазерного стэнда", Price = 3000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(7) { CarBrand = "Mitsubishi Outlander", CarYear = new DateTime(2018, 8, 24), ServiceCompletDate = new DateTime(2019, 11, 5, 10, 30, 0) });
                db.ServedCars.Add(new ServedCar(8) { CarBrand = "Mitsubishi Pajero", CarYear = new DateTime(2017, 8, 21), ServiceCompletDate = new DateTime(2019, 7, 15, 11, 20, 0) });
                db.ServedCars.Add(new ServedCar(9) { CarBrand = "Mitsubishi Pajero Sport", CarYear = new DateTime(2015, 4, 11), ServiceCompletDate = new DateTime(2017, 6, 14, 18, 40, 0) });
                db.ServedCars.Add(new ServedCar(7) { CarBrand = "Mitsubishi Eclipse Cross", CarYear = new DateTime(2017, 9, 25), ServiceCompletDate = new DateTime(2019, 4, 3, 14, 20, 0) });
                db.ServedCars.Add(new ServedCar(8) { CarBrand = "Mitsubishi ASX", CarYear = new DateTime(2017, 7, 14), ServiceCompletDate = new DateTime(2019, 2, 21, 13, 50, 0) });
                db.ServedCars.Add(new ServedCar(9) { CarBrand = "Mitsubishi L200", CarYear = new DateTime(2019, 3, 27), ServiceCompletDate = new DateTime(2019, 11, 21, 10, 40, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "Porsche Service", Description = "Ремонт и обслуживание автомобилей марок Porshe" });
                db.SaveChanges();

                db.Services.Add(new Service(4) { Name = "Замена и регулировка плунжеров", Description = "Замена и регулировка плунжерного стэка системы подачи топлива", Price = 25000 });
                db.Services.Add(new Service(4) { Name = "Замена форсунок омывателя лобового стекла", Description = "Замена форсунок омывателя лобового стекла", Price = 3000 });
                db.Services.Add(new Service(4) { Name = "Замена троса ручника", Description = "Замена троса ручного тормоза", Price = 12000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(10) { CarBrand = "718 Cayman", CarYear = new DateTime(2017, 6, 14), ServiceCompletDate = new DateTime(2019, 5, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(11) { CarBrand = "911 Carrera", CarYear = new DateTime(2018, 1, 12), ServiceCompletDate = new DateTime(2019, 10, 25, 10, 10, 0) });
                db.ServedCars.Add(new ServedCar(12) { CarBrand = "911 Turbo S", CarYear = new DateTime(2017, 6, 19), ServiceCompletDate = new DateTime(2018, 4, 25, 11, 50, 0) });
                db.ServedCars.Add(new ServedCar(10) { CarBrand = "911 Targa 4S", CarYear = new DateTime(2016, 2, 27), ServiceCompletDate = new DateTime(2017, 8, 23, 16, 10, 0) });
                db.ServedCars.Add(new ServedCar(11) { CarBrand = "Panamera 4S Executive", CarYear = new DateTime(2017, 12, 27), ServiceCompletDate = new DateTime(2019, 7, 3, 9, 40, 0) });
                db.ServedCars.Add(new ServedCar(12) { CarBrand = "Panamera 4E-Hybrid", CarYear = new DateTime(2018, 10, 8), ServiceCompletDate = new DateTime(2020, 2, 17, 11, 20, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "MAN Trans", Description = "Ремонт и обслуживание большегрузных автомобилей MAN" });
                db.SaveChanges();

                db.Services.Add(new Service(5) { Name = "Замена сальников в гидро-подъемнике кузова", Description = "Замена сальников в гидро-подъемнике кузова. Сальники предоставляет владелец авто.", Price = 8000 });
                db.Services.Add(new Service(5) { Name = "Замена задних фонарей", Description = "Замена задних фонарей. Предоставляются автовладельцем.", Price = 2000 });
                db.Services.Add(new Service(5) { Name = "Регулировка давления в гидронасосе подъемника кузова", Description = "Регулировка давления в гидронасосе подъемника кузова.", Price = 6000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(13) { CarBrand = "MAN TGA", CarYear = new DateTime(2000, 5, 17), ServiceCompletDate = new DateTime(2015, 6, 4, 15, 20, 0) });
                db.ServedCars.Add(new ServedCar(14) { CarBrand = "MAN TGX", CarYear = new DateTime(2007, 3, 20), ServiceCompletDate = new DateTime(2014, 3, 24, 10, 20, 0) });
                db.ServedCars.Add(new ServedCar(15) { CarBrand = "MAN TGM", CarYear = new DateTime(2005, 10, 17), ServiceCompletDate = new DateTime(2012, 8, 22, 9, 30, 0) });
                db.ServedCars.Add(new ServedCar(13) { CarBrand = "MAN L2000", CarYear = new DateTime(1993, 8, 22), ServiceCompletDate = new DateTime(2006, 5, 12, 10, 40, 0) });
                db.ServedCars.Add(new ServedCar(14) { CarBrand = "MAN Lion Coach", CarYear = new DateTime(2007, 7, 24), ServiceCompletDate = new DateTime(2012, 1, 8, 15, 20, 0) });
                db.ServedCars.Add(new ServedCar(15) { CarBrand = "MAN Lion", CarYear = new DateTime(2005, 9, 14), ServiceCompletDate = new DateTime(2010, 6, 17, 13, 50, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "Mercedes Trans", Description = "Ремонт и обслуживание легковых, грузовых автомобилей и автобусов Mercedess" });
                db.SaveChanges();

                db.Services.Add(new Service(6) { Name = "Ремонт электро-замков пассажирских дверей", Description = "Ремонт электро-замков пассажирских дверей", Price = 8000 });
                db.Services.Add(new Service(6) { Name = "Прфилактика ремонт кондиционара салона", Description = "Прфилактика и ремонт кондиционара салона", Price = 12000 });
                db.Services.Add(new Service(6) { Name = "Замена тормозной жидкости и прокачка тормозной системы", Description = "Замена тормозной жидкости и прокачка тормозной системы. Тормозная жидкость предоставляется автовладельцем.", Price = 4300 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(16) { CarBrand = "Mercedes AMG GT Roadster", CarYear = new DateTime(2016, 6, 25), ServiceCompletDate = new DateTime(2019, 2, 11, 10, 50, 0) });
                db.ServedCars.Add(new ServedCar(17) { CarBrand = "Mercedes C-class Coupe", CarYear = new DateTime(2018, 2, 19), ServiceCompletDate = new DateTime(2020, 2, 11, 17, 20, 0) });
                db.ServedCars.Add(new ServedCar(18) { CarBrand = "Mercedes EQC", CarYear = new DateTime(2018, 9, 5), ServiceCompletDate = new DateTime(2020, 1, 2, 12, 40, 0) });
                db.ServedCars.Add(new ServedCar(16) { CarBrand = "Mercedes GLS", CarYear = new DateTime(2019, 3, 12), ServiceCompletDate = new DateTime(2020, 2, 12, 10, 40, 0) });
                db.ServedCars.Add(new ServedCar(17) { CarBrand = "Mercedes V-class", CarYear = new DateTime(2014, 3, 12), ServiceCompletDate = new DateTime(2017, 5, 4, 15, 20, 0) });
                db.ServedCars.Add(new ServedCar(18) { CarBrand = "Mercedes Vito Van", CarYear = new DateTime(2014, 8, 2), ServiceCompletDate = new DateTime(2018, 11, 2, 11, 30, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "AUDI Service", Description = "Обслуживание и ремонт автомобилей Audi" });
                db.SaveChanges();

                db.Services.Add(new Service(7) { Name = "Замена турбины", Description = "Замена турбины в системе нагнетания воздуха", Price = 32000 });
                db.Services.Add(new Service(7) { Name = "Замена и регулировка тяг рулевой рейки", Description = "Замена и регулировка всех тяг рулевой рейки", Price = 24000 });
                db.Services.Add(new Service(7) { Name = "Установка сигнализации с центральным замком", Description = "Установка сигнализации с центральным замком. Сигнализацию предоставляет автовладелец.", Price = 31000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(19) { CarBrand = "Audi A3 Sportback", CarYear = new DateTime(2016, 1, 5), ServiceCompletDate = new DateTime(2017, 4, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(20) { CarBrand = "Audi A4", CarYear = new DateTime(2015, 4, 11), ServiceCompletDate = new DateTime(2017, 6, 14, 18, 40, 0) });
                db.ServedCars.Add(new ServedCar(21) { CarBrand = "Audi e-tron", CarYear = new DateTime(2018, 9, 5), ServiceCompletDate = new DateTime(2020, 1, 2, 12, 40, 0) });
                db.ServedCars.Add(new ServedCar(19) { CarBrand = "Audi Q5", CarYear = new DateTime(2016, 1, 5), ServiceCompletDate = new DateTime(2017, 4, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(20) { CarBrand = "Audi RS 4 Avant", CarYear = new DateTime(2017, 6, 14), ServiceCompletDate = new DateTime(2019, 5, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(21) { CarBrand = "Audi S5", CarYear = new DateTime(2016, 2, 27), ServiceCompletDate = new DateTime(2017, 8, 23, 16, 10, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "NISSAN Service", Description = "Обслуживание автомобилей Nissan" });
                db.SaveChanges();

                db.Services.Add(new Service(8) { Name = "Замена электро-стеклоподъемников в дверях", Description = "Замена электро-стеклоподъемников в дверях. Новые стеклоподъемники предоставляются автовладельцем.", Price = 21000 });
                db.Services.Add(new Service(8) { Name = "Установка газового оборудования 4-го поколения", Description = "Установка газового оборудования 4-го поколения. ГБО предоставляется автовладельцем.", Price = 44000 });
                db.Services.Add(new Service(8) { Name = "Ремонт приборов в щитке приборной панели", Description = "Ремонт приборов в щитке приборной панели на центральной консоли торпедо.", Price = 11000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(22) { CarBrand = "Nissan Almera", CarYear = new DateTime(2013, 8, 15), ServiceCompletDate = new DateTime(2018, 6, 17, 14, 40, 0) });
                db.ServedCars.Add(new ServedCar(23) { CarBrand = "Nissan GT-R", CarYear = new DateTime(2016, 1, 5), ServiceCompletDate = new DateTime(2017, 4, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(24) { CarBrand = "Nissan X-Trail", CarYear = new DateTime(2018, 9, 5), ServiceCompletDate = new DateTime(2020, 1, 2, 12, 40, 0) });
                db.ServedCars.Add(new ServedCar(22) { CarBrand = "Nissan Murano", CarYear = new DateTime(2015, 4, 11), ServiceCompletDate = new DateTime(2017, 6, 14, 18, 40, 0) });
                db.ServedCars.Add(new ServedCar(23) { CarBrand = "Nissan Terrano", CarYear = new DateTime(2017, 6, 14), ServiceCompletDate = new DateTime(2019, 5, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(24) { CarBrand = "Nissan Qashqai", CarYear = new DateTime(2014, 8, 2), ServiceCompletDate = new DateTime(2018, 11, 2, 11, 30, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "Lamborgini SportService", Description = "Обслуживание и ремонт спортивных автомобилей Lamborgini" });
                db.SaveChanges();

                db.Services.Add(new Service(9) { Name = "Установка усиленной подвески ходовой части", Description = "Установка усиленной подвески ходовой части. Все комплектуюущие предоставляет автовладелец.", Price = 154000 });
                db.Services.Add(new Service(9) { Name = "Замена титановых дисков колес", Description = "Замена титановых дисков колес. Новые диски предоставляет автовладелец.", Price = 8000 });
                db.Services.Add(new Service(9) { Name = "Усовершенствование системы подачи топлива", Description = "Усовершенствование системы подачи топлива путем добавления ускорителя впрыска. Все сопутствующие комплектующие поставляются автовладельцем.", Price = 36000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(25) { CarBrand = "Lamborghini Aventador", CarYear = new DateTime(2013, 8, 15), ServiceCompletDate = new DateTime(2018, 6, 17, 14, 40, 0) });
                db.ServedCars.Add(new ServedCar(26) { CarBrand = "Lamborghini Diablo", CarYear = new DateTime(2007, 7, 24), ServiceCompletDate = new DateTime(2012, 1, 8, 15, 20, 0) });
                db.ServedCars.Add(new ServedCar(27) { CarBrand = "Lamborghini Gallardo", CarYear = new DateTime(2014, 8, 2), ServiceCompletDate = new DateTime(2018, 11, 2, 11, 30, 0) });
                db.ServedCars.Add(new ServedCar(25) { CarBrand = "Lamborghini Murcielago", CarYear = new DateTime(2005, 10, 17), ServiceCompletDate = new DateTime(2012, 8, 22, 9, 30, 0) });
                db.ServedCars.Add(new ServedCar(26) { CarBrand = "Lamborghini Reventon", CarYear = new DateTime(2017, 6, 14), ServiceCompletDate = new DateTime(2019, 5, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(27) { CarBrand = "Lamborghini Huracan LP 610", CarYear = new DateTime(2018, 9, 5), ServiceCompletDate = new DateTime(2020, 1, 2, 12, 40, 0) });
                db.SaveChanges();

                db.Stations.Add(new Station { Name = "Toyota Trans", Description = "Обслуживание и ремонт автомобилей Toyota" });
                db.SaveChanges();

                db.Services.Add(new Service(10) { Name = "Ремонт ходовой части", Description = "Ремонт ходовой части. Замена неисправных частей.", Price = 27000 });
                db.Services.Add(new Service(10) { Name = "Замена поршневой группы", Description = "Замена поршневой группы в цилиндрах.", Price = 52000 });
                db.Services.Add(new Service(10) { Name = "Диагностика всех систем", Description = "Диагностика всех систем с использованием ПК и подключением к бортовому компьютеру.", Price = 5000 });
                db.SaveChanges();

                db.ServedCars.Add(new ServedCar(28) { CarBrand = "Camry", CarYear = new DateTime(2014, 8, 2), ServiceCompletDate = new DateTime(2018, 11, 2, 11, 30, 0) });
                db.ServedCars.Add(new ServedCar(29) { CarBrand = "RAV4", CarYear = new DateTime(2015, 4, 11), ServiceCompletDate = new DateTime(2017, 6, 14, 18, 40, 0) });
                db.ServedCars.Add(new ServedCar(30) { CarBrand = "Fortuner", CarYear = new DateTime(2016, 1, 5), ServiceCompletDate = new DateTime(2017, 4, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(28) { CarBrand = "Hilux", CarYear = new DateTime(2017, 6, 14), ServiceCompletDate = new DateTime(2019, 5, 7, 14, 10, 0) });
                db.ServedCars.Add(new ServedCar(29) { CarBrand = "Alphard", CarYear = new DateTime(2018, 9, 5), ServiceCompletDate = new DateTime(2020, 1, 2, 12, 40, 0) });
                db.ServedCars.Add(new ServedCar(30) { CarBrand = "Land Cruiser 200", CarYear = new DateTime(2018, 3, 19), ServiceCompletDate = new DateTime(2020, 2, 18, 11, 20, 0) });
                db.SaveChanges();
            }
        }

        #endregion
    }
}

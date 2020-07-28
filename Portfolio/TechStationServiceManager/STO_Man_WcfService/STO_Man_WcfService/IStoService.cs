using System;
using System.Collections.Generic;
using System.ServiceModel;
using STO_Man_WcfService.Model;

namespace STO_Man_WcfService
{
    [ServiceContract]
    public interface IStoService
    {
        [OperationContract]
        List<Station> SelectStations();//Выбрать все СТО
        [OperationContract]
        Station SelectStation(int stationId);//Выбрать конкретную СТО
        [OperationContract]
        Station CreateStation(Station station);//Создать новую СТО и добавить в БД
        [OperationContract]
        Station UpdateStation(Station station);//Обновить СТО в БД
        [OperationContract]
        Station CreateOrUpdateStation(Station station);//Создание или обновление СТО в БД
        [OperationContract]
        void DeleteStation(Station station);//Удаление СТО

        [OperationContract]
        List<Service> SelectServices(int stationId);//Выбор Услуги в конкретном СТО
        [OperationContract]
        Service SelectService(int serviceId);//Выбор конкретной Услуги
        [OperationContract]
        Service CreateService(Service service);//Создание новой Услуги и добавление ее в БД
        [OperationContract]
        Service UpdateService(Service service);//Обновление Услуги в БД
        [OperationContract]
        Service CreateOrUpdateService(Service service);//Создание или обновление Услуги в БД
        [OperationContract]
        void DeleteService(Service service);//Удаление Услуги

        
        [OperationContract]
        List<ServedCar> SelectServedCars(int serviceId);//выбор Обслуженных Авто по конкретной услуге
        [OperationContract]
        ServedCar SelectServedCar(int servedCarId);//Выбор конкретного Обслуживаемого Авто
        [OperationContract]
        ServedCar CreateServedCar(ServedCar servedCar);//Создание нового Обслуживаемого Авто и добавление его в БД
        [OperationContract]
        ServedCar UpdateServedCar(ServedCar servedCar);//Обновление Обслуживаемого Авто в БД
        [OperationContract]
        ServedCar CreateOrUpdateServedCar(ServedCar servedCar);//Создание или обновление Обслуживаемого Авто в БД
        [OperationContract]
        void DeleteServedCar(ServedCar servedCar);//Удаление Обслуживаемого Авто

        //Сводка
        [OperationContract]
        List<SummaryByStation> SelectServedCarsByStation(int stationId);//Запрос Сводки по конкретным СТО

        [OperationContract]
        List<SummaryByAllStations> SelectServedCarsByAllStation(DateTime dateBegin, DateTime dateEnd); //Запрос Сводки по всем СТО

         //Загрузка первоначальных данных в БД
         [OperationContract]
        void LoadFirstData();
    }
}

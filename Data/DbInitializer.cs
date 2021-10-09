using System.Linq;
using Entities;

namespace Data
{
    /// <summary>
    ///     DbInitializer используется для проверки наличия БД,
    ///     создания БД (если её ещё нет) и для первичного добавления данных
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        ///     метод Initialize проверяет, создана ли уже БД, если нет, то создаёт её
        ///     и добавляет первичные данные
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(DataContext context)
        {
            var createdNow = context.Database.EnsureCreated();

            // если БД уже существовала
            if (!createdNow) return;

            var companies = new CompanyEntity[]
            {
                new CompanyEntity()
                {
                    CompanyId = 101,
                    CompanyName = "Yandex",
                    IsDeleted = false
                },

                new CompanyEntity()
                {
                    CompanyId = 102,
                    CompanyName = "Uber",
                    IsDeleted = false
                },

                new CompanyEntity()
                {
                    CompanyId = 103,
                    CompanyName = "YouDrive",
                    IsDeleted = false
                },

                new CompanyEntity()
                {
                    CompanyId = 104,
                    CompanyName = "Делимобиль",
                    IsDeleted = false
                },

                new CompanyEntity()
                {
                    CompanyId = 105,
                    CompanyName = "MyTaxi",
                    IsDeleted = false
                }
            };

            context.Companies.AddRange(companies);

            var drivers = new DriverEntity[]
            {
                new DriverEntity()
                {
                    Name = "William",
                    CompanyId = 101,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Mary",
                    CompanyId = 105,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Marta",
                    CompanyId = 102,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Bobby",
                    CompanyId = 103,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Jack",
                    CompanyId = 103,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Summer",
                    CompanyId = 101,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Rose",
                    CompanyId = 104,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Deacon",
                    CompanyId = 103,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Rory",
                    CompanyId = 101,
                    IsDeleted = false
                },

                new DriverEntity()
                {
                    Name = "Cara",
                    CompanyId = 102,
                    IsDeleted = false
                }
            };

            context.Drivers.AddRange(drivers);

            context.SaveChanges();

            // так как CompanyId ТС должен совпадать с CompanyId водителя данного ТС, 
            // а какой водитель под каким Id окажется мы узнаем только после того, 
            // как водители будут добавлены в БД, то приходится получать добавленных 
            // водителей из БД, вычислять их CompanyId и добавлять в CompanyId ТС
            var driversFromDb = context.Drivers.ToList();

            var vehicles = new VehicleEntity[]
            {
                new VehicleEntity()
                {
                    Model = "Honda Civic",
                    GovernmentNumber = "О561ВС178",
                    DriverId = 1,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 1).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Nissan GT-R",
                    GovernmentNumber = "Х260СР178",
                    DriverId = 2,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 2).CompanyId,
                    IsDeleted = false
                },
                new VehicleEntity()
                {
                    Model = "Hyundai Tucson",
                    GovernmentNumber = "К327ТУ98",
                    DriverId = 3,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 3).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Honda CR-V",
                    GovernmentNumber = "О578ОР198",
                    DriverId = 4,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 4).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Lada Xray",
                    GovernmentNumber = "В666РМ198",
                    DriverId = 5,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 5).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Toyota Supra",
                    GovernmentNumber = "В452НМ47",
                    DriverId = 6,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 6).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Audi TT",
                    GovernmentNumber = "Х500АК78",
                    DriverId = 7,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 7).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Nissan 350Z",
                    GovernmentNumber = "В202УК47",
                    DriverId = 8,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 8).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Kia Mohave",
                    GovernmentNumber = "К597РС147",
                    DriverId = 9,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 9).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "BMW M2",
                    GovernmentNumber = "Н267РЕ47",
                    CompanyId = 102,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Hyundai Solaris",
                    GovernmentNumber = "К763НК55",
                    DriverId = 7,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 7).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Kia Rio",
                    GovernmentNumber = "Р811СР63",
                    DriverId = 7,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 7).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Audi R8",
                    GovernmentNumber = "Р568РВ90",
                    DriverId = 2,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 2).CompanyId,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "BMW X5",
                    GovernmentNumber = "У540ВУ46",
                    CompanyId = 105,
                    IsDeleted = false
                },

                new VehicleEntity()
                {
                    Model = "Lada Vesta",
                    GovernmentNumber = "С426ОА17",
                    DriverId = 8,
                    CompanyId = driversFromDb.FirstOrDefault(driver => driver.Id == 8).CompanyId,
                    IsDeleted = false
                }
            };

            context.Vehicles.AddRange(vehicles);

            context.SaveChanges();
        }
    }
}

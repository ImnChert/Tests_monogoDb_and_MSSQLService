using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Services.Implementations.Functional_entities;
using ApplicationCore.Services.Implementations.Validations;
using ApplicationCore.Services.Interfaces.Functional_entities;

namespace IntegratedTests
{
	[TestCaseOrderer("IntegratedTests.Configuration", "IntegratedTests")]
	public class ScheduleFunctionIntegratedTest
	{
		private readonly string _connectionString
			= @"Data Source=DESKTOP-CTBUCT0\SQLEXPRESS;;Initial Catalog=CP;Integrated Security=True";

		private Schedule GetTestSchedule()
			=> new Schedule
			{
				Id = 1,
				Hall = new Hall { Id = 1 },
				Date = new DateTime()
			};

		[Fact, TestPriority(0)]
		public async Task CreateTestData()
		{
			try
			{
				// Arrange
				Schedule schedule = GetTestSchedule();
				var scheduleFunction = new ScheduleFunction(new ScheduleValidation(schedule), schedule);

				IPlaneService planeService = new PlaneService(new PlaneRepositoryMS(_stringConnection));
				ICountryService countryRepository = new CountryService(new CountryRepositoryMS(_stringConnection));
				ICityService cityService = new CityService(new CityRepositoryMS(_stringConnection));
				ITripService tripService = new TripService(new TripRepositoryMS(_stringConnection));
				ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
					new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));

				var plane = GetPlane();
				var countryName = "countryName";
				string firstCityName = "some1";
				string secondCityName = "some2";

				Trip addTrip = new Trip()
				{
					StartDate = new DateTime(2022, 11, 20, 9, 00, 00),
					EndDate = new DateTime(2022, 11, 20, 11, 30, 00),
					Capacity = 10,
					Price = 40
				};

				// Act
				await planeService.Create(plane);
				var planeData = await planeService.GetByName(plane.Name);
				await countryRepository.Create(new Country() { Name = countryName });
				var countryData = await countryRepository.GetAll();
				int countryId = countryData.Data.Max(x => x.Id);

				await cityService.Create(new City() { Name = firstCityName, CountryId = countryId });
				await cityService.Create(new City() { Name = secondCityName, CountryId = countryId });

				var foundcity1 = await cityService.GetByCityName(firstCityName);
				var foundcity2 = await cityService.GetByCityName(secondCityName);
				addTrip.StartCityId = foundcity1.Data.Id;
				addTrip.EndCityId = foundcity2.Data.Id;
				addTrip.PlaneId = planeData.Data.Id;

				await tripService.Create(addTrip);
				var trips = await tripService.GetAll();

				_foundTrip = trips.Data.Last();
				await tripDetailService.Create(new TripDetails() { Place = 1, UserId = 3, TripId = _foundTrip.Id });
				await tripDetailService.Create(new TripDetails() { Place = 2, UserId = 3, TripId = _foundTrip.Id });
				await tripDetailService.Create(new TripDetails() { Place = 3, UserId = 3, TripId = _foundTrip.Id });
				await tripDetailService.Create(new TripDetails() { Place = 4, UserId = 3, TripId = _foundTrip.Id });
				await tripDetailService.Create(new TripDetails() { Place = 6, UserId = 3, TripId = _foundTrip.Id });

				Assert.True(true);
			}
			catch (System.Exception)
			{

				Assert.True(false);
			}
		}
	}
}

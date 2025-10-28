using AutoMapper;
using TripSplit.Application.DTOs;
using TripSplit.Application.Features.Trips.CreateTrip;
using TripSplit.Web.Models.Cars;
using TripSplit.Web.Models.Trips;
using CreateCarCommand = TripSplit.Application.Features.Cars.CreateCar.CreateCarCommand;

namespace TripSplit.Web.Mapping
{
    public sealed class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // TRIPS: DTO -> VM
            CreateMap<TripDto, TripDetailsVm>();
            CreateMap<TripDto, TripListItemVm>();
            CreateMap<TripDto, TripParticipantsVm>()
                .ForMember(d => d.TripId, m => m.MapFrom(s => s.Id));

            // TRIPS: VM -> Command
            CreateMap<TripCreateVm, CreateTripCommand>()
                .ConvertUsing(vm => new CreateTripCommand(
                    vm.StartName,
                    vm.EndName,
                    vm.StartLat,
                    vm.StartLon,
                    vm.EndLat,
                    vm.EndLon,
                    vm.DistanceKm,
                    vm.FuelPricePerL,
                    vm.AverageConsumptionLper100,
                    vm.LitersUsed,
                    vm.PeopleCount,
                    vm.ParkingCost,
                    vm.ExtraCosts,
                    vm.CarId
                ));

            // CARS: DTO -> VM
            CreateMap<CarDto, CarListItemVm>();
            CreateMap<CarDto, CarDetailsVm>();

            // CARS: VM -> CreateCarCommand
            CreateMap<CarCreateVm, CreateCarCommand>()
                .ConvertUsing(vm => new CreateCarCommand(
                    vm.Name,
                    vm.FuelType,
                    vm.AverageConsumptionLper100,
                    vm.TankCapacityL,
                    null,
                    null
                ));

        }
    }
}

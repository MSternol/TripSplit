using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripSplit.Application.Features.Trips.CreateTrip;
using TripSplit.Application.Features.Trips.DeleteTrip;
using TripSplit.Application.Features.Trips.GetTripDetails;
using TripSplit.Application.Features.Trips.ListTrips;
using TripSplit.Application.Features.Trips.SetParticipants;
using TripSplit.Application.Features.Trips.Share;
using TripSplit.Application.Features.Trips.UpdateTripCosts;
using TripSplit.Web.Models.Trips;

namespace TripSplit.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public sealed class TripsController(IMediator mediator, IMapper mapper) : Controller
    {
        // GET /Trips
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var dtos = await mediator.Send(new ListTripsQuery(), ct);
            var vms = dtos.Select(mapper.Map<TripListItemVm>).ToList();
            return View(vms);
        }

        // GET /Trips/Create
        [HttpGet]
        public IActionResult Create() => View(new TripCreateVm());

        // POST /Trips/Create
        [HttpPost]
        public async Task<IActionResult> Create(TripCreateVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var cmd = mapper.Map<CreateTripCommand>(vm);
            var id = await mediator.Send(cmd, ct);

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET /Trips/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken ct)
        {
            var dto = await mediator.Send(new GetTripDetailsQuery(id), ct);
            if (dto is null) return NotFound();

            var vm = mapper.Map<TripDetailsVm>(dto);
            return View(vm);
        }

        // GET /Trips/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
        {
            var dto = await mediator.Send(new GetTripDetailsQuery(id), ct);
            if (dto is null) return NotFound();

            var vm = new TripEditVm
            {
                Id = dto.Id,
                StartName = dto.StartName,
                EndName = dto.EndName,
                FuelPricePerL = dto.FuelPricePerL,
                AverageConsumptionLper100 = dto.AverageConsumptionLper100,
                LitersUsed = dto.LitersUsed,
                DistanceKm = dto.DistanceKm,
                ParkingCost = dto.ParkingCost,
                ExtraCosts = dto.ExtraCosts,
                PeopleCount = dto.PeopleCount
            };

            return View(vm);
        }

        // POST /Trips/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(TripEditVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var ok = await mediator.Send(new UpdateTripCostsCommand(
                Id: vm.Id,
                FuelPricePerL: vm.FuelPricePerL,
                ParkingCost: vm.ParkingCost,
                ExtraCosts: vm.ExtraCosts,
                AverageConsumptionLper100: vm.AverageConsumptionLper100,
                LitersUsed: vm.LitersUsed,
                PeopleCount: vm.PeopleCount,
                DistanceKm: vm.DistanceKm
            ), ct);

            if (!ok) return BadRequest();
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        // GET /Trips/SetParticipants/{id}
        [HttpGet]
        public async Task<IActionResult> SetParticipants(Guid id, CancellationToken ct)
        {
            var dto = await mediator.Send(new GetTripDetailsQuery(id), ct);
            if (dto is null) return NotFound();

            var vm = mapper.Map<TripParticipantsVm>(dto);
            return View(vm);
        }

        // POST /Trips/SetParticipants
        [HttpPost]
        public async Task<IActionResult> SetParticipants(TripParticipantsVm vm, CancellationToken ct)
        {
            if (vm.TripId == Guid.Empty) return BadRequest();

            var ok = await mediator.Send(new SetParticipantsCommand(
                TripId: vm.TripId,
                Driver: (vm.DriverFirstName ?? "", vm.DriverLastName ?? ""),
                Passenger1: (vm.P1FirstName ?? "", vm.P1LastName ?? ""),
                Passenger2: (vm.P2FirstName ?? "", vm.P2LastName ?? ""),
                Passenger3: (vm.P3FirstName ?? "", vm.P3LastName ?? ""),
                Passenger4: (vm.P4FirstName ?? "", vm.P4LastName ?? "")
            ), ct);

            if (!ok) return BadRequest();
            return RedirectToAction(nameof(Details), new { id = vm.TripId });
        }

        // POST /Trips/GenerateShareLink
        [HttpPost]
        public async Task<IActionResult> GenerateShareLink(Guid id, DateTime? expiresAtUtc, CancellationToken ct)
        {
            if (id == Guid.Empty) return BadRequest();

            var token = await mediator.Send(new GenerateShareLinkCommand(id, expiresAtUtc), ct);
            TempData["ShareUrl"] = Url.Action("EnterName", "Share", new { token }, Request.Scheme);

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST /Trips/UpdateCosts
        [HttpPost]
        public async Task<IActionResult> UpdateCosts(
            Guid id,
            decimal? fuelPricePerL,
            decimal? parkingCost,
            decimal? extraCosts,
            double? averageConsumptionLper100,
            double? litersUsed,
            int? peopleCount,
            double? distanceKm,
            CancellationToken ct)
        {
            if (id == Guid.Empty) return BadRequest();

            var ok = await mediator.Send(new UpdateTripCostsCommand(
                Id: id,
                FuelPricePerL: fuelPricePerL,
                ParkingCost: parkingCost,
                ExtraCosts: extraCosts,
                AverageConsumptionLper100: averageConsumptionLper100,
                LitersUsed: litersUsed,
                PeopleCount: peopleCount,
                DistanceKm: distanceKm
            ), ct);

            if (!ok) return BadRequest();
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST /Trips/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty) return BadRequest();

            var ok = await mediator.Send(new DeleteTripCommand(id), ct);
            if (!ok) return BadRequest();

            return RedirectToAction(nameof(Index));
        }
    }
}

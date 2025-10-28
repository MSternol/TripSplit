using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TripSplit.Application.Features.Cars.CarInspections;
using TripSplit.Application.Features.Cars.CarInsurances;
using TripSplit.Application.Features.Cars.CreateCar;
using TripSplit.Application.Features.Cars.DeleteCar;
using TripSplit.Application.Features.Cars.GetCarDetails;
using TripSplit.Application.Features.Cars.ListCars;
using TripSplit.Application.Features.Cars.UpdateCar;
using TripSplit.Web.Models.Cars;

namespace TripSplit.Web.Controllers
{
    [Authorize]
    public sealed class CarsController : Controller
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /Cars
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtos = await _mediator.Send(new ListCarsQuery());
            var vms = dtos.Select(d => new CarListItemVm
            {
                Id = d.Id,
                Name = d.Name,
                FuelType = d.FuelType,
                AverageConsumptionLper100 = d.AverageConsumptionLper100,
                TankCapacityL = d.TankCapacityL,
                InsuranceDaysLeft = d.InsuranceDaysLeft,
                InspectionDaysLeft = d.InspectionDaysLeft,
                InsuranceValidTo = d.InsuranceValidTo,
                InspectionValidTo = d.InspectionValidTo
            }).ToList();

            return View(vms);
        }

        // GET: /Cars/Create
        [HttpGet]
        public IActionResult Create() => View(new CarCreateVm());

        // POST: /Cars/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateVm vm)
        {
            if (vm.IncludeInsurance && vm.Insurance is null)
                ModelState.AddModelError(string.Empty, "Zaznaczono dodanie OC, ale nie podano danych OC.");
            if (vm.IncludeInspection && vm.Inspection is null)
                ModelState.AddModelError(string.Empty, "Zaznaczono dodanie badania, ale nie podano danych badania.");

            if (!ModelState.IsValid) return View(vm);

            var carId = await _mediator.Send(new CreateCarCommand(
                name: vm.Name,
                fuelType: vm.FuelType,
                averageConsumptionLper100: vm.AverageConsumptionLper100,
                tankCapacityL: vm.TankCapacityL,
                remindersEnabled: null,
                reminderLeadTime: null
            ));

            if (carId == Guid.Empty)
            {
                ModelState.AddModelError("", "Nie udało się utworzyć pojazdu.");
                return View(vm);
            }

            if (vm.IncludeInsurance && vm.Insurance is not null)
            {
                await _mediator.Send(new UpsertCarInsuranceCommand(
                    CarId: carId,
                    Company: vm.Insurance.InsuranceCompany,
                    PolicyNumber: vm.Insurance.InsurancePolicyNumber,
                    ValidFrom: vm.Insurance.InsuranceValidFrom,
                    ValidTo: vm.Insurance.InsuranceValidTo
                ));
            }

            if (vm.IncludeInspection && vm.Inspection is not null)
            {
                await _mediator.Send(new UpsertCarInspectionCommand(
                    CarId: carId,
                    ValidFrom: vm.Inspection.InspectionValidFrom,
                    ValidTo: vm.Inspection.InspectionValidTo
                ));
            }

            return RedirectToAction(nameof(Details), new { id = carId });
        }

        // GET: /Cars/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var dto = await _mediator.Send(new GetCarDetailsQuery(id));
            if (dto is null) return NotFound();

            var vm = new CarDetailsVm
            {
                Id = dto.Id,
                Name = dto.Name,
                FuelType = dto.FuelType,
                AverageConsumptionLper100 = dto.AverageConsumptionLper100,
                TankCapacityL = dto.TankCapacityL,
                InsuranceCompany = dto.InsuranceCompany,
                InsurancePolicyNumber = dto.InsurancePolicyNumber,
                InsuranceValidFrom = dto.InsuranceValidFrom,
                InsuranceValidTo = dto.InsuranceValidTo,
                InspectionValidFrom = dto.InspectionValidFrom,
                InspectionValidTo = dto.InspectionValidTo,
                RemindersEnabled = dto.RemindersEnabled,
                ReminderLeadTime = dto.ReminderLeadTime
            };

            return View(vm);
        }

        // GET: /Cars/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var dto = await _mediator.Send(new GetCarDetailsQuery(id));
            if (dto is null) return NotFound();

            var vm = new CarEditVm
            {
                Id = dto.Id,
                Name = dto.Name,
                FuelType = dto.FuelType,
                AverageConsumptionLper100 = dto.AverageConsumptionLper100,
                TankCapacityL = dto.TankCapacityL,

                IncludeInsurance = dto.InsuranceValidTo.HasValue
                                   || dto.InsuranceValidFrom.HasValue
                                   || !string.IsNullOrWhiteSpace(dto.InsuranceCompany)
                                   || !string.IsNullOrWhiteSpace(dto.InsurancePolicyNumber),
                Insurance = new CarInsuranceVm
                {
                    InsuranceCompany = dto.InsuranceCompany,
                    InsurancePolicyNumber = dto.InsurancePolicyNumber,
                    InsuranceValidFrom = dto.InsuranceValidFrom,
                    InsuranceValidTo = dto.InsuranceValidTo
                },

                IncludeInspection = dto.InspectionValidTo.HasValue || dto.InspectionValidFrom.HasValue,
                Inspection = new CarInspectionVm
                {
                    InspectionValidFrom = dto.InspectionValidFrom,
                    InspectionValidTo = dto.InspectionValidTo
                }
            };

            return View(vm);
        }

        // POST: /Cars/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarEditVm vm)
        {
            if (vm.IncludeInsurance && vm.Insurance is null)
                ModelState.AddModelError(string.Empty, "Zaznaczono edycję OC, ale nie podano danych OC.");
            if (vm.IncludeInspection && vm.Inspection is null)
                ModelState.AddModelError(string.Empty, "Zaznaczono edycję badania, ale nie podano danych badania.");

            if (!ModelState.IsValid) return View(vm);

            var ok = await _mediator.Send(new UpdateCarCommand(
                id: vm.Id,
                name: vm.Name,
                fuelType: vm.FuelType,
                averageConsumptionLper100: vm.AverageConsumptionLper100,
                tankCapacityL: vm.TankCapacityL,
                remindersEnabled: null,
                reminderLeadTime: null
            ));

            if (!ok) return NotFound();

            if (vm.IncludeInsurance && vm.Insurance is not null)
            {
                await _mediator.Send(new UpsertCarInsuranceCommand(
                    CarId: vm.Id,
                    Company: vm.Insurance.InsuranceCompany,
                    PolicyNumber: vm.Insurance.InsurancePolicyNumber,
                    ValidFrom: vm.Insurance.InsuranceValidFrom,
                    ValidTo: vm.Insurance.InsuranceValidTo
                ));
            }
            else
            {
                await _mediator.Send(new DeleteCarInsuranceCommand(vm.Id));
            }

            if (vm.IncludeInspection && vm.Inspection is not null)
            {
                await _mediator.Send(new UpsertCarInspectionCommand(
                    CarId: vm.Id,
                    ValidFrom: vm.Inspection.InspectionValidFrom,
                    ValidTo: vm.Inspection.InspectionValidTo
                ));
            }
            else
            {
                await _mediator.Send(new DeleteCarInspectionCommand(vm.Id));
            }

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        // POST: /Cars/Delete
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction(nameof(Index));

            await _mediator.Send(new DeleteCarCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}

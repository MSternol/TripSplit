using System;
using AutoMapper;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Entities;

namespace TripSplit.Application.Mapping
{
    public sealed class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // TRIP
            CreateMap<Trip, TripDto>()
                .ForMember(d => d.StartName, o => o.MapFrom(s => s.Start.Name))
                .ForMember(d => d.EndName, o => o.MapFrom(s => s.End.Name));

            // CAR -> CarDto
            CreateMap<Car, CarDto>()
                .ForMember(d => d.InsuranceCompany, m => m.MapFrom(s => s.Insurance != null ? s.Insurance.Company : null))
                .ForMember(d => d.InsurancePolicyNumber, m => m.MapFrom(s => s.Insurance != null ? s.Insurance.PolicyNumber : null))
                .ForMember(d => d.InsuranceValidFrom, m => m.MapFrom(s => s.Insurance != null ? (DateTime?)s.Insurance.ValidFrom : null))
                .ForMember(d => d.InsuranceValidTo, m => m.MapFrom(s => s.Insurance != null ? (DateTime?)s.Insurance.ValidTo : null))
                .ForMember(d => d.InspectionValidFrom, m => m.MapFrom(s => s.Inspection != null ? (DateTime?)s.Inspection.ValidFrom : null))
                .ForMember(d => d.InspectionValidTo, m => m.MapFrom(s => s.Inspection != null ? (DateTime?)s.Inspection.ValidTo : null))
                .ForMember(d => d.RemindersEnabled, m => m.MapFrom(s => s.RemindersEnabled))
                .ForMember(d => d.ReminderLeadTime, m => m.MapFrom(s => s.ReminderLeadTime))
                .AfterMap((src, dest) =>
                {
                    var today = DateTime.UtcNow.Date;

                    if (dest.InsuranceValidTo.HasValue)
                    {
                        dest.InsuranceDaysLeft = (dest.InsuranceValidTo.Value.Date - today).Days;
                        dest.InsuranceExpired = dest.InsuranceDaysLeft.Value < 0;
                    }

                    if (dest.InspectionValidTo.HasValue)
                    {
                        dest.InspectionDaysLeft = (dest.InspectionValidTo.Value.Date - today).Days;
                        dest.InspectionExpired = dest.InspectionDaysLeft.Value < 0;
                    }
                });

            // Dependent entities -> DTO
            CreateMap<CarInsurance, CarInsuranceDto>();
            CreateMap<CarInspection, CarInspectionDto>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Services
{
    public static class TripCostCalculator
    {
        public static double AverageConsumption(double distanceKm, double litersUsed)
        => distanceKm <= 0 ? 0 : Math.Round((litersUsed / distanceKm) * 100.0, 2);


        public static double LitersUsed(double distanceKm, double avgLper100)
        => Math.Round((distanceKm * Math.Max(0, avgLper100)) / 100.0, 3);


        public static decimal FuelCost(decimal fuelPricePerL, double litersUsed)
        => Math.Round(fuelPricePerL * (decimal)litersUsed, 2);


        public static decimal TotalCost(decimal fuelCost, decimal parking, decimal extras)
        => Math.Round(fuelCost + parking + extras, 2);


        public static decimal PerPerson(decimal total, int people)
        => Math.Round(people <= 0 ? total : total / people, 2);


        public static decimal FullTankCost(double tankCapacityL, decimal fuelPricePerL)
        => Math.Round((decimal)tankCapacityL * fuelPricePerL, 2);
    }
}

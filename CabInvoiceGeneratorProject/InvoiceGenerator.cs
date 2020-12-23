using System;
using System.Collections.Generic;
using System.Text;

namespace CabInvoiceGeneratorProject
{
    public class InvoiceGenerator
    {
        RideType rideType;
        private RideRepository rideRepository;
        private readonly double MINIMUM_COST_PER_KM;
        private readonly int COST_PER_TIME;
        private readonly double MINIMUM_FARE;

        /// <summary>
        /// Method to Decide Ride type and based on it values are setting
        /// </summary>
        /// <param name="rideType">RideType of User</param>
        public InvoiceGenerator(RideType rideType)
        {
            this.rideType = rideType;
            this.rideRepository = new RideRepository();
            try
            {
                if (rideType.Equals(RideType.PREMIUM))
                {
                    this.MINIMUM_COST_PER_KM = 15;
                    this.COST_PER_TIME = 2;
                    this.MINIMUM_FARE = 20;
                }
                else if (rideType.Equals(RideType.NORMAL))
                {
                    this.MINIMUM_COST_PER_KM = 10;
                    this.COST_PER_TIME = 1;
                    this.MINIMUM_FARE = 5;
                }
            }
            catch (CabInvoiceException)
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_RIDE_TYPE, "Invalid Ride Type");
            }
        }

        /// <summary>
        /// Method to calculate Fare based on distance and time given
        /// </summary>
        /// <param name="distance">distance given</param>
        /// <param name="time">time given</param>
        /// <returns></returns>
        public double CalculateFare(double distance, int time)
        {
            double totalFare = 0;
            try
            {
                totalFare = distance * MINIMUM_COST_PER_KM + time * COST_PER_TIME;
            }
            catch (CabInvoiceException)
            {
                if (rideType.Equals(null))
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_RIDE_TYPE, "Invalid ride type");
                }
                if (distance <= 0)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_DISTANCE, "Invalid Distance");
                }
                if (distance >= 0)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_TIME, "Invalid Time");
                }
            }
            return Math.Max(totalFare, MINIMUM_FARE);
        }

        public InvoiceSummary CalculateFare(Ride[] rides)
        {
            double totalFare = 0;
            try
            {
                //Calculating Total Fare For All Rides
                foreach (Ride ride in rides)
                {
                    totalFare += this.CalculateFare(ride.distance, ride.time);
                }
            }
            catch (CabInvoiceException)
            {
                if (rides == null)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "Rides Are Null");
                }
            }
            return new InvoiceSummary(rides.Length, totalFare);
        }

        public InvoiceSummary CalculateFare(Ride[] rides, int numOfRides, double averageFarePerRide)
        {
            double totalFare = 0;
            numOfRides = 0;
            averageFarePerRide = 0;
            try
            {
                //Calculating Total Fare For All Rides
                foreach (Ride ride in rides)
                {
                    totalFare += this.CalculateFare(ride.distance, ride.time);
                }
                averageFarePerRide = totalFare / numOfRides;
            }
            catch (CabInvoiceException)
            {
                if (rides == null)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "Rides Are Null");
                }
            }
            return new InvoiceSummary(rides.Length, totalFare, averageFarePerRide);
        }

        /// <summary>
        /// Method to Add Rides For UserId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="rides">rides</param>
        public void AddRides(string userId, Ride[] rides)
        {
            try
            {
                //Adding Ride To Specifided User
                rideRepository.AddRide(userId, rides);
            }
            catch (CabInvoiceException)
            {
                if (rides == null)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES,"Rides are Null"); 
                }
            }
        }

        public InvoiceSummary GetInvoiceSummary(string userId)
        {
            try
            {
                return this.CalculateFare(rideRepository.GetRides(userId));
            }
            catch (CabInvoiceException)
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_USER_ID,"Invalid UserId");
            }
        }
    }
}

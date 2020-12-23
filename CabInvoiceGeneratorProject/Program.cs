using System;

namespace CabInvoiceGeneratorProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Cab Invoice Generator Project!");
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator(RideType.NORMAL);
            double fare = invoiceGenerator.CalculateFare(2.0, 3);
            Console.WriteLine($"Fare: {fare}");
        }
    }
}

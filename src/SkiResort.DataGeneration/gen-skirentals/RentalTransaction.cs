using System;

namespace Ski.Rentals.Generator
{
    class RentalTransaction
    {
        public int CustomerId { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}

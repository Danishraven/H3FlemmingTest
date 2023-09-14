using System;

namespace H3FlemmingTest.Models
{
    public class Reserved
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Hall? Hallobj { get; set; }
        public int SeatId { get; set; }
        //true = free, false = booked
        public bool Booked { get; set; }
    }
}

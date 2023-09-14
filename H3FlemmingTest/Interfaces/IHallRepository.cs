using H3FlemmingTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace H3FlemmingTest.Interfaces
{
    public interface IHallRepository
    {
        public Task<Hall> ReadSeatsInHall(int HallId);
        public Task<List<Reserved>> ReadReservedSeats(int HallId);
        public Task Delete(bool choice);
        public Task Delete(bool choice, int hallId); // mere advanceret
        public Task Create(int rowCount, int colCount);
        public Task CreateReserved(Hall hall); // no Task here... its easier without
        public Task<List<Reserved>> Reserved(DateTime whatYouWant);
        public Task BookSeat(int hallId, int seatRow, int seatColoumn, DateTime dateTime);
    }
}

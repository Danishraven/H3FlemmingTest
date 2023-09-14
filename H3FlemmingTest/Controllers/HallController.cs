using H3FlemmingTest.Interfaces;
using H3FlemmingTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H3FlemmingTest.Controllers
{
    public class HallController : Controller, IHallRepository
    {
        public IHallRepository HallRepository;
        public HallController(IHallRepository hallRepository)
        {
            HallRepository = hallRepository;
        }

        [HttpPut("reserved")]
        public Task BookSeat(int hallId, int seatRow, int seatColoumn, DateTime dateTime)
        {
            return HallRepository.BookSeat(hallId, seatRow, seatColoumn, dateTime);
        }

        [HttpPost("hall")]
        public async Task Create(int rowCount, int colCount)
        {
            await HallRepository.Create(rowCount, colCount);
        }
        [HttpPost("reserved")]
        public Task CreateReserved(Hall hall)
        {
            return HallRepository.CreateReserved(hall);
        }
        [HttpDelete("all")]
        public async Task Delete(bool choice)
        {
            await HallRepository.Delete(choice);
        }
        [HttpDelete("hall")]
        public async Task Delete(bool choice, int hallId)
        {
            await HallRepository.Delete(choice, hallId);
        }
        [HttpGet("all")]
        public async Task<List<Reserved>> ReadReservedSeats(int hallId)
        {
            return await HallRepository.ReadReservedSeats(hallId);
        }
        [HttpGet("hall")]
        public async Task<Hall> ReadSeatsInHall(int HallId)
        {
            return await HallRepository.ReadSeatsInHall(HallId);
        }
        [HttpGet("date")]
        public async Task<List<Reserved>> Reserved(DateTime whatYouWant)
        {
            return await Reserved(whatYouWant);
        }
    }
}

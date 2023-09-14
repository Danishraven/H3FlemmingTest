using H3FlemmingTest.Interfaces;
using H3FlemmingTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H3FlemmingTest.Repositories
{
    public class HallRepository : IHallRepository
    {
        public DatabaseContext Context { get; set; }
        public HallRepository (DatabaseContext context)
        {
            Context = context;
        }
        
        public async Task Create(int rowCount, int colCount)
        {
            Hall? hall = new();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    Seat seat = new()
                    {
                        SeatRow = i + 1,
                        SeatCol = j + 1
                    };
                    hall.Seats.Add(seat);
                }
            }
            Context.Add(hall);
            await Context.SaveChangesAsync();
            List<Hall> exsistingHalls = await Context.Hall.Include(h => h.Seats).ToListAsync();
            foreach (Hall h in exsistingHalls)
            {
                if (h.Id >= hall.Id) hall = h;
            }
            hall.Name = "Hall " + hall.Id;
            Context.Update(hall);
            CreateReserved(hall);
        }

        public async Task CreateReserved(Hall hall)
        {
            foreach (Seat seat in hall.Seats)
            {
                Reserved reserved = new()
                {
                    SeatId = seat.Id,
                    Hallobj = hall,
                    Booked = false
                };
                Context.Reserved.Add(reserved);
            }
            await Context.SaveChangesAsync();
        }

        public async Task Delete(bool choice)
        {
            List<Seat> seats = await Context.Seat.ToListAsync();
            foreach (Seat seat in seats)
            {
                if (choice)
                {
                    if (seat.Id%2 == 0)
                    {
                        await RemoveSeat(seat);
                    }
                }
                else
                {
                    if (seat.Id%2 == 1)
                    {
                        await RemoveSeat(seat);
                    }
                }
            }
            await Context.SaveChangesAsync();
        }
            
        public async Task Delete(bool choice, int hallId)
        {
            Hall hall = await ReadSeatsInHall(hallId);
            foreach (Seat seat in hall.Seats)
            {
                if (choice)
                {
                    if (seat.Id % 2 == 0)
                    {
                        await RemoveSeat(seat);
                    }
                }
                else
                {
                    if (seat.Id % 2 == 1)
                    {
                        await RemoveSeat(seat);
                    }
                }
            }
        }

        public async Task RemoveSeat(Seat seat)
        {
            Context.Seat.Remove(seat);
            Reserved? reserved = Context.Reserved.FirstOrDefault(r => r.SeatId == seat.Id);
            if (reserved != null) Context.Reserved.Remove(reserved);
            await Context.SaveChangesAsync();

        }

        public async Task<List<Reserved>> ReadReservedSeats(int HallId)
        {
            List<Reserved> allReservedList = await GetReserveds();
            List<Reserved> reservedList = new();
            foreach(Reserved reserved in allReservedList)
            {
                if (reserved.Hallobj != null && reserved.Hallobj.Id == HallId && reserved.Booked == true) reservedList.Add(reserved);
            }
            return reservedList;
        }

        public async Task<Hall> ReadSeatsInHall(int HallId)
        {
            Hall? hall = await GetHall(HallId);
            hall ??= new();
            return hall;
        }

        public async Task<List<Reserved>> Reserved(DateTime whatYouWant)
        {
            List<Reserved> allReservedList = await GetReserveds();
            List<Reserved> reservedList = new();
            foreach (Reserved reserved in allReservedList)
            {
                if (reserved.Date == whatYouWant && reserved.Booked == true) reservedList.Add(reserved);
            }
            return reservedList;
        }

        public async Task<List<Reserved>> GetReserveds()
        {
            List<Reserved> allReservedList = await Context.Reserved.Include(r => r.Hallobj).ToListAsync();
            return allReservedList;
        }

        public async Task<Hall> GetHall(int hallId)
        {
            Hall? hall = await Context.Hall.Include(h => h.Seats).FirstOrDefaultAsync(h => h.Id == hallId);
            hall ??= new();
            return hall;
        }

        public async Task BookSeat(int hallId, int seatRow, int seatColoumn, DateTime dateTime)
        {
            Hall? hall = await GetHall(hallId);
            Seat? seat = hall.Seats.FirstOrDefault(s => s.SeatCol == seatColoumn && s.SeatRow == seatRow);
            List<Reserved> reservedList = await GetReserveds();
            Reserved? reserved = reservedList.FirstOrDefault(r => r.SeatId == seat.Id);
            reserved.Booked = true;
            Context.Update(reserved);
            await Context.SaveChangesAsync();
        }
    }
}

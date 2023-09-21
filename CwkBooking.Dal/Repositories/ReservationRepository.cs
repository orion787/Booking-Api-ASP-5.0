using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CwkBooking.Dal.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        //Partial funciality!!!
        private readonly DataContext _context;
        public ReservationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Reservation> DeleteReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation != null)
                _context.Remove(reservation);

            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Hotel)
                .Include(r => r.Room)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _context.Reservations
                .Include(r => r.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }


    }
}

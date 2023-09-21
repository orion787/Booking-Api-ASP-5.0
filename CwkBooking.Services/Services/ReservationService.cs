using CwkBooking.Dal;
using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkBooking.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IHotelsRepository _hotelRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly DataContext _context;

        public ReservationService(IHotelsRepository hotelRepo, DataContext ctx, 
            IReservationRepository reservationRepository)
        {
            _hotelRepository = hotelRepo;
            _context = ctx;
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> MakeReservationAsync(Reservation reservation)
        {
            //Step 1: Get the hotel, including all rooms
            var hotel = await _hotelRepository.GetHotelByIdAsync(reservation.HotelId);

            //Step 2: Find the specified room
            var room = hotel.Rooms.Where(r => r.RoomId == reservation.RoomId).FirstOrDefault();

            if(hotel == null || room == null)
                return null;

            //Step 3: Make sure the room is available
            bool isBusy = await _context.Reservations.AnyAsync( r =>
                (reservation.CheckInDate >= r.CheckInDate && reservation.CheckInDate <= r.CheckoutDate)
                && (reservation.CheckoutDate >= r.CheckInDate && reservation.CheckoutDate <= r.CheckoutDate));

            if (isBusy)
                return null;

            if (room.NeedsRepair)
                return null;


            //Step 4: Persist all changes to the database
            _context.Rooms.Update(room);
            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _reservationRepository.GetAllReservationsAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepository.GetReservationByIdAsync(reservationId);
        }

        public async Task<Reservation> CancelReservationByIdAsync(int reservationId)
        {
            return await _reservationRepository.DeleteReservationAsync(reservationId);
        }
    }
}

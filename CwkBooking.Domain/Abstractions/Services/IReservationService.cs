using CwkBooking.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CwkBooking.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Reservation> MakeReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task<Reservation> CancelReservationByIdAsync(int reservationId);
    }
}

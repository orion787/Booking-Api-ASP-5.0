using CwkBooking.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CwkBooking.Domain.Abstractions.Repositories
{
    //Partial funcianality!!!
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task<Reservation> DeleteReservationAsync(int reservationId);
    }
}

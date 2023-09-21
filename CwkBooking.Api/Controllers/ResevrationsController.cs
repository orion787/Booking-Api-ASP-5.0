using AutoMapper;
using CwkBooking.Api.Dtos;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CwkBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResevrationsController : ControllerBase
    {
        private readonly IReservationService _reservationsService;
        private readonly IMapper _mapper;
        public ResevrationsController(IReservationService reservationService, IMapper mapper)
        {
            _reservationsService = reservationService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationPutPostDto reservationDto)
        {
            var reservation = _mapper.Map<Reservation>(reservationDto);
            var result = await _reservationsService.MakeReservationAsync(reservation);

            if (result == null)
                return BadRequest("Cannot create reservation");

            var mappedReservation = _mapper.Map<ReservationGetDto>(result);

            return Ok(mappedReservation);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationsService.GetAllReservationsAsync();
            var mappedreservations = _mapper.Map<List<ReservationGetDto>>(reservations);

            return Ok(mappedreservations);
        }

        [HttpGet]
        [Route("{reservationId}")]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            var reservation = await _reservationsService.GetReservationByIdAsync(reservationId);

            if (reservation == null)
                return NotFound($"No reservation found by id {reservationId}");

            var mappedReservation = _mapper.Map<ReservationGetDto>(reservation);

            return Ok(mappedReservation);
        }

        [HttpDelete]
        [Route("{reservationId}")]
        public async Task<IActionResult> CancelReservationById(int reservationId)
        {
            var reservation = _reservationsService.CancelReservationByIdAsync(reservationId);
            if (reservation == null)
                return NotFound();

            return NoContent();
        }
    }
}

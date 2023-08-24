using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Data;
using HotelBookingAPI.Model;



namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBookinController : ControllerBase
    {
        private readonly ApiContext _context;  

        public HotelBookinController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult CreateEdit(HotelBooking booking)
        {
            if (booking.Id == 0) { 
            _context.Bookings.Add(booking);
            }
            else
            {
                var bookingInDb=_context.Bookings.Find(booking.Id); 

                if (bookingInDb == null) { 
                return new JsonResult(NotFound());
                    bookingInDb = booking;
                }

            }
            _context.SaveChanges(); 

            return new JsonResult(Ok(booking)); 
        }
        [HttpGet]
        public JsonResult Get(int id) 
        {
            var result=_context.Bookings.Find(id); 

            if (result == null) {
                return new JsonResult(NotFound());
                }
            return new JsonResult(Ok(result));
        }


    }
}

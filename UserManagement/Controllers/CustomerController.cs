using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        [HttpGet, Authorize]
        public List<string> Customers() 
        {
            var customers = new List<string>()
            {
                "Customer 1: Oguh Kelechi",
                "Customer 2: Larry King"
            };

            return customers;
        }
    }
}

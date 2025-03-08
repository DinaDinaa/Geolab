using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizPlatform.Repository;
using QuizPlatform.Repository.Database;


namespace QuizPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public bool Get ()
        {


           new SeedData(_context).SeedDatabase();
            return true;

        }
    }
}

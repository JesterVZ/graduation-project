using API_FOR_MAP.Services;
using API_FOR_MAP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FOR_MAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        public readonly RatingService _ratingService;
        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<Result>> GetRatingAsync()
        {
            IReadOnlyCollection<Result> houses = await _ratingService.GetHousesAsync();
            return houses;
        }
    }
}

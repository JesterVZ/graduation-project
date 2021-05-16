using API_FOR_MAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FOR_MAP.Services
{
    public class RatingService
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));
        }
        public async Task<IReadOnlyCollection<Result>> GetHousesAsync()
        {
            IReadOnlyCollection<Result> results = await _ratingRepository.GetHousesAsync();

            return results;
        }
    }
}

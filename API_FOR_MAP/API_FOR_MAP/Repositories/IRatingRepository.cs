using API_FOR_MAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FOR_MAP.Services
{
    public interface IRatingRepository
    {
        Task<IReadOnlyCollection<Result>> GetHousesAsync();
    }
}

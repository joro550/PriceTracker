using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Server.Extensions;

namespace Prices.Web.Server.Controllers
{
    public class PriceController : Controller
    {
        private readonly IItemPriceRepository _itemPriceRepository;

        public PriceController(IItemPriceRepository itemPriceRepository) 
            => _itemPriceRepository = itemPriceRepository;

        public async Task<IActionResult> PriceChartData()
        {
            var prices = await _itemPriceRepository.GetAll();
            if (!prices.Any())
                return NoContent();
            return Ok(ChartDataFactory.FromItemPrices(prices));
        }
    }
}
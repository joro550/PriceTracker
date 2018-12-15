using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Extensions;
using Prices.Web.Server.Handlers.Data;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/prices")]
    public class PriceController : Controller
    {
        private readonly IItemPriceRepository _itemPriceRepository;

        public PriceController(IItemPriceRepository itemPriceRepository) 
            => _itemPriceRepository = itemPriceRepository;

        [Route("ChartData")]
        public async Task<IActionResult> PriceChartData()
        {
            var prices = await _itemPriceRepository.GetAll();
            if (!prices.Any())
                return NoContent();
            return Ok(ChartDataFactory.FromItemPrices(prices));
        }
    }
}
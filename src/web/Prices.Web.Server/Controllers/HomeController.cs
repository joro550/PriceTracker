using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Server.Extensions;
using Prices.Web.Shared.Models;
using Prices.Web.Shared.Models.Home;

namespace Prices.Web.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemPriceRepository _itemPriceRepository;
        private readonly IMapper _mapper;

        public HomeController(IItemRepository itemRepository, IItemPriceRepository itemPriceRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _itemPriceRepository = itemPriceRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepository.GetAll();
            var viewModel = _mapper.Map<List<Item>>(items);
            return Ok(viewModel);
        }

        public async Task<IActionResult> Prices(string itemId)
        {
            var prices = await _itemPriceRepository.ByPartitionKey(itemId);
            return Ok(new ItemModel
            {
                Id = itemId,
                Prices = ChartDataFactory.FromItemPrices(prices)
            });
        }

        public async Task<IActionResult> AllPrices()
        {
            var prices = await _itemPriceRepository.GetAll();
            return Ok(prices.Any() ? ChartDataFactory.FromItemPrices(prices) : new ChartData());
        }
    }
}

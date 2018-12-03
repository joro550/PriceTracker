using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Data;
using PriceChat.Web.Models;
using PriceChat.Web.Models.Home;

namespace PriceChat.Web.Controllers
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
            return View(viewModel);
        }

        public async Task<IActionResult> Prices(string itemId)
        {
            var prices = await _itemPriceRepository.ByPartitionKey(itemId);
            return View(new ItemModel
            {
                Id = itemId,
                Prices = ChartData.FromItemPrices(prices)
            });
        }

        public async Task<IActionResult> AllPrices()
        {
            var prices = await _itemPriceRepository.GetAll();
            return View(prices.Any() ? ChartData.FromItemPrices(prices) : new ChartData());
        }
    }
}

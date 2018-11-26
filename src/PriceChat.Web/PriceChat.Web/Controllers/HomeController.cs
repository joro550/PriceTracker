using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Data;
using Item = PriceChat.Web.Models.Item;
using ItemPrice = PriceChat.Web.Models.ItemPrice;

namespace PriceChat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemRepository _itemRepository;
        private readonly ItemPriceRepository _itemPriceRepository;
        private readonly IMapper _mapper;

        public HomeController(ItemRepository itemRepository, ItemPriceRepository itemPriceRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _itemPriceRepository = itemPriceRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepository.GetAll();
            var itemViewModel = _mapper.Map<List<Item>>(items);
            return View(itemViewModel);
        }

        public async Task<IActionResult> Prices(string itemId)
        {
            var prices = await _itemPriceRepository.ByPartitionKey(itemId);
            var itemPriceViewModel = _mapper.Map<List<ItemPrice>>(prices);
            return View(itemPriceViewModel);
        }
    }
}

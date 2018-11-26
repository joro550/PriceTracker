using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> AllPrices()
        {
            var prices = await _itemPriceRepository.GetAll();
            var groupedPrices = prices.GroupBy(price => price.PartitionKey);

            var chartData = new ChartData
            {
                Labels = prices.Select(p => p.Timestamp.ToString("yyyy-M-d")).Distinct().ToList()
            };

            foreach (var groupedPrice in groupedPrices)
            {
                var dataSet = new ChatDataSets{Label = groupedPrice.Key};
                foreach (var itemPrice in groupedPrice.OrderBy(price => price.Timestamp))
                {
                    dataSet.Data.Add(itemPrice.Price.Remove(0, 1));
                }

                chartData.DataSets.Add(dataSet);
            }

            return View(chartData);
        }
    }

    public class ChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<ChatDataSets> DataSets { get; set;  } = new List<ChatDataSets>();
    }

    public class ChatDataSets
    {
        public string Label { get; set; }
        public List<string> Data { get; set;  } = new List<string>();
    }
}

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Data;
using PriceChat.Web.Models.Items;

namespace PriceChat.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public ItemController(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Add()
            => View(new ItemModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ItemModel itemModel)
        {
            var validator = new ItemModelValidator();
            var validationResult = validator.Validate(itemModel);

            if (!validationResult.IsValid)
                return View(itemModel);

            await _repository.Add(_mapper.Map<Item>(itemModel));
            return View();
        }
    }
}
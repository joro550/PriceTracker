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
            => View(new AddItemModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddItemModel addItemModel)
        {
            var validator = new ItemModelValidator();
            var validationResult = validator.Validate(addItemModel);

            if (!validationResult.IsValid)
            {
                addItemModel.Errors = validationResult.Errors;
                return View(addItemModel);
            }

            await _repository.Add(_mapper.Map<ItemEntity>(addItemModel));
            return View();
        }
    }
}
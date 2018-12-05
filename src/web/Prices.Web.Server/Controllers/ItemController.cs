using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Server.Controllers
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
            => Ok(new AddItemModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddItemModel addItemModel)
        {
            var validator = new ItemModelValidator();
            var validationResult = validator.Validate(addItemModel);

            if (!validationResult.IsValid)
            {
                addItemModel.Errors = validationResult.Errors;
                return Ok(addItemModel);
            }

            await _repository.Add(_mapper.Map<ItemEntity>(addItemModel));
            return Ok(new AddItemModel {Success = true});
        }
    }
}
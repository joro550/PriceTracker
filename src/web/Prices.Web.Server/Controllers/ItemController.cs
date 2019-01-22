using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Prices.Web.Shared.Models.Home;
using Prices.Web.Shared.Models.Items;
using Prices.Web.Server.Handlers.Data;
using Microsoft.AspNetCore.Authorization;

namespace Prices.Web.Server.Controllers
{
    [ApiController, Route("/api/items")]
    public class ItemController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _repository;

        public ItemController(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateItem([FromBody]AddItemModel model)
        {
            var validator = new ItemModelValidator();
            var validationResult = await validator.ValidateAsync(model);
            return BadRequest(validationResult.Errors);
        }

//        [HttpPost]
//        public async Task<ActionResult> Add(AddItemModel addItemModel)
//        {
//            var validator = new ItemModelValidator();
//            var validationResult = validator.Validate(addItemModel);
//
//            if (!validationResult.IsValid)
//            {
//                addItemModel.Errors = validationResult.Errors;
//                return Ok(addItemModel);
//            }
//
//            await _repository.Add(_mapper.Map<ItemEntity>(addItemModel));
//            return Ok(new AddItemModel {Success = true});
//        }

        [Route("")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _repository.GetAll();
            if (!items.Any())
                return NoContent();
            return Ok(_mapper.Map<List<Item>>(items));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Shared.Models.Home;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Server.Controllers
{
    [ApiController]
    [Route("/api/items")]
    public class ItemController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _repository;

        public ItemController(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromBody] AddItemModel model)
        {
            var validator = new ItemModelValidator();
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            await _repository.Add(_mapper.Map<ItemEntity>(model));
            return Ok();
        }

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
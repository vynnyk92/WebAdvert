using Microsoft.AspNetCore.Mvc;
using WebAdvert.Api.DTOs;
using WebAdvert.Api.Services;
using WebAdvert.Models;
using WebAdvert.Models.Messages;

namespace WebAdvert.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertStorageService _storageService;
        private readonly IMessagePublisher _messagePublisher;

        public AdvertController(IAdvertStorageService storageService, IMessagePublisher messagePublisher)
        {
            _storageService = storageService;
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(CreateAdvertResponse), 200)]
        public async Task<IActionResult> Create(AdvertModel advertModel)
        {
            try
            {
                var result = await _storageService.Add(advertModel);
                return Ok(new CreateAdvertResponse(result));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }

        [HttpPut]
        [Route("confirm")]
        [ProducesResponseType(400)]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel advertModel)
        {
            try
            {
                await _storageService.Confirm(advertModel);
                await _messagePublisher.PublishAsync(advertModel);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(advertModel.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

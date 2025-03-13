using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRWebApp237.Services;

namespace SignalRWebApp237.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IFileService _fileService;

        public OfferController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<double> Get()
        {
            var data = await _fileService.Read();
            return data;
        }

        [HttpGet("Room")]
        public async Task<double> Get(string room)
        {
            return await _fileService.Read(room);
        }

        [HttpGet("IncreaseRoom")]
        public async Task<ActionResult> IncreaseRoom(string room,double data)
        {
            var result = await _fileService.Read(room) + data;
            await _fileService.Write(room, result);
            return Ok(result);
        }

        [HttpGet("Increase")]
        public async Task<ActionResult> Increase(double data)
        {
            var result = (await _fileService.Read()) + data;
            await _fileService.Write(result);
            return Ok(result);
        }

    }
}

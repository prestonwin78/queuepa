using Microsoft.AspNetCore.Mvc;
using queuepa.Server.services;
using System.Drawing;

namespace queuepa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly QueueService _queueService;

        public QueueController(QueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQueue(int streamerId, string queueName)
        {
            await _queueService.CreateQueue(streamerId, queueName);
            return Ok();
        }

        [HttpDelete("delete/{queueId}")]
        public async Task<IActionResult> DeleteQueue(int queueId)
        {
            await _queueService.DeleteQueue(queueId);
            return Ok();
        }

        [HttpPost("addSong")]
        public async Task<IActionResult> AddSong(int queueId, int userId, string songTitle, string songUrl)
        {
            try
            {
                await _queueService.AddSongToQueue(queueId, userId, songTitle, songUrl);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("removeSong/{songId}")]
        public async Task<IActionResult> RemoveSong(int songId)
        {
            await _queueService.RemoveSongFromQueue(songId);
            return Ok();
        }

        [HttpPost("promoteSong/{songId}")]
        public async Task<IActionResult> PromoteSong(int songId, [FromQuery] int positions)
        {
            await _queueService.PromoteSong(songId, positions);
            return Ok();
        }

        [HttpPost("highlightSong/{songId}")]
        public async Task<IActionResult> HighlightSong(int songId, Color color)
        {
            await _queueService.HighlightSong(songId, color);
            return Ok();
        }

        [HttpGet("checkUserStillHere/{songId}")]
        public async Task<IActionResult> CheckUserStillHere(int songId)
        {
            var stillHere = await _queueService.CheckUserStillHere(songId);
            return Ok(stillHere);
        }
    }

}

using CRUDMVC.Data;
using CRUDMVC.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRUDMVC.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Load old messages
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.ChatMessages
                .OrderBy(m => m.Timestamp)
                .Join(_context.Users,
                      chat => chat.SenderId,
                      user => user.Id,
                      (chat, user) => new
                      {
                          chat.Message,
                          Username = user.UserName
                      })
                .ToListAsync();

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage(string message)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.ChatMessages.Add(new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = senderId, // dummy (not used now)
                Message = message
            });

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
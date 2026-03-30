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
                ReceiverId = senderId,
                Message = message,
                Timestamp = DateTime.Now,
                IsRead = false
            });

            await _context.SaveChangesAsync();
            return Ok();
        }


        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var count = await _context.ChatMessages
                .Where(m => m.SenderId != userId && !m.IsRead)
                .CountAsync();

            return Json(count);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var unreadMessages = await _context.ChatMessages
                .Where(m => m.SenderId != userId && !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
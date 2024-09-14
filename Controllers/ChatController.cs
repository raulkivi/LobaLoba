using Lobabot.models;
using Lobabot.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lobabot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // In-memory store for conversation history (for simplicity)
        private static List<ChatMessageWithRole> conversationHistory = new List<ChatMessageWithRole>();

        // In-memory store for button states (for simplicity)
        private static ButtonStates buttonStates = new ButtonStates
        {
            IsSoundOn = false,
            IsRecordingOn = false,
            IsSystemMessagesOn = false
        };

        // POST api/chat
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatMessage message)
        {
            // Create user message
            var userMessage = new ChatMessageWithRole
            {
                Role = "user",
                Text = message.Text
            };

            // You can add logic here to process the user's input
            // For simplicity, we are just echoing back the input
            string botResponseText = $"👩🏻‍🚀 You said '{message.Text}'";

            // Create bot message
            var botMessage = new ChatMessageWithRole
            {
                Role = "bot",
                Text = botResponseText
            };

            // Save the chat to the conversation history
            conversationHistory.Add(userMessage);

            // Wait for 1 second before adding the bot response
            await Task.Delay(1000);

            conversationHistory.Add(botMessage);

            return Ok(new { response = botResponseText, conversationHistory });
        }

        // GET api/chat/buttonstates
        [HttpGet("buttonstates")]
        public IActionResult GetButtonStates()
        {
            return Ok(buttonStates);
        }

        // POST api/chat/buttonstates
        [HttpPost("buttonstates")]
        public IActionResult UpdateButtonStates([FromBody] ButtonStates newButtonStates)
        {
            buttonStates = newButtonStates;
            return Ok(buttonStates);
        }
    }
}

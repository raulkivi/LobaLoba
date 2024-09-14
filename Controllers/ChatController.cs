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
            conversationHistory.Add(userMessage);

            // You can add logic here to process the user's input
            // For simplicity, we are just echoing back the input
            string botResponseText = $"👩🏻‍🚀 You said '{message.Text}'";

            // Create bot message
            AddBotMessage(botResponseText);

            AddBotMessage("1. Kala");
            AddBotMessage("2. Liha");

            // Wait for 1 second before adding the bot response
            await Task.Delay(1000);



            return Ok(new { response = botResponseText, conversationHistory });
        }

        private static void AddBotMessage(string botResponseText)
        {
            var botMessage = new ChatMessageWithRole
            {
                Role = "bot",
                Text = botResponseText
            };
            conversationHistory.Add(botMessage);
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

using Lobabot.models;
using Lobabot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lobabot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly ILogger<ChatController> _logger;
        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }
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

            _logger.LogInformation("Received user message: {Message}", message.Text);

            // Create user message
            var userMessage = new ChatMessageWithRole
            {
                Role = "user",
                Text = message.Text
            };
            conversationHistory.Add(userMessage);

            // You can add logic here to process the user's input
            // For simplicity, we are just echoing back the input
            string botResponseText = AddBotMessage("Available options:");

            // Create bot message
            botResponseText += AddBotMessage("1) Apple");
            botResponseText += AddBotMessage("2) Microsoft");

            // Wait for 1 second before adding the bot response
            await Task.Delay(1000);

            _logger.LogInformation("Updated conversation history: {Message}", JsonConvert.SerializeObject(conversationHistory, Formatting.Indented));

            return Ok(new { response = botResponseText, conversationHistory });
        }

        private static string AddBotMessage(string botResponseText)
        {
            var botMessage = new ChatMessageWithRole
            {
                Role = "bot",
                Text = botResponseText
            };
            conversationHistory.Add(botMessage);

            return $" {botResponseText}; \n";
        }

        // GET api/chat/buttonstates
        [HttpGet("buttonstates")]
        public IActionResult GetButtonStates()
        {
            return Ok(buttonStates);
        }

        // POST api/chat/buttonstates
        [HttpPost("buttonstates")]
        public async Task<IActionResult> UpdateButtonStatesAsync([FromBody] ButtonStates newButtonStates)
        {
            _logger.LogInformation($"Buttons state changed {newButtonStates}");

            buttonStates = newButtonStates;
            return Ok(buttonStates);
        }

        // POST api/chat/audio
        [HttpPost("audio")]
        public async Task<IActionResult> PostAudio()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                return BadRequest("No audio file uploaded.");
            }

            _logger.LogInformation($"Received audio {file.Length} bytes");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                // Process the audio file here
                // For simplicity, we are just echoing back a fixed response
                string botResponseText = "👩🏻‍🚀 I received your audio message.";

                // Create bot message
                AddBotMessage(botResponseText);

                return Ok(new { response = botResponseText, conversationHistory });
            }
        }
    }
}

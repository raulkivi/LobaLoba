namespace Lobabot.Models
{
    public class ChatMessageWithRole
    {
        public required string Role { get; set; }
        public required string Text { get; set; }
    }
}

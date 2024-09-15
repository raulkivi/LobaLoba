namespace Lobabot.Models
{
    public class ButtonStates
    {
        public bool IsSoundOn { get; set; }
        public bool IsRecordingOn { get; set; }
        public bool IsSystemMessagesOn { get; set; }

        public override string ToString()
        {
            return $"IsSoundOn: {IsSoundOn}, IsRecordingOn: {IsRecordingOn}, IsSystemMessagesOn: {IsSystemMessagesOn}";
        }
    }
}

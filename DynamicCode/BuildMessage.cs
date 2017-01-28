namespace DynamicCode
{


    public enum BuildMessageSeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }

    public class BuildMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public int Character { get; set; }
        public BuildMessageSeverity Severity { get; set; }
    }
}

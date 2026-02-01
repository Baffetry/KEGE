namespace KEGE_Station.Models.Exceptions
{
    internal class OptionNotFoundException : Exception
    {
        public OptionNotFoundException() { }
        public OptionNotFoundException(string message) : base(message) { }
        public OptionNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}

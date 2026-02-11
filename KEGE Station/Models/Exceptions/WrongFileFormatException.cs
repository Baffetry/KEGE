namespace KEGE_Station.Models.Exceptions
{
    public class WrongFileFormatException : Exception
    {
        public WrongFileFormatException() { }
        public WrongFileFormatException(string? message) : base(message) { }
        public WrongFileFormatException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}

namespace KEGE_Station.Models.Exceptions
{
    public class WrongDirectoryException : Exception
    {
        public WrongDirectoryException() { }
        public WrongDirectoryException(string? message) : base(message) { }
        public WrongDirectoryException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}

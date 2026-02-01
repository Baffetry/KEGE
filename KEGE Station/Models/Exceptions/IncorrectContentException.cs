namespace KEGE_Station.Models.Exceptions
{
    internal class IncorrectContentException : Exception
    {
        public IncorrectContentException() { }
        public IncorrectContentException(string? message) : base(message) { }
        public IncorrectContentException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}

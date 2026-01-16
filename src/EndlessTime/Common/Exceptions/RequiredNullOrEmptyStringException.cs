namespace Common.Exceptions
{
    public class RequiredNullOrEmptyStringException : Exception
    {
        public RequiredNullOrEmptyStringException(string message) : base(message) { }
    }
}

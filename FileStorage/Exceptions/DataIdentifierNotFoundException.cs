using System;
using System.Runtime.Serialization;

namespace FileStorage.Exceptions
{
    public class DataIdentifierNotFoundException : Exception
    {
        public DataIdentifierNotFoundException()
        {
        }

        public DataIdentifierNotFoundException(string message)
            : base(message)
        {
        }

        public DataIdentifierNotFoundException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public DataIdentifierNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DataIdentifierNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected DataIdentifierNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
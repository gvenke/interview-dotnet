using System;

namespace GroceryStore.Utility
{
    public class DuplicateEntityKeyException : Exception
    {
        public DuplicateEntityKeyException() : base() { }

        public DuplicateEntityKeyException(string message, object key) : base(message)
        {
            DuplicateKey = key;
        }

        public object DuplicateKey { get; private set; }
    }
}

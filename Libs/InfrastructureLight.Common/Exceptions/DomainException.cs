﻿using System;
using System.Runtime.Serialization;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {        
        public DomainException() { }

        public DomainException(string message)
          : base(message) { }
          
        public DomainException(string format, params object[] args) 
          : base(string.Format(format, args)) { }

        public DomainException(string message, Exception innerException)
           : base(message, innerException) { }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

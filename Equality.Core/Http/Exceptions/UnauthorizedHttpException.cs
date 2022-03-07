﻿using System;
using System.Net.Http;

namespace Equality.Http
{
    public class UnauthorizedHttpException : HttpRequestException
    {
        public UnauthorizedHttpException() : base("Unauthenticated.")
        {
        }

        public UnauthorizedHttpException(string message) : base(message)
        {
        }

        public UnauthorizedHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
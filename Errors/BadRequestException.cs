﻿namespace Usuarios.Errors
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {           
        }
    }
}

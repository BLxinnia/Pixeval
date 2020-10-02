﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Runtime.Serialization;

namespace Pixeval.Objects.Exceptions
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException()
        {
        }

        protected TokenNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TokenNotFoundException(string message) : base(message)
        {
        }

        public TokenNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
﻿using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application
{
    public abstract class Response
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
    }
}
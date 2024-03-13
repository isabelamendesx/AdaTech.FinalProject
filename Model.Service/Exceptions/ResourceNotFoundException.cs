﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public int StatusCode { get; }

        public ResourceNotFoundException(string resourceName) : base($"{resourceName} not found.")
        {
            StatusCode = 404;
        }

    }
}

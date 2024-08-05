﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Services
{
    public interface IAuthService
    {
        Task<bool> AuthenticateMobile(string mobile);
        Task<bool> ValidateOTP(string code);
    }
}
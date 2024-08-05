using FirebaseExperiment01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Services;

/// <summary>
/// 
/// </summary>
public class FirebaseService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public User Login(string email)
    {
        return Users.NOTLOGINED;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Models;

public class User
{
    public bool IsLogined = false;
}

public static class Users
{
    public static readonly User NOTLOGINED = new User();
}

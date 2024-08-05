using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Helpers;

public class NavigationHelper
{
    public static INavigation? Navigation { get; private set; }
    public static Shell Shell => Shell.Current;

    public static void Initialize(INavigation navi) => Navigation = navi;

}

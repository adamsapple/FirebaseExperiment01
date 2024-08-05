using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Helpers;

public static class ServiceHelper
{
    public static IServiceProvider? Services { get; private set; }

    public static void Initialize(IServiceProvider serviceProvider) => Services = serviceProvider;
//#if ANDROID
        public static T? GetService<T>() => Services!.GetService<T>();
//#endif
    public static object? GetService(Type type) => Services!.GetService(type);
}

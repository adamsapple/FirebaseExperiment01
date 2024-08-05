using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Services.Preferences;

public interface IPreferencesService
{
    bool ContainsKey(string key);
    void Remove(string key);
    void Clear();

    string? GetString(string key, string? defaultValue = null);
    bool GetBool(string key, bool defaultValue = false);
    long GetLong(string key, long defaultValue = 0);

    void Set(string key, string value);
    void Set(string key, bool value);
    void Set(string key, long value);
}

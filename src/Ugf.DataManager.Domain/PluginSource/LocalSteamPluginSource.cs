#if !DISABLE_STEAM_MODULE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Secyud.Ugf.Modularity;
using Secyud.Ugf.Modularity.Plugins;

namespace Ugf.DataManager.PluginSource
{
    public class LocalSteamPluginSource : IPlugInSource
    {
        private readonly List<Type> _modules = [];

        public LocalSteamPluginSource()
        {
            string path = "../steam-mod-info.binary";
            if (File.Exists(path))
            {
                using FileStream stream = File.OpenRead(path);
                using BinaryReader reader = new(stream);

                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string modPath = reader.ReadString();
                    string infoPath = Path.Combine(modPath, "info.json");
                    if (!File.Exists(infoPath)) continue;
                    SteamModInfo.LocalInfo info = SteamModInfo.LocalInfo.CreateFromContent(infoPath);
                    if (info?.ModuleAssemblyName is null) continue;
                    string assemblyPath = Path.Combine(modPath, info.ModuleAssemblyName);
                    if (!File.Exists(assemblyPath)) continue;
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    Type type = assembly.GetTypes().FirstOrDefault(u => typeof(IUgfModule).IsAssignableFrom(u));
                    if (type is not null)
                    {
                        _modules.Add(type);
                    }
                }
            }
        }

        public IEnumerable<Type> GetModules()
        {
            return _modules;
        }
    }
}

#endif
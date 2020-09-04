// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Newtonsoft.Json;
using StartMenuManager.Core.DataStructures;

namespace StartMenuManager.Core.Serialization
{
    /// <summary>
    /// Serialization methods for Configs
    /// </summary>
    public static class Serializer
    {
        public static string SerializeConfig(Config config)
        {
            return JsonConvert.SerializeObject(config, Formatting.Indented);
        }

        public static Config DeserializeConfig(string configText)
        {
            return JsonConvert.DeserializeObject<Config>(configText);
        }

        public static string SerializeShortcut(Shortcut shorcut)
        {
            return JsonConvert.SerializeObject(shorcut, Formatting.Indented);
        }

        public static Shortcut DeserializeShortcut(string shortcutText)
        {
            return JsonConvert.DeserializeObject<Shortcut>(shortcutText);
        }
    }
}

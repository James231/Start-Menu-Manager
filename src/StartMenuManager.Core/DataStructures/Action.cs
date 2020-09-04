// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using JsonSubTypes;
using Newtonsoft.Json;
using StartMenuManager.Core.DataStructures.Actions;

namespace StartMenuManager.Core.DataStructures
{
    /// <summary>
    /// An Action for use within a Shortcut.
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(CommandAction), "command")]
    [JsonSubtypes.KnownSubType(typeof(FileAction), "file")]
    [JsonSubtypes.KnownSubType(typeof(FolderAction), "folder")]
    [JsonSubtypes.KnownSubType(typeof(SoftwareAction), "software")]
    [JsonSubtypes.KnownSubType(typeof(WebsiteAction), "website")]
    public class Action
    {
        public string Type { get; set; }

        public virtual ValidationError IsValid()
        {
            return null;
        }

        public virtual Action Duplicate()
        {
            throw new NotImplementedException("Action base class cannot be duplicated.");
        }
    }
}

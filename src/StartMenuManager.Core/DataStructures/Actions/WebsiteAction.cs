// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;

namespace StartMenuManager.Core.DataStructures.Actions
{
    /// <summary>
    /// Action which opens a URL within a specified web browser.
    /// </summary>
    public class WebsiteAction : Action
    {
        public WebsiteAction(string url)
        {
            Type = "website";
            Url = url;
        }

        public WebsiteAction()
        {
            Type = "website";
            Url = "https://google.com";
        }

        public string Url { get; set; }

        public override ValidationError IsValid()
        {
            Uri uriResult;
            if (!Uri.TryCreate(Url, UriKind.Absolute, out uriResult))
            {
                return new ValidationError("Url not valid!", this);
            }

            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return new ValidationError("Url should start with 'http'!", this);
            }

            return null;
        }

        public override Action Duplicate()
        {
            return new WebsiteAction(Url);
        }
    }
}

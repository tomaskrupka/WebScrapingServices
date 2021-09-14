﻿using System.Net;

namespace WebScrapingServices.Authenticated.Browser
{
    public struct WebClientSettings
    {
        public string UserProfileName;
        public bool UseProxy;
        public WebProxy? Proxy;
        public RdpClientImplementation RdpClientImplementation;
    }
    public enum RdpClientImplementation
    {
        Any, Selenium
    }
}

﻿using System.Net;

namespace WebScrapingServices.Authenticated.Browser
{
    public struct WebClientSettings
    {
        public string UserProfileName;
        public bool UseProxy;
        public WebProxy? Proxy;
        public bool IgnoreSslCertificateErrors;
        public RdpClientImplementation RdpClientImplementation;
        public bool TriggerKeyboardCommandListener;
        public string BrowserLocation;
    }
    public enum RdpClientImplementation
    {
        BaristaLabsCdtr, Selenium
    }
}

﻿using OpenQA.Selenium.Chrome;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Net;

namespace WebScrapingServices.Authenticated.Browser.Selenium
{
    public class SeleniumChromeClient : IWebClient
    {
        private ChromeDriver _driver;
        private SeleniumRdpSession _rdpSession;
        private SeleniumChromeBrowserWindow _browserWindow;

        private ILoggerFactory _loggerFactory;
        private ILogger _logger;


        public SeleniumChromeClient(ILoggerFactory loggerFactory, WebClientSettings settings)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<SeleniumChromeClient>();
            (_driver, _browserWindow, _rdpSession) = LaunchAndConnect(settings);
        }

        public IRdpSession RdpClient => _rdpSession;

        public IBrowserWindow BrowserWindow => _browserWindow;

        public CookieContainer Cookies
        {
            get
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in _driver.Manage().Cookies.AllCookies)
                {
                    cookieContainer.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                }
                return cookieContainer;
            }
        }

        public void Dispose()
        {
            _browserWindow.Dispose();
            _rdpSession.Dispose();
            _driver.Dispose();
        }

        private (ChromeDriver, SeleniumChromeBrowserWindow, SeleniumRdpSession) LaunchAndConnect(WebClientSettings settings)
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument($"user-data-dir={Environment.CurrentDirectory}\\SeleniumProfiles\\" + settings.UserProfileName + "\\");

            if (settings.UseProxy)
            {
                var proxyAuthority = settings.Proxy?.Address?.Authority;
                if (proxyAuthority == null)
                {
                    throw new ArgumentException("Invalid settings provided, proxy authority must not be null if UseProxy set to true.");
                }

                _logger.LogInformation("Launching Selenium Chrome with proxy.");

                options.Proxy = new Proxy
                {
                    Kind = ProxyKind.Manual,
                    IsAutoDetect = false,
                    HttpProxy = proxyAuthority,
                    SslProxy = proxyAuthority,
                };
            }
            else
            {
                _logger.LogInformation("Launching Selenium Chrome without proxy.");
            }

            if (settings.IgnoreSslCertificateErrors)
            {
                _logger.LogWarning("Launching Selenium Chrome with ignore-certificate-errors flag on.");
                options.AddArgument("ignore-certificate-errors");
            }

            var driver = new ChromeDriver(Environment.CurrentDirectory, options);
            var session = driver.GetDevToolsSession();
            var seleniumRdpSessionLogger = _loggerFactory.CreateLogger<SeleniumRdpSession>();

            var browserWindow = new SeleniumChromeBrowserWindow(_loggerFactory.CreateLogger<SeleniumChromeBrowserWindow>(), driver);
            var rdpSession = new SeleniumRdpSession(seleniumRdpSessionLogger, session);

            return (driver, browserWindow, rdpSession);
        }
    }
}

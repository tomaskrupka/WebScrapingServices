﻿using Gripper.WebClient;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Gripper.Test.IWebClientTests
{
    public class ExecuteRdpCommandAsyncTests : UnitTestBase
    {

        [Test]
        public async Task NetworkSetCookieIsCommandSuccess()
        {
            var cookieObject = Fakers.GetCookieObject();
            var commandResult = await _webClient.ExecuteRdpCommandAsync("Network.setCookie", JToken.FromObject(cookieObject));
            bool isCommandSuccessful = (bool)commandResult["success"];

            Assert.IsTrue(isCommandSuccessful);
        }
    }
}

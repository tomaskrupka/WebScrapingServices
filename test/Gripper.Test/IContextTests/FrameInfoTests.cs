﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gripper.Test.IContextTests
{
    public class FrameInfoTests : UnitTestBase
    {
        [Test]
        public void IsNotNull()
        {
            Assert.IsNotNull(_webClient.MainContext.FrameInfo);
        }
    }
}

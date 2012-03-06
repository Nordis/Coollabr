using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoolabrThird.Models;
using NUnit.Framework;

namespace CoolabrThird.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_CanSetAndValidatePassword()
        {
            User user = new User().SetPassword("password");

            Assert.IsTrue(user.ValidatePassword("password"));
        }

        [Test]
        public void User_UserGetsEnabledWhenCreated()
        {
            User user = new User().SetPassword("password");
            user.Enabled = true;

            Assert.IsTrue(user.Enabled);
        }
    }
}

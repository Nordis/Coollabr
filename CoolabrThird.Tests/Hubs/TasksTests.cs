using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoolabrThird.Hubs;
using CoolabrThird.Models;
using NUnit.Framework;

namespace CoolabrThird.Tests.Hubs
{
    [TestFixture]
    public class TasksTests
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddTask_Throws_NullReference_When_DocumentStore_Not_Available()
        {
            throw new NullReferenceException("Test");
        }
    }
}

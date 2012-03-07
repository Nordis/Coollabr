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
        public void AddTask_Throws_NullReference_When_DocumentStore_Not_Available()
        {
            var hub = new Tasks();

            Assert.Catch<NullReferenceException>(() => hub.Add(new Task
                                                                   {
                                                                       Title = "Test"
                                                                   }));
        }
    }
}

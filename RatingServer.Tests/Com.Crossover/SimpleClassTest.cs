using Com.Crossover;
using Com.Crossover.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingServer.Tests.Com.Crossover
{
    [TestClass]

    public class SimpleClassTest
    {
        readonly SimpleClass simpleClass = new SimpleClass();

        [TestMethod]
        public void GivenRatingGreaterThanRatingCeiling_ShouldFail()
        {
            int rating = 2;
            int ratingCeling = 1;
            using (ShimsContext.Create())
            {
                Should.Throw<ArgumentException>(() => {
                    simpleClass.CreateRatingString(rating, ratingCeling);
                }).Message.ShouldBe("Cannot be over the hard ceiling");
            }
        }
        [TestMethod]
        public void GivenRatingIsEqualToRatingCeiling_ShouldContainTOP()
        {
            int rating = 2;
            int ratingCeling = 2;
            using (ShimsContext.Create())
            {
                String result = simpleClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("TOP+2", result);
            }
        }
        [TestMethod]
        public void GivenRatingIsLowerThanHalfOfRatingCeiling_ShouldContainLOW()
        {
            int rating = 1;
            int ratingCeling = 4;
            using (ShimsContext.Create())
            {
                String result = simpleClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("LOW-1", result);
            }
        }
        [TestMethod]
        public void GivenRatingIsGreaterThanOrEqualToHalfOfRatingCeiling_ShouldContainHIGH()
        {
            int rating = 2;
            int ratingCeling = 4;
            using (ShimsContext.Create())
            {
                String result = simpleClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("HIGH=2", result);
            }
        }
    }
}

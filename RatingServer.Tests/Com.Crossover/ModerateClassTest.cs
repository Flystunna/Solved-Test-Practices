using Com.Crossover;
using Com.Crossover.Services;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;

namespace RatingServer.Tests.Com.Crossover
{
    [TestClass]
    public class ModerateClassTest
    {
        private readonly Mock<NotificationService> notificationService = new Mock<NotificationService>();

        private readonly Mock<ExternalRatingApprovalService> externalRatingApprovalService = new Mock<ExternalRatingApprovalService>();

        private ModerateClass moderateClass;

        [TestMethod]
        public void GivenRatingGreaterThanRatingCeiling_ShouldFail()
        {
            //ARRANGE AND ACT
            int rating = 2;
            int ratingCeling = 1;
            using (ShimsContext.Create())
            {            
                moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
                //ASSERT
                Should.Throw<ArgumentException>(() => {
                    moderateClass.CreateRatingString(rating, ratingCeling);
                }).Message.ShouldBe("Cannot be over the hard ceiling");
            }
        }

        [TestMethod]
        public void GivenRatingIsEqualToRatingCeilingAndExternalRatingIsApprovedFalse_ShouldContainNOTAPP()
        {
            //ARRANGE AND ACT
            int rating = 2;
            int ratingCeling = 2;
            using (ShimsContext.Create())
            {
                moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
                externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(false);
                string result = moderateClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("NOT-APP", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
            }
        }
        [TestMethod]
        public void ShouldFail_WhenExternalRatingApprovalServiceIsNotApprovedAndRatingIsLowerThanRatingCeiling()
        {
            int rating = 1;
            int ratingCeling = 2;
            //ARRANGE
            externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(false);
            moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
            //ACT
            using (ShimsContext.Create())
            {
                string result = moderateClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("NOT-APP", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
            } 
        }
        [TestMethod]
        public void GivenRatingIsEqualToRatingCeilingAndExternalRatingIsApproved_ShouldContainTOP()
        {
            //ARRANGE AND ACT
            int rating = 2;
            int ratingCeling = 2;
            using (ShimsContext.Create())
            {
                moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
                externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(true);
                string result = moderateClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("TOP+2", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
                notificationService.Verify(mock => mock.Notify(rating), Times.Once());
            }
        }

        [TestMethod]
        public void GivenRatingIsLowerThanHalfOfRatingCeiling_ShouldContainLOW()
        {
            int rating = 1;
            int ratingCeling = 4;
            externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(true);
            moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
            using (ShimsContext.Create())
            {
                String result = moderateClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("LOW-1", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
                notificationService.Verify(mock => mock.Notify(rating), Times.Once());
            }
        }
        [TestMethod]
        public void GivenRatingIsGreaterThanOrEqualToHalfOfRatingCeiling_ShouldContainHIGH()
        {
            int rating = 2;
            int ratingCeling = 4;
            externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(true);
            moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
            using (ShimsContext.Create())
            {
                String result = moderateClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("HIGH=2", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
                notificationService.Verify(mock => mock.Notify(rating), Times.Once());
            }
        }
        [TestMethod]
        public void GivenRatingIsGreaterThanOrEqualToHalfOfRatingCeilingAndLastRatingIsEqualToRating_ShouldContainHighAndCACHED()
        {
            int rating = 2;
            int ratingCeling = 4;
            externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(true);
            moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
            using (ShimsContext.Create())
            {
                PrivateObject privSub = new PrivateObject(moderateClass, new PrivateType(typeof(ModerateClass)));
                privSub.SetField("lastRating", 2);
                String result = moderateClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("HIGH=2-CACHED", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
                notificationService.Verify(mock => mock.Notify(rating), Times.Once());             
            }
        }
        [TestMethod]
        public void GivenRatingIsLowerThanHalfOfRatingCeilingAndLastRatingIsEqualToRating_ShouldContainLOWAndCACHED()
        {
            int rating = 1;
            int ratingCeling = 4;
            externalRatingApprovalService.Setup(mock => mock.IsApproved(rating)).Returns(true);
            moderateClass = new ModerateClass(notificationService.Object, externalRatingApprovalService.Object);
            using (ShimsContext.Create())
            {
                PrivateObject privSub = new PrivateObject(moderateClass, new PrivateType(typeof(ModerateClass)));
                privSub.SetField("lastRating", 1);
                String result = moderateClass.CreateRatingString(rating, ratingCeling);
                Assert.AreEqual("LOW-1-CACHED", result);
                externalRatingApprovalService.Verify(mock => mock.IsApproved(rating), Times.Once());
                notificationService.Verify(mock => mock.Notify(rating), Times.Once());
            }
        }
    }
}

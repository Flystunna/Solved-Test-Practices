using Com.Crossover;
using Com.Crossover.Fakes;
using Com.Crossover.Services;
using Com.Crossover.Services.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RatingServer.Tests.Com.Crossover
{
    [TestClass]
    public class HardClassTest
    {
        private readonly Mock<NotificationService> notificationService = new Mock<NotificationService>();

        private readonly Mock<ExternalRatingApprovalService> externalRatingApprovalService = new Mock<ExternalRatingApprovalService>();

        [TestMethod]
        public void ShouldFail_WhenRatingIsHigherThanCeiling()
        {
            //ARRANGE AND ACT
            int rating = 2;
            int ratingCeling = 1;
            using (ShimsContext.Create())
            {
                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();
                //ASSERT
                Should.Throw<ArgumentException>(() => {
                    testClass.CreateRatingString(rating, ratingCeling);
                }).Message.ShouldBe("Cannot be over the hard ceiling");
                // rest of the test logic
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
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return false; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };

                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
               // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("NOT-APP", result);
            }
        }
        [TestMethod]
        public void ShouldFail_WhenExternalRatingApprovalServiceIsNotApprovedAndRatingIsLowerThanRatingCeiling()
        {
            //ARRANGE
            int rating = 1;
            int ratingCeling = 2;        
            //ACT
            using (ShimsContext.Create())
            {
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return false; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };

                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
                // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("NOT-APP", result);
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
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return true; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };
                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
                // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("TOP+"+rating+"True", result);           
            }
        }
        [TestMethod]
        public void GivenRatingIsLowerThanHalfOfRatingCeilingAndExternalRatingIsApproved_ShouldContainTOPAndLow()
        {
            //ARRANGE AND ACT
            int rating = 1;
            int ratingCeling = 4;
            using (ShimsContext.Create())
            {
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return true; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };
                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
                // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("LOW-"+rating+"True", result);
            }
        }
        [TestMethod]
        public void GivenRatingIsGreaterThanOrEqualToHalfOfRatingCeiling_ShouldContainHIGH()
        {
            //ARRANGE AND ACT
            int rating = 2;
            int ratingCeling = 4;
            using (ShimsContext.Create())
            {
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return true; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };
                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
                // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("HIGH="+rating+"True", result);
            }
        }
        [TestMethod]
        public void GivenRatingIsSameAsHardCache_ShouldContainCached()
        {
            //ARRANGE AND ACT
            int rating = 22;
            int ratingCeling = 44;
            using (ShimsContext.Create())
            {
                ShimNotificationService.AllInstances.NotifyInt32 = (c, s) => { };
                ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (c, s) => { return true; };
                ShimUtils.AllInstances.GetRatingDecoration = (s) => { return true; };
                ShimHardClass.StaticConstructor = () => { };
                var testClass = new HardClass();

                PrivateObject privSub = new PrivateObject(testClass, new PrivateType(typeof(HardClass)));
                // var privateObject = new PrivateObject(testClass);
                privSub.SetField("notificationService", new NotificationService());
                privSub.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
                FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
                UTILS.SetValue("UTILS", new Utils());
                FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
                HARD_CACHE.SetValue("HARD_CACHE", 22);
                string result = testClass.CreateRatingString(rating, ratingCeling);
                //ASSERT
                Assert.AreEqual("HIGH="+rating+"-CACHEDTrue", result);
            }
        }

        private void InitShimDependencies(Boolean externalRatingApproved, Boolean rationgDecoration)
        {
            ShimNotificationService.AllInstances.NotifyInt32 = (@this, s) => { };
            ShimExternalRatingApprovalService.AllInstances.IsApprovedInt32 = (@this, s) => { return externalRatingApproved; };
            ShimUtils.AllInstances.GetRatingDecoration = (@this) => { return rationgDecoration; };
            ShimHardClass.StaticConstructor = () =>
            {
            };
            var testClass = new HardClass();
            var privateObject = new PrivateObject(testClass);
            privateObject.SetField("notificationService", new NotificationService());
            privateObject.SetField("externalRatingApprovalService", new ExternalRatingApprovalService());
            FieldInfo UTILS = typeof(HardClass).GetField("UTILS", BindingFlags.NonPublic | BindingFlags.Static);
            UTILS.SetValue("UTILS", new Utils());
            FieldInfo HARD_CACHE = typeof(HardClass).GetField("HARD_CACHE", BindingFlags.NonPublic | BindingFlags.Static);
            HARD_CACHE.SetValue("HARD_CACHE", 22);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using TaskAPI.AspNetCore.Web;
using TaskAPI.AspNetCore.Web.Models.Persistent;
using TaskAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TaskAPI.AspnetCore.Tests
{
    [TestClass]
    public class UserControllerUnitTests
    {

        [TestMethod]
        public void GetAll()
        {
            var usrList = new List<User>() { new User() { EmailAddress = "test@email.com" } };
            Moq.Mock<ITaskService> obj = new Moq.Mock<ITaskService>();
            obj.Setup(x => x.Users).Returns(usrList);

            var userCtrl = new UserController(obj.Object);
            var result = userCtrl.GetAll();

            Assert.AreEqual(usrList.Count, result.ToList().Count);
        }

        [TestMethod]
        public void GetUser()
        {
            var email = "test@email.com";
            var userid = Guid.NewGuid().ToString();
            var usrList = new List<User>() { new User() { EmailAddress = email, userId = userid, IsDeleted = false } };
            Moq.Mock<ITaskService> obj = new Moq.Mock<ITaskService>();
            obj.Setup(x => x.Users).Returns(usrList);

            var usrCtrl = new UserController(obj.Object);

            var result = usrCtrl.Get(email);

            Assert.AreEqual(result.userId, userid);

        }

        [TestMethod]
        public void BlankRequestPostUser()
        {
            var email = "test@email.com";
            var userid = Guid.NewGuid().ToString();
            var usrList = new List<User>() { new User() { EmailAddress = email, userId = userid, IsDeleted = false } };
            Moq.Mock<ITaskService> obj = new Moq.Mock<ITaskService>();
            obj.Setup(x => x.Users).Returns(usrList);

            var usrCtrl = new UserController(obj.Object);
            var createUserRequest = new CreateUserRequest() { EmailAddress = email };
            var result = usrCtrl.Post(createUserRequest) as StatusCodeResult;

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void InvalidDeleteUser()
        {
            var email = "test@email.com";
            var userid = Guid.NewGuid().ToString();
            var usrList = new List<User>() { new User() { EmailAddress = email, userId = userid, IsDeleted = false } };
            Moq.Mock<ITaskService> obj = new Moq.Mock<ITaskService>();
            obj.Setup(x => x.Users).Returns(usrList);

            var usrCtrl = new UserController(obj.Object);
            var deleteUserRequest = new DeleteUserRequest() { UserId = Guid.NewGuid().ToString() };
            var result = usrCtrl.Delete(deleteUserRequest) as StatusCodeResult;

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}

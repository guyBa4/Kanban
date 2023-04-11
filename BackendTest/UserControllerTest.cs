using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
using Moq;
using System;

namespace BackendTest
{
    public class Tests
    {
            UserController controller;
               [SetUp]
            public void Setup()
            { 
                controller = new UserController();
            }

        //legalMail
        [Test]
            [TestCase("user@mail.com")]
            public void LegalEmail_GoodEmails_Success(string mail)
            {
                //arrange
                bool legal;
                //act
                legal = controller.LegalEmail(mail);
                //assert
                Assert.IsTrue(legal, "Registration got good email input but failed");
            }

            [Test]
            [TestCase(null)]
            [TestCase("")]
            [TestCase("@mail.com")]
            [TestCase("user@mail")]
            [TestCase("user@mail.a")]
            public void LegalEmail_WrongInputs_Fails(string mail)
            {
                //arrange
                bool res;
                //act
                res = controller.LegalEmail(mail);
                //assert
                Assert.IsFalse(res, "Wrong Input for email registration should fail");
            }

        //register
        [Test]
        [TestCase("user@mail.com")]
        public void register_Success(string mail)
            {
            //arrange
            User user = new User(mail, "Abc123");
            User u;
            //act
            try
            {
                u = controller.mockRegister(mail, "Abc123");
            } catch (Exception)
            {
                u = new User("bad@mail.com", "Ajh123");
            }
                //assert
                Assert.AreEqual(controller.GetUser(mail).Email, user.Email, "register should succeed");
            }

        [Test]
        [TestCase(null)]
        public void register_nullMail_Failed(string mail)
        {
            //arrange
            bool failed = false;
            User u;
            //act
            try
            {
                u = controller.mockRegister(mail, "Abc123");
            }
            catch (Exception)
            {
                failed = true;
            }
            //assert
            Assert.IsTrue(failed, "register should failed");
        }
        [Test]
        [TestCase(null)]
        public void register_nullPass_Failed(string pass)
        {
            {
                //arrange
                bool failed = false;
                User u;
                //act
                try
                {
                    u = controller.mockRegister("test@mail.com", pass);
                }
                catch (Exception)
                {
                    failed = true;
                }
                //assert
                Assert.IsTrue(failed, "register should failed");
            }
        }

        //isUnique
        [Test]
        [TestCase(null)]
        [TestCase("test@mail.com")]
        public void IsUniqueEmail_Unique_Failed(string mail)
        {
            {
                //arrange
                try
                {
                    controller.mockRegister(mail, "Abc123");
                }
                catch { }
                bool failed = false;
                //act
                try
                {
                    failed = !controller.IsUniqueEmail(mail);
                }
                catch (Exception)
                {
                    failed = true;
                }
                //assert
                Assert.IsTrue(failed, "email is not Unique but did not fail");
            }
        }

        [Test]
        [TestCase("test@mail.com")]
        public void IsUniqueEmail_Unique_Success(string mail)
        {
            
                //arrange
                
                bool failed = false;
                //act
                try
                {
                    failed = controller.IsUniqueEmail(mail);
                }
                catch (Exception)
                {
                    failed = false;
                }
                //assert
                Assert.IsTrue(failed, "email is unique but didnt pass");
        }


    }
}
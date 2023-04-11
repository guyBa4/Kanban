
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.Generic;
using System;
using System.Linq;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private BoardController boardCon;
        private UserController userCon;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserService()
        {
            boardCon = new BoardController();
            userCon = boardCon.GetUserController();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        public UserController GetUserCon()
        {
            return userCon;
        }

        public BoardController GetBoardCon()
        {
            return boardCon;
        }

        public ILog GetLog()
        {
            return log;
        }
        public Response LoadData()
        {
            try
            {
                userCon.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("couldn't load data");
                return new Response(e.Message);
            }
        }
        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData()
        {
            try
            {
                userCon.DeleteData();
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("couldn't load data");
                return new Response(e.Message);
            }
        }

        public Response Register(string userEmail, string password)
        {
            try
            {
                userCon.Register(userEmail, password);
                if (userCon.HasUser(userEmail))
                    log.Info("Register Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("Register attempt failed");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            try
            {
                BusinessLayer.User userB = userCon.GetUser(email);
                User userS = new User(userB.Email);
                userCon.Login(email, password);
                if (userCon.GetUser(email).logedin)
                    log.Info("Login Successfully");
                return Response<User>.FromValue(userS);
            }
            catch (Exception e)
            {
                log.Error("Login attempt failed");
                return Response<User>.FromError(e.Message);
            }
        }
        /// <summary>        
        /// Log out an logged in user.
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            try
            {
                userCon.Logout(email);
                if (!userCon.GetUser(email).logedin)
                    log.Info("Logeout Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("Logout attempt failed");
                return new Response(e.Message);
            }
        }
    }
}

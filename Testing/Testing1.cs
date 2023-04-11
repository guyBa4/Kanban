
using IntroSE.Kanban;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Frontend
{
    class Testing1
    {
        private Service userService;
        public Service UserService
        {
            get { return userService; }
        }

        public Testing1(Service userService)
        {
            this.userService = userService;
        }
        public void GetColName()
        {
            Response<string> Name = userService.GetColumnName("tamuz@gmail.com", "tamuz@gmail.com","Tboard",0);
            if (Name.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(Name.ErrorMessage);
            }
            else
            {
                Console.WriteLine("col name 0 -- " + Name.Value);
            }

            Response<string> Name1 = userService.GetColumnName("tamuz@gmail.com", "tamuz@gmail.com", "Tboard", 1);
            if (Name.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(Name1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("col name 1 -- " + Name1.Value);
            }

            Response<string> Name2 = userService.GetColumnName("tamuz@gmail.com", "tamuz@gmail.com", "Tboard", 2);
            if (Name.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(Name2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("col name 2 -- " + Name2.Value);
            }


        }


        public void RegisterTest()
        {
            Response delete = userService.DeleteData();
            if (delete.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(delete.ErrorMessage);
            }
            else
            {
                Console.WriteLine("delete data");
            }
            Console.Write("should work: ");
            Response reg1 = userService.Register("galh@gmail.com", "aA1235ss");
            if (reg1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            Console.Write("should work: ");
            Response reg2 = userService.Register("tamuz@gmail.com", "aA1235");
            if (reg2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            Console.Write("should work: ");
            Response reg5 = userService.Register("guy@gmail.com", "aY1111");
            if (reg5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            //shouldnt work - email
            Console.Write("should not work: ");
            Response reg3 = userService.Register("oran22@gmail.c", "aB1235@");
            if (reg3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            // shouldnt work - null email incorrect password
            Console.Write("should not work: ");
            Response reg4 = userService.Register(null, "1235");
            if (reg4.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            // shouldnt work - incorrect password
            Console.Write("should not work: ");
            Response reg55 = userService.Register("oran22@gmail.com", "aA123 5");
            if (reg55.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg55.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
        }

        public void LoginTests()
        {
            Console.Write("should work: ");
            Response res5 = userService.Login("tamuz@gmail.com", "aA1235");
            if (res5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }

            Console.Write("should work: ");
            Response res6 = userService.Login("galh@gmail.com", "aA1235ss");
            if (res6.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }
        }

        public void RemoveBoardTest()
        {
            Console.Write("------1---------- should work: ");
            Response RemoveBoard1 = userService.RemoveBoard("galh@gmail.com", "galh@gmail.com", "testBoardTamuz");
            if (RemoveBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(RemoveBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("removed board successfully");
            }

            Console.Write("----------2------------should not work: ");
            Response RemoveBoard2 = userService.RemoveBoard("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz");
            if (RemoveBoard2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(RemoveBoard2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("removed board successfully");
            }

            Console.Write("-----------3---------------should work: ");
            Response RemoveBoard3 = userService.RemoveBoard("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz");
            if (RemoveBoard3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(RemoveBoard3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("removed board successfully");
            }

            Console.Write("-------------4------------should work: ");
            Response RemoveBoard4 = userService.RemoveBoard("galh@gmail.com", "galh@gmail.com", "testBoardGal");
            if (RemoveBoard4.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(RemoveBoard4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("removed board successfully");
            }

            // should not work
            Console.Write("should not work: ");
            Response RemoveBoard5 = userService.RemoveBoard("galh@gmail.com", "galh@gmail.com", "testBoardGal");
            if (RemoveBoard5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(RemoveBoard5.ErrorMessage + ": should not work");
            }
            else
            {
                Console.WriteLine("removed board successfully" + ": should not work");
            }
        }


        public void ColumnAndBoardTests()
        {
            Console.Write("should work: ");
            Response addBoard1 = userService.AddBoard("tamuz@gmail.com", "testBoardTamuz");
            if (addBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }

            Console.Write("should not work: ");
            Response addBoard2 = userService.AddBoard("galh@gmail.com", "testBoardTamuz");
            if (addBoard2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }

            Console.Write("should work: ");
            Response addBoard3 = userService.AddBoard("galh@gmail.com", "testBoardGal");
            if (addBoard3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }

            Console.Write("should work: ");
            Response joinBoard = userService.JoinBoard("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz");
            if (joinBoard.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(joinBoard.ErrorMessage);
            }
            else
            {
                Console.WriteLine("joinBoard");
            }
            Console.Write("should work: ");
            Response joinBoard1 = userService.JoinBoard("tamuz@gmail.com", "galh@gmail.com", "testBoardGal");
            if (joinBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(joinBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("joinBoard");
            }

            DateTime duedate = DateTime.Today.AddMonths(3);
            Console.WriteLine(duedate);
            Console.WriteLine(duedate.CompareTo(DateTime.Now));
            Console.WriteLine("eeeeeeeeeeeeeeeeeee");
            Console.Write("should work: ");
            Response addTask1 = userService.AddTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", "TestTask1", "DescriptionTest1", new DateTime(2022, 12, 01));
            if (addTask1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            // should not work - not logged in
            Console.Write("should not work: ");
            Response addTask2 = userService.AddTask("guy@gmail.com", "galh@gmail.com", "testBoardGal", "TestTask6666", "DescriptionTest2", new DateTime(2022, 12, 01));
            if (addTask2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Console.Write("should work: ");
            Response addTask3 = userService.AddTask("galh@gmail.com", "galh@gmail.com", "testBoardGal", "TestTask2", "DescriptionTest2", new DateTime(2022, 12, 01));
            if (addTask3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            // shouldnt work - guy isnt creator of board
            Console.Write("should not work: ");
            Response addTask4 = userService.AddTask("tamuz@gmail.com", "guy@gmail.com", "testBoardGal", "TestTask66666", "DescriptionTest4", new DateTime(2022, 12, 01));
            if (addTask4.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Console.Write("should work: ");
            Response addTask5 = userService.AddTask("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", "TestTask3", "DescriptionTest3", new DateTime(2022, 12, 01));
            if (addTask5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Console.Write("should work: ");
            Response addTask6 = userService.AddTask("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", "TestTask4", "DescriptionTest4", new DateTime(2022, 12, 01));
            if (addTask6.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Console.Write("should work: ");
            Response addTask7 = userService.AddTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", "TestTask5", "DescriptionTest5", new DateTime(2022, 12, 01));
            if (addTask7.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Response addTask8 = userService.AddTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", "TestTask6", "DescriptionTest6", new DateTime(2022, 12, 01));
            if (addTask8.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addTask8.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added task");
            }

            Console.Write("should work:  1 1 1");
            Response updateTitle = userService.UpdateTaskTitle("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 0, 2, "gal :)");
            if (updateTitle.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(updateTitle.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Updated task title");
            }

            Console.Write("should work:  2 2 2");
            Response updateDescription = userService.UpdateTaskDescription("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0, 0, "gal is king:)");
            if (updateDescription.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(updateDescription.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Updated task description");
            }

            Console.Write("should work:  3 3 3");
            Response updateDueDate = userService.UpdateTaskDueDate("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 0, 3, new DateTime(2050, 12, 01));
            if (updateDueDate.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(updateDueDate.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Updated task due date");
            }

            Console.Write("should work: ");
            Response AdvanceTask1 = userService.AdvanceTask("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 0, 2);
            if (AdvanceTask1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }

            Console.Write("should work: ");
            Response AdvanceTask2 = userService.AdvanceTask("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 0, 2);
            if (AdvanceTask1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }

            Console.Write("should work: ");
            Response AdvanceTask3 = userService.AdvanceTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0, 5);
            if (AdvanceTask3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }

            Console.Write("should work: ");
            Response AdvanceTask4 = userService.AdvanceTask("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0, 6);
            if (AdvanceTask4.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }
            Response AdvanceTask5 = userService.AdvanceTask("galh@gmail.com", "galh@gmail.com", "testBoardGal", 0, 1);
            if (AdvanceTask5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }
            Response AssignTask5 = userService.AssignTask("galh@gmail.com", "galh@gmail.com", "testBoardGal", 1, 1, "tamuz@gmail.com");
            if (AssignTask5.ErrorOccured)
            {
                Console.Write("failed: -------------------");
                Console.WriteLine(AssignTask5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("AssignTask5 task -------------");
            }
            Console.Write("should work: ");
            Response GetColumn0 = userService.GetColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 0);
            if (GetColumn0.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(GetColumn0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Got column - 0");
            }

            Console.Write("should not work: ");
            Response GetColumnF = userService.GetColumn("guy@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 1);
            if (GetColumnF.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(GetColumnF.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Got column - 1");
            }

            Console.Write("should work: ");
            Response GetColumn1 = userService.GetColumn("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 1);
            if (GetColumn1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(GetColumn1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Got column - 1");
            }

            Console.Write("should work: ");
            Response GetColumn2 = userService.GetColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 2);
            if (GetColumn2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(GetColumn2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Got column - 2");
            }

            // should not work - negative limit
            Console.Write("should not work: ");
            Response limitColumn1 = userService.LimitColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 0, -2);
            if (limitColumn1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(limitColumn1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limiting to -2");
            }

            Console.Write("should work: ");
            Response limitColumn2 = userService.LimitColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 0, 11);
            if (limitColumn2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(limitColumn2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limiting to 11");
            }

            Console.Write("should work: ");
            Response getLimit = userService.GetColumnLimit("galh@gmail.com", "galh@gmail.com", "testBoardGal", 0);
            if (getLimit.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(getLimit.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limited to 11");
            }


            if (getLimit.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(getLimit.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limited to 11");
            }
            Response removeCol = userService.RemoveColumn("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0);
            if (removeCol.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(removeCol.ErrorMessage);
            }
            else
            {
                Console.WriteLine("remove column backlog");
            }
            Response renamCol = userService.RenameColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 1, "Gal's column");
            if (renamCol.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(renamCol.ErrorMessage);
            }
            else
            {
                Console.WriteLine("rename column in progress");
            }
            Response AddColumn = userService.AddColumn("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 2, "blablablablabala");
            if (AddColumn.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AddColumn.ErrorMessage);
            }
            else
            {
                Console.WriteLine("added  column in 2 blabla");
            }
            //Response MoveColumn = userService.MoveColumn("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 2, -2);
            //if (MoveColumn.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(MoveColumn.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("move from 2 to 0");
            //}
        }

        public void JoinBoardTest()
        {
            Console.WriteLine("should not work: ");
            Response JoinBoard1 = userService.JoinBoard("galh@gmail.com", "galh@gmail.com", "testBoardTamuz");
            if (JoinBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(JoinBoard1.ErrorMessage);
            }

            else
            {
                Console.WriteLine("Created user successfully");
            }

            Console.Write("should work: ");
            Response JoinBoard2 = userService.JoinBoard("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz");
            if (JoinBoard2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(JoinBoard2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            Console.WriteLine("should not work: ");
            Response JoinBoard3 = userService.JoinBoard("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz");
            if (JoinBoard3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(JoinBoard3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
        }


        public void loggedOutTest()
        {
            Console.Write("should work: ");
            Response Logout = userService.Logout("tamuz@gmail.com");
            if (Logout.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(Logout.ErrorMessage);
            }
            else
            {
                Console.WriteLine("logout tamuz");
            }

            Console.Write("should work: ");
            Response addBoard1 = userService.AddBoard("tamuz@gmail.com", "testBoardTamuz");
            if (addBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }
        }
        public void TamuzTest()
        {
            Response DeleteData = userService.DeleteData();
            if (DeleteData.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(DeleteData.ErrorMessage);
            }
            else
            {
                Console.WriteLine("delete data");
            }



            Response reg1 = userService.Register("Tamuz@gmail.com", "aA1235ss");
            if (reg1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }




            Response reg2 = userService.Register("gINDES@gmail.com", "aA1235ssSS");
            if (reg2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }




            Console.Write("should work: ");
            Response res3 = userService.Login("Tamuz@gmail.com", "aA1235ss");
            if (res3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }



            Console.Write("should work: ");
            Response res4 = userService.Login("gINDES@gmail.com", "aA1235ssSS");
            if (res4.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }



            Console.Write("should work: ");
            Response addBoard1 = userService.AddBoard("Tamuz@gmail.com", "testBoarD1");
            if (addBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }



            Console.Write("should work: ");
            Response addBoard3 = userService.AddBoard("gINDES@gmail.com", "testBoard2");
            if (addBoard3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }


            Console.Write("should work: ");
            Response JOINBAORD = userService.JoinBoard("gINDES@gmail.com", "Tamuz@gmail.com", "testBoarD1");
            if (JOINBAORD.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(JOINBAORD.ErrorMessage);
            }
            else
            {
                Console.WriteLine("JOINED board");
            }


            Console.Write("should work: ");
            Response JOINBAORD1 = userService.JoinBoard("Tamuz@gmail.com", "gINDES@gmail.com", "testBoard2");
            if (JOINBAORD1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(JOINBAORD1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("JOINED board");
            }


        }
        public void RunTests()
        {
            Console.Write("should work: ");
            Response DeleteData = userService.DeleteData();
            if (DeleteData.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(DeleteData.ErrorMessage);
            }
            else
            {
                Console.WriteLine("delete data");
            }

            RegisterTest();
            LoginTests();
            ColumnAndBoardTests();
            JoinBoardTest();
            //RemoveBoardTest();
            //loggedOutTest();
            //Console.Write("should work: ");
            Response res5 = userService.Login("tamuz@gmail.com", "aA1235");
            if (res5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }
            Console.Write("should work: ");
            Response LoadData = userService.LoadData();
            if (LoadData.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(LoadData.ErrorMessage);
            }
            else
            {
                Console.WriteLine("load data");
            }
            Response res51 = userService.Login("tamuz@gmail.com", "aA1235");
            if (res51.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(res51.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged in successfully");
            }
            Console.Write("should work: ");
            Response inProgress = userService.InProgressTasks("tamuz@gmail.com");
            if (inProgress.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(inProgress.ErrorMessage);
            }
            else
            {
                Console.WriteLine("inProgress succsess");

            }

            //Response delete = userService.DeleteData();
            //if (delete.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(delete.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("delete data");
            //}
        }
    }

}
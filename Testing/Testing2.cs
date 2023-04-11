using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Testing2
    {
        private Service userService;
        public Service UserService
        {
            get { return userService; }
        }
        public Testing2(Service userService)
        {
            this.userService = userService;
        }
        public void RunTest()
        {
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
            Console.Write("should work: ");
            Response reg1 = userService.Login("galh@gmail.com", "aA1235ss");
            if (reg1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("loging user successfully");
            }

            Response reg5 = userService.Login("tamuz@gmail.com", "aA1235");
            if (reg5.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("loging user successfully");
            }
            Response reg2 = userService.InProgressTasks("galh@gmail.com");
            if (reg2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("inProgress tasks returned successfully");
            }
            Response reg3 = userService.InProgressTasks("tamuz@gmail.com");
            if (reg3.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(reg3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("inProgress tasks returned successfully");
            }
            Response updateTitle = userService.UpdateTaskTitle("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 1, 2, "tamuz :)");
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
            Response updateDescription = userService.UpdateTaskDescription("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0, 0, "tamuz is a princess:)");
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
            Response updateDueDate = userService.UpdateTaskDueDate("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 0, 3, new DateTime(2023, 12, 01));
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
            Response AdvanceTask1 = userService.AdvanceTask("tamuz@gmail.com", "galh@gmail.com", "testBoardGal", 1, 2);
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
            Response AdvanceTask2 = userService.AdvanceTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 1, 5);
            if (AdvanceTask2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(AdvanceTask2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Advanced task");
            }
            Console.Write("should work: ");
            //Response AdvanceTask22 = userService.AdvanceTask("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 0, 0);
            //if (AdvanceTask22.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(AdvanceTask22.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("Advanced task");
            //}
            //Response JOINBAORD1 = userService.JoinBoard("tamuz@gmail.com", "galh@gmail.com", "testBoardTamuz");
            //if (JOINBAORD1.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(JOINBAORD1.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("JOINED board");
            //}
            Response addBoard1 = userService.AddBoard("tamuz@gmail.com", "testBoardTamuz121212");
            if (addBoard1.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(addBoard1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board");
            }
            Response limitColumn2 = userService.LimitColumn("galh@gmail.com", "tamuz@gmail.com", "testBoardTamuz121212", 1, 111);
            if (limitColumn2.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(limitColumn2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limiting to 111");
            }



            Response adding = userService.AddTask("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz121212", "--------", "--------", DateTime.Now.AddDays(5));
            if (adding.ErrorOccured)
            {
                Console.Write("failed: ");
                Console.WriteLine(adding.ErrorMessage);
            }
            else
            {
                Console.WriteLine("adding task after load data");
            }
            // userService.check();
            //Response removeCol = userService.RemoveColumn("tamuz@gmail.com", "tamuz@gmail.com", "testBoardTamuz", 1);
            //if (removeCol.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(removeCol.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("remove column in progress");
            //}
            //Response renamCol = userService.RenameColumn("galh@gmail.com", "galh@gmail.com", "testBoardGal", 1, "Gal's column");
            //if (renamCol.ErrorOccured)
            //{
            //    Console.Write("failed: ");
            //    Console.WriteLine(renamCol.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("rename column in progress");
            //}
        }
    }
}

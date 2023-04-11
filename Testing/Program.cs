using IntroSE.Kanban.Frontend;
using System;
using Testing;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            IntroSE.Kanban.Frontend.Testing1 testers = new IntroSE.Kanban.Frontend.Testing1(new IntroSE.Kanban.Backend.ServiceLayer.Service());
            Testing2 testers2 = new Testing2(testers.UserService);
            testers.RunTests();
            //testers2.RunTest();
        }
    }
}

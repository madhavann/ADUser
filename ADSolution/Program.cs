using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ActiveDirectoryHelper;
using System.IO;
namespace ADSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            StreamReader fsEmpId = new StreamReader(@"C:\Users\143968\Desktop\EmpId.txt");
            ActiveDirectoryHelper.ActiveDirectoryHelper adh = new ActiveDirectoryHelper.ActiveDirectoryHelper();
            while ((line = fsEmpId.ReadLine()) != null)
            {
                ADUserDetail userDetail = adh.GetUserByLoginName(line);
                if(userDetail.Manager!=null)
                    Console.WriteLine(userDetail.LoginName + "," + userDetail.Manager.LoginName );
                else
                    Console.WriteLine(userDetail.LoginName + "," + userDetail.ManagerName);
            }
            Console.ReadLine();
            

        }
    }
}

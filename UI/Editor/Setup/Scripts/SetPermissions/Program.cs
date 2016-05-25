using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wsu.DairyCafo.UI.Editor.Setup.SetPermissions
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
               "DairyCropSyst");

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory not found, aborting");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                try
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;

                    Console.WriteLine(file.Name);
                    Console.WriteLine(file.Attributes.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error setting permissions: " +
                        e.Message);
                }

                grantAccess(file.FullName);
            }
            foreach(var di in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                try
                {
                    di.Attributes &= ~FileAttributes.ReadOnly;

                    Console.WriteLine(di.Name);
                    Console.WriteLine(di.Attributes.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error setting permissions: " +
                        e.Message);
                }

                grantAccess(di.FullName);
            }
            dir.Attributes &= ~FileAttributes.ReadOnly;
            grantAccess(dir.FullName);
        }

        private static bool grantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }
    }
}

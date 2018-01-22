using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace WIXInstaller.Services
{
    class InternetConnection
    {        
        public static bool Check()
        {
            IPStatus status = IPStatus.Unknown;
            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send(@"google.com");
                status = pr.Status;
            }
            catch(Exception exc) { Debug.WriteLine(exc.Message); }

            if (status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }
    }
}

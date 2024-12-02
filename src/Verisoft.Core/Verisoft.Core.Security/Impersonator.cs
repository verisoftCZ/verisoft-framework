using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;

namespace Verisoft.Core.Security
{
    public class Impersonator : IDisposable
    {

        private SafeAccessTokenHandle safeAccessTokenHandle;

        public Impersonator(string userName, string domainName, string password, bool crossDomain = false)
        {
            if (crossDomain)
            {
                ImpersonateValidUser(userName, domainName, password, 9, 3);
            }
            else
            {
                ImpersonateValidUser(userName, domainName, password, 2, 0);
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(string lpszUserName, string lpszDomain, string lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        public void Dispose()
        {
            safeAccessTokenHandle.Dispose();
        }

        /*
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        */

        public void RunAsImpersonated(Action action)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(safeAccessTokenHandle, action);
#pragma warning restore CA1416 // Validate platform compatibility
        }

        private void ImpersonateValidUser(string userName, string domain, string password,
            int type, int provider)
        {
                //The RevertToSelf function terminates the impersonation of a client application.
                if (RevertToSelf())
                {
                if (!LogonUser(userName, domain, password, type,
                        provider, out safeAccessTokenHandle))
                    {
                        int ret = Marshal.GetLastWin32Error();
                        throw new Win32Exception(ret);
                    }
                    }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
    }
}
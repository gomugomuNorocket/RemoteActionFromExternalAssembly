
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActionsScriptLibrary
{
    public class ActionScript
    {
 
        #region Select Window
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);
        ///https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-showwindow
        private enum ShowWindowEnum
        {
            SW_HIDE             = 0,
            SW_SHOWNORMAL       = 1,
            SW_SHOWMINIMIZED    = 2,
            SW_MAXIMIZE         = 3,
            SW_SHOWMAXIMIZED    = 3,
            SW_SHOWNOACTIVATE   = 4,
            SW_SHOW             = 5,
            SW_MINIMIZE         = 6,
            SW_SHOWMINNOACTIVE  = 7,
            SW_SHOWNA           = 8,
            SW_RESTORE          = 9,
            SW_SHOWDEFAULT      = 10,
            SW_FORCEMINIMIZE    = 11
        };
        /// <summary>
        /// Select Window
        /// </summary>
        public void SelectWindow(string windowName)
        {
            try
            {           
                List<Process> Processes = Process.GetProcessesByName(windowName).ToList();
                if (Processes != null && Processes.Count > 0)
                {
                    Processes = Processes.Where(x => x.MainWindowHandle != IntPtr.Zero).ToList();
                    if (Processes != null && Processes.Count > 0)
                    {
                        foreach(Process process in Processes)
                        {
                            ShowWindow(process.MainWindowHandle, ShowWindowEnum.SW_RESTORE);

                            SetForegroundWindow(process.MainWindowHandle);
                        }        
                    }
                    else
                    {
                        throw new Exception(string.Format("Process with name: '{0}' doesn't have active MainWindowHandle!", windowName));
                    }                   
                }
                else
                {
                    throw new Exception(string.Format("Process with name: '{0}' doens't exist!", windowName));
                }  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Move Mouse
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        /// <summary>
        /// Move Mouse
        /// </summary>
        public void MoveMouse(Int64 x, Int64 y)
        {
            try
            {
                SetCursorPos((int)x, (int)y);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Click Mouse
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;      
        /// <summary>
        /// Click Mouse
        /// </summary>
        public void ClickMouse()
        {
            try
            {
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Send Text
        /// <summary>
        /// Send Text
        /// </summary>
        public void SendText(string text)
        {
            try
            {
                Console.WriteLine(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}

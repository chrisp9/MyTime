using System;
using System.Text;
using MyTime.Common;

namespace MyTime.Collector
{
   public class WindowHooker : IRunnable
   {
      // Hold a reference to the delegate so that it doesn't get Garbage Collected.
      private NativeMethods.WinEventDelegate _windowProcedureDelegate;

      private readonly INativeMethodWrapper _nativeMethods;

      public WindowHooker(INativeMethodWrapper nativeMethods)
      {
         _nativeMethods = nativeMethods;
      }

      public void Run()
      {
         _windowProcedureDelegate = WinEventProc;

         var hooked = _nativeMethods.SetWinEventHook(
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            IntPtr.Zero, 
            _windowProcedureDelegate, 0, 0, 
            NativeMethods.WINEVENT_OUTOFCONTEXT);
      }

      // TODO: Doesn't detect Lock screen. Need SystemEvents.SessionSwitch for this.
      // TODO: Probably should detect when the Active Desktop is changed (e.g. CTRL ALT DEL)
      // TODO: Need to consider when we switch back to the CURRENT desktop. Also what about Windows 10?
      // TODO: For Windows 10 probably need to install the hook for every desktop (will Antivirus stop this?)
      public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
      {
         Console.WriteLine(GetActiveWindowTitle() + "\r\n");
      }

      private string GetActiveWindowTitle()
      {
         const int maxChars = 256;

         // TODO: Use this to get owner process, then main window of this process.
         IntPtr windowHandle = IntPtr.Zero;

         StringBuilder buffer = new StringBuilder(maxChars);
         windowHandle = _nativeMethods.GetForegroundWindow();

         return _nativeMethods.GetWindowText(windowHandle, buffer, maxChars) > 0 
            ? buffer.ToString() 
            : null;
      }
   }
}

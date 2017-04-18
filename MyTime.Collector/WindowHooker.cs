using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
         _windowProcedureDelegate = new NativeMethods.WinEventDelegate(WinEventProc);

         IntPtr m_hhook = _nativeMethods.SetWinEventHook(
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            IntPtr.Zero, 
            _windowProcedureDelegate, 0, 0, NativeMethods.WINEVENT_OUTOFCONTEXT);
      }

      public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
      {
         Console.WriteLine(GetActiveWindowTitle() + "\r\n");
      }

      private string GetActiveWindowTitle()
      {
         const int nChars = 256;
         IntPtr handle = IntPtr.Zero;
         StringBuilder Buff = new StringBuilder(nChars);
         handle = _nativeMethods.GetForegroundWindow();

         if (_nativeMethods.GetWindowText(handle, Buff, nChars) > 0)
         {
            return Buff.ToString();
         }
         return null;
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyTime.Collector
{
   public static class NativeMethods
   {
      [DllImport("user32.dll")]
      internal static extern IntPtr GetForegroundWindow();

      [DllImport("user32.dll")]
      internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

      internal const uint WINEVENT_OUTOFCONTEXT = 0;
      internal const uint EVENT_SYSTEM_FOREGROUND = 3;

      public delegate void WinEventDelegate(
         IntPtr hWinEventHook, 
         uint eventType, 
         IntPtr hwnd, 
         int idObject, 
         int idChild, 
         uint dwEventThread,
         uint dwmsEventTime);

      [DllImport("user32.dll")]
      internal static extern IntPtr SetWinEventHook(
         uint eventMin, 
         uint eventMax, 
         IntPtr hmodWinEventProc, 
         WinEventDelegate lpfnWinEventProc, 
         uint idProcess,
         uint idThread, 
         uint dwFlags);
   }
}

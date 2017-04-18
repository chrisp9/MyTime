using System;
using System.Text;

namespace MyTime.Collector
{
   public interface INativeMethodWrapper
   {
      IntPtr GetForegroundWindow();
      int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

      IntPtr SetWinEventHook(
         uint eventMin,
         uint eventMax,
         IntPtr hmodWinEventProc,
         NativeMethods.WinEventDelegate lpfnWinEventProc,
         uint idProcess,
         uint idThread,
         uint dwFlags);
   }

   public class NativeMethodWrapper : INativeMethodWrapper
   {
      public IntPtr GetForegroundWindow()
      {
         return NativeMethods.GetForegroundWindow();
      }

      public int GetWindowText(IntPtr hWnd, StringBuilder text, int count)
      {
         return NativeMethods.GetWindowText(hWnd, text, count);
      }

      public IntPtr SetWinEventHook(
         uint eventMin,
         uint eventMax,
         IntPtr hmodWinEventProc,
         NativeMethods.WinEventDelegate lpfnWinEventProc,
         uint idProcess,
         uint idThread,
         uint dwFlags)
      {
         return NativeMethods.SetWinEventHook(
            eventMin,
            eventMax,
            hmodWinEventProc,
            lpfnWinEventProc,
            idProcess,
            idThread,
            dwFlags);
      }

   }
}

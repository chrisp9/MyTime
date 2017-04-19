using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MyTime.Common;
using ThreadingLib.Interfaces;

namespace MyTime.Collector
{
   public interface IProcessInformationListener
   {
      void OnProcessInformation(ProcessInformation processInfo);
   }

   public class ProcessInformationCollector : IProcessInformationListener
   {
      public void OnProcessInformation(ProcessInformation processInfo)
      {
         Console.WriteLine(processInfo.FriendlyName);
      }
   }

   public interface IProcessInformationSource
   {
      void Subscribe(IProcessInformationListener listener);
      void Unsubscribe(IProcessInformationListener listener);
   }

   public class WindowHooker : IRunnable, IProcessInformationSource
   {
      // Hold a reference to the delegate so that it doesn't get Garbage Collected.
      private NativeMethods.WinEventDelegate _windowEventDelegate;

      private readonly INativeMethodWrapper _nativeMethods;
      private readonly IActionQueue _queue;

      private readonly List<IProcessInformationListener> _listeners =
         new List<IProcessInformationListener>();

      public WindowHooker(INativeMethodWrapper nativeMethods, IActionQueue queue)
      {
         _nativeMethods = nativeMethods;
         _queue = queue;
      }

      public void Run()
      {
         _windowEventDelegate = WinEventProc;

         var hooked = _nativeMethods.SetWinEventHook(
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            NativeMethods.EVENT_SYSTEM_FOREGROUND, 
            IntPtr.Zero, 
            _windowEventDelegate, 0, 0, 
            NativeMethods.WINEVENT_OUTOFCONTEXT);
      }

      // TODO: Doesn't detect Lock screen. Need SystemEvents.SessionSwitch for this.
      // TODO: Probably should detect when the Active Desktop is changed (e.g. CTRL ALT DEL)
      // TODO: Need to consider when we switch back to the CURRENT desktop. Also what about Windows 10?
      // TODO: For Windows 10 probably need to install the hook for every desktop (will Antivirus stop this?)
      public void WinEventProc(
         IntPtr hWinEventHook,
         uint eventType,
         IntPtr hwnd,
         int idObject,
         int idChild, 
         uint dwEventThread, 
         uint dwmsEventTime)
      {
         var metadata = GetMetadata(GetActiveWindowProcess());

         foreach (var listener in _listeners)
         {
            _queue.QueueWorkItem(() => listener.OnProcessInformation(metadata));
         }
      }

      private Process GetActiveWindowProcess()
      {
         var windowHandle = _nativeMethods.GetForegroundWindow();

         uint pid = 0;
         _nativeMethods.GetWindowThreadProcessId(windowHandle, out pid);

         return Process.GetProcessById((int)pid);
      }

      private ProcessInformation GetMetadata(Process process)
      {
         const int maxChars = 256;

         // TODO: Use this to get owner process, then main window of this process.
         var windowHandle = IntPtr.Zero;

         StringBuilder buffer = new StringBuilder(maxChars);
         windowHandle = process.MainWindowHandle;

         uint pid = 0;
         _nativeMethods.GetWindowThreadProcessId(windowHandle, out pid);

         var title = _nativeMethods.GetWindowText(windowHandle, buffer, maxChars) > 0
            ? buffer.ToString()
            : null;

         return ProcessInformation.From(process, title);
      }

      public void Subscribe(IProcessInformationListener listener)
      {
         _listeners.Add(listener);
      }

      public void Unsubscribe(IProcessInformationListener listener)
      {
         _listeners.Remove(listener);
      }
   }
}

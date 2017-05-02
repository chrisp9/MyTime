using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTime.Common
{
   public class ProcessInformation : IProcessInformation, IEquatable<ProcessInformation>
   {
      public bool Equals(ProcessInformation other)
      {
         return string.Equals(ProcessName, other?.ProcessName) && string.Equals(ProcessDescription, other?.ProcessDescription);
      }

      private const string UnknownProcess = "Unknown Process";
      private readonly Process _process;

      public string ProcessName { get; private set; }

      public string ProcessDescription { get; private set; }

      public string FriendlyName => ProcessDescription ?? ProcessName ?? UnknownProcess;

      public static ProcessInformation From(Process process, string mainWindowTitle)
      {
         return new ProcessInformation(process, mainWindowTitle);
      }

      private ProcessInformation(Process process, string mainWindowTitle)
      {
         _process = process;
         Safely(() => ProcessDescription = _process.MainModule.FileVersionInfo.FileDescription);
         Safely(() => ProcessName = _process.ProcessName);
      }

      private void Safely(Action a)
      {
         try
         {
            a();
         }
         catch(Win32Exception e) { }
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return obj.GetType() == GetType() && Equals((ProcessInformation)obj);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            return ((ProcessName?.GetHashCode() ?? 0) * 397) ^ (ProcessDescription?.GetHashCode() ?? 0);
         }
      }
   }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTime.Common
{
   public class ProcessInformation
   {
      private readonly Process _process;
      public string FriendlyName { get; }

      public static ProcessInformation From(Process process, string mainWindowTitle)
      {
         return new ProcessInformation(process, mainWindowTitle);
      }

      private ProcessInformation(Process process, string mainWindowTitle)
      {
         _process = process;
         FriendlyName = mainWindowTitle ?? process.ProcessName;
      }
   }
}

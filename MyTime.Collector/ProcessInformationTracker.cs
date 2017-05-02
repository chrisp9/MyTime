using System;
using System.Collections.Generic;
using MyTime.Common;

namespace MyTime.Collector
{
   public interface IProcessInformationTracker
   {
      void SwitchTo(IProcessInformation process);
   }

   public class ProcessInformationTracker : IProcessInformationTracker
   {
      private readonly ICurrentTime _time;
      private readonly Dictionary<IProcessInformation, TimeSpan> _lookup;
      private IProcessInformation _activeProcess;
      private DateTime _activeProcessTime;

      public ProcessInformationTracker(ICurrentTime time)
      {
         _time = time;
         _lookup = new Dictionary<IProcessInformation, TimeSpan>();
      }

      public void SwitchTo(IProcessInformation process)
      {
         if (_activeProcess != null)
         {
            var totalTimeForProcess = new TimeSpan(_time.UtcNow.Ticks - _activeProcessTime.Ticks);
            _lookup.AddOrUpdate(process, totalTimeForProcess);
         }

         _activeProcess = process;
         _activeProcessTime = _time.UtcNow;
      }

      public TimeSpan GetTotalTime(IProcessInformation p)
      {
         return _lookup.GetTotalTime(p);
      }
   }
}

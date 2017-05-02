using System;

namespace MyTime.Common
{
   public interface ICurrentTime
   {
      DateTime UtcNow { get; }
   }

   public class CurrentTime : ICurrentTime
   {
      public DateTime UtcNow => DateTime.UtcNow;
   }
}

using System;
using System.Collections.Generic;

namespace MyTime.Common
{
   public static class DictionaryExtensions
   {
      public static void AddOrUpdate<TKey>(
         this Dictionary<TKey, TimeSpan> dictionary,
         TKey key,
         TimeSpan value)
      {
         TimeSpan existing;
         if (dictionary.TryGetValue(key, out existing))
         {
            dictionary[key] = existing + value;
         }
         else
         {
            existing = value;
            dictionary[key] = existing; 
         }
      }

      public static TimeSpan GetTotalTime<TKey>(
         this Dictionary<TKey, TimeSpan> dictionary,
         TKey key)
      {
         TimeSpan v;
         return dictionary.TryGetValue(key, out v) ? v : TimeSpan.Zero;
      }
   }
}

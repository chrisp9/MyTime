using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTime.Shared
{
   public static class DictionaryExtensions
   {
      public static void AddOrUpdate<TKey(
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
            dictionary[key] = existing; _
         }

         return value;
      }
   }
}

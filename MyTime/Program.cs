using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using MyTime.Collector;
using MyTime.Common;

namespace MyTime
{
   class Program
   {
      static void Main(string[] args)
      {
         var cb = new ContainerBuilder();
         cb.RegisterType<NativeMethodWrapper>().AsImplementedInterfaces().SingleInstance();
         cb.RegisterType<WindowHooker>().AsImplementedInterfaces().SingleInstance();

         var container = cb.Build();

         foreach (var service in container.Resolve<IEnumerable<IRunnable>>())
         {
            service.Run();
         }
         Application.Run();

         Console.ReadLine();
      }
   }
}

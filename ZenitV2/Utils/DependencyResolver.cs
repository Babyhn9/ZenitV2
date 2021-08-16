using Autofac;
using Autofac.Builder;
using ZenitV2.BL.Interfaces;
using ZenitV2.BL.Realizations;
using ZenitV2.MVVMCore;
using ZenitV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZenitV2.Utils
{
    public static class DependencyResolver
    {
        private static IContainer _container;
        public static ILifetimeScope Scope { get; private set; }
        public static List<MVVMBind> Binds { get; set; } = new List<MVVMBind>();
        

        public static void Implement()
        {
            var builder = new ContainerBuilder();
            ImplementUIElements(builder);
            ImplementBuissnesLayer(builder);
            _container = builder.Build();
            Scope = _container.BeginLifetimeScope();
        }

        private static void ImplementBuissnesLayer(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.DefinedTypes.Where(el => el.GetCustomAttribute<BuissnesAttribute>() != null);
            
            foreach(var type in types)
            {
                foreach(var implementedInterface in type.GetInterfaces())
                {
                    builder.RegisterType(type).As(implementedInterface);
                }
            }

        }

        private static void ImplementUIElements(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var winds = new List<TypeInfo>();
            var vms = new List<TypeInfo>();

            foreach (var type in assembly.DefinedTypes)
            {
                if (type?.BaseType?.Name == "Window")
                {
                    winds.Add(type);
                    builder.RegisterType(type);
                    continue;
                }

                if (type?.BaseType?.Name == nameof(BaseViewModel))
                {
                    vms.Add(type);
                    builder.RegisterType(type);
                    continue;
                }
            }


            foreach (var vm in vms)
            {
                var attr = vm.GetCustomAttribute<AttachToWindowAttribute>();

                if (attr == null)
                {
                    MessageBox.Show($"Внимание, обнаруженна ошибка: нет связи для {vm.Name}, сообщите об этой проблеме разработчику!",
                        "Ошибка!",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    continue;
                }

                var win = winds.FirstOrDefault(el => el.Name == attr.WinName);

                if(win == null)
                {
                    MessageBox.Show($"Внимание, обнаруженна ошибка: для {vm.Name} не найденно окно, сообщите об этой проблеме разработчику!",
                        "Ошибка!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    continue;
                }

                Binds.Add(new MVVMBind(win, vm));
            }
        }






    }
}

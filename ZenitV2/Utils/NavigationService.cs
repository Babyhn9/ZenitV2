using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZenitV2.Utils
{
    public static class NavigationService
    {
        public static void ShowWindowDialog<T>() where T : Window =>    GetWindow<T>().ShowDialog();
        public static void ShowWindow<T>() where T : Window => GetWindow<T>().Show();

        private static Window GetWindow<T>() where T : Window
        {
            var win = DependencyResolver.Scope.Resolve<T>();
            var bind = DependencyResolver.Binds.FirstOrDefault(el => el.WindowType.Name == typeof(T).Name);
            var vm = DependencyResolver.Scope.Resolve(bind.ViewModelType);
            win.DataContext = vm;
            return win;
        }


    }
}

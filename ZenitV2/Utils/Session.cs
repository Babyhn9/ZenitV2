using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace ZenitV2.Utils
{
    public class Session
    {
        public static Session Current { get; } = new Session();


        private Application _lastReturnedApplication;
        private List<int> _applicationProcessesId;
        private Session()
        {

        }


        /// <summary>
        /// Если перед вызовом метода прошлое приложение не было закрыто, оно автоматически закрывается
        /// </summary>
        /// <returns></returns>
        public Application CreateWordApplication()
        {
            if (_lastReturnedApplication != null)
                DestroyWordApplication();
            var wordProcessesBeforeStart = Process.GetProcessesByName("WINWORD").Select(el => el.Id);
            _lastReturnedApplication = new Application();
            var wordProcessesAfterStart = Process.GetProcessesByName("WINWORD").Select(el => el.Id).ToList();

            wordProcessesAfterStart.AddRange(wordProcessesBeforeStart);
            _applicationProcessesId = wordProcessesAfterStart.Distinct().ToList();
            return _lastReturnedApplication;
        }

        public void DestroyWordApplication()
        {
            var process = _applicationProcessesId.Select(el => Process.GetProcessById(el)).ToList();
            process.ForEach(el => el?.Kill());
            _lastReturnedApplication = null;
        }

    }
}

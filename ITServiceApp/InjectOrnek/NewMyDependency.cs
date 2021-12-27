using System;
using System.Diagnostics;

namespace ITServiceApp.InjectOrnek
{
    public class NewMyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine(message:$"{DateTime.Now}-{message}");
        }
    }
}

using System.Diagnostics;

namespace ITServiceApp.InjectOrnek
{
    public class MyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}

using System.Reflection;

namespace S7TechIntegracao.API.Utils
{
    public static class Log4Net
    {
        public static log4net.ILog Log { get { return log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); ; } }
    }
}
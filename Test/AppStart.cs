using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    ///       Running the InitDataBasePath method at app start
    /// </summary>
    public class AppStart
    {
        /// <summary>
        ///       Initializes the database path to the current path of the application
        /// </summary>
        public static void InitDataBasePath()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    public class Logger
    {
        public static void info(object message)
        {
            Console.WriteLine("[INFO] " + message);
        }
    }
}

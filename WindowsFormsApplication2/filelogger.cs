using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication2
{
    class filelogger
    {
        public String filePath { get; set; }

        public String GenerateFilename(String prefix, String ext)
        {
            return prefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid( ).ToString("N") + "." + ext;
        }

        public void Log(string message)
        {
            if(filePath.Length == 0) return;
            File.AppendAllText(filePath, message + "\n");
        }
    }
}

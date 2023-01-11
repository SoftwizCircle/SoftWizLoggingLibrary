using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public class SWCLoggingOptions
    {
        private string _folderPath = "";
        public virtual string FilePath { get; set; }

        public virtual string FolderPath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_folderPath) ? _folderPath : System.IO.Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "\\SWCLOG";
            }

            set { _folderPath = value; }
        }
    }
}
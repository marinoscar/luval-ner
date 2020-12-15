using edu.stanford.nlp.ie.crf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luval.UiPath.NER.Activities
{
    public static class Helper
    {

        private static CRFClassifier _classifier;

        public static CRFClassifier GetClassifier()
        {
            if (_classifier != null) return _classifier;

            var modelDir = new DirectoryInfo(Path.Combine(GetExecDirLocation(), "Models"));
            if (!modelDir.Exists) throw new Exception(string.Format("The required NER model directory {0} is missing", modelDir.FullName));
            var modelFile = new FileInfo(Path.Combine(modelDir.FullName, "english.muc.7class.distsim.crf.ser.gz"));
            if (!modelFile.Exists) throw new Exception(string.Format("The required NER model file {0} is missing", modelFile.Name));
            
            _classifier = CRFClassifier.getClassifierNoExceptions(modelFile.FullName);
            return _classifier;
        }

        public static string GetExecDirLocation()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}

using System.IO;
using System.Reflection;

namespace AuthorizerTests
{
    public class TesterUtils
    {
        public static string GetFileContents(string sampleFile)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = string.Format("AuthorizerTests.Resources.{0}", sampleFile);
            string[] names = asm.GetManifestResourceNames();
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}
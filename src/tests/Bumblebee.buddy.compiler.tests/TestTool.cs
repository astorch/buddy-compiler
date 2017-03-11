using System;
using System.IO;
using System.Reflection;

namespace Bumblebee.buddy.compiler.tests {
    public static class TestTool {
        public static string GetResourceFileContent(string fullQualifiedPath) {
            using (Stream rscStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullQualifiedPath)) {
                if (rscStream == null) throw new InvalidOperationException("There is no resource with the given path " + fullQualifiedPath);
                using (StreamReader streamReader = new StreamReader(rscStream)) {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
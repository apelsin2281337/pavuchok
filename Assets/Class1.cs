using System.Reflection;

namespace Spider_Solitaire
{
    public class Class1
    {
        public static Stream GetEmbeddedAsset(string resourceName)
        {
            // Get the current executing assembly (DLL)
            var assembly = Assembly.GetExecutingAssembly();

            // Build the full resource name (namespace + filename)
            string resourcePath = "Spider_Solitaire.assets" + resourceName;

            // Open a stream to the embedded resource
            Stream resourceStream = assembly.GetManifestResourceStream(resourcePath);

            if (resourceStream == null)
            {
                throw new FileNotFoundException("Embedded resource not found: " + resourcePath);
            }

            return resourceStream;
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace WeddingApp.Lib.Services
{
    public static class ResourceService
    {
        /// <summary>
        /// Returns the string contained in the named resource from the
        /// given assembly.
        /// </summary>
        /// <param name="resourceName">The name of the resource to load.</param>
        /// <param name="fromAssembly">The assembly to load the resource from.</param>
        public static async Task<string> GetResourceString(string resourceName, Assembly? fromAssembly = null)
        {
            fromAssembly ??= Assembly.GetAssembly(typeof(ResourceService));
            if (fromAssembly is null) throw new ArgumentNullException(nameof(Assembly));
            
            using var reader = new StreamReader(
                fromAssembly.GetManifestResourceStream(resourceName)
                ?? throw new ArgumentNullException($"Could not get stream for embedded resource named {resourceName} in {fromAssembly}."));
            return await reader.ReadToEndAsync();
        }
    }
}
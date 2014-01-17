
namespace Examples.ExtensionsImplements
{
    // Added to WWB.NET language by AddExtension("Imports Example1.Extensions.ScriptingLanguage"
    [Scriptable]
    public static class ScriptingLanguage
    {
        // accessible by all classes in this assembly
        internal static IHost Host { get; private set; }

        // make TheIncident available to WinWrap Basic script
        [Scriptable]
        public static Incident TheIncident { get { return Host.TheIncident; } }

        public static void SetHost(IHost host)
        {
            Host = host;
        }
    }
}

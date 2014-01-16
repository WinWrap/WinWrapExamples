
namespace Examples.Extensions
{
    /// AddScriptableReference(..., "Examples.Extensions.ScriptingLanguage")
    /// adds this static class to the scripting language directly
    [Scriptable] public static class ScriptingLanguage
    {
        // accessible by all classes in this assembly
        internal static IHost Host { get; private set; }

        // make TheIncident available to the script
        [Scriptable] public static Incident TheIncident { get { return Host.TheIncident; } }

        // accessible by application, not script
        public static void SetHost(IHost host)
        {
            Host = host;
        }
    }
}

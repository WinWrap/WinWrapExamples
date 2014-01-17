
namespace Examples.ExtensionsImplements
{
    // used by application, not used by WWB.NET scripts
    public interface IHost
    {
        Incident TheIncident { get; }
        void Log(string text);
    }
}


namespace Examples.Extensions
{
    // used by application, not used by WinWrap Basic scripts
    public interface IHost
    {
        Incident TheIncident { get; }
        void Log(string text);
    }
}

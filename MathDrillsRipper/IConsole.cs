namespace MathDrillsRipper
{
    public interface IConsole
    {
        void WriteInfo(string format, params object[] values);
        void WriteWarning(string format, params object[] values);
        void WriteError(string format, params object[] values);
    }
}
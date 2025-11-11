using System.Diagnostics;
using System.Text;

namespace OOP2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new DebugLogger();
            ConsoleLogger consoleLogger = new ConsoleLogger();
            consoleLogger.Error("Midagi on valesti");
            consoleLogger.Warn("Midagi on vasrti valesti");

            // --------------------------------------------
            // Siit edasi ei muuda
            // --------------------------------------------
            //logger.Debug("Tekkis viga");
            //logger.Info("Programm käivitus");
            //logger.Warn("Arve summa on null");
            //logger.Error("Veebiteenus ei vasta");
            var watch = new Stopwatch();
            watch.Start();

            for (var i = 0; i < 1000; i++)
            {
                logger.Info("See on informatiivne teade");
            }

            watch.Stop();
            Debug.WriteLine(watch.Elapsed);
        }
    }

    public interface ILogger
    {
        public void Debug(string message);
        public void Info(string message);
        public void Warn(string message);
        public void Error(string message);
    }

    public abstract class BaseLogger : ILogger
    {
        public virtual void Debug(string message)
        {
            Write("Debug", message);
        }

        public virtual void Error(string message)
        {
            Write("Error", message);
        }

        public virtual void Info(string message)
        {
            Write("Info", message);
        }

        public virtual void Warn(string message)
        {
            Write("Warn", message);
        }

        protected string FormatLog(string logLevel, string message)
        {
            if (message == null)
            {
                return null;
            }

            var builder = new StringBuilder(message.Length + 30);
            builder.Append(DateTime.Now);
            builder.Append(" ");
            builder.Append(logLevel.ToUpper());
            builder.Append(": ");
            builder.AppendLine(message);

            return builder.ToString();
        }

        protected abstract void Write(string logLevel, string message);
    }

    public class NullLogger : BaseLogger
    {
        protected override void Write(string logLevel, string message)
        {
        }
    }

    public class DebugLogger : BaseLogger
    {
        protected override void Write(string logLevel, string message)
        {
            var logMessage = FormatLog(logLevel, message);

            System.Diagnostics.Debug.WriteLine(logMessage);
        }
    }

    public class ConsoleLogger : BaseLogger
    {
        private ConsoleColor _algneVarv = Console.ForegroundColor;
        private ConsoleColor _hoiatus = ConsoleColor.Yellow;
        private ConsoleColor _error = ConsoleColor.Red;
        protected override void Write(string logLevel, string message)
        {
            var logMessage = FormatLog(logLevel, message);
            Console.WriteLine(logMessage);
        }

        public override void Warn(string message)
        {

            Console.ForegroundColor = _hoiatus;
            Write("Warn", message);
            Console.ForegroundColor = _algneVarv;
        }
        public override void Error(string message)
        {
            Console.ForegroundColor = _error;
            Write("Error", message);
            Console.ForegroundColor = _algneVarv;
        }
    }
}

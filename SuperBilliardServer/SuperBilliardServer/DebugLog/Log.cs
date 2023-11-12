using GameFramework;
using SuperBilliardServer.Debug.MessageStyle;
using System.Diagnostics;

namespace SuperBilliardServer.Debug
{
    public static class Log
    {

        public enum DebugType
        {
            Info,
            Warning,
            Error,
            Debug,
        }
        public static bool OnDebug = true;
        private static void LogMessage(DebugType debugType, object message)
        {
            if (message == null)
                return;

            switch (debugType)
            {
                case DebugType.Info:
                    Console.WriteLine(message.ToString() + ReadStackMessage());
                    break;
                case DebugType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(message.ToString() + ReadStackMessage());
                    ReadStackMessage();
                    Console.ResetColor();
                    break;
                case DebugType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message.ToString() + ReadStackMessage());
                    ReadStackMessage();
                    Console.ResetColor();
                    break;
                case DebugType.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(message.ToString() + ReadStackMessage());
                    ReadStackMessage();
                    Console.ResetColor();
                    break;
                default:
                    break;
            }

        }

        private static string ReadStackMessage()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(3);
            string methodName = stackFrame.GetMethod().Name;
            string className = stackFrame.GetMethod().DeclaringType.Name;
            return string.Format("---The message on :Type:{0}.{1},Line:{2}", className, methodName, stackFrame.GetFileLineNumber());
        }

        public static void Info(string message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, message);
        }
        public static void Info<T1>(T1 message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, message);
        }
        public static void Info<T1>(string message, T1 t1)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1));
        }
        public static void Info<T1, T2>(string message, T1 t1, T2 t2)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2));
        }
        public static void Info<T1, T2, T3>(string message, T1 t1, T2 t2, T3 t3)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3));
        }
        public static void Info<T1, T2, T3, T4>(string message, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4));
        }
        public static void Info<T1, T2, T3, T4, T5>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4, t5));
        }
        public static void Info<T1, T2, T3, T4, T5, T6>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4, t5, t6));
        }
        public static void Info<T1, T2, T3, T4, T5, T6, T7>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4, t5, t6, t7));
        }

        public static void Warning(string message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, message);
        }
        public static void Warning<T1>(T1 message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, message);
        }
        public static void Warning<T1>(string message, T1 t1)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, string.Format(message, t1));
        }
        public static void Warning<T1, T2>(string message, T1 t1, T2 t2)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, string.Format(message, t1, t2));
        }
        public static void Warning<T1, T2, T3>(string message, T1 t1, T2 t2, T3 t3)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, string.Format(message, t1, t2, t3));
        }
        public static void Warning<T1, T2, T3, T4>(string message, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, string.Format(message, t1, t2, t3, t4));
        }
        public static void Warning<T1, T2, T3, T4, T5>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4, t5));
        }
        public static void Warning<T1, T2, T3, T4, T5, T6>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Info, string.Format(message, t1, t2, t3, t4, t5, t6));
        }
        public static void Warning<T1, T2, T3, T4, T5, T6, T7>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Warning, string.Format(message, t1, t2, t3, t4, t5, t6, t7));
        }

        public static void Error(string message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, message);
        }
        public static void Error<T1>(T1 message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, message);
        }
        public static void Error<T1>(string message, T1 t1)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1));
        }
        public static void Error<T1, T2>(string message, T1 t1, T2 t2)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2));
        }
        public static void Error<T1, T2, T3>(string message, T1 t1, T2 t2, T3 t3)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3));
        }
        public static void Error<T1, T2, T3, T4>(string message, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3, t4));
        }
        public static void Error<T1, T2, T3, T4, T5>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3, t4, t5));
        }
        public static void Error<T1, T2, T3, T4, T5, T6>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3, t4, t5, t6));
        }
        public static void Error<T1, T2, T3, T4, T5, T6, T7>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3, t4, t5, t6, t7));
        }
        public static void Debug(string message)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, message);
        }
        public static void Debug<T1>(T1 t1)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, t1);
        }
        public static void Debug<T1>(string message, T1 t1)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1));
        }
        public static void Debug<T1, T2>(string message, T1 t1, T2 t2)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1, t2));
        }
        public static void Debug<T1, T2, T3>(string message, T1 t1, T2 t2, T3 t3)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1, t2, t3));
        }
        public static void Debug<T1, T2, T3, T4>(string message, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1, t2, t3, t4));
        }
        public static void Debug<T1, T2, T3, T4, T5>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1, t2, t3, t4, t5));
        }
        public static void Debug<T1, T2, T3, T4, T5, T6>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Error, string.Format(message, t1, t2, t3, t4, t5, t6));
        }
        public static void Debug<T1, T2, T3, T4, T5, T6, T7>(string message, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (OnDebug == false)
            {
                return;
            }
            LogMessage(DebugType.Debug, string.Format(message, t1, t2, t3, t4, t5, t6, t7));
        }
        public static void ShowStyle<T>(params string[] strings) where T : class, IMessageStyle, new()
        {
            IMessageStyle messageStyle = ReferencePool.Acquire<T>();
            LogMessage(DebugType.Info, messageStyle.MessageParser(strings));
            ReferencePool.Release(messageStyle);
        }
    }
}

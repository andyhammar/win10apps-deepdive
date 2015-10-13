using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace UwpDeepDive.MainApp.Helpers
{
    public class AppLog
    {
        static readonly CultureInfo _culture = new CultureInfo("sv-se");

        public static void Write([CallerFilePath]string callerFilePath = null, [CallerMemberName] string callerName = null, string text = null)
        {
            var callerFileName = Path.GetFileNameWithoutExtension(callerFilePath);
            Debug.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ttt", _culture)} [{callerFileName}.{callerName}] {text}");
        }
    }
}
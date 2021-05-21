using sudo.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace sudo
{
    /// <summary>
    /// Sudo is a passthrough program which will execute the arguments in an elevated context
    /// and copy the output back into the current command line window.
    /// 
    /// <para>
    /// Syntax:
    /// </para>
    /// <para>
    /// * $> sudo command {arguments}
    /// </para>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            while (count < args.Length)
            {
                string arg = args[count++];
                if (arg.StartsWith("--"))
                    Flags.Add(arg);
                else if (arg.StartsWith("-"))
                    Properties.Add(arg, args[count++]);
                else
                    break;
            }

            if (count > 1)
                ArrayUtil.ShrinkRight(ref args, count - 1);

            if (args.Length == 0)
                throw new ArgumentException($"No command specified.");

            if (Flags.Contains(Flags.UNESCAPE_CHARS))
            {
                Dictionary<string, string> escapes = new Dictionary<string, string> {
                    { "\\^\\&", "&" },
                    { "\\^\\<", "<" },
                    { "\\^\\>", ">" },
                    { "\\^\\|", "|" },
                    { "\\^\\\\", "\\" },
                    { "\\^\\^", "^" },
                    { "\\^\\^\\!", "!" },
                };
                for (int i = 0; i < args.Length; i++)
                {
                    foreach (KeyValuePair<string, string> escapedChar in escapes)
                    {
                        args[i] = Regex.Replace(args[i], escapedChar.Key, escapedChar.Value);
                    }
                }
            }

            string stdOut = Flags.Contains(Flags.SHOW_WINDOW) ? string.Empty : Properties.Get(Properties.STDOUT, Path.GetTempFileName);
            string stdErr = Flags.Contains(Flags.SHOW_WINDOW) ? string.Empty : Properties.Get(Properties.STDERR, Path.GetTempFileName);

            string command = "cmd";
            string arguments = $"/c \"{string.Join(" ", args)}\"";
            
            if (!Flags.Contains(Flags.SHOW_WINDOW))
                arguments += $" > {stdOut} 2> {stdErr}";

            if (Flags.Contains(Flags.DEBUG))
            {
                bool isElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                string loggedInUser = $"{Environment.UserDomainName}\\{Environment.UserName}";

                if (Flags.Count > 0)
                    Console.WriteLine($"Flags are {string.Join(", ", Flags.Values)}");
                if (Properties.Count > 0) {
                    Console.WriteLine($"Properties are");
                    foreach (KeyValuePair<string, string> kv in Properties.Values) {
                        Console.WriteLine($"    {kv.Key}{(Properties.IsDefault(kv.Key) ? " (default)" : "")} => {kv.Value}");
                    }
                }
                Console.WriteLine($"------------------------------------------------------------------");
                Console.WriteLine($"Currently logged in as {loggedInUser}{(isElevated ? " (admin)" : "")}");
                Console.WriteLine($"Attempting to execute as {loggedInUser} (admin)");
                Console.WriteLine($"$> {command} {arguments}");
            }

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo {
                WindowStyle = Flags.Contains(Flags.SHOW_WINDOW) ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                FileName = command,
                Arguments = arguments,
                Verb = "runas",
            };
            process.Start();

            Exception processException = null;
            try
            {
                process.WaitForExit();
            }
            catch (Exception ex) {
                processException = ex;
            }

            if (!Flags.Contains(Flags.SHOW_WINDOW))
            {
                StdOutErrUtil.WriteAndDelete(stdOut, Properties.STDOUT, Console.Out);
                StdOutErrUtil.WriteAndDelete(stdErr, Properties.STDERR, Console.Error);
            }

            if (processException != null) {
                Console.Error.WriteLine(processException);
            }

            // pass the exit code through
            if (!Flags.Contains(Flags.DEBUG) || process.ExitCode != 0)
                Environment.Exit(process.ExitCode);
        }
    }
}

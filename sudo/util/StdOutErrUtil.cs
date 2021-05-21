using System;
using System.IO;

namespace sudo.util
{
    public static class StdOutErrUtil
    {
        public static void WriteAndDelete(string tempFile, string propName, TextWriter outStream)
        {
            if (File.Exists(tempFile))
            {
                string outStr = File.ReadAllText(tempFile);
                if (outStr.Length > 0)
                    outStream.Write(outStr);
                if (!Flags.Contains(Flags.SAVE_TEMP) && Properties.IsDefault(propName))
                {
                    try
                    {
                        File.Delete(tempFile);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unable to delete {propName} file: {ex}");
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RNAseqAnalysis_0529
{
    class PythonInterface
    {
        private static string pythonpath = @"C:\Users\tjmcg\AppData\Local\Programs\Python\Python38\python.exe";
        public static void Linregpython(Configurator cfg)
        {
            string stdOut, stdErr = "None";
            try
            {
                File.Delete(cfg.predpath);
                using (File.Create(cfg.predpath)) { }
            }
            catch
            {
                using (File.Create(cfg.predpath)) { }
            }
            using (var proc = new Process())
            {
                try
                {
                    ProcessStartInfo procsi = new ProcessStartInfo();
                    procsi.FileName = pythonpath;
                    var script = cfg.linregpath;
                    var patharg1 = cfg.datapath;
                    int arraysize = Util.artlength(cfg);
//                    Console.WriteLine(arraysize.ToString());
                    procsi.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"", script, cfg.datapath, cfg.genpath, cfg.predpath, arraysize, cfg.attrpath, cfg.valdatapath);
                    procsi.CreateNoWindow = false;
                    procsi.UseShellExecute = false;
                    procsi.RedirectStandardOutput = true;
                    procsi.RedirectStandardError = true;
                    procsi.RedirectStandardInput = true;
                    proc.StartInfo = procsi;
                    Stopwatch pythonruntime = new Stopwatch();
                    pythonruntime.Start();
                    proc.Start();
                    Console.WriteLine("Python Loading: Success");
                    stdErr = proc.StandardError.ReadToEnd();
                    stdOut = proc.StandardOutput.ReadToEnd();
                    Console.WriteLine($"\tErrors: \n{stdErr}\n\tResults:\n{stdOut}");
                    pythonruntime.Stop();
                    Console.WriteLine($"\tPython Run-Time:\nHours: {pythonruntime.Elapsed.Hours}\nMinutes: {pythonruntime.Elapsed.Minutes}\nSeconds: {pythonruntime.Elapsed.Seconds}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot find Python Environment.\n\tError: \t{e.Message}");
                }
            }
        }
    }
}

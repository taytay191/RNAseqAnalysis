using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RNAseqAnalysis_0529
{
    class ModData
    {
        public static void WDGenConcurrent(Configurator cfg)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string[] files =  Directory.GetFiles(cfg.rawpath);

            File.Delete(cfg.genpath);
            int batchsize = 750;

            string UUID = Util.GetId(files[0]);

            double survival = Util.IdSurvival(UUID, cfg);
            Patient original = Util.TxttoPatient(files[0], survival, UUID);

            var arts = new List<Patient>();
            arts.Add(original);
            List<string> artlist = Util.FullAttrList(arts, cfg, false);
            int arraysize = original.GeneInfo.Count;
            int[] referencerow = genrefrow(cfg);
            StringBuilder sb = new StringBuilder();
            sb.Append("Changed ID,");
            sb.Append(String.Join(",", artlist.ToArray()));
            List<string> temp = new List<string>();
            temp.Add(sb.ToString());
            File.WriteAllLines(cfg.genpath, temp);
            List<int[]> genrows = new List<int[]>();
            var temp1 = new int[arraysize + 1];
            Array.ConstrainedCopy(referencerow, 0, temp1, 1, arraysize);
            temp1[0] = -1;
            genrows.Add(temp1);
            for (int i = 0; i < arraysize; i++)
            {
                var thisrow = new int[arraysize + 1];
                Array.ConstrainedCopy(referencerow, 0, thisrow, 1, arraysize);
                thisrow[0] = i;
                genrows.Add(thisrow);
                if (i % batchsize == 0)
                {
                    batchpush(genrows, artlist, cfg);
                    Console.WriteLine($"Wrote {i} lines to file");
                    genrows.Clear();
                }
            }
            batchpush(genrows, artlist, cfg);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string timeloss = $"\nHours: {ts.Hours}\nMinutes: {ts.Minutes} \nSeconds: {ts.Seconds}";
            Console.WriteLine($"Final array constructed with {arraysize} data points. \n\tTime Elapsed: {timeloss}");
        }
        private static int[] genrefrow(Configurator cfg)
        {
            string[] input = File.ReadAllLines(cfg.datapath);
            int arraysize = Convert.ToInt32(input[1].Split(',').Length) - 2;
            int arraylength = Convert.ToInt32(input.Length) - 1;
            string[][] master = new string[arraylength][];
            int[] referencerow = new int[arraysize];
            for (int i = 1; i < arraylength; i++)
            {
                string[] temp = input[i].Split(',');
                master[i] = temp;
            }
            for (int o = 2; o < arraysize; o++)
            {
                int temp = 0;
                for (int i = 1; i < arraylength; i++)
                {
                    var temp1 = master[i];
                    temp += Convert.ToInt32(temp1[o]);
                }
                referencerow[o] = temp / arraylength;
            }
            return referencerow;
        }
        private static void batchpush(List<int[]> genrows, List<string> artlist, Configurator cfg)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            int interval = Convert.ToInt32(cfg.Interval);
            Parallel.ForEach(genrows, (hypos) =>
            {
                StringBuilder sb = new StringBuilder();
                if (hypos[0] == -1)
                {
                    sb.Append("Original");
                    for (int i = 1; i < artlist.Count + 1; i++)
                    {
                        var temp = hypos[i].ToString();
                        sb.Append($",{temp}");
                    }
                }
                else
                {
                    int index = hypos[0];
                    sb.Append(artlist[index]);
                    hypos[index] *= interval;
                    for (int i = 1; i < artlist.Count + 1; i++)
                    {
                        var temp = hypos[i].ToString();
                        sb.Append($",{temp}");
                    }
                }
                output.Add(sb.ToString());
            });
            File.AppendAllLines(cfg.genpath, output);
        }

    }
}

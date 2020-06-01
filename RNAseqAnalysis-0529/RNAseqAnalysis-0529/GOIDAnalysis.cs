using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RNAseqAnalysis_0529
{
    class GOIDAnalysis
    {
        public static void GOIDanalysis(Configurator cfg)
        {
            var files = Directory.EnumerateFiles(cfg.gopath);

            Dictionary<string, double> original = new Dictionary<string, double>();
            string[] weights = File.ReadAllLines(cfg.weightpath);
            foreach (string s in weights)
            {
                // Console.WriteLine(s);
                string[] temp = s.Split(',');
                //Console.WriteLine($"s[0]: {temp[0]}\ts[1]: {temp[1]}");
                original.Add(temp[0], Convert.ToDouble(temp[1]));
            }
            foreach (string file in files)
            {
                string[] temp1 = File.ReadAllLines(file);
                string UUID = Getgoid(file);

                Console.WriteLine($"GO ID: {UUID} found. \t{(temp1.Length - 1).ToString()} potential genes identified");
                string path = $"{cfg.godir}\\{UUID}({cfg.Interval}fold).csv";
                Dictionary<string, string> idens = new Dictionary<string, string>();

                foreach (string d in temp1)
                {
                    int temp = d.IndexOf("ENSG");
                    if (temp != -1)
                    {
                        string ens = d.Substring(temp, 15);
                        string offic = d.Substring(0, temp);
                        idens.Add(ens, offic);
                    }
                    List<string> keys = new List<string>(idens.Keys);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        foreach (string key in keys)
                        {
                            try
                            {
                                //                                Console.WriteLine($"Gene:{idens[key]}\tEnsembl ID:{key}\tWeight:{original[key].ToString()}");
                                sw.WriteLine($"{idens[key]},{key},{original[key]}");
                            }
                            catch
                            {
                                Console.WriteLine($"Key: {key} was not identified in one or all of the Dictionary objects.");
                            }
                        }
                    }
                }
            }

        }
        static string Getgoid(string path)
        {
            string resid = @"C:\Programs\RNAseqAnalysis\GOID\";
            string result = string.Empty;
            result = path.Remove(0, resid.Length);
            string[] resid2 = result.Split(".");
            string fresult = resid2[0];
            return fresult.ToLower();
        }
    }
}

using RNAseqFinal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RNAseqAnalysis_0529
{
    class Util
    {
        public static string GetId(string path)
        {
            string resid = @"C:\Code\RNASeqAnalysis\Data\RawDir\";
            string result = string.Empty;
            result = path.Remove(0, resid.Length);
            string[] resid2 = result.Split(".");
            string fresult = resid2[0];
            return fresult.ToLower();
        }

        public static int IdSurvival(string UUID, string SurvivalPath)
        {
            string[] list = File.ReadAllLines(SurvivalPath);
            int q = 0;
            foreach (string s in list)
            {
                string[] r = s.Split(',');
                //Console.WriteLine(s);
                if (r[0].ToLower() == UUID)
                {
                    q = Convert.ToInt32(r[1]);
                    //Console.WriteLine(r[1]);
                }
            }
            if (q == 0)
            {
                Console.WriteLine($"Error identifying survival for UUID: {UUID}");
            }
            return q;
        }

        public static Patient TxttoPatient(string filePath, int survival, string UUID)
        {
            string[] raw = File.ReadAllLines(filePath);

            Patient info = new Patient();
            info.UUID = UUID;
            info.LifeExpectancy = survival;

            foreach (string value in raw)
            {
                var temp = value.Split('\t');
                Gene spec = new Gene();
                spec.Name = temp[0];
                try
                {
                    spec.Value = Convert.ToInt32(temp[1]);
                    info.GeneInfo.Add(spec);
                }
                catch
                {
                    Console.WriteLine($"Error in Integer Conversion TxttoPatient. Gene: {temp[0]}");
                }
            }
            return info;
        }
        public static List<string> FullAttrList(List<Patient> data, Configurator cfg, bool write)
        {
            List<string> comp = new List<string>();

            foreach (Patient x in data)
            {
                List<Gene> z = x.GeneInfo;

                foreach (Gene y in z)
                {
                    comp.Add(y.Name);
                }
            }

            List<string> susinct = comp.Distinct().ToList();

            //if (write == true)
            //{
            //    try
            //    {
            //        File.Delete(cfg.attrpath);
            //        File.WriteAllLines(cfg.attrpath, susinct);
            //    }
            //    catch
            //    {
            //        File.WriteAllLines(cfg.attrpath, susinct);
            //    }
            //}
            //string susinctstring = String.Join(",", susinct.ToArray());
            return susinct;
        }
        public static int artlength(Configurator cfg)
        {
            int i = 0;
            //string[] s = File.ReadAllLines(cfg.attrpath);
            //i = s.ToList().Count;
            return i;
        }
        public static void Predanalysis(Configurator cfg)
        {
            //File.Delete(cfg.weightpath);
            //File.Delete(cfg.transferpath);
            //string[] og = File.ReadAllLines(cfg.predpath);
            //Dictionary<string, double> dep = new Dictionary<string, double>();
            //foreach (string s in og)
            //{
            //    string[] temp = s.Split(",");
            //    try
            //    {
            //        dep.Add(temp[0].Split('.')[0], Convert.ToDouble(temp[1]));
            //    }
            //    catch
            //    {
            //        dep.Add(temp[0], Convert.ToDouble(temp[1]));
            //    }
            //    //Console.WriteLine(temp[0]);
            //}
            //double survival = dep["Original"];
            ////            double survival = 14.166;
            //List<string> keylist = new List<string>(dep.Keys);
            //int arraylength = Convert.ToInt32(keylist.Count);
            //for (int i = 0; i < arraylength; i++)
            //{
            //    string tempname = keylist[i];
            //    try
            //    {
            //        dep[tempname] -= survival;
            //    }
            //    catch
            //    {
            //        //Console.WriteLine($"Error in finding change in Survival for gene {tempname}");
            //        dep.Add(tempname, 0);
            //    }
            //}
            //int itemp = 1;
            //var org = from pair in dep orderby Math.Abs(pair.Value) descending select pair;
            //using (StreamWriter sw = File.CreateText(cfg.transferpath))
            //{
            //    foreach (var pair in org)
            //    {
            //        sw.WriteLine($"{pair.Key}, ");
            //    }
            //}
            //using (StreamWriter sw = File.CreateText(cfg.weightpath))
            //{
            //    foreach (var pair in org)
            //    {
            //        //Console.WriteLine($"Position: {itemp.ToString()}\tGene Name: {pair.Key}  Change in Survival: {pair.Value.ToString()}");
            //        sw.WriteLine($"{pair.Key},{pair.Value.ToString()}");
            //        itemp++;
            //    }
            //}
        }
    }
}
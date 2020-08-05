using RNAseqAnalysis_0529;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RNAseqFinal
{
    class Cleaner
    {
        public static void init(Configurator cfg)
        {
            try
            {
                File.Delete(cfg.CentData);
            }
            catch
            {
             
            }
            var files = Directory.EnumerateFiles(cfg.RawDir);

            List<Patient> Data = new List<Patient>();
            foreach(string file in files)
            {
                string name = Util.GetId(file);
                int survival = Util.IdSurvival(name, cfg.Survival);
                Patient temp = Util.TxttoPatient(file, survival, name);
                Data.Add(temp);
            }

            List<string> AttrList = Util.FullAttrList(Data);
            PopulateReal(AttrList, Data, cfg.CentData);
        }
        private static void PopulateReal(List<string> AttrList, List<Patient> Data, string path)
        {
            int num = 1;
            string[] ans = new string[Data.Count + 1];
            StringBuilder hsb = new StringBuilder();
            hsb.Append("UUID,Survival");
            foreach (string a in AttrList)
            {
                hsb.Append($",{a}");
            }
            ans[0] = hsb.ToString();
            foreach (Patient p in Data)
            {
                List<Gene> pdata = p.GeneInfo;
                Dictionary<string, string> dicdata = new Dictionary<string, string>();
                foreach (Gene tt in pdata)
                {
                    dicdata.Add(tt.Name, tt.Value.ToString());
                }
                string temp1 = p.UUID + "," + p.LifeExpectancy.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(temp1);
                foreach (string aaa in AttrList)
                {
                    try
                    {
                        string temp2 = "," + dicdata[aaa];
                        sb.Append(temp2);
                    }
                    catch
                    {
                        Console.WriteLine($"Data Missing for UUID: {p.UUID}, Gene: {aaa}");
                        sb.Append(",0");
                    }
                }
                ans[num] = sb.ToString();
                ///Console.WriteLine($"Patient Data for UUID {p.UUID} completed as line {num + 1}, and index {num}.");
                num++;
            }
            File.WriteAllLines(path, ans);
            ///Console.WriteLine(File.Exists(datapath).ToString());
        }
    }
}

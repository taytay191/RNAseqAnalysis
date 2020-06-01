using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RNAseqAnalysis_0529
{
    class Cleaner
    {
        public static void Data(Configurator cfg)
        {
            File.Delete(cfg.datapath);
            string origin = cfg.rawpath;

            // string[] set = Pathfinder(origin);
            var files = Directory.EnumerateFiles(cfg.rawpath);

            List<Patient> data = new List<Patient>();
            
            foreach (string file in files)
            {
                string identifier = Util.GetId(file);
                double survival = Util.IdSurvival(identifier, cfg);
                Patient p1 = Util.TxttoPatient(file, survival, identifier);
                data.Add(p1);
            }
            
            List<string> Attrlist = Util.FullAttrList(data, cfg, true);
            
            if (File.Exists(cfg.attrpath))
                File.Delete(cfg.attrpath);
            
            File.WriteAllLines(cfg.attrpath, Attrlist);
            PopulateReal(Attrlist, data, cfg.datapath);

            var valfiles = Directory.EnumerateFiles(cfg.valpatientdir);

            List<Patient> valdata = new List<Patient>();

            foreach (string file in valfiles)
            {
                string identifier = Util.GetIdVal(file);
                double survival = Util.IdSurvival(identifier, cfg);
                Patient p1 = Util.TxttoPatient(file, survival, identifier);
                valdata.Add(p1);
            }
            PopulateReal(Attrlist, valdata, cfg.valdatapath);

            Console.WriteLine("Data Cleaning: Success.");
        }


        private static void PopulateReal(List<string> ARTlist, List<Patient> data, string path)
        {
            int num = 1;
            string[] ans = new string[data.Count + 1];
            StringBuilder hsb = new StringBuilder();
            hsb.Append("UUID,Survival");
            foreach (string a in ARTlist)
            {
                hsb.Append($",{a}");
            }
            ans[0] = hsb.ToString();
            foreach (Patient p in data)
            {
                List<GeneInfoModel> pdata = p.GeneInfo;
                Dictionary<string, string> dicdata = new Dictionary<string, string>();
                foreach (GeneInfoModel tt in pdata)
                {
                    dicdata.Add(tt.GeneName, tt.Value.ToString());
                }
                string temp1 = p.UUID + "," + p.LifeExpectancy.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(temp1);
                foreach (string aaa in ARTlist)
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

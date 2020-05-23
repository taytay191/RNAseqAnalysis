using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RNAseq_Data_Analysis
{
    public class Patient
    {
        public string UUID { get; set; }
        public double LifeExpectancy { get; set; }
        public List<GeneInfoModel> GeneInfo = new List<GeneInfoModel>();
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Patient objAsModel = obj as Patient;
            if (objAsModel == null) return false;
            else return Equals(objAsModel);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(Patient other)
        {
            if (other == null) return false;
            return (this.LifeExpectancy.Equals(other.LifeExpectancy));
        }
        int position = -1;
        public bool Movenext()
        {
            position++;
            return (position < GeneInfo.Count);
        }
        public void Reset()
        { position = 0; }
        public object Current
        {
            get { return GeneInfo[position]; }
        }

    }

    public class GeneInfoModel : IEquatable<GeneInfoModel>
    {
        public string GeneName { get; set; }
        public double Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            GeneInfoModel objAsModel = obj as GeneInfoModel;
            if (objAsModel == null) return false;
            else return Equals(objAsModel);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals (GeneInfoModel other)
        {
            if (other == null) return false;
            return (this.Value.Equals(other.Value));
        }
    }

    class Program1
    {
        static void Main(string[] args)
        {
            string origin = @"C:\DataStorage\Rawtxt";
            string[] set = Pathfinder(origin);
            int q = 1;
            List<Patient> data = new List<Patient>();
            foreach (string p in set)
            {
                Console.WriteLine("Q Value: "+q.ToString());
                q++;
                string identifier = Getid(p);
                double survival = idsurvival(identifier);
                Patient p1 = TxttoPatient(p, survival, identifier);
                data.Add(p1);
            }
            List<string> ARTlist = fullartlist(data);
            //populate(ARTlist, data);
            Console.WriteLine("Sucess.");
            Console.ReadKey();
        }
        static void populate (List<string> ARTlist, List<Patient> data)
        {
            int num = 1;
            string[] ans = new string[data.Count+1];
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
                foreach(string aaa in ARTlist)
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
                Console.WriteLine($"Patient Data for UUID {p.UUID} completed as line {num + 1}, and index {num}.");
                num++;
            }
            File.WriteAllLines(@"C:\DataStorage\data.csv", ans);
        }
        static List<string> fullartlist(List<Patient> data)
        {
            List<string> comp = new List<string>();
            foreach (Patient x in data)
            {
                List<GeneInfoModel> z = x.GeneInfo;
                foreach (GeneInfoModel y in z)
                {
                    comp.Add(y.GeneName);
                }
            }
            List<string> susinct = comp.Distinct().ToList();
            string susinctstring = String.Join(",", susinct.ToArray());
            File.WriteAllLines(@"C:\DataStorage\ATR.csv", susinct);
            return susinct;
        }
        static double idsurvival(string UUID)
        {
            string[] list = File.ReadAllLines(@"C:\DataStorage\Survival\Survival.txt");
            double q = 0;
            foreach (string s in list)
            {
                string[] r = s.Split(',');
                //Console.WriteLine(s);
                if(r[0].ToLower() == UUID)
                {
                    q = Convert.ToDouble(r[1]);
                    //Console.WriteLine(r[1]);
                }
            }
            if(q == 0)
            {
                Console.WriteLine($"Error identifying survival for UUID: {UUID}");
            }
            return q;
        }
        static Patient TxttoPatient (string path, double survival, string UUID)
        {
            string[] raw = File.ReadAllLines(path);
            Patient info = new Patient();
            info.UUID = UUID;
            info.LifeExpectancy = survival;
            foreach (string value in raw)
            {
                var temp = value.Split('\t');
                GeneInfoModel spec = new GeneInfoModel();
                spec.GeneName = temp[0];
                try
                {
                    spec.Value = Convert.ToDouble(temp[1]);
                    info.GeneInfo.Add(spec);
                }
                catch
                {
                    Console.WriteLine($"Error in Integer Conversion TxttoPatient. Gene: {temp[0]}");
                }
            }
            return info;
        }
        static string[] Pathfinder(string target)
        {
            string[] Pathset = Directory.GetFiles(target);
            return Pathset;
        }
        static string Getid(string path)
        {
            string resid = @"C:\DataStorage\Rawtxt\";
            string result = string.Empty;
            result = path.Remove(0, resid.Length);
            string[] resid2 = result.Split(".");
            string fresult = resid2[0];
            return fresult.ToLower();
        }
    }
}

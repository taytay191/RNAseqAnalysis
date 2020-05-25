using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace RNAseq_Data_Analysis
{
    class Program
    {
        private static string rawpath = @"C:\Programs\RNAseqAnalysis\Rawtxt";
        private static string genpath = @"C:\Programs\RNAseqAnalysis\generateddata.csv";
        private static string datapath = @"C:\Programs\RNAseqAnalysis\data.csv";
        private static string pythonpath = @"C:\Program Files\Python36\python.exe";
        private static string linregpypath = @"C:\Programs\RNAseqAnalysis\linearregressionmodel.py";
        private static string survivalpath = @"C:\Programs\RNAseqAnalysis\Survival.txt";
        public int interval = 100;

        static void Main(string[] Arguments)
        {
            logiccomplex(Arguments);
        }
        static void logiccomplex(string[] Arguments)
        {
            try
            {
                Cleaninglogic(Arguments[0]);
                try
                {
                    GenLogic(Arguments[1]);
                }
                catch
                {
                    Console.WriteLine("Error in arguments for data generation sequence.")
                    GenLogic("");
                }
            }
            catch
            {
                Console.WriteLine("Error in arguments for cleaning sequence.");
                Cleaninglogic("");
                try
                {
                    GenLogic(Arguments[1]);
                }
                catch
                {
                    Console.WriteLine("Error in arguments for data generation sequence.")
                    GenLogic("");
                }

            }
        }
        static void GenLogic(string arg)
        {
            if (arg == "gen")
            {
                Console.WriteLine("Starting Generation of data.")
                Console.WriteLine("Would you like to set the interval value? y/n");
                var resp1 = ConsoleReadkey();
                if (resp1 = 'y')
                {
                    Console.WriteLine("Please enter desired interval value below.")
                    var resp2 = Console.ReadLine();
                    try
                    {
                        var twoint = Console.ToInt32(resp2);
                        Console.WriteLine($"Using {resp2} as interval.")
                        WDGenConcurrent(twoint);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error converting response to Int32. Restarting logic sequence.");
                        GenLogic(arg);
                    }
                }
                else if (resp2 = 'n')
                {
                    Console.WriteLine($"Using Default interval: {interval}");
                    WDGenConcurrent(interval);
                }
                else
                {
                    Console.WriteLine("Input invalid. Restarting logic sequence.");
                    GenLogic(arg);
                }
            }
            else if (arg == "def")
            {
                if(File.Exists(genpath) = false)
                {
                    Console.WriteLine("No generated data file identified. Generate data now? y/n");
                    var resp3 = Console.ReadKey();
                    if(resp3 == 'y')
                    {
                        GenLogic("gen");
                    }
                    else if (resp3 == 'n')
                    {
                        Console.WriteLine("No generated data file identified. Program closing.");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        Console.WriteLine("Input invalid. Restarting logic sequence.");
                        GenLogic(arg);
                    }
                }
                else
                {
                    Console.WriteLine("Generated data file identified.");
                }
            }
            else
            {
                Console.WriteLine("Argument input not recognized. Would you like to generate data? y/n");
                var resp4 = Console.ReadKey()
                if (resp4 == 'y')
                {

                }

            }
        }
        static void WDGenConcurrent (int intervals)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string[] pathes = Pathfinder(rawpath);
            File.Delete(genpath);
            int batchsize = 500;
            string UUID = Getid(pathes[0]);
            double survival = idsurvival(UUID);
            Patient original =  TxttoPatient(pathes[0], survival, UUID);
            var arts = new List<Patient>();
            arts.Add(original);
            List<string> artlist = fullartlist(arts);
            int arraysize = original.GeneInfo.Count;
            int[] referencerow = new int[arraysize];
            int itemp = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("Changed ID");
            sb.Append(String.Join(",",artlist.ToArray()));
            sb.Append(Environment.NewLine);
            foreach (GeneInfoModel gene in original.GeneInfo)
            {
                referencerow[itemp] = Convert.ToInt32(gene.Value);
            }
            List<int[]> genrows = new List<int[]>();            
            for (int i = 0; i < arraysize; i++)
            {
                var thisrow = new int[arraysize+1];
                Array.ConstrainedCopy(referencerow, 0, thisrow, 1, arraysize);
                thisrow[0] = i;
                genrows.Add(thisrow);
                if (i % batchsize == 0)
                {
                    batchpush(genrows, artlist, inverals);
                    Console.WriteLine($"Wrote {i} lines to file");
                    genrows.Clear();
                }
            }
            batchpush(genrows, artlist, intervals);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string timeloss = $"\nHours: {ts.Hours}\nMinutes: {ts.Minutes} \nSeconds: {ts.Seconds}";
            Console.WriteLine($"Final array constructed with {arraysize} data points. \n\tTime Elapsed: {timeloss}");
        }
        static void batchpush (List<int[]> genrows, List<string> artlist, int intervals)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            Parallel.ForEach(genrows, (hypos) =>
            {
                StringBuilder sb= new StringBuilder();
                sb.Append(artlist[hypos[0]]);
                hypos[hypos[0]] += intervals;
                for(int i =1; i<artlist.Count;i++)
                {
                    var temp = hypos[i].ToString();
                    sb.Append($",{temp}");
                }
                output.Add(sb.ToString());
            });
            File.AppendAllLines(genpath, output);
        }
        static void Cleaninglogic(string arg)
        {
            if (arg == "-f")
            {
                datacleaner();
            }
            else if (arg == "-t")
            {
                if (File.Exists(datapath) == false)
                {
                    Console.WriteLine($"Data file not found, run cleaning program on '{rawpath}' y/n?");
                    string response = Console.ReadKey().ToString();
                    if (response == "y")
                    {
                        datacleaner();
                    }
                    else if (response == "n")
                    {
                        Console.WriteLine("No cleaned data: program ending.");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        Console.WriteLine("Response unclear, please either choose yes or no.");
                        Cleaninglogic(arg);
                    }
                }
                else
                {
                    Console.WriteLine("Data file identified.");
                }
            }
            else
            {
                Console.WriteLine($"Cleaning logic unclear: run cleaning program on '{rawpath}'? y/n");
                string response = Console.ReadKey().ToString();
                if (response == "y")
                {
                    datacleaner();
                }
                else if (response == "n")
                {
                    Console.WriteLine("No cleaned data: program ending.");
                    Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine("Response unclear, please either choose yes or no.");
                    Cleaninglogic(arg);
                }
            }
        }
        static void Pycontroller ()
        {
            string stdOut,stdErr = "None";
            using (var proc = new Process())
            {
                try
                {
                    ProcessStartInfo procsi = new ProcessStartInfo();
                    procsi.FileName = pythonpath;
                    var script = linregpypath;
                    var patharg = datapath;
                    procsi.Arguments = string.Format("\"{0}\" \"{1}\"", script, patharg);
                    procsi.CreateNoWindow = true;
                    procsi.UseShellExecute = false;
                    procsi.RedirectStandardOutput = true;
                    procsi.RedirectStandardError = true;
                    procsi.RedirectStandardInput = true;
                    proc.StartInfo = procsi;
                    proc.Start();
                    Console.WriteLine("Python Loading: Success");
                    stdOut = proc.StandardOutput.ReadToEnd();
                    stdErr = proc.StandardError.ReadToEnd();
                    Console.WriteLine($"\tErrors: \n{stdErr}\n\tResults:\n{stdOut}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot find Python Environment.\n\tError: \t{e.Message}");
                }
            }
        }
        static void datacleaner()
        {
            File.Delete(datapath);
            string origin = rawpath;
            string[] set = Pathfinder(origin);
            int q = 1;
            List<Patient> data = new List<Patient>();
            foreach (string p in set)
            {
                //Console.WriteLine("Q Value: "+q.ToString());
                q++;
                string identifier = Getid(p);
                double survival = idsurvival(identifier);
                Patient p1 = TxttoPatient(p, survival, identifier);
                data.Add(p1);
            }
            List<string> ARTlist = fullartlist(data);
            populatereal(ARTlist, data);
            Console.WriteLine("Data Cleaning: Success.");
            //Console.ReadKey();

        }
        static void populatereal (List<string> ARTlist, List<Patient> data)
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
                ///Console.WriteLine($"Patient Data for UUID {p.UUID} completed as line {num + 1}, and index {num}.");
                num++;
            }
            File.WriteAllLines(datapath, ans);
            ///Console.WriteLine(File.Exists(datapath).ToString());
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
            ///File.WriteAllLines(@"C:\Programs\RNAseq\AnalysisATR.csv", susinct);
            return susinct;
        }
        static double idsurvival(string UUID)
        {
            string[] list = File.ReadAllLines(survivalpath);
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
            string resid = @"C:\Programs\RNAseqAnalysis\Rawtxt\";
            string result = string.Empty;
            result = path.Remove(0, resid.Length);
            string[] resid2 = result.Split(".");
            string fresult = resid2[0];
            return fresult.ToLower();
        }
    }
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

}
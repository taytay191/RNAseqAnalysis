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
        private static string wspath = @"C:\Programs\RNAseqAnalysis\Predsurvival.csv";
        private static string artpath = @"C:\Programs\RNAseqAnalysis\artlist.csv";
        private static string weightpath = @"C:\Programs\RNAseqAnalysis\weights.csv";
        public static int interval = 100;
        private static string transferpath = @"C:\Programs\RNAseqAnalysis\keylist.txt";
        private static string gopath = @"C:\Programs\RNAseqAnalysis\GOID";

        static void Main(string[] Arguments)
        {
            logiccomplex(Arguments);
        }
        static void logiccomplex(string[] Arguments)
        {
            Cleaninglogic(Arguments);
            GenLogic(Arguments);
            Pythonlogic(Arguments);
            Weightlogic(Arguments);
        }
        static void GOIDanalysis()
        {
            string[] pathes = Pathfinder(gopath);
            foreach (string s in pathes)
            {
                
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

        static void Weightlogic(string[] Argument)
        {
            string arguse = "";
            try
            {
                arguse = Argument[3];
            }
            catch
            {
                Console.WriteLine("Error for arguments in weight logic sequence.");
            }
            if (arguse == "weigh")
            {
                Console.WriteLine("Running weight analysis.");
                Predanalysis();
            }
            else if (arguse == "ignore")
            {
                if (File.Exists(weightpath)== true)
                {
                    Console.WriteLine("Weight Path found.");
                }
                else
                {
                    Console.WriteLine("No path found. Run weighing process? y/n");
                    string input = Console.ReadLine();
                    string[] temp = new string[3];
                    if(input == "y" )
                    {
                        temp[3] = "weigh";
                        Weightlogic(temp);
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("No data.");
                        Environment.Exit(-1);
                    } 
                    else
                    {
                        Console.WriteLine("Input invalid. Restarting logic sequence.");
                        temp[3] = "ignore";
                        Weightlogic(temp);
                    }
                }
            }
            else
            {
                Console.WriteLine("Input invalid. Please either write 'weigh' to weigh data or 'ignore' to pass over the data.");
                string resp = Console.ReadLine();
                string[] temp = new string[3];
                temp[3] = resp;
                Weightlogic(temp);
            }
        }
        static void Predanalysis()
        {
            File.Delete(weightpath);
            File.Delete(transferpath);
            string[] og = File.ReadAllLines(wspath);
            Dictionary<string, double> dep = new Dictionary<string,double>();
            foreach(string s in og)
            {
                string[] temp = s.Split(",");
                try
                {
                    dep.Add(temp[0].Split('.')[0], Convert.ToDouble(temp[1]));
                }
                catch
                {
                    dep.Add(temp[0], Convert.ToDouble(temp[1]));
                }
                //Console.WriteLine(temp[0]);
            }
            double survival = dep["Original"];
//            double survival = 14.166;
            List<string> keylist = new List<string>(dep.Keys);
            int arraylength = Convert.ToInt32(keylist.Count);
            for (int i = 0; i < arraylength; i++)
            {
                string tempname = keylist[i];
                try
                {
                    dep[tempname] -= survival;
                }
                catch
                {
                    //Console.WriteLine($"Error in finding change in Survival for gene {tempname}");
                    dep.Add(tempname, 0);
                }
            }
            int itemp=1;
            var org = from pair in dep orderby Math.Abs(pair.Value) descending select pair;
            using (StreamWriter sw= File.CreateText(transferpath))
            {
                foreach (var pair in org)
                {
                    sw.WriteLine($"{pair.Key}, ");
                }
            }
            using (StreamWriter sw = File.CreateText(weightpath))
            {
                foreach (var pair in org)
                {
                    //Console.WriteLine($"Position: {itemp.ToString()}\tGene Name: {pair.Key}  Change in Survival: {pair.Value.ToString()}");
                    sw.WriteLine($"{pair.Key},{pair.Value.ToString()}");
                    itemp++;
                }
            }
        }
        static void Pythonlogic(string[] arg)
        {
            string arguse = "";
            try
            {
                arguse = arg[2];
            }
            catch
            {
                Console.WriteLine("Error in arguments for python control sequence.");
            }
            if (arguse == "linreg")
            {
                Console.WriteLine("Linear regression model starting now.");
                Pycontroller();
            }
            else if (arguse == "keras")
            {
                Console.WriteLine("Keras Model not developed yet. Please wait for another time.");
            }
            else if (arguse == "skip")
            {
                if (File.Exists(wspath))
                {
                    Console.WriteLine("Predicted Survival path detected.");
                }
                else
                {
                    Console.WriteLine("Input invalid, no data file exists. Would you like to run a model? Please type 'linreg' to run linear regression model, 'keras' to run the Keras model.");
                    string resp = Console.ReadLine();
                    string[] temp = new string[2];
                    temp[2] = resp;
                    Pythonlogic(temp);
                }
            }
            else
            {
                Console.WriteLine("Input invalid. Would you like to run a model? Please type 'linreg' to run linear regression model, 'keras' to run the Keras model, or 'skip' to indicate that the data already exists.");
                string resp = Console.ReadLine();
                string[] temp = new string[2];
                Pythonlogic(temp);
            }
        }
        static void GenLogic(string[] arg)
        {
            string arguse = "";
            try
            {
                arguse = arg[1];
            }
            catch
            {
                Console.WriteLine("Error in arguments for data generation sequence.");
            }
            if (arguse == "gen")
            {
                Console.WriteLine("Starting Generation of data.");
                Console.WriteLine("Would you like to set the interval value? y/n");
                var resp1 = Console.ReadLine();
                if (Convert.ToChar(resp1) == 'y')
                {
                    Console.WriteLine("Please enter desired interval value below.");
                    var resp2 = Console.ReadLine();
                    try
                    {
                        var twoint = Convert.ToInt32(resp2);
                        Console.WriteLine($"Using {resp2} as interval.");
                        WDGenConcurrent(twoint);
                    }
                    catch
                    {
                        Console.WriteLine("Error converting response to Int32. Restarting logic sequence.");
                        string[] temp = new string[1];
                        temp[1] = arguse;
                        GenLogic(temp);
                    }
                }
                else if (Convert.ToChar(resp1) == 'n')
                {
                    Console.WriteLine($"Using Default interval: {interval}");
                    WDGenConcurrent(interval);
                }
                else
                {
                    Console.WriteLine("Input invalid. Restarting logic sequence.");
                    string[] temp = new string[1];
                    temp[1] = arguse;
                    GenLogic(temp);
                }
            }
            else if (arguse == "def")
            {
                if(File.Exists(genpath) == false)
                {
                    Console.WriteLine("No generated data file identified. Generate data now? y/n");
                    var resp3 = Console.ReadLine();
                    if(Convert.ToChar(resp3) == 'y')
                    {
                        string[] temp = new string[1];
                        temp[1] = "gen";
                        GenLogic(temp);
                    }
                    else if (Convert.ToChar(resp3) == 'n')
                    {
                        Console.WriteLine("No generated data file identified. Program closing.");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        Console.WriteLine("Input invalid. Restarting logic sequence.");
                        string[] temp = new string[1];
                        temp[1] = arguse;
                        GenLogic(temp);
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
                var resp4 = Console.ReadLine();
                if (Convert.ToChar(resp4) == 'y')
                {
                    string[] temp = new string[1];
                    temp[1] = "gen";
                    GenLogic(temp);
                }
                else if (Convert.ToChar(resp4) == 'n')
                {
                    string[] temp = new string[1];
                    temp[1] = "def";
                    GenLogic(temp);
                }
                else
                {
                    Console.WriteLine("Input invalid. Restarting logic sequence.");
                    string[] temp = new string[1];
                    temp[1] = "";
                    GenLogic(temp);
                }
            }
        }
        static int[] genrefrow()
        {
            string[] input = File.ReadAllLines(datapath);
            int arraysize = Convert.ToInt32(input[1].Split(',').Length)-2;
            int arraylength = Convert.ToInt32(input.Length)-1;
            string[][] master = new string[arraylength][];
            int[] referencerow = new int[arraysize];
            for (int i = 1; i<arraylength; i++)
            {
                string[] temp = input[i].Split(',');
                master[i]=temp;
            }
            for (int o = 2; o<arraysize; o++)
            {
                int temp = 0;
                for (int i =1; i<arraylength; i++)
                {
                    var temp1 = master[i];
                    temp += Convert.ToInt32(temp1[o]);
                }
                referencerow[o] = temp/arraylength;
            }
            return referencerow;
        }
        static void WDGenConcurrent (int intervals)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string[] pathes = Pathfinder(rawpath);
            File.Delete(genpath);
            int batchsize = 750;
            string UUID = Getid(pathes[0]);
            double survival = idsurvival(UUID);
            Patient original =  TxttoPatient(pathes[0], survival, UUID);
            var arts = new List<Patient>();
            arts.Add(original);
            List<string> artlist = fullartlist(arts);
            int arraysize = original.GeneInfo.Count;
            int[] referencerow = genrefrow();
            StringBuilder sb = new StringBuilder();
            sb.Append("Changed ID,");
            sb.Append(String.Join(",",artlist.ToArray()));
            List<string> temp = new List<string>();
            temp.Add(sb.ToString());
            File.WriteAllLines(genpath, temp);
            List<int[]> genrows = new List<int[]>();
            var temp1 = new int[arraysize+1];
            Array.ConstrainedCopy(referencerow,0,temp1,1,arraysize);
            temp1[0] = -1;
            genrows.Add(temp1);            
            for (int i = 0; i < arraysize; i++)
            {
                var thisrow = new int[arraysize+1];
                Array.ConstrainedCopy(referencerow, 0, thisrow, 1, arraysize);
                thisrow[0] = i;
                genrows.Add(thisrow);
                if (i % batchsize == 0)
                {
                    batchpush(genrows, artlist, intervals);
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
                if(hypos[0] ==-1)
                {
                    sb.Append("Original");
                    for(int i =1; i<artlist.Count+1; i++)
                    {
                        var temp = hypos[i].ToString();
                        sb.Append($",{temp}");
                    }
                }
                else
                {
                    sb.Append(artlist[hypos[0]]);
                    hypos[hypos[0]] += intervals;
                    for(int i =1; i<artlist.Count+1;i++)
                    {
                        var temp = hypos[i].ToString();
                        sb.Append($",{temp}");
                    }

                }
                output.Add(sb.ToString());
            });
            File.AppendAllLines(genpath, output);
        }
        static void Cleaninglogic(string[] arg)
        {
            string arguse = "";
            try
            {
                arguse = arg[0];
            }
            catch
            {
                Console.WriteLine("Error in arguments for cleaning sequence.");
            }
            if (arguse == "--f")
            {
                datacleaner();
            }
            else if (arguse == "--t")
            {
                if (File.Exists(datapath) == false)
                {
                    Console.WriteLine($"Data file not found, run cleaning program on '{rawpath}' y/n?");
                    string response = Console.ReadLine().ToString();
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
                        string[] temp = new string[1];
                        temp[0] = arguse;
                        Cleaninglogic(temp);
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
                string response = Console.ReadLine().ToString();
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
                    string[] temp = new string[1];
                    temp[0] = arguse;
                    Cleaninglogic(temp);
                }
            }
        }
        static int artlength()
        {
            string[] s = File.ReadAllLines(artpath);
            int i = s.ToList().Count;
            return i;
        }
        static void Pycontroller ()
        {
            string stdOut, stdErr = "None";
            File.Delete(wspath);
            using(File.Create(wspath)) {}
            using (var proc = new Process())
            {
                try
                {
                    ProcessStartInfo procsi = new ProcessStartInfo();
                    procsi.FileName = pythonpath;
                    var script = linregpypath;
                    var patharg1 = datapath;
                    int arraysize = artlength();
                    procsi.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", script, datapath, genpath, wspath, arraysize, artpath);
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
            if(File.Exists(artpath) == true)
            {
                File.Delete(artpath);
            }
            File.WriteAllLines(artpath, ARTlist);
            populatereal(ARTlist, data);
            Console.WriteLine("Data Cleaning: Success.");
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
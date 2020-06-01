using CommandLine;
using Microsoft.VisualBasic;

namespace RNAseqAnalysis_0529
{
    public class Configurator
    {
        public string ActionOption { get; set; }
        public string Interval { get; set; }
        public string ModelOption { get; set; }
        public string rawpath { get; set; } 
        public string gopath { get; set; }
        public string datapath { get; set; }
        public string attrpath { get; set; }
        public string survivalpath { get; set; }
        public string genpath { get; set; }
        public string predpath { get; set; }
        public string linregpath { get; set; }
        public string weightpath { get; set; }
        public string godir { get; set; }
        public string transferpath { get; set; }
        public string valdatapath { get; set; }
        public string valpatientdir { get; set; }
        public string keras1path { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var cfg = new Configurator();

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                cfg.Interval = o.Interval;
                cfg.rawpath = o.patientdir;
                cfg.gopath = o.gopath;
                cfg.datapath = o.datapath;
                cfg.attrpath = o.attrpath;
                cfg.survivalpath = o.survivalpath;
                cfg.ActionOption = o.ActionOption;
                cfg.ModelOption = o.ModelOption;
                cfg.linregpath = o.linregpath;
                cfg.godir = o.godir;
                cfg.transferpath = o.transferpath;
                cfg.valdatapath = o.valdatapath;
                cfg.valpatientdir = o.valpatientdir;
                cfg.keras1path = o.Keras1path;
                cfg.genpath = @"C:\Code\RNASeqData\generateddata\Modulated\generateddata(" + cfg.Interval + "fold).csv";
                cfg.predpath = @"C:\Code\RNASeqData\generateddata\survival\Predsurvival(" + cfg.Interval + "fold).csv";
                cfg.weightpath = @"C:\Code\RNASeqData\generateddata\weighted\weights(" + cfg.Interval + "fold).csv";
            });

            switch (cfg.ActionOption)
            {
                case "clean":
                    Cleaner.Data(cfg);
                    break;

                case "genmoddata":
                    ModData.WDGenConcurrent(cfg);
                    break;

                case "runmodel":
                    if(cfg.ModelOption == "regression")
                    {
                        PythonInterface.Linregpython(cfg); 
                    }
                    if(cfg.ModelOption == "Keras1")
                    {

                    }
                    break;

                case "goid":
                    Util.Predanalysis(cfg);
                    GOIDAnalysis.GOIDanalysis(cfg);
                    break;

                case "all":
                    Cleaner.Data(cfg);
                    ModData.WDGenConcurrent(cfg);
                    if(cfg.ModelOption == "regression")
                    {
                        PythonInterface.Linregpython(cfg);
                    }
                    Util.Predanalysis(cfg);
                    GOIDAnalysis.GOIDanalysis(cfg);
                    break;

                default:
                    break;
            }

            /*
                clean, 
                generated modulated data, 
                model generated data, 
                interpret model
            */
        }
    }
}

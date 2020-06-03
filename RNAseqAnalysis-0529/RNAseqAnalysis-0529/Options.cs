using CommandLine;

namespace RNAseqAnalysis_0529
{
    class Options
    {
        [Option("linregpath",
        Default = @"C:\Code\RNAseqAnalysis-0529-Python\linearregressionmodel1.py",
        Required = false,
        HelpText = "Path to Python Linear Regression Script")]
        public string linregpath { get; set; }

        [Option("transferpath",
        Default = @"C:\Code\RNASeqData\goid\cleaned\keylist.csv",
        Required = false,
        HelpText = "The comprehensive key list from the GOIDs selected.")]
        public string transferpath { get; set; }

        [Option("godir",
        Default = @"C:\Code\RNASeqData\goid\cleaned",
        Required = false,
        HelpText = "Directory to save the different GIOD weight information.")]
        public string godir { get; set; }


        [Option("action",
            Default = "",
            Required = false,
            HelpText = "Available actions: clean, goid, genmoddata, runmodel ... ")]
        public string ActionOption { get; set; }

        [Option("Keras1path",
            Default = @"C:\Code\RNAseqAnalysis-0529-Python\kerasmodel2.py",
            Required = false,
            HelpText = "Path for the first Keras model in python")]
        public string Keras1path { get; set; }

        [Option("model",
            Default = "deep",
            Required = false,
            HelpText = "available options: regression, keras1, keras2")]
        public string ModelOption { get; set; }

        [Option("interval",
            Default = "2",
            Required = false,
            HelpText = "interval used for genmoddata ")]
        public string Interval { get; set; }

        [Option("gopath",
            Default = @"C:\Code\RNASeqData\goid\genelists",
            Required = false,
            HelpText = "Path to all of the GoId files.")]
        public string gopath { get; set; }

        [Option("patientdir",
            Default = @"C:\Code\RNASeqData\gdcpatientdata\Training Data",
            Required = false,
            HelpText = "Directory with Patient data loaded.")]
        public string patientdir { get; set; }

        [Option("datapath",
            Default = @"C:\Code\RNASeqData\generateddata\cleaned\data.csv",
            Required = false,
            HelpText = "Where to save cleaned data from raw patient data.")]
        public string datapath { get; set; }

        [Option("attrpath",
            Default = @"C:\Code\RNASeqData\generateddata\cleaned\attrlist.csv",
            Required = false,
            HelpText = "CSV file where the list of genes will be saved.")]
        public string attrpath { get; set; }

        [Option("survivalpath",
            Default = @"C:\Code\RNASeqData\gdcpatientdata\survival data\Survival.txt",
            Required = false,
            HelpText = "Text file with survival statistics indexed by UUID and separated by commas.")]
        public string survivalpath { get; set; }
    }
}

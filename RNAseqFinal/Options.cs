using CommandLine;

namespace RNAseqAnalysis_0529
{
    class Options
    {
        [Option("RawDir",
        Default = @"C:\Code\RNASeqAnalysis\Data\RawDir",
        Required = false,
        HelpText = "Path to Python Model Script")]
        public string RawDir {get; set;}

        [Option("Survival",
        Default = @"C:\Code\RNASeqAnalysis\Data\Survival.txt",
        Required = false,
        HelpText = "Path to raw TCGA data")]
        public string Survival { get; set; }

        [Option("CentData",
        Default = @"C:\Code\RNASeqAnalysis\Data\CentData.csv",
        Required = false,
        HelpText = "Path to raw TCGA data")]
        public string CentData { get; set; }

    }
}
using CommandLine;

namespace RNAseqAnalysis_0529
{
    class Options
    {
        [Option("ModelPath",
        Default = @"C:\",
        Required = false,
        HelpText = "Path to Python Model Script")]
        public string ModelPath {get; set;}

        [Option("RawDir",
        Default = @"C:\",
        Required = false,
        HelpText = "Path to raw TCGA data")]
        public string RawDir { get; set; }
    }
}
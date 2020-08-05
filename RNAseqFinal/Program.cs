using System;
using Microsoft.VisualBasic;
using CommandLine;
using RNAseqAnalysis_0529;

namespace RNAseqFinal
{
    class Program
    {
        public class Configurator
        {
            public string ModelPath { get; set; }
            public string RawDir { get; set; }
        }
        static void Main(string[] args)
        {
            Configurator cfg = new Configurator();

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                cfg.ModelPath = o.ModelPath;
                cfg.RawDir = o.RawDir;
            });

        }
    }
}

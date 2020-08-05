using System;
using Microsoft.VisualBasic;
using CommandLine;
using RNAseqAnalysis_0529;
using System.Collections.Generic;

namespace RNAseqFinal
{
    public class Patient
    {
        public string UUID { get; set; }
        public int LifeExpectancy { get; set; }
        public List<Gene> GeneInfo = new List<Gene>();
    }
    public class Gene
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    public class Configurator
    {
        public string Survival { get; set; }
        public string RawDir { get; set; }
        public string CentData { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Configurator cfg = new Configurator();

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                cfg.Survival = o.Survival;
                cfg.RawDir = o.RawDir;
                cfg.CentData = o.CentData;
            });

            Cleaner.init(cfg);

        }
    }
}

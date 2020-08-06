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
    public class GOID
    {
        public string UUID { get; set; }
        public Dictionary<string, string> Genes = new Dictionary<string, string>();
    }

    public class Configurator
    {
        public string Survival { get; set; }
        public string RawDir { get; set; }
        public string CentData { get; set; }
        public string GOIDDir { get; set; }
        public List<GOID> GOIDs = new List<GOID>();
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
                cfg.GOIDDir = o.GOIDDir;
            });

            //Cleaner.init(cfg);
            cfg = GOIDselection.Entry(cfg);
            Console.WriteLine($"{cfg.GOIDs.Count} GOID(s) identified.");
            Console.ReadKey();

        }
    }
}

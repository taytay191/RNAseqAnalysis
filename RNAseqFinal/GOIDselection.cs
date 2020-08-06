using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RNAseqFinal
{
    class GOIDselection
    {
        public static Configurator Entry(Configurator cfg)
        {
            var files = Directory.EnumerateFiles(cfg.GOIDDir);
            foreach (string file in files)
            {
                var temp = new GOID();
                temp.UUID = GOIDUUID(file);
                temp.Genes = GOIDGenes(file);
                cfg.GOIDs.Add(temp);
            }
            return cfg;
        }
        private static string GOIDUUID(string path)
        {
            string resid = @"C:\Code\RNASeqAnalysis\Data\GOIDs";
            string result = path.Remove(0, resid.Length);
            string resp = result.Split(".")[0];
            return resp;
        }
        private static Dictionary<string, string> GOIDGenes (string path)
        {
            var rawtext = File.ReadAllLines(path);
            var Final = new Dictionary<string, string>();

            foreach (string gene in rawtext)
            {
                int temp = gene.IndexOf("ENSG");
                if (temp != -1)
                {
                    string ENSG = gene.Substring(temp, 15);
                    string Normal = gene.Substring(0, temp);
                    Final.Add(ENSG, Normal);
                }
            }
            return Final;
        }

    }
}

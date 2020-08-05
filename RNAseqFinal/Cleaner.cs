using RNAseqAnalysis_0529;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RNAseqFinal
{
    class Cleaner
    {
        public static void init(Configurator cfg)
        {
            File.Delete(cfg.CentData);
            var files = Directory.EnumerateFiles(cfg.RawDir);
            List<Patient> Data = new List<Patient>();

            foreach(string file in files)
            {
                string name = Util.GetId(file);
                int survival = Util.IdSurvival(name, cfg.Survival);
                //                Console.WriteLine($"Patient: {temp.UUID} Survived: {temp.LifeExpectancy}");
                Patient temp = Util.TxttoPatient(file, survival, name);
                Data.Add(temp);

            }
            //            Console.ReadKey();
        }
    }
}

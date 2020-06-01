using System;
using System.Collections.Generic;
using System.Text;

namespace RNAseqAnalysis_0529
{
    public class Patient
    {
        public string UUID { get; set; }
        public double LifeExpectancy { get; set; }

        public List<GeneInfoModel> GeneInfo = new List<GeneInfoModel>();
    }


    public class GeneInfoModel
    {
        public string GeneName { get; set; }
        public double Value { get; set; }
    }

}

using System;
using System.Collections.Generic;

namespace ML.Lib.Fuzzy
{
    public class FuzzifierGroup
    {
        public List<IFuzzifier> Fuzzifiers;
        public string GroupName;

        public FuzzifierGroup(IEnumerable<IFuzzifier> fuzzifiers, string groupName)
        {
            GroupName = groupName;
            Fuzzifiers = new List<IFuzzifier>(fuzzifiers);
        }

        public FuzzifierGroup(string groupName)
        {
            GroupName = groupName;
            Fuzzifiers = new List<IFuzzifier>();
        }

    }


}
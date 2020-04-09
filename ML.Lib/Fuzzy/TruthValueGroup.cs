using System.Collections.Generic;


namespace ML.Lib.Fuzzy
{
    public class TruthValueGroup
    {
        public string GroupID;
        public Dictionary<string, double> valuePairs;

        public TruthValueGroup(string groupID)
        {
            GroupID = groupID;
            valuePairs = new Dictionary<string, double>();
        }

    }

}
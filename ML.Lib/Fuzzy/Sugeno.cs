using System.Collections.Generic;
using System;
using System.Linq;
namespace ML.Lib.Fuzzy
{
    public class Sugeno
    {
        //Collection of user determined fuzzifiers used in fuzzifying process
        //each dictionary key is equvialent to the input index number
        List<FuzzifierGroup> fuzzifierGroups = new List<FuzzifierGroup>();
        public List<FuzzifierGroup> FuzzifierGroups
        {
            get { return fuzzifierGroups; }
        }

        //Collection of user determined truthValues together with their respective IDs
        List<TruthValueGroup> truthValuesGroups = new List<TruthValueGroup>();
        public List<TruthValueGroup> TruthValuesGroups
        {
            get { return truthValuesGroups; }
        }

        List<SugenoRule> rules = new List<SugenoRule>();
        public List<SugenoRule> Rules
        {
            get { return rules; }
        }


        public List<SugenoExpectedValue> expectedValues = new List<SugenoExpectedValue>();

        public Sugeno()
        {

        }

        //Performs fuzzy-system operations
        //IMPORTANT: order of inputs is used for using appropiate groups of fuzzifiers
        public double Compile(SugenoInput input)
        {
            TruthValuesGroups.Clear();
            double result = 0;

            foreach (FuzzifierGroup fuzzGroup in FuzzifierGroups)
            {
                TruthValueGroup currentGroup = new TruthValueGroup(fuzzGroup.GroupName);
                TruthValuesGroups.Add(currentGroup);
                foreach (IFuzzifier f in fuzzGroup.Fuzzifiers)
                {
                    currentGroup.valuePairs.Add(f.Label, f.Fuzzify(input.values[fuzzGroup.GroupName]));
                }
            }


            List<double> ruleValues = new List<double>(); // Contains values of appropiate rules

            foreach (SugenoRule rule in rules)
            {
                double val1 = TruthValuesGroups.Find(x => x.GroupID == rule.truthValueGroupIDs[0]).valuePairs[rule.truthValueIDs[0]];
                double val2 = TruthValuesGroups.Find(x => x.GroupID == rule.truthValueGroupIDs[1]).valuePairs[rule.truthValueIDs[1]];
                ruleValues.Add(rule.ruleMethod(val1, val2));
            }

            double decision = 0;
            for (int i = 0; i < expectedValues.Count; i++)
            {
                decision += ruleValues[i] * expectedValues[i].value;
            }
            double tmp = 0;
            for (int i = 0; i < ruleValues.Count; i++)
            {
                tmp += ruleValues[i];
            }
            result = decision / tmp;
            return result;
        }

    }
}
using System;
using System.Collections.Generic;
namespace ML.Lib.Fuzzy
{

    //Represents node of a tree of rules
    public class SugenoRule
    {

        public Func<double, double, double> ruleMethod = AND;
        public string[] truthValueGroupIDs;
        public string[] truthValueIDs;



        public SugenoRule(Func<double, double, double> ruleMethod, string variableNameA, string valueA, string variableNameB, string valueB)
        {
            truthValueIDs = new string[2];
            truthValueGroupIDs = new string[2];
            truthValueIDs[0] = valueA;
            truthValueIDs[1] = valueB;
            truthValueGroupIDs[0] = variableNameA;
            truthValueGroupIDs[1] = variableNameB;
            this.ruleMethod = ruleMethod;
        }



        public static double AND(double a, double b)
        {
            return Math.Min(a,b);
        }

        public static double OR(double a, double b)
        {
            return Math.Max(a,b);
        }

        public static double NOT(double a, double _ = 0)
        {
            return 1 - a;
        }
    }
}
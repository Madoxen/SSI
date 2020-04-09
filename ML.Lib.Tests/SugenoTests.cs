using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML.Lib.Fuzzy;

namespace ML.Lib.Tests
{
    [TestClass]
    public class SugenoTests
    {
        [TestMethod]
        public void SugenoTest()
        {
            Sugeno sugenoSystem = new Sugeno();
            FuzzifierGroup sunlightGroup = new FuzzifierGroup("sunlight");
            sunlightGroup.Fuzzifiers.Add(new TrapezoidFuzzifier(0, 0.0, 0.2, 0.3, "niskie"));
            sunlightGroup.Fuzzifiers.Add(new TriangleFuzzifier(0.2, 0.5, 0.8, "średnie"));
            sunlightGroup.Fuzzifiers.Add(new TrapezoidFuzzifier(0.6, 0.8, 1.0, 1.0, "duże"));
            sugenoSystem.FuzzifierGroups.Add(sunlightGroup);


            FuzzifierGroup pollutionGroup = new FuzzifierGroup("pollution");
            pollutionGroup.Fuzzifiers.Add(new TrapezoidFuzzifier(0, 0, 0.2, 0.3, "niskie"));
            pollutionGroup.Fuzzifiers.Add(new TriangleFuzzifier(0.1, 0.4, 0.7, "średnie"));
            pollutionGroup.Fuzzifiers.Add(new TrapezoidFuzzifier(0.5, 0.7, 1.0, 1.0, "duże"));
            sugenoSystem.FuzzifierGroups.Add(pollutionGroup);

            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "niskie", "pollution", "niskie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "niskie", "pollution", "średnie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "niskie", "pollution", "duże"));

            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "średnie", "pollution", "niskie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "średnie", "pollution", "średnie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "średnie", "pollution", "duże"));

            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "duże", "pollution", "niskie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "duże", "pollution", "średnie"));
            sugenoSystem.Rules.Add(new SugenoRule(SugenoRule.AND, "sunlight", "duże", "pollution", "duże"));


            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.6, label = "OK" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.3, label = "BAD" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.1, label = "VERY BAD" });

            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.8, label = "GOOD" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.5, label = "OK" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.2, label = "BAD" });

            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 1.0, label = "VERY GOOD" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.7, label = "GOOD" });
            sugenoSystem.expectedValues.Add(new SugenoExpectedValue() { value = 0.3, label = "BAD" });





            List<SugenoInput> inputs = new List<SugenoInput>();
            inputs.Add(new SugenoInput("Warszawa", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.6,
                ["pollution"] = 0.3
            }));
            inputs.Add(new SugenoInput("Kraków", new Dictionary<string, double>()
            {
                ["sunlight"] = 1.0,
                ["pollution"] = 0.1
            }));
            inputs.Add(new SugenoInput("Gdańsk", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.9,
                ["pollution"] = 0.9
            }));
            inputs.Add(new SugenoInput("Wrocław", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.8,
                ["pollution"] = 0.7
            }));
            inputs.Add(new SugenoInput("Katowice", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.3,
                ["pollution"] = 0.1
            }));
            inputs.Add(new SugenoInput("Poznań", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.7,
                ["pollution"] = 0.6
            }));
            inputs.Add(new SugenoInput("Gliwice", new Dictionary<string, double>()
            {
                ["sunlight"] = 0.3,
                ["pollution"] = 0.1
            }));



            inputs.ForEach(x => Console.WriteLine("Living standards for " + x.Label + " are " + sugenoSystem.Compile(x)));
        }
    }
}
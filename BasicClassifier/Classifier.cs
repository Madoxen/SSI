using System.Collections.Generic;

namespace BasicClassifier
{
    public class Classifier
    {

        public static List<ClassifierInfo> Classify(List<IdentifiedObject> input, List<ClassifierInfo> parameters)
        {
            List<ClassifierInfo> result = new List<ClassifierInfo>();
            foreach (IdentifiedObject obj in input)
            {
                ClassifierInfo r = new ClassifierInfo();
                r.label = obj.id;
                foreach (ClassifierInfo i in parameters)
                {
                    if (obj.properties.Exists(x=> x == i.label))
                    {
                        r.match += i.match;
                    }
                }
                result.Add(r);
            }
            return result;
        }
    }

    public struct ClassifierInfo
    {
        public double match;
        public string label;

        public ClassifierInfo(string _label, double _match)
        {
            match = _match;
            label = _label;
        }

    }

}
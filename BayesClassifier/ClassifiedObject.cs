using System.Collections.Generic;

namespace BayesClassifier
{
    public class ClassifiedObject
    {

        public string Classification { get; set; } //Classification of this object
        public List<string> Properties { get; set; } //all properties of this object
        public string Label { get; set; } //A human readable ID

        public ClassifiedObject()
        {
            Properties = new List<string>();

        }
    }
}
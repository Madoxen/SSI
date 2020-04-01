using System.Collections.Generic;

namespace ML.Lib.Classification
{
    public class ClassifiedObject<T>
    {

        public string Classification { get; set; } //Classification of this object
        public List<T> Properties { get; set; } //all properties of this object
        public string Label { get; set; } //A human readable ID of this particular object

        public ClassifiedObject()
        {
            Properties = new List<T>();
        }
    }
}
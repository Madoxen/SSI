using System.Collections.Generic;
using System.Linq;
using ML.Lib;

namespace BayesClassifier
{
    public class Classifier
    {
        List<ClassifiedObject> _currentDataset; //Raw data
        Dictionary<string, List<ClassifiedObject>> _classes = new Dictionary<string, List<ClassifiedObject>>(); //Segragated data by Class
        List<List<string>> _possibleValues = new List<List<string>>(); //Contains list of unique values in given column
        public void LoadDataset(List<ClassifiedObject> input, int props) //Loads and enumerates given dataset
        {
            _currentDataset = new List<ClassifiedObject>(input);
            for (int i = 0; i < props; i++)
            {
                _possibleValues.Add(new List<string>());
            }


            foreach (ClassifiedObject o in input)
            {
                if (!_classes.ContainsKey(o.Classification))
                {
                    _classes.Add(o.Classification, new List<ClassifiedObject>() { o });
                }
                else
                {
                    _classes[o.Classification].Add(o);
                }

                for (int i = 0; i < o.Properties.Count; i++) //For each of current row-column
                {
                    if (!_possibleValues[i].Contains(o.Properties[i]))
                        _possibleValues[i].Add(o.Properties[i]);
                }
            }
        }


        public void Classify(ClassifiedObject input, double probMod = 0)
        {
            double[][] probabilities = new double[_classes.Count][];
            double[] results = new double[_classes.Count];
            
            for(int i = 0; i < results.Length; i++)
                results[i] = (double)_classes.ElementAt(i).Value.Count / (double)_currentDataset.Count; //Probability of being in _classes[i] - class


            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = new double[input.Properties.Count]; //All properties + probability of being in a class
            }

            for (int i = 0; i < _classes.Count; i++) //Chooses class
            {
                for (int j = 0; j < input.Properties.Count; j++) //Chooses property COLUMN in given class
                {
                    //add all cases in class[i] that have property[j]
                    probabilities[i][j] = _classes.ElementAt(i).Value.FindAll(x => x.Properties.Exists(y => y == input.Properties[j])).Count; //TODO: this is awful
                    if (probabilities[i][j] == 0) //solve for zero
                    {
                        probabilities[i][j] = 1 / (_classes.ElementAt(i).Value.Count + probMod * _possibleValues[j].Count);
                    }
                    else
                    {
                        probabilities[i][j] /= _classes.ElementAt(i).Value.Count;
                    }

                    results[i] *= probabilities[i][j]; //product of probabilities of being in _classes[i] class and having property j
                }
            }

            input.Classification = _classes.ElementAt(results.MaxAt()).Key;
             
        }


    }


}
using System.Collections.Generic;

namespace CommandBuilder.Configurations
{
    public class InsertValuesConfiguration
    {
        internal IList<object> Values { get; }

        public InsertValuesConfiguration()
        {
            Values = new List<object>();
        }

        public InsertValuesConfiguration Value(object value)
        {
            Values.Add(value);
            return this;
        }
    }
}
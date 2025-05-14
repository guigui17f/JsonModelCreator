using System;
using System.Collections.Generic;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// data to describe a model used to represent a json object
    /// </summary>
    [Serializable]
    public class JsonModelData
    {
        public string ModelName;
        public bool UseStruct;
        public List<JsonModelFieldData> FieldList;
    }
}
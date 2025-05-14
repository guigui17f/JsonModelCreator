using System;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// data used to describe a field in a json model
    /// </summary>
    [Serializable]
    public class JsonModelFieldData
    {
        /// <summary>
        /// the type of this field
        /// </summary>
        public string TypeName;
        /// <summary>
        /// the name of this field
        /// </summary>
        public string FieldName;
        /// <summary>
        /// is this field a array, a list or just a single value
        /// </summary>
        public JsonModelCollectionType CollectionType;
    }
}
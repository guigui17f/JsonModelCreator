using System.Collections.Generic;
using UnityEngine;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// scriptableObject data used to save current workspace as the cache
    /// </summary>
    public class JsonModelCacheData : ScriptableObject
    {
        /// <summary>
        /// the model list used to save in the cache
        /// </summary>
        public List<JsonModelData> ModelList;
    }
}
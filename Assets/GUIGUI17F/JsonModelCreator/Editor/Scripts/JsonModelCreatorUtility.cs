using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Defective.JSON;
using UnityEditor;
using UnityEngine;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// utility methods for Json Model Creator
    /// </summary>
    public class JsonModelCreatorUtility
    {
        /// <summary>
        /// create json model list from JSONObject data
        /// </summary>
        /// <param name="jsonObject">source JSONObject data</param>
        /// <param name="useStruct">whether use struct as the default type for models</param>
        /// <param name="useArray">whether use array as the default collection type for fields</param>
        /// <returns>json model list corresponding to the source JSONObject data</returns>
        public static List<JsonModelData> AnalyseJsonModels(JSONObject jsonObject, bool useStruct, bool useArray)
        {
            List<JsonModelData> modelList = new List<JsonModelData>();
            Stack<JSONObject> objectStack = new Stack<JSONObject>();
            Stack<JsonModelData> modelStack = new Stack<JsonModelData>();

            if (jsonObject.type == JSONObject.Type.Object)
            {
                objectStack.Push(jsonObject);
                modelStack.Push(new JsonModelData {UseStruct = useStruct, ModelName = "ModelName0"});
            }
            else if (jsonObject.type == JSONObject.Type.Array)
            {
                for (int i = 0; i < jsonObject.count; i++)
                {
                    if (jsonObject[i].type == JSONObject.Type.Object)
                    {
                        objectStack.Push(jsonObject[i]);
                        modelStack.Push(new JsonModelData {UseStruct = useStruct, ModelName = "ModelName0"});
                        break;
                    }
                }
            }

            JSONObject currentObject;
            JSONObject currentField;
            JsonModelData currentModel;
            JsonModelCollectionType collectionType;
            bool hasValidField;
            string modelName;
            while (objectStack.Count > 0)
            {
                currentObject = objectStack.Pop();
                currentModel = modelStack.Pop();
                currentModel.FieldList = new List<JsonModelFieldData>();
                for (int i = 0; i < currentObject.count; i++)
                {
                    collectionType = JsonModelCollectionType.Single;
                    currentField = currentObject[i];
                    if (currentField.type == JSONObject.Type.Array)
                    {
                        collectionType = useArray ? JsonModelCollectionType.Array : JsonModelCollectionType.List;
                        hasValidField = false;
                        for (int j = 0; j < currentField.count; j++)
                        {
                            if (currentField[j].type != JSONObject.Type.Array)
                            {
                                currentField = currentField[j];
                                hasValidField = true;
                                break;
                            }
                        }
                        if (!hasValidField)
                        {
                            continue;
                        }
                    }
                    if (currentField.type == JSONObject.Type.Object)
                    {
                        objectStack.Push(currentField);
                        modelName = GetModifiedName(currentObject.keys[i], collectionType);
                        if (!modelList.Exists(item => item.ModelName.Equals(modelName)))
                        {
                            modelStack.Push(new JsonModelData {UseStruct = useStruct, ModelName = modelName});
                        }
                        currentModel.FieldList.Add(new JsonModelFieldData {TypeName = modelName, FieldName = currentObject.keys[i], CollectionType = collectionType});
                    }
                    else
                    {
                        currentModel.FieldList.Add(new JsonModelFieldData {TypeName = GetSimpleTypeName(currentField), FieldName = currentObject.keys[i], CollectionType = collectionType});
                    }
                }
                modelList.Add(currentModel);
            }
            return modelList;
        }

        /// <summary>
        /// get the simple types supported by json
        /// </summary>
        public static List<string> GetSimpleTypeList()
        {
            return new List<string>
            {
                "int",
                "float",
                "double",
                "bool",
                "string"
            };
        }

        /// <summary>
        /// load the model list cache from the scriptableObject file
        /// </summary>
        public static JsonModelCacheData LoadCacheData()
        {
            return Resources.Load<JsonModelCacheData>("JsonModelCreator/JsonModelCacheData");
        }

        /// <summary>
        /// save the model list to the scriptableObject cache file
        /// </summary>
        public static void SaveCacheData(JsonModelCacheData data)
        {
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// generate the C# code files based on the given model list
        /// </summary>
        /// <param name="modelList">the source json model list</param>
        /// <param name="savePath">the save path for the generated code files</param>
        /// <param name="nameSpace">the namespace for the generated json models</param>
        public static void GenerateJsonModelFiles(List<JsonModelData> modelList, string savePath, string nameSpace)
        {
            StringBuilder builder = new StringBuilder();
            foreach (JsonModelData model in modelList)
            {
                CreateJsonModel(model, Path.Combine(savePath, $"{model.ModelName}.cs"), nameSpace, builder);
            }
        }

        private static string GetModifiedName(string originName, JsonModelCollectionType collectionType)
        {
            List<char> modifyName = new List<char>(originName);
            for (int i = 0; i < modifyName.Count; i++)
            {
                if (modifyName[i] == '_' || modifyName[i] == '-')
                {
                    modifyName.RemoveAt(i);
                    if (i < modifyName.Count)
                    {
                        modifyName[i] = char.ToUpper(modifyName[i]);
                    }
                }
            }
            modifyName[0] = char.ToUpper(modifyName[0]);
            if (collectionType != JsonModelCollectionType.Single && modifyName[modifyName.Count - 1] == 's')
            {
                modifyName.RemoveAt(modifyName.Count - 1);
            }
            return new string(modifyName.ToArray());
        }

        private static string GetSimpleTypeName(JSONObject jsonObject)
        {
            switch (jsonObject.type)
            {
                case JSONObject.Type.Number:
                    return "float";
                case JSONObject.Type.Bool:
                    return "bool";
                case JSONObject.Type.String:
                    return "string";
                default:
                    return "int";
            }
        }

        private static void CreateJsonModel(JsonModelData model, string savePath, string nameSpace, StringBuilder builder)
        {
            builder.Clear();
            bool hasList = false;

            builder.AppendLine("using System;");
            int referenceEnd = builder.Length;
            builder.AppendLine();
            builder.Append("namespace ");
            builder.AppendLine(nameSpace);
            builder.AppendLine("{");
            builder.AppendLine("    [Serializable]");
            builder.Append(model.UseStruct ? "    public struct " : "    public class ");
            builder.AppendLine(model.ModelName);
            builder.AppendLine("    {");

            foreach (JsonModelFieldData field in model.FieldList)
            {
                builder.Append("        public ");
                if (field.CollectionType == JsonModelCollectionType.List)
                {
                    hasList = true;
                    builder.Append("List<");
                }
                builder.Append(field.TypeName);
                if (field.CollectionType == JsonModelCollectionType.Array)
                {
                    builder.Append("[]");
                }
                else if (field.CollectionType == JsonModelCollectionType.List)
                {
                    builder.Append(">");
                }
                builder.Append(' ');
                builder.Append(field.FieldName);
                builder.AppendLine(";");
            }

            builder.AppendLine("    }");
            builder.Append('}');
            if (hasList)
            {
                builder.Insert(referenceEnd, $"using System.Collections.Generic;{Environment.NewLine}");
            }

            File.WriteAllText(savePath, builder.ToString(), Encoding.UTF8);
        }
    }
}
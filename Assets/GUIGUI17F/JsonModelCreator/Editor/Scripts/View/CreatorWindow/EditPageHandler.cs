using System;
using System.Collections.Generic;
using Defective.JSON;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// class used to handle logic in edit json model page
    /// </summary>
    public class EditPageHandler
    {
        private JsonModelCacheData _cacheData;
        private bool _useStruct;
        private List<string> _typeList;
        private VisualTreeAsset _modelAsset;
        private VisualTreeAsset _fieldAsset;
        private VisualElement _modelRoot;
        private int _modelNameIndex;

        private Signal<string, string> _typeNameChangedSignal;
        private Signal _typeListChangedSignal;
        private Action _backCallback;
        private Action<List<JsonModelData>> _saveCallback;

        /// <summary>
        /// load edit page using JSONObject parsed from user input json text
        /// </summary>
        /// <param name="root">visual element root</param>
        /// <param name="jsonObject">JSONObject parsed from user input</param>
        /// <param name="useStruct">whether use struct as the default type for models</param>
        /// <param name="useArray">whether use array as the default collection type for fields</param>
        /// <param name="backCallback">callback for back to guide page</param>
        /// <param name="saveCallback">callback for save model list</param>
        public void LoadPage(VisualElement root, JSONObject jsonObject, bool useStruct, bool useArray, Action backCallback, Action<List<JsonModelData>> saveCallback)
        {
            List<JsonModelData> modelList = JsonModelCreatorUtility.AnalyseJsonModels(jsonObject, useStruct, useArray);
            InitializePage(root, modelList, useStruct, backCallback, saveCallback);
        }
        
        /// <summary>
        /// load edit page using last cache data
        /// </summary>
        /// <param name="root">visual element root</param>
        /// <param name="useStruct">whether use struct as the default type for models</param>
        /// <param name="backCallback">callback for back to guide page</param>
        /// <param name="saveCallback">callback for save model list</param>
        public void LoadPage(VisualElement root, bool useStruct, Action backCallback, Action<List<JsonModelData>> saveCallback)
        {
            if (LoadCacheData())
            {
                List<JsonModelData> modelList = _cacheData.ModelList;
                InitializePage(root, modelList, useStruct, backCallback, saveCallback);
            }
        }

        /// <summary>
        /// save current model list to cache file
        /// </summary>
        /// <returns>current model list data</returns>
        public List<JsonModelData> SaveModelCache()
        {
            List<JsonModelData> modelList = new List<JsonModelData>();
            _modelRoot.Query<JsonModelElement>().ForEach(element => modelList.Add(element.GetModelData()));
            if (_cacheData != null || LoadCacheData())
            {
                _cacheData.ModelList = modelList;
                JsonModelCreatorUtility.SaveCacheData(_cacheData);
            }
            return modelList;
        }

        private bool LoadCacheData()
        {
            _cacheData = JsonModelCreatorUtility.LoadCacheData();
            if (_cacheData == null)
            {
                EditorUtility.DisplayDialog("Warning", "Cache file is missing, reimport the plugin to fix this issue.", "OK");
                return false;
            }
            return true;
        }

        private void InitializePage(VisualElement root, List<JsonModelData> modelList, bool useStruct, Action backCallback, Action<List<JsonModelData>> saveCallback)
        {
            _useStruct = useStruct;
            _backCallback = backCallback;
            _saveCallback = saveCallback;
            _modelNameIndex = 1;
            _typeNameChangedSignal = new Signal<string, string>();
            _typeListChangedSignal = new Signal();

            _typeList = JsonModelCreatorUtility.GetSimpleTypeList();
            foreach (JsonModelData model in modelList)
            {
                if (!_typeList.Contains(model.ModelName))
                {
                    _typeList.Add(model.ModelName);
                }
            }

            root.Clear();
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("JsonModelCreator/edit-page");
            visualTree.CloneTree(root);
            _modelRoot = root.Q<VisualElement>("unity-content-container");
            root.Q<Button>("add-new-button").RegisterCallback<MouseUpEvent>(OnAddNewModel);
            root.Q<Button>("back-button").RegisterCallback<MouseUpEvent>(OnGoBack);
            root.Q<Button>("save-button").RegisterCallback<MouseUpEvent>(OnSaveModels);

            _modelAsset = Resources.Load<VisualTreeAsset>("JsonModelCreator/model-template");
            _fieldAsset = Resources.Load<VisualTreeAsset>("JsonModelCreator/field-template");
            foreach (JsonModelData model in modelList)
            {
                VisualElement modelElement = _modelAsset.CloneTree();
                modelElement.Q<JsonModelElement>().Initialize(model, _typeList, _fieldAsset, _typeNameChangedSignal, _typeListChangedSignal);
                _modelRoot.Add(modelElement);
            }
        }

        private void OnAddNewModel(MouseUpEvent evt)
        {
            while (_typeList.Contains("ModelName" + _modelNameIndex))
            {
                _modelNameIndex++;
            }
            JsonModelData model = new JsonModelData
            {
                ModelName = "ModelName" + _modelNameIndex,
                UseStruct = _useStruct,
                FieldList = new List<JsonModelFieldData>()
            };

            VisualElement modelElement = _modelAsset.CloneTree();
            modelElement.Q<JsonModelElement>().Initialize(model, _typeList, _fieldAsset, _typeNameChangedSignal, _typeListChangedSignal);
            _modelRoot.Add(modelElement);

            _typeList.Add(model.ModelName);
            _typeListChangedSignal.Dispatch();
        }

        private void OnGoBack(MouseUpEvent evt)
        {
            SaveModelCache();
            _backCallback();
        }

        private void OnSaveModels(MouseUpEvent evt)
        {
            List<JsonModelData> modelList = SaveModelCache();
            _saveCallback(modelList);
        }
    }
}
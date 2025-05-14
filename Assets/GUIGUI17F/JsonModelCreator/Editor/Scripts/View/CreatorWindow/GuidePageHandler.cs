using System;
using System.Text;
using Defective.JSON;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// class used to handle logic in create json model guide page
    /// </summary>
    public class GuidePageHandler
    {
        private readonly string[] _whiteSpaces;

        private TextField _jsonField;
        private Toggle _structToggle;
        private Toggle _arrayToggle;
        private Toggle _strictToggle;
        private Action<string, JSONObject, bool, bool> _createCallback;
        private Action<string, bool> _loadCallback;

        public GuidePageHandler()
        {
            _whiteSpaces = new string[JSONObject.Whitespace.Length];
            for (int i = 0; i < JSONObject.Whitespace.Length; i++)
            {
                _whiteSpaces[i] = JSONObject.Whitespace[i].ToString();
            }
        }

        /// <summary>
        /// load create json model guide page
        /// </summary>
        /// <param name="root">visual element root</param>
        /// <param name="inputText">last user input text</param>
        /// <param name="createCallback">callback for load edit page using user input json text</param>
        /// <param name="loadCallback">callback for load edit page using last edit page cache</param>
        public void LoadPage(VisualElement root, string inputText, Action<string, JSONObject, bool, bool> createCallback, Action<string, bool> loadCallback)
        {
            _createCallback = createCallback;
            _loadCallback = loadCallback;
            root.Clear();
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("JsonModelCreator/guide-page");
            visualTree.CloneTree(root);
            root.Q<Button>("create-model-button").RegisterCallback<MouseUpEvent>(OnCreateModel);
            root.Q<Button>("load-cache-button").RegisterCallback<MouseUpEvent>(OnLoadCache);
            _jsonField = root.Q<TextField>("json-input-field");
            _structToggle = root.Q<Toggle>("use-struct-toggle");
            _arrayToggle = root.Q<Toggle>("use-array-toggle");
            _strictToggle = root.Q<Toggle>("strict-mode-toggle");
            if (!string.IsNullOrEmpty(inputText))
            {
                _jsonField.SetValueWithoutNotify(inputText);
            }
        }

        private void OnCreateModel(MouseUpEvent evt)
        {
            JSONObject jsonObject;
            bool success;
            if (string.IsNullOrEmpty(_jsonField.value))
            {
                jsonObject = new JSONObject();
                success = true;
            }
            else
            {
                if (_strictToggle.value)
                {
                    StringBuilder builder = new StringBuilder(_jsonField.value);
                    for (int i = 0; i < _whiteSpaces.Length; i++)
                    {
                        builder.Replace(_whiteSpaces[i], string.Empty);
                    }
                    string trimValue = builder.ToString();
                    jsonObject = new JSONObject(trimValue);
                    success = jsonObject.Print().Equals(trimValue);
                }
                else
                {
                    jsonObject = new JSONObject(_jsonField.value);
                    success = jsonObject.type != JSONObject.Type.Null;
                }
            }
            if (success)
            {
                _createCallback(_jsonField.value, jsonObject, _structToggle.value, _arrayToggle.value);
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "Parse json text failed, please check your input contents.", "OK");
            }
        }

        private void OnLoadCache(MouseUpEvent evt)
        {
            _loadCallback(_jsonField.value, _structToggle.value);
        }
    }
}
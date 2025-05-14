using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// window to setup json models save information
    /// </summary>
    public class JsonModelSaveWindow : EditorWindow
    {
        private List<JsonModelData> _modelList;
        private TextField _namespaceField;
        private TextField _pathField;
        private Action _saveCallback;

        public static void ShowWindow(List<JsonModelData> modelList, Action saveCallback)
        {
            JsonModelSaveWindow window = GetWindow<JsonModelSaveWindow>();
            window.minSize = new Vector2(410, 145);
            window.maxSize = window.minSize;
            window.titleContent = new GUIContent("Save Config");
            window._modelList = modelList;
            window._saveCallback = saveCallback;
            window.ShowModalUtility();
        }

        private void OnEnable()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("JsonModelCreator/save-page");
            visualTree.CloneTree(root);
            _namespaceField = root.Q<TextField>("namespace-field");
            _pathField = root.Q<TextField>("path-field");
            root.Q<Button>("browse-button").RegisterCallback<MouseUpEvent>(OnBrowsePath);
            root.Q<Button>("save-button").RegisterCallback<MouseUpEvent>(OnSaveModelList);
        }

        private void OnBrowsePath(MouseUpEvent evt)
        {
            string path = EditorUtility.SaveFolderPanel("Choose Model Save Path", Application.dataPath, string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
                _pathField.SetValueWithoutNotify(path);
            }
        }

        private void OnSaveModelList(MouseUpEvent evt)
        {
            string path = _pathField.value;
            string nameSpace = _namespaceField.value;
            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Warning", "Please input model save path!", "OK");
            }
            else if (string.IsNullOrEmpty(nameSpace))
            {
                EditorUtility.DisplayDialog("Warning", "Please input model namespace!", "OK");
            }
            else if (EditorUtility.DisplayDialog(
                "Warning",
                $"All files under this folder with same names will be erased, continue?",
                "OK",
                "Cancel"))
            {
                _saveCallback();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                JsonModelCreatorUtility.GenerateJsonModelFiles(_modelList, path, nameSpace);
                Process.Start(path);
                AssetDatabase.Refresh();
                Close();
            }
        }
    }
}
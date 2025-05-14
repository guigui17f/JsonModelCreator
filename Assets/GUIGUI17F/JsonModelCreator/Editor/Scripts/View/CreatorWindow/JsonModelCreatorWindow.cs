using System.Collections.Generic;
using Defective.JSON;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// the main window of JsonModelCreator
    /// </summary>
    public class JsonModelCreatorWindow : EditorWindow
    {
        private VisualElement _root;
        private GuidePageHandler _guideHandler;
        private EditPageHandler _editHandler;
        private string _inputText;

        [MenuItem("Tools/JsonModelCreator/CreateModelGuide")]
        private static void ShowWindow()
        {
            JsonModelCreatorWindow window = GetWindow<JsonModelCreatorWindow>();
            window.minSize = new Vector2(480, 480);
            window.saveChangesMessage = "Do you want to save current model list as the cache?";
            window.titleContent = new GUIContent("Json Model Creator");
        }

        private void OnEnable()
        {
            _root = rootVisualElement;
            _guideHandler = new GuidePageHandler();
            _editHandler = new EditPageHandler();
            LoadGuidePage();
        }

        public override void SaveChanges()
        {
            _editHandler?.SaveModelCache();
            base.SaveChanges();
        }

        private void LoadGuidePage()
        {
            hasUnsavedChanges = false;
            _guideHandler.LoadPage(_root, _inputText, LoadEditPage, LoadEditPage);
        }

        private void LoadEditPage(string inputText, JSONObject jsonObject, bool useStruct, bool useArray)
        {
            hasUnsavedChanges = true;
            _inputText = inputText;
            _editHandler.LoadPage(_root, jsonObject, useStruct, useArray, LoadGuidePage, ShowSaveWindow);
        }

        private void LoadEditPage(string inputText, bool useStruct)
        {
            hasUnsavedChanges = true;
            _inputText = inputText;
            _editHandler.LoadPage(_root, useStruct, LoadGuidePage, ShowSaveWindow);
        }

        private void ShowSaveWindow(List<JsonModelData> modelList)
        {
            JsonModelSaveWindow.ShowWindow(modelList, OnModelListSaving);
        }

        private void OnModelListSaving()
        {
            hasUnsavedChanges = false;
        }
    }
}
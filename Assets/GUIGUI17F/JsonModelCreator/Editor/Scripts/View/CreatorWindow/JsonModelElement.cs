using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// visual element used in edit page to manage the logic inside a json model node
    /// </summary>
    public class JsonModelElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<JsonModelElement, UxmlTraits>
        {
        }

        private JsonModelData _model;
        private List<string> _typeList;
        private VisualTreeAsset _fieldAsset;
        private VisualElement _fieldRoot;
        private CompatibleDropdownField _typeDropdown;
        private TextField _nameText;
        private Button _addButton;
        private int _fieldNameIndex;

        private Signal<string, string> _typeNameChangedSignal;
        private Signal _typeListChangedSignal;

        /// <summary>
        /// initialize the json model node
        /// </summary>
        /// <param name="model">the source model data</param>
        /// <param name="typeList">currently usable type list</param>
        /// <param name="fieldAsset">visual asset to create the field node</param>
        /// <param name="typeNameChangedSignal">signal to communicate type name changed event</param>
        /// <param name="typeListChangedSignal">signal to communicate usable type list changed event</param>
        public void Initialize(JsonModelData model, List<string> typeList, VisualTreeAsset fieldAsset, Signal<string, string> typeNameChangedSignal, Signal typeListChangedSignal)
        {
            _model = model;
            _typeList = typeList;
            _fieldAsset = fieldAsset;
            _typeNameChangedSignal = typeNameChangedSignal;
            _typeListChangedSignal = typeListChangedSignal;
            _fieldNameIndex = 0;

            _fieldRoot = this.Q<VisualElement>("field-list");
            _typeDropdown = this.Q<CompatibleDropdownField>("type-field");
            _nameText = this.Q<TextField>("name-field");
            _addButton = this.Q<Button>("add-field-button");

            _typeDropdown.SetValueWithoutNotify(_model.UseStruct ? "struct" : "class");
            _nameText.SetValueWithoutNotify(_model.ModelName);

            _nameText.RegisterCallback<FocusOutEvent>(OnModelNameFocusOut);
            _addButton.RegisterCallback<MouseUpEvent>(OnAddField);
            this.Q<Toggle>("expand-toggle").RegisterValueChangedCallback(OnExpandChanged);
            this.Q<Button>("delete-button").RegisterCallback<MouseUpEvent>(OnDeleteModel);

            foreach (JsonModelFieldData field in model.FieldList)
            {
                VisualElement fieldElement = fieldAsset.CloneTree();
                fieldElement.Q<JsonModelFieldElement>().Initialize(_model, field, _typeList, _typeNameChangedSignal, _typeListChangedSignal, OnFieldDeleted);
                _fieldRoot.Add(fieldElement);
            }
        }

        /// <summary>
        /// get current json model data represented by this node
        /// </summary>
        /// <returns></returns>
        public JsonModelData GetModelData()
        {
            _model.UseStruct = _typeDropdown.value.Equals("struct");
            _model.ModelName = _nameText.value;
            _fieldRoot.Query<JsonModelFieldElement>().ForEach(element => element.UpdateFieldData());
            return _model;
        }

        private void OnAddField(MouseUpEvent evt)
        {
            string fieldName = "FieldName" + _fieldNameIndex;
            while (_model.FieldList.Exists(item => item.FieldName.Equals(fieldName)))
            {
                _fieldNameIndex++;
                fieldName = "FieldName" + _fieldNameIndex;
            }
            JsonModelFieldData field = new JsonModelFieldData
            {
                TypeName = "int",
                FieldName = fieldName,
                CollectionType = JsonModelCollectionType.Single
            };
            _model.FieldList.Add(field);

            VisualElement fieldElement = _fieldAsset.CloneTree();
            fieldElement.Q<JsonModelFieldElement>().Initialize(_model, field, _typeList, _typeNameChangedSignal, _typeListChangedSignal, OnFieldDeleted);
            _fieldRoot.Add(fieldElement);
        }

        private void OnDeleteModel(MouseUpEvent evt)
        {
            if (EditorUtility.DisplayDialog(
                "Warning",
                $"Deleting model {_model.ModelName}, every field using this type will be deleted too, this operation can't undo, continue?",
                "OK",
                "Cancel"))
            {
                RemoveFromHierarchy();
                _typeList.Remove(_model.ModelName);
                _typeListChangedSignal.Dispatch();
            }
        }

        private void OnExpandChanged(ChangeEvent<bool> evt)
        {
            _fieldRoot.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            _addButton.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnModelNameFocusOut(FocusOutEvent evt)
        {
            string oldName = _model.ModelName;
            string newName = _nameText.value;
            if (newName != oldName)
            {
                if (string.IsNullOrEmpty(newName))
                {
                    EditorUtility.DisplayDialog("Warning", "Model name can't be empty!", "OK");
                    _nameText.SetValueWithoutNotify(oldName);
                }
                else if (_typeList.Contains(newName))
                {
                    EditorUtility.DisplayDialog("Warning", "Model name must be unique!", "OK");
                    _nameText.SetValueWithoutNotify(oldName);
                }
                else
                {
                    _model.ModelName = newName;
                    int index = _typeList.IndexOf(oldName);
                    _typeList[index] = newName;
                    _typeNameChangedSignal.Dispatch(oldName, newName);
                    _typeListChangedSignal.Dispatch();
                }
            }
        }

        private void OnFieldDeleted(JsonModelFieldData field)
        {
            _model.FieldList.Remove(field);
        }
    }
}
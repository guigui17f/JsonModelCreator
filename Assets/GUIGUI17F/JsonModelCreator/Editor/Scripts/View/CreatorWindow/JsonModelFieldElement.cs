using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// visual element used in edit page to manage the logic inside a model field node
    /// </summary>
    public class JsonModelFieldElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<JsonModelFieldElement, UxmlTraits>
        {
        }

        private JsonModelData _model;
        private JsonModelFieldData _field;
        private List<string> _typeList;
        private CompatibleDropdownField _typeDropdown;
        private TextField _nameText;
        private CompatibleDropdownField _collectionDropdown;
        private string _lastOldTypeName;
        private string _lastNewTypeName;

        private Signal<string, string> _typeNameChangedSignal;
        private Signal _typeListChangedSignal;
        private Action<JsonModelFieldData> _deletedCallback;

        /// <summary>
        /// initialize the model field node
        /// </summary>
        /// <param name="model">the source model data</param>
        /// <param name="field">the source field data</param>
        /// <param name="typeList">currently usable type list</param>
        /// <param name="typeNameChangedSignal">signal to communicate type name changed event</param>
        /// <param name="typeListChangedSignal">signal to communicate usable type list changed event</param>
        /// <param name="deletedCallback">callback for notice this field deleted</param>
        public void Initialize(JsonModelData model, JsonModelFieldData field, List<string> typeList, Signal<string, string> typeNameChangedSignal, Signal typeListChangedSignal, Action<JsonModelFieldData> deletedCallback)
        {
            _model = model;
            _field = field;
            _typeList = typeList;
            _typeNameChangedSignal = typeNameChangedSignal;
            _typeListChangedSignal = typeListChangedSignal;
            _deletedCallback = deletedCallback;
            _typeDropdown = this.Q<CompatibleDropdownField>("type-field");
            _nameText = this.Q<TextField>("name-field");
            _collectionDropdown = this.Q<CompatibleDropdownField>("collection-field");
            
            _typeDropdown.SetupChoiceList(typeList);
            _typeDropdown.SetValueWithoutNotify(field.TypeName);
            _nameText.SetValueWithoutNotify(_field.FieldName);
            _collectionDropdown.SetValueWithoutNotify(_field.CollectionType.ToString());

            _typeDropdown.RegisterValueChangedCallback(OnFieldTypeChanged);
            _nameText.RegisterCallback<FocusOutEvent>(OnFieldNameFocusOut);
            this.Q<Button>("delete-button").RegisterCallback<MouseUpEvent>(OnDeleteField);
            _typeNameChangedSignal.AddListener(OnTypeNameChanged);
            _typeListChangedSignal.AddListener(OnTypeListChanged);
        }

        /// <summary>
        /// update current node changed to the field data
        /// </summary>
        public void UpdateFieldData()
        {
            _field.TypeName = _typeDropdown.value;
            _field.FieldName = _nameText.value;
            _field.CollectionType = (JsonModelCollectionType) Enum.Parse(typeof(JsonModelCollectionType), _collectionDropdown.value);
        }

        private void OnDeleteField(MouseUpEvent evt)
        {
            if (EditorUtility.DisplayDialog(
                "Warning",
                $"Deleting field {_field.FieldName}, this operation can't undo, continue?",
                "OK",
                "Cancel"))
            {
                DeleteField();
            }
        }

        private void OnFieldTypeChanged(ChangeEvent<string> evt)
        {
            if (evt.newValue.Equals(_lastOldTypeName) && !_typeList.Contains(evt.newValue))
            {
                _typeDropdown.SetValueWithoutNotify(_lastNewTypeName);
                _field.TypeName = _lastNewTypeName;
            }
            else
            {
                _field.TypeName = evt.newValue;
            }
            _lastOldTypeName = null;
            _lastNewTypeName = null;
        }

        private void OnFieldNameFocusOut(FocusOutEvent evt)
        {
            string oldName = _field.FieldName;
            string newName = _nameText.value;
            if (newName != oldName)
            {
                if (string.IsNullOrEmpty(newName))
                {
                    EditorUtility.DisplayDialog("Warning", "Field name can't be empty!", "OK");
                    _nameText.SetValueWithoutNotify(oldName);
                }
                else if (_model.FieldList.Exists(item => item.FieldName.Equals(newName)))
                {
                    EditorUtility.DisplayDialog("Warning", "Field name must be unique inside the model!", "OK");
                    _nameText.SetValueWithoutNotify(oldName);
                }
                else
                {
                    _field.FieldName = newName;
                }
            }
        }

        private void OnTypeNameChanged(string oldName, string newName)
        {
            if (_field.TypeName.Equals(oldName))
            {
                _field.TypeName = newName;
            }
            _lastOldTypeName = oldName;
            _lastNewTypeName = newName;
        }

        private void OnTypeListChanged()
        {
            if (_typeList.Contains(_field.TypeName))
            {
                _typeDropdown.SetValueWithoutNotify(_field.TypeName);
            }
            else
            {
                DeleteField();
            }
        }

        private void DeleteField()
        {
            RemoveFromHierarchy();
            _typeNameChangedSignal.RemoveListener(OnTypeNameChanged);
            _typeListChangedSignal.RemoveListener(OnTypeListChanged);
            _deletedCallback(_field);
        }
    }
}
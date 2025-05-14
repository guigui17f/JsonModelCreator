using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// A compatible dropdown field just like the built-in one in Unity Editor 2021.2 or above
    /// use this to provide UI element support for lower Unity Editor version
    /// </summary>
    public class CompatibleDropdownField : PopupField<string>
    {
        public new class UxmlFactory : UxmlFactory<CompatibleDropdownField, UxmlTraits>
        {
        }

        public new class UxmlTraits : BaseField<string>.UxmlTraits
        {
            private readonly UxmlIntAttributeDescription _index = new UxmlIntAttributeDescription {name = "index"};
            private readonly UxmlStringAttributeDescription _choices = new UxmlStringAttributeDescription {name = "choices"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                CompatibleDropdownField dropdownField = (CompatibleDropdownField) ve;
                string choicesFromBag = _choices.GetValueFromBag(bag, cc);
                string[] choiceArray = choicesFromBag.Trim().Split(',');
                List<string> choiceList = new List<string>();
                for (int i = 0; i < choiceArray.Length; i++)
                {
                    choiceList.Add(choiceArray[i].Trim());
                }
                dropdownField.SetupChoiceList(choiceList);
                dropdownField.index = _index.GetValueFromBag(bag, cc);
            }
        }

#if !UNITY_2021_2_OR_NEWER
        private static readonly PropertyInfo DropdownChoicesInfo = typeof(CompatibleDropdownField).GetProperty("choices", BindingFlags.NonPublic | BindingFlags.Instance);
#endif

        /// <summary>
        /// update the choice list to the given string list
        /// </summary>
        public void SetupChoiceList(List<string> choiceList)
        {
#if UNITY_2021_2_OR_NEWER
            choices = choiceList;
#else
            //use reflection to bypass the limitation in old version UIElements
            DropdownChoicesInfo.SetValue(this, choiceList);
#endif
        }

        public CompatibleDropdownField() : this(null)
        {
        }

        public CompatibleDropdownField(string label) : base(label)
        {
        }

        public CompatibleDropdownField(List<string> choiceList, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
            : this(null, choiceList, defaultValue, formatSelectedValueCallback, formatListItemCallback)
        {
        }

        public CompatibleDropdownField(string label, List<string> choiceList, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
            : base(label, choiceList, defaultValue, formatSelectedValueCallback, formatListItemCallback)
        {
        }

        public CompatibleDropdownField(List<string> choiceList, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
            : this(null, choiceList, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
        {
        }

        public CompatibleDropdownField(string label, List<string> choiceList, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
            : base(label, choiceList, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
        {
        }
    }
}
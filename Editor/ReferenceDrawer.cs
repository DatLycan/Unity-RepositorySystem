using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DatLycan.Packages.RepositorySystem {
    [CustomPropertyDrawer(typeof(Reference<>))]
    public class ReferenceDrawer : PropertyDrawer {
        private string[] typeNames, typeFullNames;

        private void Initialize() {
            if (typeFullNames != null) return;
            
            Type genericType = fieldInfo.FieldType.GetGenericArguments()[0];
            Type[] repositoryTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => genericType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();

            typeNames = repositoryTypes.Select(t => t.ReflectedType == null ? t.Name : $"{t.ReflectedType.Name} + {t.Name}").ToArray();
            typeFullNames = repositoryTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Initialize();
            SerializedProperty typeIdProperty = property.FindPropertyRelative("repositoryAssemblyQualifiedName");

            if (string.IsNullOrEmpty(typeIdProperty.stringValue)) {
                typeIdProperty.stringValue = typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            int currentIndex = Array.IndexOf(typeFullNames, typeIdProperty.stringValue);
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);

            if (selectedIndex >= 0 && selectedIndex != currentIndex) {
                typeIdProperty.stringValue = typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
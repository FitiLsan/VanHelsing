using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;

namespace BeastHunter
{
    [CustomPropertyDrawer(typeof(SpawnPointData))]
    public class SpawnPointDrawer : PropertyDrawer
    {
        private const int MAX_ENTITIES_TO_SPAWN = 20;
        private const float RECALC_BUTTON_WIDTH = 50.0f;
        private const float DATA_TYPE_WIDTH = 65.0f;
        private const float SPAWNING_CHANCE_WIDTH = 30.0f;
        private const float LABEL_WIDTH = 15.0f;
        private const float DELETE_BUTTON_WIDTH = 15.0f;
        private const float GAP_WIDTH = 5.0f;

        private SerializedProperty _spawnPoint;
        private SerializedProperty _radius;
        private SerializedProperty _number;
        private ReorderableList _spawnList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Init(property);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 1;
            
            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 195;
            }
            _spawnPoint.vector3Value = EditorGUILayout.Vector3Field(new GUIContent("Center:", "Spawn point"), _spawnPoint.vector3Value);
            _radius.floatValue = EditorGUILayout.FloatField(new GUIContent("Radius:", "Radius of spawn"), _radius.floatValue);
            _number.intValue = EditorGUILayout.IntSlider(new GUIContent("Amount:", "Number of entities to spawn"), _number.intValue, 1, MAX_ENTITIES_TO_SPAWN);
            _spawnList.DoLayoutList();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void Init(SerializedProperty property)
        {
            _spawnList = new ReorderableList(property.serializedObject, property.FindPropertyRelative("_spawnDataList"), true, true, true, false);
            _spawnPoint = property.FindPropertyRelative("_spawnPoint");
            _radius = property.FindPropertyRelative("_spawnRadius");
            _number = property.FindPropertyRelative("_numberToSpawn");

            _spawnList.drawHeaderCallback = HeaderCallback;
            _spawnList.drawElementCallback = ElementCallback;
            _spawnList.onSelectCallback = SelectCallback;
            _spawnList.onAddCallback = AddCallback;
            _spawnList.onCanRemoveCallback = (ReorderableList l) => { return l.count > 1; };
        }

        private void HeaderCallback(Rect rect)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(rect, "Enemy List");

            var clicked = GUI.Button(new Rect(rect.x + rect.width - RECALC_BUTTON_WIDTH, rect.y + 2.0f, RECALC_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight - 2.0f), 
                new GUIContent("Recalc", "Recalculate spawn chances so they sum up to 100"));
            
            if (clicked)
            {
                float sum = 0.0f;
                var count = _spawnList.serializedProperty.arraySize;
                for (int i = 0; i < count; i++)
                {
                    sum += _spawnList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_spawningChance").floatValue;
                }
                sum /= 100;
                for (int i = 0; i < count; i++)
                {
                    _spawnList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_spawningChance").floatValue /= sum;
                }
            }

            EditorGUI.indentLevel = indent;
        }

        private void ElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _spawnList.serializedProperty.GetArrayElementAtIndex(index);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            rect.y += 2.0f;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, DATA_TYPE_WIDTH, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_spawningDataType"), GUIContent.none);

            var xIndent = DATA_TYPE_WIDTH + GAP_WIDTH + 2 * GAP_WIDTH + SPAWNING_CHANCE_WIDTH + LABEL_WIDTH + DELETE_BUTTON_WIDTH;
            EditorGUI.PropertyField(new Rect(rect.x + DATA_TYPE_WIDTH + GAP_WIDTH, rect.y, 
                                                rect.width - xIndent, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_spawningEnemyData"), GUIContent.none);

            rect.x += rect.width - SPAWNING_CHANCE_WIDTH - LABEL_WIDTH - DELETE_BUTTON_WIDTH;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, SPAWNING_CHANCE_WIDTH, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_spawningChance"), GUIContent.none);

            rect.x += SPAWNING_CHANCE_WIDTH;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, LABEL_WIDTH, EditorGUIUtility.singleLineHeight), "%");

            rect.x += LABEL_WIDTH;
            var clicked = GUI.Button(new Rect(rect.x, rect.y, DELETE_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight), 
                EditorGUIUtility.IconContent("Toolbar Minus", "|Remove from list"), new GUIStyle("RL FooterButton"));

            EditorGUI.indentLevel = indent;

            if (clicked)
            {
                _spawnList.serializedProperty.DeleteArrayElementAtIndex(index); //TODO
            }
        }

        private void SelectCallback(ReorderableList list)
        {
            var data = list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("_spawningEnemyData").objectReferenceValue as ScriptableObject;
            if (data)
                EditorGUIUtility.PingObject(data);
        }

        private void AddCallback(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("_spawningDataType").enumValueIndex = 0;
            element.FindPropertyRelative("_spawningEnemyData").objectReferenceValue = null;
            element.FindPropertyRelative("_spawningChance").floatValue = 0.0f;
        }
    }
}
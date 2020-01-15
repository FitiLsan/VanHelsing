using UnityEditor;
using Models;
using Character;
using System;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Editors
{
    [CustomEditor(typeof(CharacterBaseStats))]
    public class CharacterBaseStatsEditor : Editor
    {
        private CharacterBaseStats _cs;

        public void OnEnable()
        {
            _cs = (CharacterBaseStats)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            _cs.Level = EditorGUILayout.IntField("Level", _cs.Level);

            StatContainer group = null;
            foreach (var stat in (Stats[])Enum.GetValues(typeof(Stats)))
            {

                StatContainer currentContainer;
                // смотрим текущую стату
                // если её еще нет в списке, то создать и добавить
                if (!_cs.StatsList.Exists(s => s.Name == stat))
                {
                    currentContainer = new StatContainer(stat, 1);
                    _cs.StatsList.Add(currentContainer);
                }
                // в противном случае, получаем на неё ссылку
                else
                    currentContainer = _cs.StatsList.FindLast(s => s.Name == stat);

                // если это стата-группа (например Constitution),
                // то сохраняем ссылку на неё в отдельной переменной group
                if (((int)stat & 0x0f) == 0x01)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(stat.ToString(), EditorStyles.boldLabel);
                    group = currentContainer;
                    group.Value = EditorGUILayout.IntField(stat.ToString(), group.Value);
                    continue;
                }

                // для зависимых статов (для Constitution это Health и т.д.)
                // устанавлваем соответствующую связь
                // все связи описаны в BaseStatsDependency
                //currentContainer.Value = EditorGUILayout.IntField(stat.ToString(), group.Value * 10);
                currentContainer.Value = EditorGUILayout.IntField(stat.ToString(),
                                        BaseStatsDependency.ParentToChildCalculation(group, stat));
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(_cs);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace BeastHunter
{

    [CustomEditor(typeof(LocationData))]
    //[CanEditMultipleObjects]
    public class LocationDataEditor : Editor
    {
        private const float MINI_BUTTON_WIDTH = 25.0f;

        private LocationData _script = null;

        private bool _playerFold = false;
        private SerializedProperty _playerPosition;

        private bool _bossFold = false;
        private SerializedProperty _bossPosition;

        private bool _enemiesFold = false;
        private SerializedProperty _enemySpawnPoints;

        private static GUIContent addButtonContent = new GUIContent("+", "Add");
        private static GUIContent deleteButtonContent = new GUIContent("-", "Delete");

        ReorderableList list;

        private void OnEnable()
        {
            _script = (LocationData)target;
            _playerPosition = serializedObject.FindProperty("_playerSpawnPosition");
            _bossPosition = serializedObject.FindProperty("_bossSpawnPosition");
            _enemySpawnPoints = serializedObject.FindProperty("_enemySpawnPointData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField(_script.name, EditorStyles.boldLabel);

            var clicked = GUILayout.Button("Scan Scene", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            if (clicked)
            {
                ScanLocation();
            }
            EditorGUILayout.Space();

            _playerFold = EditorGUILayout.Foldout(_playerFold, "Player Spawn Point");
            if (_playerFold)
            {
                _playerPosition.vector3Value = EditorGUILayout.Vector3Field("Position:", _playerPosition.vector3Value);
            }
            EditorGUILayout.Space();

            _bossFold = EditorGUILayout.Foldout(_bossFold, "Boss Spawn Point");
            if (_bossFold)
            {
                _bossPosition.vector3Value = EditorGUILayout.Vector3Field("Position:", _bossPosition.vector3Value);
            }
            EditorGUILayout.Space();

            _enemiesFold = EditorGUILayout.Foldout(_enemiesFold, "Enemies Spawn Points");
            if (_enemiesFold)
            {
                ShowList(_enemySpawnPoints);
                EditorGUI.indentLevel += 1;
                if (GUILayout.Button(addButtonContent, EditorStyles.miniButton))
                {
                    _script.AddEnemySpawnPointData();
                }
                EditorGUI.indentLevel -= 1;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowList(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;
            if (list.isExpanded)
            {
                for (int i = 0; i < list.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    var property = list.GetArrayElementAtIndex(i);
                    property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, $"Spawn Point {i + 1}");
                    var clicked = GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, GUILayout.Width(MINI_BUTTON_WIDTH));//duplicate?

                    EditorGUILayout.EndHorizontal();

                    if (property.isExpanded)
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                    if (clicked)
                    {
                        int oldSize = list.arraySize;
                        list.DeleteArrayElementAtIndex(i);
                        if (list.arraySize == oldSize)
                        {
                            _script.AddEnemySpawnPointData();
                            list.DeleteArrayElementAtIndex(i);
                        }
                    }
                }
            }
            EditorGUI.indentLevel -= 1;
        }

        private void ScanLocation()
        {
            int playerPos = -1;
            int bossPos = -1;
            var spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
            
            if (spawnPoints != null)
            {
                _enemySpawnPoints.ClearArray();
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (spawnPoints[i].name == "PlayerSpawnpoint")
                    {
                        playerPos = i;
                        _playerPosition.vector3Value = spawnPoints[playerPos].transform.position;
                    }
                    else if (spawnPoints[i].name == "BossSpawnpoint")
                    {
                        bossPos = i;
                        _bossPosition.vector3Value = spawnPoints[bossPos].transform.position;
                    }
                    else
                    {
                        var spawnPointScript = spawnPoints[i].GetComponent<SpawnPointScript>();
                        if (spawnPointScript != null)
                        {
                            _script.AddEnemySpawnPointData(spawnPointScript.spawnEntities, spawnPoints[i].transform.position,
                                spawnPointScript.spawnRadius, spawnPointScript.numberToSpawn);
                        }
                    }
                }
                if (playerPos == -1)
                {
                    Debug.Log("No object named 'PlayerSpawnpoint' found on scene");
                }

                if (bossPos == -1)
                {
                    Debug.Log("No object named 'BossSpawnpoint' found on scene");
                }
            }
            
        }
    }
}


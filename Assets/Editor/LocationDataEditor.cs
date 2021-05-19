using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace BeastHunter
{
    [CustomEditor(typeof(LocationData))]
    public class LocationDataEditor : Editor
    {
        #region Constants

        private const float MINI_BUTTON_WIDTH = 25.0f;

        #endregion


        #region Fields

        private LocationData _script = null;

        private SerializedProperty _playerPositions;
        private SerializedProperty _bossPositions;
        private SerializedProperty _enemySpawnpoints;
        //private ReorderableList _enemySpawnpointsList;
        private SerializedProperty _interactiveObjectSpawnpoints;
        private SerializedProperty _doRandomizePlayerPosition;
        private SerializedProperty _doRandomizeBossPosition;

        private bool spawnpointExpanded = true;

        #endregion


        #region Methods

        private void OnEnable()
        {
            _script = (LocationData)target;

            _playerPositions = serializedObject.FindProperty(LocationData.PLAYER_SPAWN_POSITIONS_FIELD_NAME);
            _bossPositions = serializedObject.FindProperty(LocationData.BOSS_SPAWN_POSITIONS_FIELD_NAME);
            _enemySpawnpoints = serializedObject.FindProperty(LocationData.ENEMY_SWAPN_POINT_DATA_FIELD_NAME);
            //_enemySpawnpointsList = new ReorderableList(serializedObject, _enemySpawnpoints, true, false, true, true);
            _interactiveObjectSpawnpoints = serializedObject.FindProperty(LocationData.INTERACTIVE_OBJECTS_SPAWN_DATA_FIELD_NAME);
            _doRandomizePlayerPosition = serializedObject.FindProperty(LocationData.DO_RANDOMIZE_PLAYER_POSITION_FIELD_NAME);
            _doRandomizeBossPosition = serializedObject.FindProperty(LocationData.DO_RANDOMIZE_BOSS_POSITION_FIELD_NAME);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField(_script.name, EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            var clickedScanAndSavePoints = GUILayout.Button("Scan and save points", 
                GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            var clickedPlacePoints = GUILayout.Button("Place points on scene", 
                GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (clickedScanAndSavePoints) ScanAndSavePoints();
            if (clickedPlacePoints) PlacePointsOnScene();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_doRandomizePlayerPosition, new GUIContent("Random Player Position"), true);
            EditorGUILayout.PropertyField(_doRandomizeBossPosition, new GUIContent("Random Boss Position"), true);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_playerPositions, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_bossPositions, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_enemySpawnpoints, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_interactiveObjectSpawnpoints, true);

            serializedObject.ApplyModifiedProperties();
        }

        private void ScanAndSavePoints()
        {
            GameObject[] foundSpawnpoints = GameObject.FindGameObjectsWithTag(TagManager.SPAWNPOINT);
            
            if(foundSpawnpoints.Length > 0)
            {
                _playerPositions.ClearArray();
                _bossPositions.ClearArray();
                _enemySpawnpoints.ClearArray();
                _interactiveObjectSpawnpoints.ClearArray();
                _script.ClearLists();

                foreach (var spawnpoint in foundSpawnpoints)
                {
                    if (spawnpoint.name.Contains(LocationData.PLAYER_SPAWNPOINT_NAME))
                    {
                        _script.AddPlayerPosition(new LocationPosition(spawnpoint.transform));
                    }
                    else if(spawnpoint.name.Contains(LocationData.BOSS_SPAWNPOINT_NAME))
                    {
                        _script.AddBossPosition(new LocationPosition(spawnpoint.transform));
                    }
                    else if(spawnpoint.TryGetComponent(out SpawnPointScript foundSpawnpointScript))
                    {
                        _script.AddEnemySpawnPointData(foundSpawnpointScript.spawnEntities, 
                            new LocationPosition(spawnpoint.transform), foundSpawnpointScript.spawnRadius, 
                                foundSpawnpointScript.numberToSpawn);
                    }
                    else if(spawnpoint.TryGetComponent(out InteractiveObjectLocationInfo foundObjectLocationInfo))
                    {
                        _script.AddInteractiveObjectData(new SpawnInteractiveObjectData(
                            new LocationPosition(spawnpoint.transform), foundObjectLocationInfo.InteractiveObjectType));
                    }
                    else
                    {
                        throw new System.Exception("Found not recognized spawnpoint");
                    }

                    DestroyImmediate(spawnpoint);
                }

                serializedObject.Update();
            }
            else
            {
                ShowPopupMessage.Init("There is no spawnpoints on scene!");
            }
        }

        private void PlacePointsOnScene()
        {
            for (int i = 0; i < _script.PlayerSpawnPositions.Count; i++)
            {
                _script.PlaceObjectOnScene(GameObject.
                    Instantiate(Data.SpawnpointsData.PlayerSpawnpointPrefab), _script.PlayerSpawnPositions[i]);
            }

            for (int i = 0; i < _script.BossSpawnPositions.Count; i++)
            {
                _script.PlaceObjectOnScene(GameObject.
                    Instantiate(Data.SpawnpointsData.BossSpawnpointPrefab), _script.BossSpawnPositions[i]);
            }

            for (int i = 0; i < _script.EnemySpawnPointData.Count; i++)
            {
                GameObject newEnemySpawner = GameObject.Instantiate(Data.SpawnpointsData.EnemySpawnpointPrefab);
                _script.PlaceObjectOnScene(newEnemySpawner, _script.EnemySpawnPointData[i].SpawnPosition);
                SpawnPointScript newEnemySpawnerScript = newEnemySpawner.GetComponent<SpawnPointScript>();
                newEnemySpawnerScript.spawnEntities = _script.EnemySpawnPointData[i].SpawnDataList;
                newEnemySpawnerScript.spawnRadius = _script.EnemySpawnPointData[i].SpawnRadius;
                newEnemySpawnerScript.numberToSpawn = _script.EnemySpawnPointData[i].NumberToSpawn;
            }

            for (int i = 0; i < _script.InteractiveObjectSpawnData.Count; i++)
            {
                GameObject newInteractiveObjectSpawnpoint = GameObject.Instantiate(Data.SpawnpointsData.
                    InteractiveObjectSpawnpointPrefab);
                _script.PlaceObjectOnScene(newInteractiveObjectSpawnpoint,
                    _script.InteractiveObjectSpawnData[i].ObjectPosition);
                newInteractiveObjectSpawnpoint.GetComponent<InteractiveObjectLocationInfo>().
                    InteractiveObjectType = _script.InteractiveObjectSpawnData[i].Type;
            }
        }

        #endregion
    }
}


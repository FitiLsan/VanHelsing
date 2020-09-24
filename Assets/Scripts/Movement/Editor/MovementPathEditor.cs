using UnityEditor;
using UnityEngine;

namespace BeastHunter
{
    [CustomEditor(typeof(MovementPath))]
    public class MovementPathEditor : Editor
    {
        #region Constants

        private const float HANDLE_LENGHT = 0.1f;

        #endregion
        
        
        #region Fields

        private MovementPath _path;
        private LineRenderer _lineRenderer;
        private SerializedProperty _resolutionProp;
        private SerializedProperty _loopProp;
        private SerializedProperty _stepsProp;
        private SerializedProperty _colorProp;
        private static bool _showSteps = true;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _path = (MovementPath) target;
            _resolutionProp = serializedObject.FindProperty("_resolution");
            _loopProp = serializedObject.FindProperty("_loop");
            _stepsProp = serializedObject.FindProperty("_steps");
            _colorProp = serializedObject.FindProperty("_drawColor");
            _lineRenderer = _path.gameObject.GetComponent<LineRenderer>();
            
            if (_lineRenderer == null)
            {
                _path.gameObject.AddComponent<LineRenderer>();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            if (GUILayout.Button("Update LineRenderer"))
            {
                UpdateLineRenderer();
            }

            var newResolution = EditorGUILayout.IntField("Resolution", _resolutionProp.intValue);
            _resolutionProp.intValue = newResolution < 1 ? 1 : newResolution;

            EditorGUILayout.PropertyField(_loopProp);
            EditorGUILayout.PropertyField(_colorProp);

            _showSteps = EditorGUILayout.Foldout(_showSteps, "Steps");

            if (_showSteps)
            {
                var stepCount = _stepsProp.arraySize;

                for (var i = 0; i < stepCount; i++) DrawStepInspector(_path[i], i);

                if (GUILayout.Button("Add Step"))
                {
                    Undo.RegisterSceneUndo("Add Step");

                    var stepObject = new GameObject("Step " + _stepsProp.arraySize);
                    stepObject.transform.parent = _path.transform;
                    stepObject.transform.localPosition = Vector3.zero;
                    var newStep = stepObject.AddComponent<MovementStep>();

                    newStep.HandleStyle = MovementHandleType.Connected;
                    newStep.Path = _path;
                    newStep.HandleA = Vector3.right * HANDLE_LENGHT;
                    newStep.HandleB = -Vector3.right * HANDLE_LENGHT;

                    _stepsProp.InsertArrayElementAtIndex(_stepsProp.arraySize);
                    _stepsProp.GetArrayElementAtIndex(_stepsProp.arraySize - 1).objectReferenceValue = newStep;
                    
                    UpdateLineRenderer();
                }
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                UpdateLineRenderer();
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < _path.StepCount; i++) DrawStepSceneGUI(_path[i]);
        }

        #endregion


        #region Methods

        private void UpdateLineRenderer()
        {
            var steps = _path.GetPoints();

            _lineRenderer.positionCount = steps.Count;
            _lineRenderer.loop = _path.Loop;

            for (var i = 0; i < steps.Count; i++)
            {
                _lineRenderer.SetPosition(i, steps[i].Position);
            }
        }
        
        private void DrawStepInspector(MovementStep step, int index)
        {
            if (step == null) return;

            var serObj = new SerializedObject(step);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("✕", GUILayout.Width(20)))
            {
                Undo.RegisterSceneUndo("Remove Step");
                _stepsProp.MoveArrayElement(_path.GetStepIndex(step), _path.StepCount - 1);
                _stepsProp.arraySize--;
                _path.RemoveStep(step);
                DestroyImmediate(step.gameObject);
                UpdateLineRenderer();
                return;
            }

            EditorGUILayout.ObjectField(step.gameObject, typeof(GameObject), true);

            if (index != 0 && GUILayout.Button("↑", GUILayout.Width(25)))
            {
                var other = _stepsProp.GetArrayElementAtIndex(index - 1).objectReferenceValue;
                _stepsProp.GetArrayElementAtIndex(index - 1).objectReferenceValue = step;
                _stepsProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
            }

            if (index != _stepsProp.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(25)))
            {
                var other = _stepsProp.GetArrayElementAtIndex(index + 1).objectReferenceValue;
                _stepsProp.GetArrayElementAtIndex(index + 1).objectReferenceValue = step;
                _stepsProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            step.IsGrounded = EditorGUILayout.Toggle("Is Grounded", step.IsGrounded);
            step.WaitingTime = EditorGUILayout.FloatField("Waiting Time", step.WaitingTime);
            step.AnimationState = EditorGUILayout.TextField("Animation State", step.AnimationState);

            var newMovementHandleType = (MovementHandleType) EditorGUILayout.EnumPopup("Handle Type", step.HandleStyle);

            if (newMovementHandleType != step.HandleStyle)
            {
                step.HandleStyle = newMovementHandleType;

                switch (newMovementHandleType)
                {
                    case MovementHandleType.Connected:
                        if (step.HandleA != Vector3.zero)
                        {
                            step.HandleB = -step.HandleA;
                        }
                        else if (step.HandleB != Vector3.zero)
                        {
                            step.HandleA = -step.HandleB;
                        }
                        else
                        {
                            step.HandleA = new Vector3(HANDLE_LENGHT, 0, 0);
                            step.HandleB = new Vector3(-HANDLE_LENGHT, 0, 0);
                        }

                        break;
                    case MovementHandleType.Broken:
                        if (step.HandleA == Vector3.zero && step.HandleB == Vector3.zero)
                        {
                            step.HandleA = new Vector3(HANDLE_LENGHT, 0, 0);
                            step.HandleB = new Vector3(-HANDLE_LENGHT, 0, 0);
                        }

                        break;
                    case MovementHandleType.None:
                    default:
                        step.HandleA = Vector3.zero;
                        step.HandleB = Vector3.zero;
                        break;
                }
            }

            var newStepPos = EditorGUILayout.Vector3Field("Position", step.LocalPosition);
            if (newStepPos != step.LocalPosition)
            {
                Undo.RegisterCompleteObjectUndo(step.transform, "Move Path Step");
                step.LocalPosition = newStepPos;
            }

            if (step.HandleStyle != MovementHandleType.None)
            {
                var newHandleA = EditorGUILayout.Vector3Field("Handle A", step.HandleA);
                var newHandleB = EditorGUILayout.Vector3Field("Handle B", step.HandleB);

                if (step.HandleStyle != MovementHandleType.Connected)
                {
                    if (newHandleA != step.HandleA)
                    {
                        step.HandleA = newHandleA;
                        step.HandleB = -newHandleA;
                    }

                    else if (newHandleB != step.HandleB)
                    {
                        step.HandleA = -newHandleB;
                        step.HandleB = newHandleB;
                    }
                }
                else
                {
                    step.HandleA = newHandleA;
                    step.HandleB = newHandleB;
                }
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            if (GUI.changed)
            {
                serObj.ApplyModifiedProperties();
                EditorUtility.SetDirty(serObj.targetObject);
                UpdateLineRenderer();
            }
        }

        private static void DrawStepSceneGUI(MovementStep step)
        {
            if (step == null) return;
            
            Handles.Label(step.Position + new Vector3(0, HandleUtility.GetHandleSize(step.Position) * 0.4f, 0),
                step.gameObject.name);

            Handles.color = Color.green;
            var newPosition = Handles.FreeMoveHandle(step.Position, step.Rotation,
                HandleUtility.GetHandleSize(step.Position) * HANDLE_LENGHT, Vector3.zero, Handles.RectangleHandleCap);

            if (newPosition != step.Position)
            {
                Undo.RegisterCompleteObjectUndo(step.transform, "Move Step");
                step.Position = newPosition;
            }

            if (step.HandleStyle != MovementHandleType.None)
            {
                Handles.color = Color.cyan;
                var newGlobalA = Handles.FreeMoveHandle(step.GlobalHandleA, step.Rotation,
                    HandleUtility.GetHandleSize(step.GlobalHandleA) * HANDLE_LENGHT, Vector3.zero, Handles.CircleHandleCap);
                if (step.GlobalHandleA != newGlobalA)
                {
                    Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                    step.GlobalHandleA = newGlobalA;
                    if (step.HandleStyle == MovementHandleType.Connected)
                        step.GlobalHandleB = -(newGlobalA - step.Position) + step.Position;
                }

                var newGlobalB = Handles.FreeMoveHandle(step.GlobalHandleB, step.Rotation,
                    HandleUtility.GetHandleSize(step.GlobalHandleB) * HANDLE_LENGHT, Vector3.zero, Handles.CircleHandleCap);
                if (step.GlobalHandleB != newGlobalB)
                {
                    Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                    step.GlobalHandleB = newGlobalB;
                    if (step.HandleStyle == MovementHandleType.Connected)
                        step.GlobalHandleA = -(newGlobalB - step.Position) + step.Position;
                }

                Handles.color = Color.yellow;
                Handles.DrawLine(step.Position, step.GlobalHandleA);
                Handles.DrawLine(step.Position, step.GlobalHandleB);
            }
        }

        public static void DrawOtherSteps(MovementPath path, MovementStep caller)
        {
            foreach (var p in path.GetAnchorSteps())
                if (p != caller)
                    DrawStepSceneGUI(p);
        }

        [MenuItem("GameObject/Create Other/Movement Path")]
        public static void CreatePath(MenuCommand command)
        {
            var pathObject = new GameObject("MovementPath");
            Undo.RegisterCompleteObjectUndo(pathObject, "Undo Create Path");
            var path = pathObject.AddComponent<MovementPath>();
            var lineRenderer = pathObject.AddComponent<LineRenderer>(); 

            var p1 = path.AddStepAt(Vector3.forward * 0.5f);
            p1.HandleStyle = MovementHandleType.Connected;
            p1.HandleA = new Vector3(-0.28f, 0, 0);

            var p2 = path.AddStepAt(Vector3.right * 0.5f);
            p2.HandleStyle = MovementHandleType.Connected;
            p2.HandleA = new Vector3(0, 0, 0.28f);

            var p3 = path.AddStepAt(-Vector3.forward * 0.5f);
            p3.HandleStyle = MovementHandleType.Connected;
            p3.HandleA = new Vector3(0.28f, 0, 0);

            var p4 = path.AddStepAt(-Vector3.right * 0.5f);
            p4.HandleStyle = MovementHandleType.Connected;
            p4.HandleA = new Vector3(0, 0, -0.28f);

            path.Loop = true;
        }

        #endregion
    }
}
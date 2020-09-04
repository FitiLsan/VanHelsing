using UnityEditor;
using UnityEngine;

namespace BeastHunter
{
    [CustomEditor(typeof(MovementPath))]
    public class MovementPathEditor : Editor
    {
        #region Fields

        private MovementPath _path;
        private SerializedProperty _resolutionProp;
        private SerializedProperty _loopProp;
        private SerializedProperty _pointsProp;
        private SerializedProperty _colorProp;
        private static bool _showPoints = true;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _path = (MovementPath) target;
            _resolutionProp = serializedObject.FindProperty("_resolution");
            _loopProp = serializedObject.FindProperty("_loop");
            _pointsProp = serializedObject.FindProperty("_points");
            _colorProp = serializedObject.FindProperty("_drawColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var newResolution = EditorGUILayout.IntField("Resolution", _resolutionProp.intValue);
            _resolutionProp.intValue = newResolution < 1 ? 1 : newResolution;

            EditorGUILayout.PropertyField(_loopProp);
            EditorGUILayout.PropertyField(_colorProp);

            _showPoints = EditorGUILayout.Foldout(_showPoints, "Points");

            if (_showPoints)
            {
                var pointCount = _pointsProp.arraySize;

                for (var i = 0; i < pointCount; i++) DrawPointInspector(_path[i], i);

                if (GUILayout.Button("Add Point"))
                {
                    Undo.RegisterSceneUndo("Add Point");

                    var pointObject = new GameObject("Point " + _pointsProp.arraySize);
                    pointObject.transform.parent = _path.transform;
                    pointObject.transform.localPosition = Vector3.zero;
                    var newPoint = pointObject.AddComponent<MovementPoint>();

                    newPoint.Path = _path;
                    newPoint.HandleA = Vector3.right * 0.1f;
                    newPoint.HandleB = -Vector3.right * 0.1f;

                    _pointsProp.InsertArrayElementAtIndex(_pointsProp.arraySize);
                    _pointsProp.GetArrayElementAtIndex(_pointsProp.arraySize - 1).objectReferenceValue = newPoint;
                }
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < _path.PointCount; i++) DrawPointSceneGUI(_path[i]);
        }

        #endregion


        #region Methods

        private void DrawPointInspector(MovementPoint point, int index)
        {
            var serObj = new SerializedObject(point);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("✕", GUILayout.Width(20)))
            {
                Undo.RegisterSceneUndo("Remove Point");
                _pointsProp.MoveArrayElement(_path.GetPointIndex(point), _path.PointCount - 1);
                _pointsProp.arraySize--;
                DestroyImmediate(point.gameObject);
                return;
            }

            EditorGUILayout.ObjectField(point.gameObject, typeof(GameObject), true);

            if (index != 0 && GUILayout.Button("↑", GUILayout.Width(25)))
            {
                var other = _pointsProp.GetArrayElementAtIndex(index - 1).objectReferenceValue;
                _pointsProp.GetArrayElementAtIndex(index - 1).objectReferenceValue = point;
                _pointsProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
            }

            if (index != _pointsProp.arraySize - 1 && GUILayout.Button("↓", GUILayout.Width(25)))
            {
                var other = _pointsProp.GetArrayElementAtIndex(index + 1).objectReferenceValue;
                _pointsProp.GetArrayElementAtIndex(index + 1).objectReferenceValue = point;
                _pointsProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            point.IsGrounded = EditorGUILayout.Toggle("Is Grounded", point.IsGrounded);
            point.WaitingTime = EditorGUILayout.FloatField("Waiting Time", point.WaitingTime);

            var newHandleType = (MovementPoint.HandleType) EditorGUILayout.EnumPopup("Handle Type", point.HandleStyle);

            if (newHandleType != point.HandleStyle)
            {
                point.HandleStyle = newHandleType;

                switch (newHandleType)
                {
                    case MovementPoint.HandleType.Connected:
                        if (point.HandleA != Vector3.zero)
                        {
                            point.HandleB = -point.HandleA;
                        }
                        else if (point.HandleB != Vector3.zero)
                        {
                            point.HandleA = -point.HandleB;
                        }
                        else
                        {
                            point.HandleA = new Vector3(0.1f, 0, 0);
                            point.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    case MovementPoint.HandleType.Broken:
                        if (point.HandleA == Vector3.zero && point.HandleB == Vector3.zero)
                        {
                            point.HandleA = new Vector3(0.1f, 0, 0);
                            point.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    default:
                    case MovementPoint.HandleType.None:
                        point.HandleA = Vector3.zero;
                        point.HandleB = Vector3.zero;
                        break;
                }
            }

            var newPointPos = EditorGUILayout.Vector3Field("Position", point.LocalPosition);
            if (newPointPos != point.LocalPosition)
            {
                Undo.RegisterCompleteObjectUndo(point.transform, "Move Path Point");
                point.LocalPosition = newPointPos;
            }

            if (point.HandleStyle != MovementPoint.HandleType.None)
            {
                var newHandleA = EditorGUILayout.Vector3Field("Handle A", point.HandleA);
                var newHandleB = EditorGUILayout.Vector3Field("Handle B", point.HandleB);

                if (point.HandleStyle != MovementPoint.HandleType.Connected)
                {
                    if (newHandleA != point.HandleA)
                    {
                        point.HandleA = newHandleA;
                        point.HandleB = -newHandleA;
                    }

                    else if (newHandleB != point.HandleB)
                    {
                        point.HandleA = -newHandleB;
                        point.HandleB = newHandleB;
                    }
                }
                else
                {
                    point.HandleA = newHandleA;
                    point.HandleB = newHandleB;
                }
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            if (GUI.changed)
            {
                serObj.ApplyModifiedProperties();
                EditorUtility.SetDirty(serObj.targetObject);
            }
        }

        private static void DrawPointSceneGUI(MovementPoint point)
        {
            Handles.Label(point.Position + new Vector3(0, HandleUtility.GetHandleSize(point.Position) * 0.4f, 0),
                point.gameObject.name);

            Handles.color = Color.green;
            var newPosition = Handles.FreeMoveHandle(point.Position, point.Rotation,
                HandleUtility.GetHandleSize(point.Position) * 0.1f, Vector3.zero, Handles.RectangleHandleCap);

            if (newPosition != point.Position)
            {
                Undo.RegisterCompleteObjectUndo(point.transform, "Move Point");
                point.Position = newPosition;
            }

            if (point.HandleStyle != MovementPoint.HandleType.None)
            {
                Handles.color = Color.cyan;
                var newGlobal1 = Handles.FreeMoveHandle(point.GlobalHandleA, point.Rotation,
                    HandleUtility.GetHandleSize(point.GlobalHandleA) * 0.075f, Vector3.zero, Handles.CircleHandleCap);
                if (point.GlobalHandleA != newGlobal1)
                {
                    Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                    point.GlobalHandleA = newGlobal1;
                    if (point.HandleStyle == MovementPoint.HandleType.Connected)
                        point.GlobalHandleB = -(newGlobal1 - point.Position) + point.Position;
                }

                var newGlobal2 = Handles.FreeMoveHandle(point.GlobalHandleB, point.Rotation,
                    HandleUtility.GetHandleSize(point.GlobalHandleB) * 0.075f, Vector3.zero, Handles.CircleHandleCap);
                if (point.GlobalHandleB != newGlobal2)
                {
                    Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                    point.GlobalHandleB = newGlobal2;
                    if (point.HandleStyle == MovementPoint.HandleType.Connected)
                        point.GlobalHandleA = -(newGlobal2 - point.Position) + point.Position;
                }

                Handles.color = Color.yellow;
                Handles.DrawLine(point.Position, point.GlobalHandleA);
                Handles.DrawLine(point.Position, point.GlobalHandleB);
            }
        }

        public static void DrawOtherPoints(MovementPath path, MovementPoint caller)
        {
            foreach (var p in path.GetAnchorPoints())
                if (p != caller)
                    DrawPointSceneGUI(p);
        }

        [MenuItem("GameObject/Create Other/Movement Path")]
        public static void CreatePath(MenuCommand command)
        {
            var pathObject = new GameObject("MovementPath");
            Undo.RegisterCompleteObjectUndo(pathObject, "Undo Create Path");
            var path = pathObject.AddComponent<MovementPath>();

            var p1 = path.AddPointAt(Vector3.forward * 0.5f);
            p1.HandleStyle = MovementPoint.HandleType.Connected;
            p1.HandleA = new Vector3(-0.28f, 0, 0);

            var p2 = path.AddPointAt(Vector3.right * 0.5f);
            p2.HandleStyle = MovementPoint.HandleType.Connected;
            p2.HandleA = new Vector3(0, 0, 0.28f);

            var p3 = path.AddPointAt(-Vector3.forward * 0.5f);
            p3.HandleStyle = MovementPoint.HandleType.Connected;
            p3.HandleA = new Vector3(0.28f, 0, 0);

            var p4 = path.AddPointAt(-Vector3.right * 0.5f);
            p4.HandleStyle = MovementPoint.HandleType.Connected;
            p4.HandleA = new Vector3(0, 0, -0.28f);

            path.Loop = true;
        }

        #endregion
    }
}
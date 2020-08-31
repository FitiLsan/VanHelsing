using UnityEditor;
using UnityEngine;


namespace BeastHunter
{
    [CustomEditor(typeof(MovementPoint))]
    [CanEditMultipleObjects]
    public class MovementPointEditor : Editor
    {
        #region PrivateData

        private delegate void HandleFunction(MovementPoint point);

        private readonly HandleFunction[] _handlers = {HandleConnected, HandleBroken, HandleAbsent};

        #endregion


        #region Fields

        private MovementPoint _point;
        private SerializedProperty _handleTypeProp;
        private SerializedProperty _handleAProp;
        private SerializedProperty _handleBProp;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _point = (MovementPoint) target;

            _handleTypeProp = serializedObject.FindProperty("_handleStyle");
            _handleAProp = serializedObject.FindProperty("_handleA");
            _handleBProp = serializedObject.FindProperty("_handleB");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var newHandleType = (MovementPoint.HandleType) EditorGUILayout.EnumPopup("Handle Type",
                (MovementPoint.HandleType) _handleTypeProp.intValue);

            if (newHandleType != (MovementPoint.HandleType) _handleTypeProp.intValue)
            {
                _handleTypeProp.intValue = (int) newHandleType;

                if ((int) newHandleType == 0)
                {
                    if (_handleAProp.vector3Value != Vector3.zero)
                    {
                        _handleBProp.vector3Value = -_handleAProp.vector3Value;
                    }
                    else if (_handleBProp.vector3Value != Vector3.zero)
                    {
                        _handleAProp.vector3Value = -_handleBProp.vector3Value;
                    }
                    else
                    {
                        _handleAProp.vector3Value = new Vector3(0.1f, 0, 0);
                        _handleBProp.vector3Value = new Vector3(-0.1f, 0, 0);
                    }
                }

                else if ((int) newHandleType == 1)
                {
                    if (_handleAProp.vector3Value == Vector3.zero && _handleBProp.vector3Value == Vector3.zero)
                    {
                        _handleAProp.vector3Value = new Vector3(0.1f, 0, 0);
                        _handleBProp.vector3Value = new Vector3(-0.1f, 0, 0);
                    }
                }

                else if ((int) newHandleType == 2)
                {
                    _handleAProp.vector3Value = Vector3.zero;
                    _handleBProp.vector3Value = Vector3.zero;
                }
            }

            if (_handleTypeProp.intValue != 2)
            {
                var newHandleA = EditorGUILayout.Vector3Field("Handle A", _handleAProp.vector3Value);
                var newHandleB = EditorGUILayout.Vector3Field("Handle B", _handleBProp.vector3Value);

                if (_handleTypeProp.intValue == 0)
                {
                    if (newHandleA != _handleAProp.vector3Value)
                    {
                        _handleAProp.vector3Value = newHandleA;
                        _handleBProp.vector3Value = -newHandleA;
                    }

                    else if (newHandleB != _handleBProp.vector3Value)
                    {
                        _handleAProp.vector3Value = -newHandleB;
                        _handleBProp.vector3Value = newHandleB;
                    }
                }

                else
                {
                    _handleAProp.vector3Value = newHandleA;
                    _handleBProp.vector3Value = newHandleB;
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
            Handles.color = Color.green;
            var newPosition = Handles.FreeMoveHandle(_point.Position, _point.transform.rotation,
                HandleUtility.GetHandleSize(_point.Position) * 0.2f, Vector3.zero, Handles.CubeHandleCap);

            if (_point.Position != newPosition) _point.Position = newPosition;

            _handlers[(int) _point.HandleStyle](_point);

            Handles.color = Color.yellow;
            Handles.DrawLine(_point.Position, _point.GlobalHandleA);
            Handles.DrawLine(_point.Position, _point.GlobalHandleB);

            MovementPathEditor.DrawOtherPoints(_point.Path, _point);
        }

        #endregion


        #region Methods

        private static void HandleConnected(MovementPoint p)
        {
            Handles.color = Color.cyan;

            var newGlobalA = Handles.FreeMoveHandle(p.GlobalHandleA, p.transform.rotation,
                HandleUtility.GetHandleSize(p.GlobalHandleA) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalA != p.GlobalHandleA)
            {
                Undo.RegisterCompleteObjectUndo(p, "Move Handle");
                p.GlobalHandleA = newGlobalA;
                p.GlobalHandleB = -(newGlobalA - p.Position) + p.Position;
            }

            var newGlobalB = Handles.FreeMoveHandle(p.GlobalHandleB, p.transform.rotation,
                HandleUtility.GetHandleSize(p.GlobalHandleB) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalB != p.GlobalHandleB)
            {
                Undo.RegisterCompleteObjectUndo(p, "Move Handle");
                p.GlobalHandleA = -(newGlobalB - p.Position) + p.Position;
                p.GlobalHandleB = newGlobalB;
            }
        }

        private static void HandleBroken(MovementPoint point)
        {
            Handles.color = Color.cyan;

            var newGlobalA = Handles.FreeMoveHandle(point.GlobalHandleA, Quaternion.identity,
                HandleUtility.GetHandleSize(point.GlobalHandleA) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
            var newGlobalB = Handles.FreeMoveHandle(point.GlobalHandleB, Quaternion.identity,
                HandleUtility.GetHandleSize(point.GlobalHandleB) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalA != point.GlobalHandleA)
            {
                Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                point.GlobalHandleA = newGlobalA;
            }

            if (newGlobalB != point.GlobalHandleB)
            {
                Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                point.GlobalHandleB = newGlobalB;
            }
        }

        private static void HandleAbsent(MovementPoint point)
        {
            point.HandleA = Vector3.zero;
            point.HandleB = Vector3.zero;
        }

        #endregion
    }
}
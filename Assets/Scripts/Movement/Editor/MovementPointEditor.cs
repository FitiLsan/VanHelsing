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

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _point = (MovementPoint) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _point.IsGrounded = EditorGUILayout.Toggle("Is Grounded", _point.IsGrounded);
            _point.WaitingTime = EditorGUILayout.FloatField("Waiting Time", _point.WaitingTime);
            _point.AnimationState = EditorGUILayout.TextField("Animation State", _point.AnimationState);

            var newHandleType = (MovementPoint.HandleType) EditorGUILayout.EnumPopup("Handle Type", _point.HandleStyle);

            if (newHandleType != _point.HandleStyle)
            {
                _point.HandleStyle = newHandleType;

                switch (newHandleType)
                {
                    case MovementPoint.HandleType.Connected:
                        if (_point.HandleA != Vector3.zero)
                        {
                            _point.HandleB = -_point.HandleA;
                        }
                        else if (_point.HandleB != Vector3.zero)
                        {
                            _point.HandleA = -_point.HandleB;
                        }
                        else
                        {
                            _point.HandleA = new Vector3(0.1f, 0, 0);
                            _point.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    case MovementPoint.HandleType.Broken:
                        if (_point.HandleA == Vector3.zero && _point.HandleB == Vector3.zero)
                        {
                            _point.HandleA = new Vector3(0.1f, 0, 0);
                            _point.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    default:
                    case MovementPoint.HandleType.None:
                        _point.HandleA = Vector3.zero;
                        _point.HandleB = Vector3.zero;
                        break;
                }
            }

            if (_point.HandleStyle != MovementPoint.HandleType.None)
            {
                var newHandleA = EditorGUILayout.Vector3Field("Handle A", _point.HandleA);
                var newHandleB = EditorGUILayout.Vector3Field("Handle B", _point.HandleB);

                if (_point.HandleStyle != MovementPoint.HandleType.Connected)
                {
                    if (newHandleA != _point.HandleA)
                    {
                        _point.HandleA = newHandleA;
                        _point.HandleB = -newHandleA;
                    }

                    else if (newHandleB != _point.HandleB)
                    {
                        _point.HandleA = -newHandleB;
                        _point.HandleB = newHandleB;
                    }
                }
                else
                {
                    _point.HandleA = newHandleA;
                    _point.HandleB = newHandleB;
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
            var newPosition = Handles.FreeMoveHandle(_point.Position, _point.Rotation,
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

        private static void HandleConnected(MovementPoint point)
        {
            Handles.color = Color.cyan;

            var newGlobalA = Handles.FreeMoveHandle(point.GlobalHandleA, point.Rotation,
                HandleUtility.GetHandleSize(point.GlobalHandleA) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalA != point.GlobalHandleA)
            {
                Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                point.GlobalHandleA = newGlobalA;
                point.GlobalHandleB = -(newGlobalA - point.Position) + point.Position;
            }

            var newGlobalB = Handles.FreeMoveHandle(point.GlobalHandleB, point.Rotation,
                HandleUtility.GetHandleSize(point.GlobalHandleB) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalB != point.GlobalHandleB)
            {
                Undo.RegisterCompleteObjectUndo(point, "Move Handle");
                point.GlobalHandleA = -(newGlobalB - point.Position) + point.Position;
                point.GlobalHandleB = newGlobalB;
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
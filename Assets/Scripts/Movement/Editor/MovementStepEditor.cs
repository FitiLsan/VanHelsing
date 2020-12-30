using UnityEditor;
using UnityEngine;


namespace BeastHunter
{
    [CustomEditor(typeof(MovementStep))]
    [CanEditMultipleObjects]
    public class MovementStepEditor : Editor
    {
        #region PrivateData

        private delegate void HandleFunction(MovementStep step);

        private readonly HandleFunction[] _handlers = {HandleConnected, HandleBroken, HandleAbsent};

        #endregion


        #region Fields

        private MovementStep _step;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _step = (MovementStep) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _step.IsGrounded = EditorGUILayout.Toggle("Is Grounded", _step.IsGrounded);
            _step.WaitingTime = EditorGUILayout.FloatField("Waiting Time", _step.WaitingTime);
            _step.AnimationState = EditorGUILayout.TextField("Animation State", _step.AnimationState);

            var newMovementHandleType = (MovementHandleType) EditorGUILayout.EnumPopup("Handle Type", _step.HandleStyle);

            if (newMovementHandleType != _step.HandleStyle)
            {
                _step.HandleStyle = newMovementHandleType;

                switch (newMovementHandleType)
                {
                    case MovementHandleType.Connected:
                        if (_step.HandleA != Vector3.zero)
                        {
                            _step.HandleB = -_step.HandleA;
                        }
                        else if (_step.HandleB != Vector3.zero)
                        {
                            _step.HandleA = -_step.HandleB;
                        }
                        else
                        {
                            _step.HandleA = new Vector3(0.1f, 0, 0);
                            _step.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    case MovementHandleType.Broken:
                        if (_step.HandleA == Vector3.zero && _step.HandleB == Vector3.zero)
                        {
                            _step.HandleA = new Vector3(0.1f, 0, 0);
                            _step.HandleB = new Vector3(-0.1f, 0, 0);
                        }

                        break;
                    default:
                    case MovementHandleType.None:
                        _step.HandleA = Vector3.zero;
                        _step.HandleB = Vector3.zero;
                        break;
                }
            }

            if (_step.HandleStyle != MovementHandleType.None)
            {
                var newHandleA = EditorGUILayout.Vector3Field("Handle A", _step.HandleA);
                var newHandleB = EditorGUILayout.Vector3Field("Handle B", _step.HandleB);

                if (_step.HandleStyle != MovementHandleType.Connected)
                {
                    if (newHandleA != _step.HandleA)
                    {
                        _step.HandleA = newHandleA;
                        _step.HandleB = -newHandleA;
                    }

                    else if (newHandleB != _step.HandleB)
                    {
                        _step.HandleA = -newHandleB;
                        _step.HandleB = newHandleB;
                    }
                }
                else
                {
                    _step.HandleA = newHandleA;
                    _step.HandleB = newHandleB;
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
            var newPosition = Handles.FreeMoveHandle(_step.Position, _step.Rotation,
                HandleUtility.GetHandleSize(_step.Position) * 0.2f, Vector3.zero, Handles.CubeHandleCap);

            if (_step.Position != newPosition) _step.Position = newPosition;

            _handlers[(int) _step.HandleStyle](_step);

            Handles.color = Color.yellow;
            Handles.DrawLine(_step.Position, _step.GlobalHandleA);
            Handles.DrawLine(_step.Position, _step.GlobalHandleB);

            MovementPathEditor.DrawOtherSteps(_step.Path, _step);
        }

        #endregion


        #region Methods

        private static void HandleConnected(MovementStep step)
        {
            Handles.color = Color.cyan;

            var newGlobalA = Handles.FreeMoveHandle(step.GlobalHandleA, step.Rotation,
                HandleUtility.GetHandleSize(step.GlobalHandleA) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalA != step.GlobalHandleA)
            {
                Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                step.GlobalHandleA = newGlobalA;
                step.GlobalHandleB = -(newGlobalA - step.Position) + step.Position;
            }

            var newGlobalB = Handles.FreeMoveHandle(step.GlobalHandleB, step.Rotation,
                HandleUtility.GetHandleSize(step.GlobalHandleB) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalB != step.GlobalHandleB)
            {
                Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                step.GlobalHandleA = -(newGlobalB - step.Position) + step.Position;
                step.GlobalHandleB = newGlobalB;
            }
        }

        private static void HandleBroken(MovementStep step)
        {
            Handles.color = Color.cyan;

            var newGlobalA = Handles.FreeMoveHandle(step.GlobalHandleA, Quaternion.identity,
                HandleUtility.GetHandleSize(step.GlobalHandleA) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
            var newGlobalB = Handles.FreeMoveHandle(step.GlobalHandleB, Quaternion.identity,
                HandleUtility.GetHandleSize(step.GlobalHandleB) * 0.15f, Vector3.zero, Handles.SphereHandleCap);

            if (newGlobalA != step.GlobalHandleA)
            {
                Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                step.GlobalHandleA = newGlobalA;
            }

            if (newGlobalB != step.GlobalHandleB)
            {
                Undo.RegisterCompleteObjectUndo(step, "Move Handle");
                step.GlobalHandleB = newGlobalB;
            }
        }

        private static void HandleAbsent(MovementStep step)
        {
            step.HandleA = Vector3.zero;
            step.HandleB = Vector3.zero;
        }

        #endregion
    }
}
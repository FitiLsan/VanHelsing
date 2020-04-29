using UnityEngine;


[CreateAssetMenu(fileName = "CharacterData")]
public sealed class CharacterData : ScriptableObject
{
    #region PrivateData

    public CharacterStruct _characterStruct;

    #endregion


    #region Fields

    private Vector3 _movementVector;
    private float _currentAngleVelocity;

    #endregion


    #region Metods

    public void MoveForward(Transform prefabTransform, float moveSpeed)
    {
        _movementVector = Vector3.forward * moveSpeed * Time.deltaTime;
        prefabTransform.Translate(_movementVector, Space.Self);
    }

    public void Jump(Rigidbody rigitbody, float force)
    {
        rigitbody.velocity = Vector3.up * force;
    }

    #endregion
}

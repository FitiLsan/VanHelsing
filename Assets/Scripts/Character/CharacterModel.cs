using UnityEngine;
using Cinemachine;


public sealed class CharacterModel
{
    #region Properties

    public Camera CharacterCamera { get; }
    public CinemachineFreeLook CharacterCinemachineCamera { get; }
    public CapsuleCollider CharacterCapsuleCollider { get; }
    public Transform CharacterTransform { get; }
    public Rigidbody CharacterRigitbody { get; }
    public GameObject CameraTarget { get; }
    public Transform CameraTargetTransform { get; }
    public CharacterData CharacterData { get; }
    public CharacterStruct CharacterStruct { get; }
    public Animator CharacterAnimator { get; }

    #endregion


    #region ClassLifeCycle

    public CharacterModel(GameObject prefab, CharacterData characterData)
    {
        CharacterData = characterData;
        CharacterStruct = CharacterData._characterStruct;
        CharacterTransform = prefab.transform;
        CharacterTransform.rotation = Quaternion.Euler(0, CharacterStruct.InstantiateDirection, 0);
        CharacterTransform.name = "Player";
        CharacterTransform.tag = "Player";

        if (prefab.GetComponent<Rigidbody>() != null)
        {
            CharacterRigitbody = prefab.GetComponent<Rigidbody>();
        }
        else
        {
            CharacterRigitbody = prefab.AddComponent<Rigidbody>();
            CharacterRigitbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            CharacterRigitbody.mass = CharacterStruct.RigitbodyMass;
            CharacterRigitbody.drag = CharacterStruct.RigitbodyDrag;
            CharacterRigitbody.angularDrag = CharacterStruct.RigitbodyAngularDrag;
        }

        if (prefab.GetComponent<CapsuleCollider>() != null)
        {
            CharacterCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
        }
        else
        {
            CharacterCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
            CharacterCapsuleCollider.center = CharacterStruct.CapsuleColliderCenter;
            CharacterCapsuleCollider.radius = CharacterStruct.CapsuleColliderRadius;
            CharacterCapsuleCollider.height = CharacterStruct.CapsuleColliderHeight;
        }

        CharacterCamera = GameObject.Instantiate(CharacterStruct._characterCamera);
        CharacterCamera.name = "CharacterCamera";
        CharacterCinemachineCamera = GameObject.Instantiate(CharacterStruct._characterCinemachineCamera);
        CharacterCinemachineCamera.name = "CharacterCinemachineCamera";

        CameraTarget = new GameObject
        {
            name = "CameraTarget"
        };
        CameraTarget.transform.SetParent(CharacterTransform);
        CameraTargetTransform = CameraTarget.transform;
        CameraTargetTransform.localPosition = Vector3.zero;
        CameraTargetTransform.localRotation = Quaternion.Euler(0, 0, 0);

        CharacterCinemachineCamera.Follow = CameraTargetTransform;
        CharacterCinemachineCamera.LookAt = CameraTargetTransform;

        CharacterCamera.transform.rotation = Quaternion.Euler(0, CharacterStruct.InstantiateDirection, 0);

        for (var rig = 0; rig < 3; rig++)
        {
            CharacterCinemachineCamera.GetRig(rig).LookAt = CameraTargetTransform;
            CharacterCinemachineCamera.GetRig(rig).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y =
                CharacterStruct.CameraHeight;
        }
        
        if (prefab.GetComponent<Animator>() != null)
        {
            CharacterAnimator = prefab.GetComponent<Animator>();
        }
        else
        {
            CharacterAnimator = prefab.AddComponent<Animator>();
        }

        CharacterAnimator.runtimeAnimatorController = CharacterStruct._characterAnimator;
        CharacterAnimator.applyRootMotion = false;
    }

    #endregion
}

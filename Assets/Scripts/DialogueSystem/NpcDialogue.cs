using UnityEngine;
using UnityEngine.UI;

public class NpcDialogue : MonoBehaviour, IGetNpcInfo
{
    public Vector3 npcPos;
    public int _npcID;

    private void Awake()
    {
        npcPos = gameObject.transform.position;
    }
    public (int, Vector3) GetInfo()
    {
        var turple = (_npcID, npcPos);
        return turple;
    }

}

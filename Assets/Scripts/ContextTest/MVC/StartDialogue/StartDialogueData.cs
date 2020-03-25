using BeastHunter;
using UnityEngine;
[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/StartDialogue", order = 0)]
public sealed class StartDialogueData : ScriptableObject
{
    #region Fields

    public StartDialogueStruct StartDialogueStruct;
    #endregion
    #region Metods

    public Transform GetParent()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player.transform;
    }
    #endregion
}

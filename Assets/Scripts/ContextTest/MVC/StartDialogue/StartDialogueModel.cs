using BeastHunter;
using UnityEngine;

public class StartDialogueModel
{
    #region Fields
    public GameContext _context;
    #endregion
    #region Properties

    public Transform StartDialogueTransform;// { get; }
    public StartDialogueData StartDialogueData;
    public StartDialogueStruct StartDialogueStruct;
    public Transform parentTransform { get; private set; }

    #endregion
    public float canvasOffset = 1.5f;
    public bool _startDialogFlag;
    public bool dialogAreaEnter;

    #region ClassLifeCycle

    public StartDialogueModel(GameObject prefab, StartDialogueData startDialogueData, GameContext context)
    {
        StartDialogueData = startDialogueData;
        StartDialogueStruct = startDialogueData.StartDialogueStruct;
        StartDialogueTransform = prefab.transform;
        _context = context;
    }

    #endregion
    #region Metods

    public void Initilize()
    {
       parentTransform= StartDialogueData.GetParent();
        // create view;
    }

    #endregion
}

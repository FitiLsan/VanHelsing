using System;
using UnityEngine;


[Serializable]
public sealed class GameControllerParametersData
{
    #region Fields

    [SerializeField] private bool _doImplementPlayer;
    [SerializeField] private bool _doImplementInput;
    [SerializeField] private bool _doImplementBoss;
    [SerializeField] private bool _doImplementMobs;
    [SerializeField] private bool _doImplementInteractiveObjects;
    [Tooltip("Not safe to use, bugs may appear!")] [Obsolete] [SerializeField] private bool _doImplementDialogSystem;
    [Tooltip("Not safe to use, bugs may appear!")]  [Obsolete] [SerializeField] private bool _doImplementQuestSystem;

    #endregion


    #region Properties

    public bool DoImplementPlayer => _doImplementPlayer;
    public bool DoImplementInput => _doImplementInput;
    public bool DoImplementBoss => _doImplementBoss;
    public bool DoImplementMobs => _doImplementMobs;
    public bool DoImplementInteractiveObjects => _doImplementInteractiveObjects;
    public bool DoImplementDialogSystem => _doImplementDialogSystem;
    public bool DoImplementQuestSystem => _doImplementQuestSystem;

    #endregion


    #region Methods

    public void CheckParametersCorrectInput()
    {
        _doImplementPlayer = DoImplementDialogSystem || DoImplementQuestSystem ? true : _doImplementPlayer;
        _doImplementInput = DoImplementPlayer ? true : _doImplementInput;
    }

    #endregion
}

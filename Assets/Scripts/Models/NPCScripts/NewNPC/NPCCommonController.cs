using UnityEngine;
using Models;
using UnityEngine.AI;

public class NPCCommonController : BaseScripts.BaseController
{
    #region PrivateData

    private NewNPCModel _npcModel;
    private GameObject _npcPrefab;
    private NavMeshAgent _navMeshAgent;
    private NPCPoint[] _dayNavigationPoints;
    private NPCPoint[] _nightNavigationPoints;
    private NPCPoint _safePlacePoint;
    private NPCPoint _targetPoint;
    private bool _isDay;
    private bool _isOnPoint;
    private bool _isWalking;
    private float _timeOnPoint;

    #endregion


    #region Properties

    public Vector3 Position;

    public string NPCName;
    public string EnemyName;

    public int NPCID;
    public int EnemyID;
    public int CurrentPointIndex;

    public float BaseWalkSpeed;
    public float BaseRunSpeed;

    public bool IsDead;
    public bool CanDie;
    public bool CanBeEnemy;
    public bool IsEnemy;
    public bool IsEscaping;
    public bool IsInteractable;

    #endregion


    #region Methods

    public void Init(NewGuardianModel npcModel, GameObject prefab)
    {
        _npcModel = npcModel;
        _npcPrefab = prefab;
        _navMeshAgent = _npcPrefab.GetComponent<NavMeshAgent>();

        CanBeEnemy = _npcModel.CanBeEnemy;

        Position = _npcModel.NPCPosition;
        NPCName = _npcModel.Name;
        CurrentPointIndex = _npcModel.CurrentPointIndex;
        BaseWalkSpeed = _npcModel.BaseWalkSpeed;
        BaseRunSpeed = _npcModel.BaseRunSpeed;
        IsDead = _npcModel.IsDead;
        CanDie = _npcModel.CanDie;
        IsEnemy = _npcModel.IsEnemy;
        IsEscaping = _npcModel.IsEscaping;
        IsInteractable = _npcModel.IsInteractabe;

        _dayNavigationPoints = _npcModel._dayNavigationPoints;
        _nightNavigationPoints = _npcModel._nightNavigationPoints;
        _safePlacePoint = _npcModel._safePlacePoint;

        OnAwake();
    }

    public override void OnAwake()
    {
        base.OnAwake();

        if (!IsActive || IsDead) return;

        _isDay = true; // DELETE later
        _isOnPoint = false;

        if (_isDay && !IsEscaping)
        {
            _targetPoint = _dayNavigationPoints[CurrentPointIndex];
        }
        else if (!_isDay && !IsEscaping)
        {
            _targetPoint = _nightNavigationPoints[CurrentPointIndex];
        }
        else
        {
            _targetPoint = _safePlacePoint;
        }

        _navMeshAgent.SetDestination(_targetPoint.PointPosition);

    }

    public override void Tick()
    {
        base.Tick();

        if (!IsActive || IsDead) return;

        UpdatePosition();
        MakeRoutineActions();
    }

    private void UpdatePosition()
    {
        Position = _npcPrefab.transform.position;
    }

    private void MakeRoutineActions()
    {
        if (_isOnPoint)
        {
            WaitOnPoint();
        }
        else
        {
            if (IsEscaping) return;
            CheckDistance();
        }
    }

    private void ComeToPoint()
    {
        if (_isOnPoint) return;

        _isOnPoint = true;
        _timeOnPoint = _targetPoint.TimeOnPoint;

        // animation methods here (also gotta be walking animation worked somehow
    }

    private void WaitOnPoint()
    {
        _timeOnPoint -= Time.deltaTime;

        if (_timeOnPoint <= 0)
        {
            ChangeToNextPoint();
        }
    }

    private void CheckDistance()
    {
        if (_isOnPoint || !Distance()) return;
        ComeToPoint();
    }

    /// <summary>
    /// Needed to be removed - checks if the path can be calculated
    /// </summary>
    private void IsCurrentPointReachable()
    {
        NavMeshPath path = new NavMeshPath();
        _navMeshAgent.CalculatePath(_targetPoint.PointPosition, path);
        Debug.LogError(path.status);
    }

    private void ChangeToNextPoint()
    {
        if (_isDay)
        {
            if (CurrentPointIndex == _dayNavigationPoints.Length - 1)
            {
                CurrentPointIndex = 0;
            }
            else
            {
                CurrentPointIndex++;
            }

            _targetPoint = _dayNavigationPoints[CurrentPointIndex];
        }
        else
        {
            if (CurrentPointIndex == _nightNavigationPoints.Length - 1)
            {
                CurrentPointIndex = 0;
            }
            else
            {
                CurrentPointIndex++;
            }

            _targetPoint = _nightNavigationPoints[CurrentPointIndex];
        }

        _isOnPoint = false;
        _navMeshAgent.SetDestination(_targetPoint.PointPosition);
        IsCurrentPointReachable();
    }

    private bool Distance()
    {
        var dist = Mathf.Sqrt(Mathf.Pow(Position.x - _targetPoint.PointPosition.x, 2) +
                              Mathf.Pow(Position.y - _targetPoint.PointPosition.y, 2) +
                              Mathf.Pow(Position.z - _targetPoint.PointPosition.z, 2));

        if (dist > _targetPoint.DistanceToActivate) return false;
        return true;
    }

    // gotta be an event
    private void DangerAppear()
    {
        IsEscaping = true;
        _navMeshAgent.SetDestination(_safePlacePoint.PointPosition);
        _navMeshAgent.speed = BaseRunSpeed;
        _timeOnPoint = 0;
    }

    // gotta be an event
    private void DangerEnd()
    {
        IsEscaping = false;
        _navMeshAgent.speed = BaseWalkSpeed;
        ChangeToNextPoint();
    }

    // gotta be an event
    private void DaytimeChange()
    {
        _isDay = !_isDay;
        ChangeToNextPoint();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {

    }

    #endregion

   
}

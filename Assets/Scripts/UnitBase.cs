using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBase : MonoBehaviour
{
    /// <summary>
    /// Prefabs & children
    /// </summary>
    public GameObject model;
    public Animator animator;
    public GameObject cameraSpot;
    public PlayerUI unitUI;

    /// <summary>
    /// Navigation Agent
    /// </summary>
    public NavMeshAgent agent;
    public float moveSpeed;
    public float angularSpeed;

    /// <summary>
    /// Identification
    /// </summary>
    public bool IsLocalPlayer { get; set; }
    public int PlayerId {
        get
        {
            return playerInfo != null ? playerInfo.id : -1;
        }
    }

    /// <summary>
    /// Server side events
    /// </summary>
    // messages received from the server
    public bool IsDirtyFlag { get; private set; }
    PlayerData playerInfo;
    Queue<PlayerCommand> commandQueue = new Queue<PlayerCommand>();

    protected void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        if( agent )
            agent.speed = moveSpeed;
    }

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        Reset();
    }

    public void Reset()
    {
        IsDirtyFlag = false;
        model.SetActive( true );
    }   

    protected virtual void FixedUpdate()
    {
        if( IsLocalPlayer )
        {

        }
        else
        {
            if( unitUI )
                unitUI.gameObject.transform.rotation = Camera.main.transform.rotation;
        }

        if( commandQueue.Count > 0 )
        {
            ExecuteCommand( commandQueue.Dequeue() );
        }
    }

    #region Commands
    public void AddCommand( string commandStr )
    {
        PlayerCommand command;
        if( System.Enum.TryParse( commandStr, out command ) )
            AddCommand( command );
    }

    public void AddCommand( PlayerCommand command )
    {
        Debug.Log( "[" + Time.time.ToString() + "] receive command : " + command.ToString() );
        commandQueue.Enqueue( command );
    }

    void ExecuteCommand( PlayerCommand command )
    {
        Debug.Log( "[ " + Time.time.ToString() + "] execute command : " + command.ToString() );
        switch( command )
        {
            default:
                break;
        }
    }

    #endregion

    #region User Data

    public void SetPlayerData( PlayerData data, bool isLocal )
    {
        playerInfo = data;
        if( unitUI )
            unitUI.SetUserData( data.id, isLocal );

        transform.position = data.position;
        transform.rotation = data.rotation;
        SetColor( data.color );

        IsLocalPlayer = isLocal;
        if( IsLocalPlayer )
        {
            Camera.main.transform.parent = cameraSpot.transform;
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        IsDirtyFlag = true;
    }

    public void SetColor( Color color )
    {
        Material mat = model.GetComponent<Renderer>().material;
        mat.color = color;
    }

    public PlayerData GetPlayerData()
    {
        playerInfo.position = transform.position;
        playerInfo.rotation = transform.rotation;
        return playerInfo;
    }

    #endregion

    #region Nav Mesh Agent
    public void StartNavigation()
    {
        if( agent )
            agent.isStopped = false;
    }

    public void StopMove()
    {
        if( agent )
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    public void MoveTo( Vector3 position )
    {
        if( agent )
        {
            agent.SetDestination( position );
        }
        else
        {
            transform.position = position;
        }
    }

    public void MoveBy( Vector3 direction )
    {
        if( agent )
        {
            agent.velocity = direction * moveSpeed;
        }
        else
        {
            transform.Translate( direction * moveSpeed, Space.World );
        }
    }

    public bool IsReachedTarget()
    {
        if( !agent.pathPending )
        {
            if( agent.remainingDistance <= agent.stoppingDistance )
            {
                if( !agent.hasPath || agent.velocity.sqrMagnitude <= float.Epsilon )
                    return true;
            }
        }
        return false;
    }


    #endregion
}

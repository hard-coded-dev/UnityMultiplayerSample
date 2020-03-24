using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerCommand
{
    MoveForward,
    MoveBackward,
    MoveLeft,
    MoveRight,
    RotateLeft,
    RotateRight,
    LookUp,
    LookDown,
    FireBullet,
};

public class PlayerController : Singleton<PlayerController>
{
    [HideInInspector] public UnitBase localPlayer;

    // messages to be sent to the server
    Queue<PlayerCommand> messageQueue = new Queue<PlayerCommand>();
    public bool IsDirtyFlag { get; private set; }

    public void Awake()
    {
    }

    private void Start()
    {
    }

    public void FixedUpdate()
    {
        if( localPlayer )
        {
            Transform curTransform = localPlayer.transform;
            Vector3 moveVector = Vector3.zero;
            if( Input.GetKey( KeyCode.W ) )
            {
                moveVector += curTransform.forward;
            }
            if( Input.GetKey( KeyCode.S ) )
            {
                moveVector += -curTransform.forward;
            }
            if( Input.GetKey( KeyCode.A ) )
            {
                moveVector += -curTransform.right;
            }
            if( Input.GetKey( KeyCode.D ) )
            {
                moveVector += curTransform.right;
            }
            if( moveVector != Vector3.zero )
            {
                localPlayer.MoveBy( moveVector );
                IsDirtyFlag = true;
            }
            else
                localPlayer.StopMove();

            // mouse right drag
            if( Input.GetMouseButton( 1 ) )
            {
                float axis = Input.GetAxis( "Mouse X" );
                if( axis != 0.0 )
                {
                    float rotation = axis * localPlayer.angularSpeed;
                    curTransform.Rotate( Vector3.up, rotation );
                    IsDirtyFlag = true;
                }
            }
            else
            {
                if( Input.GetKey( KeyCode.Q ) )
                {
                    float rotation = Time.deltaTime * -localPlayer.angularSpeed;
                    curTransform.Rotate( Vector3.up, rotation );
                    IsDirtyFlag = true;
                }
                else if( Input.GetKey( KeyCode.E ) )
                {
                    float rotation = Time.deltaTime * localPlayer.angularSpeed;
                    curTransform.Rotate( Vector3.up, rotation );
                    IsDirtyFlag = true;
                }
            }

            //StartCoroutine( UpdateTransform( curTransform, CanvasManager.Instance.prediction.isOn ? NetworkManager.Instance.estimatedLag : 0.0f ) );
        }
    }

    IEnumerator UpdateTransform( Transform newTransform, float waitingTime )
    {
        yield return new WaitForSeconds( waitingTime );
        localPlayer.transform.position = newTransform.position;
        localPlayer.transform.rotation = newTransform.rotation;
    }

    public void SendCommand( PlayerCommand command )
    {
        Debug.Log( "[ " + Time.time.ToString() + "] Send command : " + command.ToString() );
        messageQueue.Enqueue( command );
    }

    public bool HasMessage()
    {
        return messageQueue.Count > 0;
    }

    public string PopMessage()
    {
        return messageQueue.Dequeue().ToString();
    }

    public void ClearDirtyFlag()
    {
        IsDirtyFlag = false;
    }
}

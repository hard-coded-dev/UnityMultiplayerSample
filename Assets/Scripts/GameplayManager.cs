using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    // the player(owner) of this client
    public int localPlayerId;
    public UnitBase playerPrefab;
    Dictionary<int, UnitBase> playerUnits = new Dictionary<int, UnitBase>();
    Dictionary<int, PlayerData> playersData = new Dictionary<int, PlayerData>();
    Dictionary<int, PlayerData> prevPlayerData = new Dictionary<int, PlayerData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer( PlayerData playerInfo, bool isLocalPlayer )
    {
        if( playerUnits.ContainsKey( playerInfo.id ) )
        {
            Debug.Assert( false, playerInfo.id + " player already  exist." );
        }
        else
        {
            UnitBase player = Instantiate( playerPrefab );
            player.SetPlayerData( playerInfo, isLocalPlayer );
            playerUnits.Add( playerInfo.id, player );

            if( isLocalPlayer )
            {
                localPlayerId = playerInfo.id;
                PlayerController.Instance.localPlayer = player;
            }
        }
    }

    public void UpdatePlayer( PlayerData playerData, PlayerData prevPlayerData, float delta )
    {
        if( playerUnits.ContainsKey( playerData.id ) )
        {
            if( playerData.id != localPlayerId )
            {
                Vector3 nextPos = playerData.position;
                Quaternion nextRotation = playerData.rotation;
                if( CanvasManager.Instance.reconciliation.isOn )
                {
                    // To be implemented
                }
                if( CanvasManager.Instance.interpolation.isOn && prevPlayerData != null )
                {
                    nextPos = Vector3.Lerp( prevPlayerData.position, playerData.position, delta );
                    nextRotation = Quaternion.Lerp( prevPlayerData.rotation, playerData.rotation, delta );
                }

                playerUnits[playerData.id].MoveTo( nextPos );
                playerUnits[playerData.id].transform.rotation = nextRotation;
            }
            else
            {
                // PlayerController will change the transform directly by now.
            }

            //if( playerData.command != null && playerData.command != "" )
            //{
            //    playerUnits[playerData.id].AddCommand( playerData.command );
            //}
        }
    }

    public void UpdatePlayers( List<PlayerData> playersData )
    {
        foreach( var player in playersData )
        {
            if( playerUnits.ContainsKey( player.id ) )
            {
                if( player.id != localPlayerId )
                {
                    Vector3 nextPos = player.position;
                    Quaternion nextRotation = player.rotation;
                    if( CanvasManager.Instance.reconciliation.isOn )
                    {
                        // To be implemented
                    }
                    //if( CanvasManager.Instance.interpolation.isOn && prevPlayerData != null )
                    //{
                    //    nextPos = Vector3.Lerp( prevPlayerData.position, player.position, delta );
                    //    nextRotation = Quaternion.Lerp( prevPlayerData.rotation, player.rotation, delta );
                    //}

                    playerUnits[player.id].MoveTo( nextPos );
                    playerUnits[player.id].transform.rotation = nextRotation;
                }
            }
            else
            {
                SpawnPlayer( player, false );
            }
        }
    }

    public void DisconnectPlayer( int playerId )
    {
        if( playerUnits.ContainsKey( playerId ) )
        {
            if( playerId == localPlayerId )
            {
                Debug.LogWarning( "You has been disconnected." );
            }
            else
            {
                Destroy( playerUnits[playerId].gameObject );
                playerUnits.Remove( playerId );
            }
        }
    }

}

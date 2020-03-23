﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    // the player(owner) of this client
    public int localPlayerId;
    public UnitBase playerPrefab;
    Dictionary<int, UnitBase> playerUnits = new Dictionary<int, UnitBase>();
    Dictionary<int, PlayerInfoData> prevPlayerData = new Dictionary<int, PlayerInfoData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer( PlayerPacketData playerInfo, bool isLocalPlayer )
    {
        if( playerUnits.ContainsKey( playerInfo.id ) )
        {
            Debug.Assert( false, playerInfo.id + " player already  exist." );
        }
        else
        {
            UnitBase player = Instantiate( playerPrefab );
            player.transform.position = playerInfo.pos;
            player.SetUserId( playerInfo.id, isLocalPlayer );
            playerUnits.Add( playerInfo.id, player );

            if( isLocalPlayer )
            {
                localPlayerId = playerInfo.id;
                PlayerController.Instance.localPlayer = player;
            }
        }
    }

    public void UpdatePlayer( PlayerPacketData playerData, PlayerPacketData prevPlayerData, float delta )
    {
        if( playerUnits.ContainsKey( playerData.id ) )
        {
            if( playerData.id != localPlayerId )
            {
                Vector3 nextPos = playerData.pos;
                Quaternion nextRotation = playerData.rotation;
                if( CanvasManager.Instance.reconciliation.isOn )
                {
                    // To be implemented
                }
                if( CanvasManager.Instance.interpolation.isOn && prevPlayerData != null )
                {
                    nextPos = Vector3.Lerp( prevPlayerData.pos, playerData.pos, delta );
                    nextRotation = Quaternion.Lerp( prevPlayerData.rotation, playerData.rotation, delta );
                }

                playerUnits[playerData.id].MoveTo( nextPos );
                playerUnits[playerData.id].transform.rotation = nextRotation;
            }
            else
            {
                // PlayerController will change the transform directly by now.
            }

            if( playerData.command != null && playerData.command != "" )
            {
                playerUnits[playerData.id].AddCommand( playerData.command );
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

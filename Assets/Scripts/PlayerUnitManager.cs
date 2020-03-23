using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitManager
{
    Dictionary<int, PlayerData> players = new Dictionary<int, PlayerData>();

    public int NumPlayers
    {
        get { return players.Count; }
    }

    public void NewPlayer( int id )
    {
        if( !players.ContainsKey( id ) )
        {
            players.Add( id, new PlayerData( id ) );
        }
        else
        {
            Debug.LogWarning( "Player already exist : " + id.ToString() );
        }
    }

    public void RemovePlayer( int id )
    {
        if( players.ContainsKey( id ) )
        {
            players.Remove( id );
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
        }
    }

    public void UpdatePlayer( int id, PlayerData data )
    {
        if( players.ContainsKey( id ) )
        {
            players[id] = data;
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
        }
    }

    public PlayerData GetPlayer( int id )
    {
        if( players.ContainsKey( id ) )
        {
            return players[id];
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
            return null;
        }
    }
}

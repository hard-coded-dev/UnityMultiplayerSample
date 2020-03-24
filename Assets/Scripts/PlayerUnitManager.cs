using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitManager
{
    List<PlayerData> players = new List<PlayerData>();
    // have the dictionary for the quick access
    Dictionary<int, PlayerData> playersDict = new Dictionary<int, PlayerData>();
    // indicate whther to update a player information to the other clients
    public bool IsDirtyFlag { get; private set; }

    public int NumPlayers
    {
        get { return players.Count; }
    }

    public void NewPlayer( int id )
    {
        if( !playersDict.ContainsKey( id ) )
        {
            var newPlayer = new PlayerData( id );
            players.Add( newPlayer );
            playersDict.Add( id, newPlayer );
        }
        else
        {
            Debug.LogWarning( "Player already exist : " + id.ToString() );
        }
    }

    public void RemovePlayer( int id )
    {
        if( playersDict.ContainsKey( id ) )
        {
            players.Remove( playersDict[id] );
            playersDict.Remove( id );
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
        }
    }

    public void UpdatePlayer( int id, PlayerData data )
    {
        if( playersDict.ContainsKey( id ) )
        {
            int idx = players.IndexOf( playersDict[id] );
            players[idx] = data;
            playersDict[id] = data;
            IsDirtyFlag = true;
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
        }
    }

    public PlayerData GetPlayer( int id )
    {
        if( playersDict.ContainsKey( id ) )
        {
            return playersDict[id];
        }
        else
        {
            Debug.LogWarning( "Player not found : " + id.ToString() );
            return null;
        }
    }

    public void ClearDirtyFlag()
    {
        IsDirtyFlag = false;
    }

    public List<PlayerData>GetPlayers()
    {
        return players;
    }

}

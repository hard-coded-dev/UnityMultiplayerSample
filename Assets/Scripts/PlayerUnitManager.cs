using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

class PlayerUnitManager
{
    Dictionary<int, PlayerInfoData> players = new Dictionary<int, PlayerInfoData>();

    public void AddPlayer( int key )
    {
        PlayerInfoData newPlayer = new PlayerInfoData(key);

        players.Add( key, newPlayer );
    }

    public void RemovePlayer( int key )
    {
        players.Remove( key );
    }

    public PlayerInfoData[] GetPlayersInfo()
    {
        return players.Values.ToArray();
    }

    public void UpdatePlayer( int key, PlayerInfoData newData )
    {
        if( players.ContainsKey( key ) )
        {
            players[key] = newData;
        }
        else
        {
            Debug.LogWarning( "Player not foudn : " + key.ToString() );
        }
    }


}


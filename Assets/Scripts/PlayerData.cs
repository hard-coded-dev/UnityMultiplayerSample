using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData
{
    public int id;
    public Color color;
    public Vector3 position;
    public Quaternion rotation;

    public PlayerData( int _id )
    {
        id = _id;
    }
}


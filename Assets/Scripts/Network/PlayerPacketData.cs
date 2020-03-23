using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfoData
{
    public int id;

    public Transform transform;
    public Color color;

    public string command;

    public PlayerInfoData( int id )
    {
        this.id = id;
        color = new Color( Random.value, Random.value, Random.value );
        transform.position = new Vector3( Random.value, Random.value, Random.value ) * 3.0f;
    }
}


[System.Serializable]
public class PlayerPacketData
{
    public int id;
    
    public Vector3 pos;
    public Quaternion rotation;
    public Color color;

    public string command;
}

[System.Serializable]
public struct ColorData
{
    public float R;
    public float G;
    public float B;

    public static implicit operator Color( ColorData value )
    {
        return new Color( value.R, value.G, value.B );
    }
}

[System.Serializable]
public struct PositionData
{
    public float x;
    public float y;
    public float z;

    public override string ToString()
    {
        return System.String.Format( "[ {0}, {1}, {2} ]", x, y, z );
    }

    public static implicit operator Vector3( PositionData value )
    {
        return new Vector3( value.x, value.y, value.z );
    }
}

[System.Serializable]
public struct RotationData
{
    public float x;
    public float y;
    public float z;
    public float w;

    public static implicit operator Quaternion( RotationData value )
    {
        return new Quaternion( value.x, value.y, value.z, value.w );
    }
}
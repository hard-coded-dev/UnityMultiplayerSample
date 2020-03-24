using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int id;
    
    public Vector3 position;
    public Quaternion rotation;
    public Color color;

    public PlayerData( int _id )
    {
        id = _id;
        color = new Color( Random.value, Random.value, Random.value);
        position = new Vector3( Random.value - 0.5f, 0.0f, Random.value - 0.5f ) * 3.0f;
        rotation = Quaternion.identity;
    }

    public PlayerData( PlayerData other )
    {
        id = other.id;
        color = other.color;
        position = other.position;
        rotation = other.rotation;
    }

    public override string ToString()
    {
        return string.Format( "[{0}] pos = {1}, rotation = {2}", id, position, rotation );
    }
}


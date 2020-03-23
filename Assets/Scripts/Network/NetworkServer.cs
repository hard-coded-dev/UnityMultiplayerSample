using System;

using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;

public class NetworkServer : MonoBehaviour
{
    public UdpNetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;
    public ushort m_ServerPort = 12345;
    PlayerUnitManager players = new PlayerUnitManager();

    void Start ()
    {
        m_Driver = new UdpNetworkDriver(new INetworkParameter[0]);
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = m_ServerPort;
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log("Failed to bind to port " + m_ServerPort );
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    void Update ()
    {
        m_Driver.ScheduleUpdate().Complete();

        // CleanUpConnections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }
        // AcceptNewConnections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                Assert.IsTrue(true);

            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) !=
                   NetworkEvent.Type.Empty)
            {
                if( cmd == NetworkEvent.Type.Connect )
                {
                    Debug.Log( "Client connected from " + m_Connections[i].InternalId.ToString() );
                    players.AddPlayer( m_Connections[i].InternalId );
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    var readerCtx = default(DataStreamReader.Context);
                    int numBytes = stream.GetBytesRead( ref readerCtx );
                    byte[] bytes = stream.ReadBytesAsArray( ref readerCtx, numBytes );
                    string jsonText = System.Text.Encoding.ASCII.GetString( bytes );
                    Debug.Log("Got " + numBytes + " bytes from the Client : " + jsonText );
                    PlayerInfoData playerInfo = JsonUtility.FromJson<PlayerInfoData>( jsonText );
                    players.UpdatePlayer( playerInfo.id, playerInfo );
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server :" + m_Connections[i].InternalId.ToString() );
                    players.RemovePlayer( m_Connections[i].InternalId );
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }

        SendPlayersData();
    }

    void SendPlayersData()
    {
        var playersInfo = players.GetPlayersInfo();
        if( playersInfo.Length > 0 )
        {
            string messageData = JsonUtility.ToJson( playersInfo );
            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes( messageData );
            using( var writer = new DataStreamWriter( sendBytes.Length, Allocator.Temp ) )
            {
                writer.Write( sendBytes, sendBytes.Length );
                for( int i = 0; i < m_Connections.Length; i++ )
                    m_Connections[i].Send( m_Driver, writer );
            }
        }
    }
}
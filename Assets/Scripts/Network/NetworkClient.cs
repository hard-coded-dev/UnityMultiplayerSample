using System;
using System.Text;
using UnityEngine;

using Unity.Collections;
using Unity.Networking.Transport;

namespace OldNetwork
{
    //public class NetworkClient : MonoBehaviour
    //{
        //public UdpNetworkDriver m_Driver;
        //public NetworkConnection m_Connection;
        //public string m_ServerIp = "3.219.69.41";
        //public ushort m_ServerPort = 12345;
        //public bool m_Done;

        //void Start()
        //{
        //    m_Driver = new UdpNetworkDriver( new INetworkParameter[0] );
        //    m_Connection = default( NetworkConnection );

        //    var endpoint = NetworkEndPoint.Parse( m_ServerIp, m_ServerPort );
        //    m_Connection = m_Driver.Connect( endpoint );
        //    //InvokeRepeating( "SendPlayerPacket", 1.0f, 1.0f / 30.0f );
        //}

        //public void OnDestroy()
        //{
        //    m_Driver.Dispose();
        //}

        //void Update()
        //{
        //    m_Driver.ScheduleUpdate().Complete();

        //    if( !m_Connection.IsCreated )
        //    {
        //        if( !m_Done )
        //            Debug.Log( "Something went wrong during connect" );
        //        return;
        //    }

        //    DataStreamReader stream;
        //    NetworkEvent.Type cmd;

        //    while( ( cmd = m_Connection.PopEvent( m_Driver, out stream ) ) !=
        //           NetworkEvent.Type.Empty )
        //    {
        //        if( cmd == NetworkEvent.Type.Connect )
        //        {
        //            Debug.Log( "We are now connected to the server : " + m_Connection.InternalId );

        //            using( var writer = new DataStreamWriter( 4, Allocator.Temp ) )
        //            {
        //                writer.Write( m_Connection.InternalId );
        //                m_Connection.Send( m_Driver, writer );
        //            }
        //        }
        //        else if( cmd == NetworkEvent.Type.Data )
        //        {
        //            var readerCtx = default( DataStreamReader.Context );
        //            int numBytes = stream.GetBytesRead( ref readerCtx );
        //            byte[] bytes = stream.ReadBytesAsArray( ref readerCtx, numBytes );
        //            string jsonText = System.Text.Encoding.ASCII.GetString( bytes );
        //            PlayerInfoData[] playerInfo = JsonUtility.FromJson<PlayerInfoData[]>( jsonText );

        //            Debug.Log( "Got the value = " + bytes + " from the server : " + jsonText );
        //            m_Done = true;
        //            //m_Connection.Disconnect(m_Driver);
        //            //m_Connection = default(NetworkConnection);
        //        }
        //        else if( cmd == NetworkEvent.Type.Disconnect )
        //        {
        //            Debug.Log( "Client got disconnected from server" );
        //            m_Connection = default( NetworkConnection );
        //        }
        //    }
        //}

        //private void FixedUpdate()
        //{

        //}

        //void SendPlayerPacket()
        //{
        //    if( m_Connection.IsCreated )
        //    {
        //        UnitBase localPlayer = PlayerController.Instance.localPlayer;
        //        if( localPlayer )
        //        {
        //            PlayerInfoData data = new PlayerInfoData( m_Connection.InternalId );
        //            data.transform = localPlayer.transform;
        //            if( PlayerController.Instance.HasMessage() )
        //                data.command = PlayerController.Instance.PopMessage();
        //            string messageData = JsonUtility.ToJson( data );

        //            Byte[] sendBytes = Encoding.ASCII.GetBytes( messageData );
        //            using( var writer = new DataStreamWriter( sendBytes.Length, Allocator.Temp ) )
        //            {
        //                writer.Write( sendBytes, sendBytes.Length );
        //                m_Connection.Send( m_Driver, writer );
        //            }
        //        }

        //    }
        //}
    //}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 处理外部传入的参数
/// </summary>
namespace JvutberNetwork
{
    public class TcpHandler : MonoBehaviour
    {
        internal float roll = 0, pitch = 0, yaw = 0, min_ear = 1.0f, mar = 0f, mdst = 0.25f;
        private List<Vector2> points;
        public List<Vector2> Points
        {
            get
            {
                if (points == null)
                    points = new List<Vector2>();
                return points;
            }
            private set
            {
                points = value;
            }
        }


        // Thread
        Thread receiveThread;
        TcpClient client;
        TcpListener listener;
        public int port = 5066;


        // Start is called before the first frame update
        void Start()
        {
            points = new List<Vector2>();
            InitTCP();
        }

        private void InitTCP()
        {
            //设置成background thread的好处是，一旦foreground thread全部停止了，background thread也会自动终止
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void ReceiveData()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                //传过来的消息字符串如果太大，可能导致一次没有读完
                //所以开的byte数组大小要估计好
                Byte[] bytes = new Byte[2048];
                while (true)
                {
                    using (client = listener.AcceptTcpClient())
                    {
                        using (NetworkStream stream = client.GetStream())
                        {
                            int length;
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                string clientMessage = Encoding.ASCII.GetString(incommingData);
                                string[] res = clientMessage.Split(' ');
                                roll = float.Parse(res[0]) * 0.4f + roll * 0.6f;
                                pitch = float.Parse(res[1]) * 0.4f + pitch * 0.6f;
                                yaw = float.Parse(res[2]) * 0.4f + yaw * 0.6f;
                                min_ear = float.Parse(res[3]);
                                mar = float.Parse(res[4]) * 0.4f + mar * 0.6f;
                                mdst = float.Parse(res[5]);
                                //解析68个点
                                points = new List<Vector2>();
                                for (int i = 9; i < res.Length; i += 2)
                                {
                                    points.Add(new Vector2(float.Parse(res[i - 1]), float.Parse(res[i])));
                                }
                                //Debug.Log(points.Count);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }


        public Vector3 GetHeadEulerAngles()
        {
            //-pitch, yaw, -roll
            return new Vector3(pitch, yaw, -roll);
        }

        public List<Vector2> GetFaceLanmarkPoints()
        {
            return Points;
        }

        void OnApplicationQuit()
        {
            try
            {
                client.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            try
            {
                listener.Stop();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

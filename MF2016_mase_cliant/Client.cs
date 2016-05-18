using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace MF2016_mase_cliant {

    static class ClientSetting {
        public static String address = "192.168.2.142";
        public static int port = 5000;
        public static int timeout = 100;
        public static int freqMilliSec = 100;
    }

    class Client {
        //通信設定用
        private String address;
        private int port;
        private int timeoutMillisec;

        //通信用
        private System.Net.Sockets.TcpClient tcp;
        private System.Net.Sockets.NetworkStream nsr;

        private Object _lock = new Object();
        private StreamWriter sw;
        private StreamReader sr;
        Task listen;

        public event Action<Player> latestPlayer = null;

        //コンストラクタで通信先とタイムアウト時間を設定
        public Client(String a, int p, int t) {
            this.address = a;
            this.port = p;
            this.timeoutMillisec = t;
        }
        //サーバーとの通信開始
        public bool startConnect() {
            lock (_lock) {
                Console.WriteLine("サーバーと接続します");
                this.tcp = new System.Net.Sockets.TcpClient(address, port);
                Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                    ((System.Net.IPEndPoint)this.tcp.Client.RemoteEndPoint).Address,
                    ((System.Net.IPEndPoint)this.tcp.Client.RemoteEndPoint).Port,
                    ((System.Net.IPEndPoint)this.tcp.Client.LocalEndPoint).Address,
                    ((System.Net.IPEndPoint)this.tcp.Client.LocalEndPoint).Port);
                //NetworkStreamを取得する

                this.nsr = tcp.GetStream();
                this.nsr.ReadTimeout = timeoutMillisec;
                this.nsr.WriteTimeout = timeoutMillisec;

                this.sw = new StreamWriter(nsr) { NewLine = "\r\n", AutoFlush = true };
                this.sr = new StreamReader(nsr);
                return true;

            }
        }


        //Objectは障害物の型にしてくれ
        public void sentObjData(Obstacle obj) {
            //objectをjson形式にして送信
            lock (_lock) {
                sw.WriteLine(JsonConvert.SerializeObject(obj));
                Console.WriteLine(JsonConvert.SerializeObject(obj));
            }
        }


        //どうするか相談する
        public void pleyerDataListener() {
            listen =Task.Run((Action)getPlayerData);   
        }

         public async void getPlayerData() {
            try {
                while (true) {
                    string str = await sr.ReadLineAsync();
                    Player p = JsonConvert.DeserializeObject<Player>(str);
                    onDataReceived(p);
                }
            }catch{
            }
        }

        private void onDataReceived(Player p) {
            Action<Player> tmp = latestPlayer;
            if (tmp != null) {
                tmp(p);
            }
        }

        //サーバとの通信終了
        public bool endConnet() {
            lock (_lock) {
                //this.nsw.Dispose();
                this.nsr.Dispose();
                this.nsr.Close();
                //this.nsw.Close();

                this.tcp.Close();
                Console.WriteLine("通信終了");
                return true;
            }
        }
    }
}

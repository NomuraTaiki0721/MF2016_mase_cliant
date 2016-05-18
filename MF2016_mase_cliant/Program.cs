using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MF2016_mase_cliant {
    class Program {
        static Player player=null;
        static void Main(string[] args) {
            Client client = new Client(ClientSetting.address, ClientSetting.port, 100);
            client.latestPlayer += ShowPlayer;

            client.startConnect();
            Task.Delay(100);

            client.pleyerDataListener();

            
            while (true) {
                if (player != null) {
                    Console.WriteLine("aaaaa {0} ",player.HP);
                    player = null;
                    break;
                }
            }
            
            Task.Delay(100);
            Obstacle obs1 = new Obstacle(1, 1, ObstacleType.block);

            client.sentObjData(obs1);
            Obstacle obs2 = new Obstacle(2, 1, ObstacleType.block);

            client.sentObjData(obs2);
            Obstacle obs3 = new Obstacle(3, 1, ObstacleType.block);

            client.sentObjData(obs3);


            //client.sentObjData(obs);

            Task.Delay(10000);
            client.endConnet();
            Console.WriteLine("end");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void ShowPlayer(Player p) {
            player = p;
        }
    }
}

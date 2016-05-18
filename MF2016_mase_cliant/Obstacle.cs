using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF2016_mase_cliant {
    
    public enum ObstacleType { block, toge };

    public class Obstacle {
        public int X;
        public int Y;
        public ObstacleType Type;

        public Obstacle() {
        }

        public Obstacle(int x, int y, ObstacleType type) {
            X = x;
            Y = y;
            Type = type;
        }
    }
}

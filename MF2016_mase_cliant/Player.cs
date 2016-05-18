using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF2016_mase_cliant {
    public enum GameState { Start, Game, Pwin, Plose };

    public class Player {
        public GameState State;
        public double X;
        public double Y;
        public int HP;
        public Player() {
        }
        public Player(GameState state, double x, double y, int hp) {
            this.State = state;
            this.X = x;
            this.Y = y;
            this.HP = hp;
        }
    }
}

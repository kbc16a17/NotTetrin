using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotTetrin.Ingame {
    public class Ranker {
        private string name;
        private int score;

        public Ranker(string name, int score) {
            this.name = name;
            this.score = score;
        }

        public override string ToString() {
            return $"{name}: {score}";
        }
    }
}

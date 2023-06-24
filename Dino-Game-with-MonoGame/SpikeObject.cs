using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino_Game_with_MonoGame
{
    public class SpikeObject
    {
        public float x = 800;
        public readonly float y = 422;

        public SpikeObject(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public SpikeObject()
        {

        }
    }
}

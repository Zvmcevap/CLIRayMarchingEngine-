using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CLIRayMarchingEngine.View
{
    public interface IDrawable
    {        /// <summary>
        /// All objects that can be drawn on canvas, need a SDF distance method and light information
        /// </summary>
        /// <param name="canvas"></param>
        /// 

        public LightConfig LightConfig { get; set; }
        public float GetSDFDistance(Vector3 p);
    }

    public struct LightConfig
    {
        public float Ambient;
        public float DiffuseContrib;
        public float SpecularPOW;
    }
}

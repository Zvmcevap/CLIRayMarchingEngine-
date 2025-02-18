using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model.Primitives
{
    /// <summary>
    /// Floor class to add floor to the scene, not movable
    /// </summary>
    public class PlaneGO : GameObject, IDrawable
    {
        public Vector3 Position { get; set; } = new Vector3(0f, -2f, 0f);
        public Vector3 Rotation { get; set; } = new Vector3(0, 1f, 0);
        public LightConfig LightConfig { get; set; } = new() { Ambient = 0.2f, DiffuseContrib = 0.5f, SpecularPOW = 0f };

        public PlaneGO() { }
        public PlaneGO(Vector3 position)
        {
            Position = position;
        }

        public override void OnUpdate(float deltatime) { }

        public float GetSDFDistance(Vector3 p)
        {
            return Vector3.Dot(p, Rotation) + (float)Math.Sqrt(Position.X * Position.X + Position.Y * Position.Y + Position.Z * Position.Z);
        }
    }
}

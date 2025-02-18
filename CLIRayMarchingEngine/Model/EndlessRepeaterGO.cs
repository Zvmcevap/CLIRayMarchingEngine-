using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.Model.Primitives;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model
{
    /// <summary>
    /// Infinite primitives in every direction :) (PITA to debug) - only one without the builder class, because I give up
    /// </summary>
    public class EndlessRepeaterGO : MovableGO, IDrawable
    {
        public IDrawable Primitive { get; set; }

        public float RepeatDist { get; set; } = 15f;

        public LightConfig LightConfig { get; set; }

        private readonly PlaneGO? _floor;
        public float Smoothing { get; set; } = 10f;
        public EndlessRepeaterGO(Vector3 position, IDrawable primitive, PlaneGO? floor=null) : base()
        {
            Position = position;
            Primitive = primitive;
            MovableGO prim = (MovableGO)Primitive;
            prim.Parent = this.Parent;
            LightConfig = primitive.LightConfig;

            _floor = floor;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            MovableGO prim = (MovableGO)Primitive;
            prim.OnUpdate(deltaTime);
        }

        public float GetSDFDistance(Vector3 p)
        {
            Vector3 oldp = p;
            p = Vector3.Transform(p, InverseTrans);
            Vector3 q = p - RepeatDist * new Vector3(
                MathF.Floor((p.X + 0.5f * RepeatDist) / RepeatDist),
                MathF.Floor((p.Y + 0.5f * RepeatDist) / RepeatDist),
                MathF.Floor((p.Z + 0.5f * RepeatDist) / RepeatDist)
            );

            float primDist = Primitive.GetSDFDistance(q);

            if (_floor != null)
            {
                return HelpMaths.SmoothMin(primDist, _floor.GetSDFDistance(oldp), Smoothing);
            }
            return primDist;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model.Primitives
{
    /// <summary>
    /// A 3D hypercube
    /// </summary>
    public class BoxGO : MovableGO, IDrawable
    {
        public Vector3 Size { get; set; }
        public float CornerRadius { get; set; }
        public BoxGO() : base() { }

        public LightConfig LightConfig { get; set; }

        public float GetSDFDistance(Vector3 p)
        {
            p = Vector3.Transform(p, InverseTrans);
            Vector3 q = Vector3.Abs(p) - Size + new Vector3(CornerRadius);
            float outsideDist = new Vector3(
                MathF.Max(q.X, 0),
                MathF.Max(q.Y, 0),
                MathF.Max(q.Z, 0)
            ).Length();

            float insideDist = MathF.Min(MathF.Max(q.X, MathF.Max(q.Y, q.Z)), 0);

            return outsideDist + insideDist - CornerRadius;
        }

        public class Builder : Builder<BoxGO>
        {
            private float _cornerRadius = 0f;
            private Vector3 _size = Vector3.One;
            private LightConfig _lightConfig = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };

            public Builder SetLightConfig(LightConfig lightConfig)
            {
                _lightConfig = lightConfig;
                return this;
            }
            public Builder SetSize(Vector3 size)
            {
                _size = size;
                return this;
            }
            public Builder SetCornerRadius(float cornerRadius)
            {
                _cornerRadius = cornerRadius;
                return this;
            }

            public override BoxGO Build()
            {
                var movableGO = base.Build();
                var boxGO = new BoxGO()
                {
                    Size = _size,
                    CornerRadius = _cornerRadius,
                    LightConfig = _lightConfig,
                    Position = movableGO.Position,
                    Rotation = movableGO.Rotation,
                    MoveSpeed = movableGO.MoveSpeed,
                    RotateSpeed = movableGO.RotateSpeed,
                    IsMoving = movableGO.IsMoving,
                    MoveBySpeed = movableGO.MoveBySpeed,
                    InitialPosition = movableGO.InitialPosition,
                    TargetPosition = movableGO.TargetPosition,
                    LerpFunc = movableGO.LerpFunc
                };
                return boxGO;
            }
        }
    }
}

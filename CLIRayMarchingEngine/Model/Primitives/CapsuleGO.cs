using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model.Primitives
{
    public class CapsuleGO : MovableGO, IDrawable
    {
        public float Height { get; set; } = 1f;
        public float Radius { get; set; } = 0.5f;

        public CapsuleGO() { }

        public LightConfig LightConfig { get; set; } = new() { Ambient = 0f, DiffuseContrib = 0.7f, SpecularPOW = 32f };

        public float GetSDFDistance(Vector3 p)
        {
            p = Vector3.Transform(p, InverseTrans);
            float yDist = MathF.Max(0, MathF.Abs(p.Y) - Height * 0.5f);

            float cylDist = MathF.Sqrt(p.X * p.X + p.Z * p.Z);
            return MathF.Sqrt(cylDist * cylDist + yDist * yDist) - Radius;
        }

        public class Builder : Builder<CapsuleGO>
        {
            private float _height = 1f;
            private float _radius = 0.5f;

            private LightConfig _lightConfig = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };

            public Builder SetLightConfig(LightConfig lightConfig)
            {
                _lightConfig = lightConfig;
                return this;
            }
            public Builder SetHeight(float height)
            {
                _height = height;
                return this;
            }

            public Builder SetRadius(float radius)
            {
                _radius = radius;
                return this;
            }

            public override CapsuleGO Build()
            {
                var movableGO = base.Build();
                var capsuleGO = new CapsuleGO()
                {
                    Radius = _radius,
                    Height = _height,
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
                return capsuleGO;
            }
        }
    }

}


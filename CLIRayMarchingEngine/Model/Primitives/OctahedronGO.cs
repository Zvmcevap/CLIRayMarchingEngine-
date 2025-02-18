using System.Numerics;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model.Primitives
{
    public class OctahedronGO : MovableGO, IDrawable
    {
        public float Size { get; set; } = 1f;

        public LightConfig LightConfig { get; set; }

        public OctahedronGO() : base() { }

        public float GetSDFDistance(Vector3 p)
        {
            p = Vector3.Transform(p, InverseTrans);

            // Compute Octahedron SDF
            p = Vector3.Abs(p); return (p.X + p.Y + p.Z - Size) * 0.57735027f;
        }

        public class Builder : Builder<OctahedronGO>
        {
            private float _size = 1f;
            private LightConfig _lightConfig = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };

            public Builder SetLightConfig(LightConfig lightConfig)
            {
                _lightConfig = lightConfig;
                return this;
            }
            public Builder SetSize(float size)
            {
                _size = size;
                return this;
            }

            public override OctahedronGO Build()
            {
                var movableGO = base.Build();
                var octaGO = new OctahedronGO()
                {
                    Size = _size,
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
                return octaGO;
            }
        }
    }
}

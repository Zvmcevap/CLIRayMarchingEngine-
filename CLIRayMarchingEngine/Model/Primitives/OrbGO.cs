using System.Numerics;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model.Primitives
{
    // Nice and boring sphere
    public class OrbGO : MovableGO, IDrawable
    {
        public float Radius { get; set; } = 1f;
        public LightConfig LightConfig { get; set; } = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };
        public OrbGO() { }

        public float GetSDFDistance(Vector3 p)
        {
            p = Vector3.Transform(p, InverseTrans);
            return Vector3.Distance(p, Position) - Radius;
        }

        public class Builder : Builder<OrbGO>
        {
            private float _radius = 1f;
            private LightConfig _lightConfig = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };

            public Builder SetLightConfig(LightConfig lightConfig)
            {
                _lightConfig = lightConfig;
                return this;
            }
            public Builder SetRadius(float radius)
            {
                _radius = radius;
                return this;
            }

            public override OrbGO Build()
            {
                var movableGO = base.Build();
                var orbGO = new OrbGO()
                {
                    Radius = _radius,
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
                return orbGO;
            }
        }

    }
}

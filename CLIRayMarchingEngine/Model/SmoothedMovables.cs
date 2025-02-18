using System.Numerics;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model
{
    // Drawable Game Object that enables blending between objects
    public class SmoothedMovables : MovableGO, IDrawable
    {
        public float Smoothing { get; set; }
        public LightConfig LightConfig { get; set; }
        List<MovableGO> Movables { get; } = new List<MovableGO>();
        List<IDrawable> Drawables { get; } = new List<IDrawable>();

        //private float[] _distances; -- womp womp, caching doesn't work - raymarch is multithreaded

        public void AddGameObject(params GameObject[] gameObjects)
        {
            Movables.AddRange(gameObjects.Where(x => x is MovableGO).Select(x => (MovableGO)x));
            Drawables.AddRange(gameObjects.Where(x => x is IDrawable).Select(x => (IDrawable)x));
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            foreach (MovableGO mov in Movables)
            {
                mov.OnUpdate(deltaTime);
            }
        }

        public float GetSDFDistance(Vector3 p)
        {
            if (Drawables.Count == 0) return 0f;

            p = Vector3.Transform(p, InverseTrans);
            float[] distances = new float[Drawables.Count];
            // Compute distances
            for (int i = 0; i < Drawables.Count; i++)
            {
                distances[i] = Drawables[i].GetSDFDistance(p);
            }
            Array.Sort(distances);

            float sdfSmoothed = distances[0];
            for (int i = 1; i < Drawables.Count; i++)
            {
                sdfSmoothed = HelpMaths.SmoothMin(sdfSmoothed, distances[i], Smoothing);
            }
            return sdfSmoothed;
        }

        public class Builder : Builder<SmoothedMovables>
        {
            private float _smoothing = 0f;
            private LightConfig _lightConfig = new() { DiffuseContrib = 0.7f, SpecularPOW = 32f };

            public Builder SetLightConfig(LightConfig lightConfig)
            {
                _lightConfig = lightConfig;
                return this;
            }
            public Builder SetSmoothing(float smoothing)
            {
                _smoothing = smoothing;
                return this;
            }

            public override SmoothedMovables Build()
            {
                var movableGO = base.Build();
                SmoothedMovables sm = new SmoothedMovables()
                {
                    Smoothing = _smoothing,
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

                return sm;
            }
        }
    }
}

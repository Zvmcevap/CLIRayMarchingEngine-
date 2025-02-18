using System.Numerics;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine.Model
{
    /// <summary>
    /// Holds all drawable and movable objects of a scene,
    /// Holds the position of the light,
    /// Can move around, emulating the movement of the camera.
    /// </summary>
    public class Scene : MovableGO
    {
        // All objects
        public List<GameObject> GameObjects { get; } = new();
        // All drawable objects
        public List<IDrawable> Drawables { get; } = new();
        // All movable objects
        public List<MovableGO> Movables { get; } = new();

        public Vector3 LightPosition { get; set; } = new Vector3(5f, 10f, 0f);
        public Vector3 LightRotateSpeed { get; set; }
        public bool RotateLight { get; set; } = false;
        public Vector3 Velocity { get; set; } = Vector3.Zero; // Moves the scene around

        public Scene() { }

        public Scene(Vector3 lightPos)
        {
            LightPosition = lightPos;
        }

        public override void OnUpdate(float deltaTime)
        {
            Move(deltaTime);
            UpdateTransformMatrix();
            GameObjects.ForEach(go => go.OnUpdate(deltaTime));
            if (RotateLight)
            {
                LightPosition = HelpMaths.RotateZPoint(LightPosition, LightRotateSpeed.Z * deltaTime);
                LightPosition = HelpMaths.RotateXPoint(LightPosition, LightRotateSpeed.X * deltaTime);
                LightPosition = HelpMaths.RotateYPoint(LightPosition, LightRotateSpeed.Y * deltaTime);
            }
        }

        public new void Move(float deltaTime)
        {
            Position -= Velocity * MoveSpeed * deltaTime;
        }

        public void AddGameObject(params GameObject[] gameObjects)
        {
            foreach (var go in gameObjects)
            {
                GameObjects.Add(go);
                if (go is IDrawable drawable)
                {
                    Drawables.Add(drawable);
                }
                if (go is MovableGO movable)
                {
                    movable.Parent = this;
                    Movables.Add(movable);
                }
            }
        }

        /// <summary>
        /// Builder pattern for Scene.
        /// </summary>
        public class Builder : Builder<Scene>
        {
            private Vector3 _lightPosition = new Vector3(5f, 10f, 0f);
            private Vector3 _lightRotateSpeed = Vector3.Zero;
            private bool _rotateLight = false;

            public Builder SetLightPosition(Vector3 lightPosition)
            {
                _lightPosition = lightPosition;
                return this;
            }

            public Builder SetLightRotateSpeed(Vector3 lightRotateSpeed)
            {
                _lightRotateSpeed = lightRotateSpeed;
                return this;
            }

            public Builder SetRotateLight(bool rotateLight)
            {
                _rotateLight = rotateLight;
                return this;
            }

            public override Scene Build()
            {
                var scene = base.Build();
                scene.LightPosition = _lightPosition;
                scene.LightRotateSpeed = _lightRotateSpeed;
                scene.RotateLight = _rotateLight;
                return scene;
            }
        }
    }
}

namespace CLIRayMarchingEngine.Model
{
    public abstract class GameObject
    {
        public MovableGO Parent { get; set; }
        /// <summary>
        /// Simulation (or "model") Update per frame
        /// </summary>
        /// <param name="deltatime"></param>
        public abstract void OnUpdate(float deltatime);

    }
}

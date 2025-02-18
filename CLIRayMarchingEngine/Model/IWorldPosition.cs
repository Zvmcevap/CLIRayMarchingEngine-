using System.Numerics;

namespace CLIRayMarchingEngine.Model
{
    public interface IWorldPosition
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}

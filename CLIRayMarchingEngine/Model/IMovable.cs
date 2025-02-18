using System.Numerics;

namespace CLIRayMarchingEngine.Model
{
    public interface IMovable
    {
        public float MoveSpeed { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Matrix4x4 TransformMatrix { get; set; }
        public void Move(float deltatime);
        public void UpdateTransformMatrix();
    }
}

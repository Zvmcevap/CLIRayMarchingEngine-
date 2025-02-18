using System;
using System.Numerics;

namespace CLIRayMarchingEngine.Model
{
    /// <summary>
    /// 3D objects that can move, either by speed per second or interpolation between two points
    /// Can also rotate and follow their parent object.
    /// </summary>
    public class MovableGO : GameObject, IMovable, IWorldPosition
    {
        public Matrix4x4 TransformMatrix { get; set; } = Matrix4x4.Identity;
        public Matrix4x4 InverseTrans = Matrix4x4.Identity;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float MoveSpeed { get; set; }
        public Vector3 RotateSpeed { get; set; }
        public bool IsMoving { get; set; }
        public bool MoveBySpeed { get; set; }

        private float _t = 0f;
        public Vector3 InitialPosition { get; set; }
        public Vector3 TargetPosition { get; set; }

        public Func<Vector3, Vector3, float, Vector3> LerpFunc = Vector3.Lerp;

        internal MovableGO() { }

        public void Move(float deltaTime)
        {
            if (!IsMoving) return;

            Rotation += RotateSpeed * deltaTime;
            if (MoveBySpeed)
            {
                Position += Vector3.Normalize(TargetPosition - Position) * MoveSpeed * deltaTime;
                if (Vector3.DistanceSquared(TargetPosition, Position) < 0.001f)
                {
                    (InitialPosition, TargetPosition) = (TargetPosition, InitialPosition);
                }
            }
            else
            {
                _t += deltaTime * MoveSpeed;
                if (_t >= 1f)
                {
                    (InitialPosition, TargetPosition) = (TargetPosition, InitialPosition);
                    _t = 0f;
                }
                Position = LerpFunc(InitialPosition, TargetPosition, _t);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            Move(deltaTime);
            UpdateTransformMatrix();
        }

        public virtual void UpdateTransformMatrix()
        {
            // Create translation matrix
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(Position);

            // Create rotation matrix
            float toRadians = MathF.PI / 180f;
            Matrix4x4 rotationX = Matrix4x4.CreateRotationX(Rotation.X * toRadians);
            Matrix4x4 rotationY = Matrix4x4.CreateRotationY(Rotation.Y * toRadians);
            Matrix4x4 rotationZ = Matrix4x4.CreateRotationZ(Rotation.Z * toRadians);
            Matrix4x4 rotationalMatrix = rotationZ * rotationX * rotationY;

            // Apply transformations (rotation first, then translation)
            TransformMatrix = rotationalMatrix * translationMatrix;

            // Apply parent transformation if applicable
            if (Parent != null)
            {
                //TransformMatrix = Parent.TransformMatrix * TransformMatrix;
                // Extract parent's rotation (ignoring translation)
                Matrix4x4 parentRotationMatrix = Parent.TransformMatrix;
                parentRotationMatrix.Translation = Vector3.Zero; // Remove parent's translation

                // Apply parent's rotation first, then local transform
                TransformMatrix = parentRotationMatrix * TransformMatrix;

                // Apply parent's translation
                TransformMatrix = TransformMatrix with
                {
                    M41 = TransformMatrix.M41 + Parent.TransformMatrix.M41,
                    M42 = TransformMatrix.M42 + Parent.TransformMatrix.M42,
                    M43 = TransformMatrix.M43 + Parent.TransformMatrix.M43
                };
            }
            Matrix4x4.Invert(TransformMatrix, out InverseTrans);
        }

        /// <summary>
        /// Builder pattern
        /// https://en.wikipedia.org/wiki/Builder_pattern
        /// </summary>
        public class Builder<T> where T : MovableGO, new()
        {
            private Vector3 _position = Vector3.Zero;
            private Vector3 _rotation = Vector3.Zero;
            private float _moveSpeed = 0f;
            private Vector3 _rotateSpeed = Vector3.Zero;
            private bool _isMoving = false;
            private bool _moveBySpeed = false;
            private Vector3 _targetPosition = Vector3.Zero;
            private Func<Vector3, Vector3, float, Vector3> _lerpFunc = Vector3.Lerp;

            private MovableGO _parent;

            public Builder<T> SetParent(MovableGO parent)
            {
                _parent = parent; 
                return this;
            }

            public Builder<T> SetPosition(Vector3 position)
            {
                _position = position;
                return this;
            }

            public Builder<T> SetRotation(Vector3 rotation)
            {
                _rotation = rotation;
                return this;
            }

            public Builder<T> SetMoveSpeed(float speed)
            {
                _moveSpeed = speed;
                return this;
            }

            public Builder<T> SetRotateSpeed(Vector3 rotateSpeed)
            {
                _rotateSpeed = rotateSpeed;
                return this;
            }

            public Builder<T> SetMoving(bool isMoving)
            {
                _isMoving = isMoving;
                return this;
            }

            public Builder<T> SetMoveBySpeed(bool moveBySpeed)
            {
                _moveBySpeed = moveBySpeed;
                return this;
            }

            public Builder<T> SetTargetPosition(Vector3 targetPosition)
            {
                _targetPosition = targetPosition;
                return this;
            }

            public Builder<T> SetLerpFunction(Func<Vector3, Vector3, float, Vector3> lerpFunc)
            {
                _lerpFunc = lerpFunc;
                return this;
            }

            public virtual T Build()
            {
                var obj = new T()
                {
                    Position = _position,
                    Rotation = _rotation,
                    MoveSpeed = _moveSpeed,
                    RotateSpeed = _rotateSpeed,
                    IsMoving = _isMoving,
                    MoveBySpeed = _moveBySpeed,
                    InitialPosition = _position,
                    TargetPosition = _targetPosition,
                    LerpFunc = _lerpFunc,

                    Parent = _parent
                };
                return obj;
            }
        }
    }
}

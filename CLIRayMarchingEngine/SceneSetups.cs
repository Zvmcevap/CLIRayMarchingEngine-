using System.Numerics;
using CLIRayMarchingEngine.Model;
using CLIRayMarchingEngine.Model.Primitives;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine
{
    /// <summary>
    /// Helper Class to help me setup some scenes "real" quick
    /// </summary>
    public static class SceneSetups
    {
        /// <summary>
        /// Scene sin smoothing - the boring one
        /// </summary>
        /// <returns></returns>
        public static Scene GetSceneOne()
        {
            Scene scene = new Scene.Builder()
                .SetLightPosition(new(5f, 15f, -5f))
                .SetLightRotateSpeed(new(0f, 2f, 0f))
                .SetMoveSpeed(60f)
                .Build();

            BoxGO box = new BoxGO.Builder()
                .SetSize(new(.5f, 1.5f, .2f))
                .SetCornerRadius(0.2f)
                .SetPosition(new(-2f, 0f, 1f))
                .SetTargetPosition(new(2f, 0f, 1f))
                .SetMoveBySpeed(false)
                .SetMoveSpeed(0.1f)
                .SetLerpFunction(HelpMaths.EaseInOutQuad)
                .SetRotateSpeed(new(45f, 15f, 0f))
                .SetMoving(true)
                .SetParent(scene)
                .Build();

            CapsuleGO capsule = new CapsuleGO.Builder()
                .SetPosition(new Vector3(-4f, -2f, 3f))
                .SetTargetPosition(new Vector3(-4f, 2f, 0f))
                .SetMoveSpeed(.5f)
                .SetMoving(true)
                .SetLerpFunction(HelpMaths.EaseInOutQuad)
                .SetParent(scene)
                .Build();

            OctahedronGO oct = new OctahedronGO.Builder()
                .SetLightConfig(new LightConfig() { Ambient = 0.2f, DiffuseContrib = 0.5f, SpecularPOW = 3f })
                .SetParent(scene)
                .SetPosition(new(3f, 1f, 0f))
                .SetRotateSpeed(new(0f, 45f, 0f))
                .SetMoving(true)
                .Build();


            OrbGO orb = new OrbGO.Builder()
                .SetRadius(0.4f)
                .SetParent(scene)
                .SetMoving(true)
                .SetMoveBySpeed(false)
                .SetTargetPosition(new(-2f, -2f, 2f))
                .SetMoveSpeed(0.1f)
                .SetMoveBySpeed(false)
                .SetLerpFunction(HelpMaths.EaseInOutCubic)
                .Build();

            PlaneGO floor = new();

            scene.AddGameObject(oct, floor, capsule, box, orb);
            return scene;
        }

        /// <summary>
        /// Scene with Smoothing
        /// </summary>
        /// <returns></returns>
        public static Scene GetSceneTwo()
        {
            Scene scene = new Scene.Builder()
                .SetLightPosition(new(5f, 15f, -5f))
                .SetLightRotateSpeed(new(0f, 2f, 0f))
                .SetMoveSpeed(60f)
                .Build();

            SmoothedMovables extraSmooth = new SmoothedMovables.Builder()
                .SetSmoothing(1.5f)
                .SetParent(scene)
                .Build();


            BoxGO box = new BoxGO.Builder()
                .SetSize(new(.5f, 1.5f, .2f))
                .SetCornerRadius(0.2f)
                .SetPosition(new(-2f, 0f, 1f))
                .SetTargetPosition(new(2f, 0f, 1f))
                .SetMoveBySpeed(false)
                .SetMoveSpeed(0.1f)
                .SetLerpFunction(HelpMaths.EaseInOutQuad)
                .SetRotateSpeed(new(45f, 15f, 0f))
                .SetMoving(true)
                .SetParent(extraSmooth)
                .Build();

            CapsuleGO capsule = new CapsuleGO.Builder()
                .SetPosition(new Vector3(-4f, -2f, 3f))
                .SetTargetPosition(new Vector3(-4f, 2f, 0f))
                .SetMoveSpeed(.5f)
                .SetMoving(true)
                .SetLerpFunction(HelpMaths.EaseInOutQuad)
                .SetParent(extraSmooth)
                .Build();

            OctahedronGO oct = new OctahedronGO.Builder()
                .SetLightConfig(new LightConfig() { Ambient = 0.2f, DiffuseContrib = 0.5f, SpecularPOW = 3f })
                .SetParent(extraSmooth)
                .SetPosition(new(3f, 1f, 0f))
                .SetRotateSpeed(new(0f, 45f, 0f))
                .SetMoving(true)
                .Build();


            OrbGO orb = new OrbGO.Builder()
                .SetRadius(0.4f)
                .SetParent(extraSmooth)
                .SetMoving(true)
                .SetMoveBySpeed(false)
                .SetTargetPosition(new(-2f, -2f, 2f))
                .SetMoveSpeed(0.1f)
                .SetMoveBySpeed(false)
                .SetLerpFunction(HelpMaths.EaseInOutCubic)
                .Build();

            PlaneGO floor = new();

            extraSmooth.AddGameObject(box, capsule, orb, oct, floor);

            scene.AddGameObject(extraSmooth);
            return scene;
        }

        /// <summary>
        /// Endless Repeater Scene
        /// </summary>
        /// <returns></returns>
        public static Scene GetSceneThree()
        {
            Scene scene = new Scene.Builder()
                .SetLightPosition(new(5f, 15f, -15f))
                .SetLightRotateSpeed(new(0f, 2f, 0f))
                .SetRotateLight(false)
                .SetMoveSpeed(60f)
                .Build();


            scene.AddGameObject(new EndlessRepeaterGO(
                Vector3.Zero,
                (IDrawable)new BoxGO.Builder()
                    .SetSize(new(0.5f, 1.3f, 1.2f))
                    .SetPosition(new(0f, 2f, 0f))
                    .SetTargetPosition(new(0f, -2f, 0f))
                    .SetMoveSpeed(0.3f)
                    .SetRotateSpeed(new(0f, 90f, 45f))
                    .SetMoveBySpeed(false)
                    .SetMoving(true)
                    .SetLerpFunction(HelpMaths.EaseInOutQuad)
                    .Build(),
                new PlaneGO())
            {
                Parent = scene,
                Smoothing = 3f,
                RepeatDist = 10f,
                IsMoving = true,
                RotateSpeed = new Vector3(15f, 0f, 0f),
                LerpFunc = HelpMaths.EaseInOutCubic
            });

            return scene;
        }
    }
}

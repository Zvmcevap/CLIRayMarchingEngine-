using System.Numerics;

namespace CLIRayMarchingEngine.Controls
{
    public class UserControls : ControlsBase
    {
        public override void HandleEscape()
        {
            _appManager.Quit();
        }
        public override void HandleSpacebar()
        {
            int currentIndex = _appManager.Scenes.IndexOf(_appManager.CurrentScene);
            _appManager.CurrentScene = _appManager.Scenes[(currentIndex + 1) % _appManager.Scenes.Count];
        }
        public override void HandleA()
        {
            Vector3 vel = _appManager.CurrentScene.Velocity;
            _appManager.CurrentScene.Velocity = new(-1f, vel.Y, vel.Z);
        }

        public override void HandleD()
        {
            Vector3 vel = _appManager.CurrentScene.Velocity;
            _appManager.CurrentScene.Velocity = new(1f, vel.Y, vel.Z);
        }

        public override void HandleE()
        {
            _appManager.CurrentScene.RotateLight = true;
        }


        public override void HandleQ()
        {
            _appManager.CurrentScene.RotateLight = false;
        }

        public override void HandleS()
        {
            Vector3 vel = _appManager.CurrentScene.Velocity;
            _appManager.CurrentScene.Velocity = new(vel.X, vel.Y, -1f);
        }


        public override void HandleW()
        {
            Vector3 vel = _appManager.CurrentScene.Velocity;
            _appManager.CurrentScene.Velocity = new(vel.X, vel.Y, 1f);
        }

        public void PrintControls()
        {
            int y = _appManager.Height + 6;
            "Controls: Spacebar: Switch to the next scene, WASD: to move, QE to rotate light, Esc to exit".PrintOnScreen(0, y);
        }
    }
}

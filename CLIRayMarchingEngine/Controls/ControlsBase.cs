
namespace CLIRayMarchingEngine.Controls
{
    /// <summary>
    /// Abstract class to hide the switch statement
    /// </summary>
    public abstract class ControlsBase
    {
        protected readonly AppManager _appManager = AppManager.Instance;

        public void HandleInput(string input)
        {
            switch (input)
            {
                case "Escape":
                    HandleEscape();
                    break;
                case "Spacebar":
                    HandleSpacebar();
                    break;
                case "Q":
                    HandleQ();
                    break;
                case "W":
                    HandleW();
                    break;
                case "E":
                    HandleE();
                    break;
                case "A":
                    HandleA();
                    break;
                case "S":
                    HandleS();
                    break;
                case "D":
                    HandleD();
                    break;
                default:
                    return;
            }
        }

        public abstract void HandleEscape();
        public abstract void HandleSpacebar();
        public abstract void HandleQ();
        public abstract void HandleW();
        public abstract void HandleE();
        public abstract void HandleA();
        public abstract void HandleS();
        public abstract void HandleD();
    }
}

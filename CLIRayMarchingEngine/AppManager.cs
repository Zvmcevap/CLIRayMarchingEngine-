using System.Diagnostics;
using System.Numerics;
using System.Text;
using CLIRayMarchingEngine.Controls;
using CLIRayMarchingEngine.Model;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine
{
    public sealed class AppManager
    {
        public float DeltaTime { get; private set; }
        public static double TotalTime { get; private set; }
        public Vector3 CameraPos { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Canvas Canvas { get; private set; }

        public UserControls Controls { get; private set; }

        public List<Scene> Scenes { get; } = new();

        public Scene CurrentScene;

        #region Private Vars
        private bool _running;
        private float _profilerUpdateTime;
        private float _profilerTimer;
        private Stopwatch _sw;
        private StringBuilder _sb;
        #endregion
        #region Singleton stuff
        private AppManager()
        {
            _instance = this;
            _sw = new Stopwatch();
            _sb = new StringBuilder();

            Controls = new UserControls();

            Scenes.Add(SceneSetups.GetSceneOne());
            Scenes.Add(SceneSetups.GetSceneTwo());
            Scenes.Add(SceneSetups.GetSceneThree());
            CurrentScene = Scenes[0];
        }

        private static AppManager _instance;
        public static AppManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    new AppManager();
                }
                return _instance;
            }
        }

        #endregion

        public void Init(int width, int height, float profilerUpdateTimeMilliseconds, Vector3 camPos)
        {
            Console.BufferHeight = Console.LargestWindowHeight;
            Console.BufferWidth = Console.LargestWindowWidth;
            Console.SetWindowSize(width, height);

            Width = width - 10;
            Height = height - 10;

            CameraPos = camPos;
            Canvas = new Canvas(Width, Height);

            _profilerUpdateTime = profilerUpdateTimeMilliseconds;
            _running = true;

            Controls.PrintControls();
        }
        /// <summary>
        /// Main Loop
        /// </summary>
        public void Run()
        {
            if (CurrentScene == null)
            {
                return;
            }
            while (_running)
            {

                #region Check Input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    key.ToString().PrintOnScreen();
                    Controls.HandleInput(key.ToString());
                }
                else
                {
                    CurrentScene.Velocity = Vector3.Zero;
                }
                #endregion

                #region Calc DeltaTime, Clear and Draw Canvas
                _sw.Stop();
                DeltaTime = (float)_sw.Elapsed.TotalMilliseconds / 1000f;
                TotalTime += DeltaTime;
                _sw.Restart();
                DrawCanvas();
                #endregion

                #region Update Profiler
                if (_profilerTimer >= _profilerUpdateTime)
                {
                    $"\tFPS: {1f / DeltaTime:F2}; {DeltaTime * 1000:F2}ms".PrintOnScreen(0, 1);
                    _profilerTimer -= _profilerUpdateTime;
                }
                _profilerTimer += DeltaTime;
                #endregion

                #region Updates

                // Update Simulation
                CurrentScene.OnUpdate(DeltaTime);

                // Draw On Screen
                Canvas.OnUpdate(CurrentScene);

                #endregion

            }
        }

        private void DrawCanvas()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 3);

            string canvaString = Canvas.GetStringWBorder();
            Console.Write(canvaString);
        }

        public void Quit()
        {
            _running = false;
            Console.CursorVisible = true;
        }

    }
}

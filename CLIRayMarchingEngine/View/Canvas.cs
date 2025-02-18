using System.Numerics;
using System.Text;
using CLIRayMarchingEngine.Model;

namespace CLIRayMarchingEngine.View
{
    /// <summary>
    /// To which to paint on, makes a box in terminal and treats it like a monitor.
    /// Fires off ray marcher instances as if it's the step before the fragment shader.
    /// </summary>
    public class Canvas
    {
        public static readonly char[] Lumen = { ' ', '.', ':', '-', '=', '+', 'X', '%', '#', '@' };
        public static float LumenMax = Lumen.Length - 1f;
        private readonly StringBuilder _sb;

        private readonly char[,] _canvas;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private RayMarcher[,] _rayMarchers;

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;
            _canvas = new char[height, width];
            _rayMarchers = new RayMarcher[height, width];

            _sb = new StringBuilder();
            Init();
        }

        public char this[int y, int x]
        {
            get => _canvas[y, x];
            set => _canvas[y, x] = value;
        }

        /// <summary>
        /// Fill the canvas with spaces
        /// Add ray caster, sample Height half as much because Chars are twice as tall as they are wide
        /// </summary>
        private void Init()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _canvas[y, x] = ' ';

                    // Ray calculations
                    float uvX = (x * 2f - Width) / (float)Height;
                    float uvY = (y * 2f - Height) / (float)Height * 2f; // Twice as much here, apparently
                    float fov = 0.2f;
                    Vector3 rayDirection = Vector3.Normalize(new Vector3(uvX * fov, -uvY * fov, 1f));
                    _rayMarchers[y, x] = new RayMarcher(AppManager.Instance.CameraPos, rayDirection);
                }
            }
        }

        public void OnUpdate(Scene scene)
        {
            Parallel.For(0, Height, (y) =>
            {
                for (int x = 0; x < Width; x++)
                {
                    float brigthness = _rayMarchers[y, x].MarchRay(scene);
                    brigthness = Math.Clamp(brigthness, 0f, 1f);
                    _canvas[y, x] = Lumen[(int)(brigthness * LumenMax)];
                }
            });
        }



        public string GetStringWBorder(string prependex = "")
        {
            _sb.Clear();
            _sb.Append("┌" + new string('─', Width) + "┐\n"); // Top
            for (int y = 0; y < Height; y++) // Main
            {
                _sb.Append(prependex);
                _sb.Append("|");
                for (int x = 0; x < Width; x++)
                {
                    _sb.Append(_canvas[y, x]);
                }
                _sb.Append("|\n");
            }

            _sb.Append("└" + new string('─', Width) + "┘"); // Bot

            return _sb.ToString();
        }
    }
}

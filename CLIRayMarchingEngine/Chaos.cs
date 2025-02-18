using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.Model;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine
{
    internal class Chaos : GameObject, IDrawable
    {
        public LightConfig LightConfig { get; set; } = new() { Ambient = 0f, DiffuseContrib = 0.7f, SpecularPOW = 0f };

        public float LerpFactor { get; set; }
        Random _rng = new Random();
        Vector3[] _mainPoints = new Vector3[3];
        Vector3 _current;
        private int _previousIndex = -1;

        public int NumberOfPoints { get; private set; }
        public Vector3 Color { get; set; }

        public Chaos(float lerpFactor, params Vector3[] points)
        {
            _mainPoints = points;
            LerpFactor = lerpFactor;
            float l = (float)points.Length;
            _current = new Vector3(_mainPoints.Select(v => v.X).Sum() / l, _mainPoints.Select(v => v.Y).Sum() / l, 0f);
        }
        public override void OnUpdate(float deltatime)
        {

            int index = _rng.Next(_mainPoints.Length);
            while (_mainPoints.Length >= 4 && index == _previousIndex)
            {
                index = _rng.Next(_mainPoints.Length);
            }
            _previousIndex = index;
            _current = Vector3.Lerp(_current, _mainPoints[index], LerpFactor);
        }

        public void DrawOnCanvas(Canvas canvas)
        {
            Vector3 rgb = new(_current.X/(float)canvas.Width, _current.Y/(float)canvas.Height, 0f);
            canvas[(int)Math.Round(_current.Y), (int)Math.Round(_current.X)] = '▲';
            foreach (var point in _mainPoints)
            {
                canvas[(int)Math.Round(point.Y), (int)Math.Round(point.X)] = 'X';
            }
        }

        public Vector3 CalcDistance(Vector3 position)
        {
            return _current;
        }

        public float GetSDFDistance(Vector3 p)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.View;

namespace CLIRayMarchingEngine
{
    /// <summary>
    /// Static class with helpful maths functions
    /// </summary>
    public static class HelpMaths
    {
        /// <summary>
        /// DFS Smoothmin https://www.shadertoy.com/view/Ml3Gz8
        /// Used to blend between close objects
        /// </summary>
        /// <param name="a">First objects SDF value</param>
        /// <param name="b">Second objects SDF value</param>
        /// <param name="k">Blend amount, bigger numbers offer higher value</param>
        /// <returns></returns>
        public static float SmoothMin(float a, float b, float k)
        {
            float h = Math.Clamp(0.5f + 0.5f * (a - b) / k, 0.0f, 1.0f);
            float mix = Lerp(a, b, h);
            return mix - k * h * (1.0f - h);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Blinn-Phong light model https://en.wikipedia.org/wiki/Blinn%E2%80%93Phong_reflection_model
        /// </summary>
        /// <param name="rayDirection"></param>
        /// <param name="normal"></param>
        /// <param name="lightDir"></param>
        /// <param name="lightConfig"></param>
        /// <returns></returns>
        public static float CalculateLight(Vector3 rayDirection, Vector3 normal, Vector3 lightDir, LightConfig lightConfig)
        {

            Vector3 reflectDir = Vector3.Reflect(lightDir, normal);

            // Diffuse
            float diffuse = lightConfig.DiffuseContrib * Math.Max(0f, Vector3.Dot(normal, lightDir));

            // Specular
            float specular = 0f;
            if (lightConfig.SpecularPOW > 0f)
            {
                specular = (float)Math.Pow(Math.Max(0f, Vector3.Dot(rayDirection, reflectDir)), lightConfig.SpecularPOW);
            }

            // Final lighting
            return lightConfig.Ambient + diffuse + specular;

        }

        #region Rotations
        public static Vector3 RotateXPoint(this Vector3 original, float angle)
        {
            float cosTheta = MathF.Cos(angle);
            float sinTheta = MathF.Sin(angle);

            return new Vector3(
                original.X,
                original.Y * cosTheta - original.Z * sinTheta,
                original.Y * sinTheta + original.Z * cosTheta
            );
        }

        #region Lerp functions aka Easing functions

        // Ease In (Quadratic)
        public static Vector3 EaseInQuad(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t * t);
        }

        // Ease Out (Quadratic)
        public static Vector3 EaseOutQuad(Vector3 start, Vector3 end, float t)
        {
            t = 1 - (1 - t) * (1 - t); // Quadratic easing out
            return Vector3.Lerp(start, end, t);
        }

        // Ease InOut (Quadratic)
        public static Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float t)
        {
            t = t < 0.5 ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2;
            return Vector3.Lerp(start, end, t);
        }

        // Ease In (Cubic)
        public static Vector3 EaseInCubic(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t * t * t);
        }

        // Ease Out (Cubic)
        public static Vector3 EaseOutCubic(Vector3 start, Vector3 end, float t)
        {
            t = 1 - MathF.Pow(1 - t, 3);
            return Vector3.Lerp(start, end, t);
        }

        // Ease InOut (Cubic)
        public static Vector3 EaseInOutCubic(Vector3 start, Vector3 end, float t)
        {
            t = t < 0.5 ? 4 * t * t * t : 1 - MathF.Pow(-2 * t + 2, 3) / 2;
            return Vector3.Lerp(start, end, t);
        }

        // Ping Pong (Moves forward and then backward)
        public static Vector3 PingPong(Vector3 start, Vector3 end, float t)
        {
            float pingPongT = 1f - Math.Abs(t * t % 2 - 1f);
            return Vector3.Lerp(start, end, pingPongT);
        }

        #endregion

        public static Vector3 RotateYPoint(this Vector3 original, float angle)
        {
            float cosTheta = MathF.Cos(angle);
            float sinTheta = MathF.Sin(angle);

            // Rotate around Y-axis
            return new Vector3(
                original.X * cosTheta - original.Z * sinTheta,
                original.Y,
                original.X * sinTheta + original.Z * cosTheta
            );
        }

        public static Vector3 RotateZPoint(this Vector3 original, float angle)
        {
            float cosTheta = MathF.Cos(angle);
            float sinTheta = MathF.Sin(angle);

            return new Vector3(
                original.X * cosTheta - original.Y * sinTheta,
                original.X * sinTheta + original.Y * cosTheta,
                original.Z
            );
        }
        #endregion
    }

    public static class StringHelpers
    {
        public static void PrintOnScreen(this string message, int x = 0, int y = 0)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(message);
        }
    }
}

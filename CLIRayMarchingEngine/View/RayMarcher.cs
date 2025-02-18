using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CLIRayMarchingEngine.Model;
using CLIRayMarchingEngine.Model.Primitives;

namespace CLIRayMarchingEngine.View
{
    /// <summary>
    /// One of these for each 'pixel' on screen 
    /// </summary>
    public class RayMarcher
    {
        public const int NUMBEROFSTEPS = 100;
        public const float MAXDISTANCE = 100f;
        public const float MINDISTANCE = 0.1f;

        public Vector3 RayOrigin { get; set; }
        public Vector3 RayDirection { get; set; }
        public float Distance { get; set; }

        public RayMarcher(Vector3 origin, Vector3 direction)
        {
            RayOrigin = origin;
            RayDirection = direction;
        }

        public float MarchRay(Scene scene)
        {
            float distance = 0f;
            Vector3 currentPos;

            for (int step = 0; step < NUMBEROFSTEPS; step++)
            {
                currentPos = RayOrigin + RayDirection * distance;
                float currentClosest = float.MaxValue;
                IDrawable hitObject = null;
                foreach (var drawable in scene.Drawables)
                {
                    float sdfDistance = drawable.GetSDFDistance(currentPos);

                    if (sdfDistance < currentClosest)
                    {
                        currentClosest = sdfDistance;
                        hitObject = drawable;
                    }
                    // We had hit something
                    if (currentClosest <= MINDISTANCE)
                    {
                        float retValue = 1f - (float)distance / MAXDISTANCE;
                        Vector3 normal = CalculateNormal(currentPos, hitObject.GetSDFDistance);
                        Vector3 lightDir = Vector3.Normalize(scene.LightPosition - currentPos);

                        retValue = HelpMaths.CalculateLight(RayDirection, normal, lightDir, hitObject.LightConfig) * retValue;
                        return retValue;
                    }
                }
                // We had hit nothing
                if (distance >= MAXDISTANCE)
                {
                    return 0f;
                }

                distance += currentClosest;
            }

            return 0f;
        }

        Vector3 CalculateNormal(Vector3 p, Func<Vector3, float> SDF, float eps = 0.001f)
        {
            // Compute SDF values slightly offset in each direction
            float dx = SDF(p + new Vector3(eps, 0, 0)) - SDF(p - new Vector3(eps, 0, 0));
            float dy = SDF(p + new Vector3(0, eps, 0)) - SDF(p - new Vector3(0, eps, 0));
            float dz = SDF(p + new Vector3(0, 0, eps)) - SDF(p - new Vector3(0, 0, eps));

            // Construct gradient vector and normalize it
            return Vector3.Normalize(new Vector3(dx, dy, dz));
        }
    }
}

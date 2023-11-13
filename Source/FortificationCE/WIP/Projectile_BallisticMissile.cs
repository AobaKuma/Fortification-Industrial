using UnityEngine;
using CombatExtended;
using Verse;
using System;

namespace Fortification
{
    /*
    public class Projectile_BallisticMissile : ProjectileCE_Explosive
    {
        private AnimationCurve curve;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            curve = new AnimationCurve(new Keyframe(0,-0.01f), new Keyframe(0.1f,0.02f), new Keyframe(1,1f));
        }
        public override Vector3 ExactPosition 
        {
            get
            {
                Vector2 b = Vec2PositionCurve();
                return new Vector3(b.x, Height, b.y);
            }
            set => base.ExactPosition = value; 
        }
        public Vector2 DrawPosV2Curve => Vec2PositionCurve() + new Vector2(0f, Height - shotHeight * (StartingTicksToImpact-fTicks / StartingTicksToImpact));
        public override Vector3 DrawPos
        {
            get
            {
                Vector2 drawPosV = DrawPosV2Curve;
                return new Vector3(drawPosV.x, def.Altitude, drawPosV.y);
            }
        }
        public override Quaternion ExactRotation
        {
            get
            {
                return Quaternion.AngleAxis(shotRotation, Vector3.down);
            }
        }

        private Vector2 Vec2PositionCurve(float ticks = -1f)
        {
            if (ticks < 0f)
            {
                ticks = fTicks;
            }

            return Vector2.Lerp(origin, Destination, curve.Evaluate(ticks / StartingTicksToImpact)* ticks / StartingTicksToImpact);
        }
    }
    */
}

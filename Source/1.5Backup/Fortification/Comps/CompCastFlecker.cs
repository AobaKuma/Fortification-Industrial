using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace Fortification
{

    public class CompCastFlecker : ThingComp
    {
        public CompProperties_CastFlecker Props => (CompProperties_CastFlecker)this.props;
        float currentAngle;
        private Thing caster;
        public void DoBursting(float angle)
        {
            currentAngle = angle;
            BurstFleck();
        }
        public bool SpawnCheck(Thing caster)
        {
            this.caster = caster;
            TerrainDef terrain = caster.Position.GetTerrain(caster.Map);
            if (terrain.IsWater || terrain.IsFloor) return false; //在地板與水體上不會揚塵
            return true;
        }
        void BurstFleck()
        {
            FleckDef spawnFleck = Props.fleck;
            if (Props.onGroundOverride)
            {
                TerrainDef terrain = caster.Position.GetTerrain(caster.Map);
                if (terrain.IsSoil || terrain.takeFootprints)
                {
                    spawnFleck = FleckDefOf.DustPuff;
                }
                else if (terrain.holdSnow) 
                {
                    spawnFleck = FleckDefOf.AirPuff;
                }

                spawnFleck.altitudeLayer = AltitudeLayer.Filth;
            }
            for (int i = 0; i < Props.burstAmount; i++)
            {
                float angle = GetAngle();
                Vector3 p = DrawPos(angle);
                if (p.ShouldSpawnMotesAt(caster.Map, false))
                {
                    FleckCreationData dataStatic = FleckMaker.GetDataStatic(p, caster.Map, spawnFleck, GetSize());
                    dataStatic.rotationRate = Rand.Range(-30f, 30f);
                    dataStatic.velocityAngle = angle;
                    dataStatic.velocitySpeed = Rand.Range(Props.burstSpeedRange.x, Props.burstSpeedRange.y);
                    caster.Map.flecks.CreateFleck(dataStatic);
                }

            }
            float GetAngle()
            {
                return (Rand.Range(-Props.burstSway, Props.burstSway) + currentAngle + Props.burstAngle + 90) % 360;
            }
            float GetSize()
            {
                return Rand.Range(Props.fleckSizeRange.x, Props.fleckSizeRange.y) * Mathf.Lerp(Props.rangeScaleFactor.x, Props.rangeScaleFactor.y, _range / Props.burstOffsetRange.y - Props.burstOffsetRange.x);
            }
        }
        private float _range;

        Vector3 DrawPos(float angle)
        {
            //Log.Message("currentAngle:" + currentAngle);
            //Log.Message("angle:" + angle);

            Vector3 offset = Vector3.zero;
            if (Props.burstOffsetRange != Vector2.zero)
            {
                float range = Random.Range(Props.burstOffsetRange.x, Props.burstOffsetRange.y);
                _range = range;
                offset = range * new Vector3(Mathf.Sin(angle / 57.3f), 0, Mathf.Cos(angle / 57.3f));
            }
            //Log.Message("offset:" + offset);

            return caster.DrawPos + offset;
        }
    }
    public class CompProperties_CastFlecker : CompProperties
    {
        public CompProperties_CastFlecker()
        {
            compClass = typeof(CompCastFlecker);
        }
        public bool onGroundOverride = false;//如果為true的話在雪地與混凝土地上會揚白塵,在土地上揚棕塵，並且繪製高度會設置為Floor
        public FleckDef fleck = FleckDefOf.Smoke;
        public AltitudeLayer altitudeLayer = AltitudeLayer.MoteLow;
        public float burstAmount = 1;//每次更新發射幾個
        public float burstSway = 5; //扇形分布
        public float burstAngle = 0;//180 = 發射角度的反方向

        public Vector2 fleckSizeRange = Vector2.one;//尺寸
        public Vector2 burstSpeedRange = Vector2.one;//特效的移動速度
        public Vector2 burstOffsetRange = Vector2.zero; //特效生成的離心距離偏移
        public Vector2 rangeScaleFactor = Vector2.one;//最小/最大距離的效果縮放倍率
    }
}

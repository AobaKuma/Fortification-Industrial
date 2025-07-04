using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace FortificationMedieval
{
    [SerializeField]
    public class FleckPack
    {
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

    public class ModExtension_ShootWithFleck : DefModExtension
    {

        float currentAngle;
        private Thing caster;
        public List<FleckPack> flecks = new List<FleckPack>();
        public void DoBursting(float angle)
        {
            currentAngle = angle;
            foreach (var item in flecks)
            {
                BurstFleck(item);
            }
        }
        public bool SpawnCheck(Thing caster)
        {
            this.caster = caster;
            TerrainDef terrain = caster.Position.GetTerrain(caster.Map);
            if (terrain.IsWater || terrain.IsFloor) return false; //在地板與水體上不會揚塵
            return true;
        }
        void BurstFleck(FleckPack fp)
        {
            FleckDef spawnFleck = fp.fleck;
            if (fp.onGroundOverride)
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
            for (int i = 0; i < fp.burstAmount; i++)
            {
                float angle = GetAngle();
                Vector3 p = DrawPos(angle);
                if (p.ShouldSpawnMotesAt(caster.Map, false))
                {
                    FleckCreationData dataStatic = FleckMaker.GetDataStatic(p, caster.Map, spawnFleck, GetSize());
                    dataStatic.rotationRate = Rand.Range(-30f, 30f);
                    dataStatic.velocityAngle = angle;
                    dataStatic.velocitySpeed = Rand.Range(fp.burstSpeedRange.x, fp.burstSpeedRange.y);
                    caster.Map.flecks.CreateFleck(dataStatic);
                }

            }
            float GetAngle()
            {
                return (Rand.Range(-fp.burstSway, fp.burstSway) + currentAngle + fp.burstAngle + 90) % 360;
            }
            float GetSize()
            {
                return Rand.Range(fp.fleckSizeRange.x, fp.fleckSizeRange.y) * Mathf.Lerp(fp.rangeScaleFactor.x, fp.rangeScaleFactor.y, _range / fp.burstOffsetRange.y - fp.burstOffsetRange.x);
            }
            Vector3 DrawPos(float angle)
            {
                //Log.Message("currentAngle:" + currentAngle);
                //Log.Message("angle:" + angle);

                Vector3 offset = Vector3.zero;
                if (fp.burstOffsetRange != Vector2.zero)
                {
                    float range = Random.Range(fp.burstOffsetRange.x, fp.burstOffsetRange.y);
                    _range = range;
                    offset = range * new Vector3(Mathf.Sin(angle / 57.3f), 0, Mathf.Cos(angle / 57.3f));
                }
                //Log.Message("offset:" + offset);

                return caster.DrawPos + offset;
            }
        }
        private float _range;
        
    }

}

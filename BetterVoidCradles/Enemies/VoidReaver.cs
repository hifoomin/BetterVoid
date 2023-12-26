using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BetterVoid.Enemies
{
    public static class VoidReaver
    {
        public static void Init()
        {
            var voidReaverMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierMaster.prefab").WaitForCompletion();
            var voidReaverBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierBody.prefab").WaitForCompletion();
            Tweak(voidReaverMaster, voidReaverBody);
            var voidReaverAllyMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierAllyMaster.prefab").WaitForCompletion();
            var voidReaverAllyBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierAllyBody.prefab").WaitForCompletion();
            Tweak(voidReaverAllyMaster, voidReaverAllyBody);

            var deathProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierDeathBombProjectile.prefab").WaitForCompletion();
            var projectileImpactExplosion = deathProjectile.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.lifetime = 2.4f;
        }

        public static void Tweak(GameObject master, GameObject body)
        {
            AISkillDriver VoidReaverPanicFire = (from x in master.GetComponents<AISkillDriver>()
                                                 where x.customName == "PanicFireWhenClose"
                                                 select x).First();
            VoidReaverPanicFire.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

            AISkillDriver VoidReaverTrack = (from x in master.GetComponents<AISkillDriver>()
                                             where x.customName == "FireAndStrafe"
                                             select x).First();
            VoidReaverTrack.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

            AISkillDriver VoidReaverStop = (from x in master.GetComponents<AISkillDriver>()
                                            where x.customName == "FireAndChase"
                                            select x).First();
            VoidReaverStop.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

            var characterBody = body.GetComponent<CharacterBody>();
            characterBody.baseMoveSpeed = 17f;
        }
    }
}
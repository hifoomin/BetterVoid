using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BetterVoid.Enemies
{
    public static class VoidDevastator
    {
        public static void Init()
        {
            var deathProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabDeathBombProjectile.prefab").WaitForCompletion();
            var projectileImpactExplosion = deathProjectile.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.lifetime = 3.5f;
        }
    }
}
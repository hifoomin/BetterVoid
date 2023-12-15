using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;

namespace BetterVoid.Interactables
{
    public static class VoidPotential
    {
        public static void Init()
        {
            if (Main.alterPotentials.Value)
            {
                var dropTable = Addressables.LoadAssetAsync<BasicPickupDropTable>("RoR2/DLC1/VoidTriple/dtVoidTriple.asset").WaitForCompletion();
                dropTable.tier1Weight = 0.14f;
                dropTable.tier2Weight = 0.35f;
                dropTable.tier3Weight = 0.01f;
                dropTable.voidTier3Weight = 0.25f;
                dropTable.voidBossWeight = 0.25f;
            }
            if (Main.destroyVoidPotentialAfterTeleporter.Value)
            {
                var voidPotential = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidTriple/VoidTriple.prefab").WaitForCompletion();
                voidPotential.AddComponent<FuckMinMaxers>();
            }
        }
    }
}
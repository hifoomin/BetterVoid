using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BetterVoid.Enemies
{
    public static class VoidBarnacle
    {
        public static void Init()
        {
            var barnacle = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidBarnacle/VoidBarnacleBody.prefab").WaitForCompletion();
            var characterBody = barnacle.GetComponent<CharacterBody>();
            characterBody.baseRegen = 0f;
            characterBody.levelRegen = 0f;
        }
    }
}
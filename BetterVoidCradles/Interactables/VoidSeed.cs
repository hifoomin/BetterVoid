using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;
using RoR2.EntityLogic;

namespace BetterVoid.Interactables
{
    public static class VoidSeed
    {
        public static GameObject positionIndicatorPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidCamp/VoidCampPositionIndicator.prefab").WaitForCompletion();

        public static void Init()
        {
            if (Main.alterVoidSeeds.Value)
            {
                var interactables = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/VoidCamp/dccsVoidCampInteractables.asset").WaitForCompletion();
                var voidStalk = interactables.categories[0].cards[1];
                voidStalk.selectionWeight = 2;
            }
            var voidSeed = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidCamp/iscVoidCamp.asset").WaitForCompletion();
            voidSeed.maxSpawnsPerStage = Main.voidSeedLimit.Value;

            var voidSeedObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidCamp/VoidCamp.prefab").WaitForCompletion();
            var trans = voidSeedObject.transform;
            var outsideInteractableLocker = voidSeedObject.GetComponent<OutsideInteractableLocker>();
            outsideInteractableLocker.radius = Main.voidSeedRadius.Value;
            var playerSpawnInhibitor = voidSeedObject.GetComponent<PlayerSpawnInhibitor>();
            playerSpawnInhibitor.radius = Main.voidSeedRadius.Value;

            var light = trans.GetChild(3).GetComponent<Light>();
            light.color = new Color32(119, 58, 150, 255);
            light.intensity = 20f;
            light.range = Main.voidSeedRadius.Value;
            light.gameObject.SetActive(true);

            var decal = trans.GetChild(4);
            decal.localScale = Vector3.one * 2.08f * Main.voidSeedRadius.Value; // scale was 124.8 for 60m radius

            var camp1 = trans.GetChild(0);
            var campDirector = camp1.GetComponent<CampDirector>();
            campDirector.campMaximumRadius = Main.voidSeedRadius.Value;
            var sphereZone = camp1.GetComponent<SphereZone>();
            sphereZone.radius = Main.voidSeedRadius.Value;
            var combatDirector = camp1.GetComponent<CombatDirector>();
            combatDirector.maximumNumberToSpawnBeforeSkipping = 3;
            combatDirector.creditMultiplier = 2.2f;

            var camp2 = trans.GetChild(1);
            var campDirector2 = camp2.GetComponent<CampDirector>();
            campDirector2.campMaximumRadius = Main.voidSeedRadius.Value;
            campDirector2.baseInteractableCredit = Convert.ToInt32(Main.voidSeedRadius.Value / 4f);

            GameObject.Destroy(voidSeedObject.GetComponent<DelayedEvent>());

            On.EntityStates.VoidCamp.Idle.OnEnter += Idle_OnEnter;
        }

        public static void Idle_OnEnter(On.EntityStates.VoidCamp.Idle.orig_OnEnter orig, EntityStates.VoidCamp.Idle self)
        {
            orig(self);
            var positionIndicator = GameObject.Instantiate(positionIndicatorPrefab, self.transform.position, Quaternion.identity).GetComponent<PositionIndicator>();
            positionIndicator.targetTransform = self.transform;
        }
    }
}
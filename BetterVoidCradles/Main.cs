using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BetterVoid.Enemies;
using BetterVoid.Interactables;
using BetterVoid.Items;
using R2API;
using RoR2;
using UnityEngine;

namespace BetterVoid
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    [BepInDependency("com.Wolfo.WolfoQualityOfLife", BepInDependency.DependencyFlags.SoftDependency)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "HIFU";
        public const string PluginName = "BetterVoid";
        public const string PluginVersion = "1.2.1";

        public static ManualLogSource btsLogger;

        public static ConfigEntry<bool> enableCradleChanges { get; set; }
        public static ConfigEntry<float> cursePercent { get; set; }
        public static ConfigEntry<bool> destroyVoidCradleAfterTeleporter { get; set; }
        public static ConfigEntry<bool> destroyVoidPotentialAfterTeleporter { get; set; }
        public static ConfigEntry<int> choiceCount { get; set; }
        public static ConfigEntry<bool> alterPotentials { get; set; }
        public static ConfigEntry<bool> alterVoidSeeds { get; set; }
        public static ConfigEntry<bool> enableVoidInfestors { get; set; }
        public static ConfigEntry<int> voidInfestorCount { get; set; }
        public static ConfigEntry<int> voidSeedLimit { get; set; }
        public static ConfigEntry<float> voidSeedRadius { get; set; }
        public static ConfigEntry<bool> rebalanceSingularityBand { get; set; }
        public static ConfigEntry<bool> pluripotentLarvaDownside { get; set; }
        public static ConfigEntry<string> blacklistedItems { get; set; }
        public static ConfigEntry<bool> voidBarnacleTweaks { get; set; }
        public static ConfigEntry<bool> voidReaverTweaks { get; set; }
        public static ConfigEntry<bool> voidDevastatorTweaks { get; set; }

        public static BuffDef uselessBuff;

        public void Awake()
        {
            btsLogger = base.Logger;

            enableCradleChanges = Config.Bind("Void Cradle", "Enable Changes?", true, "");
            cursePercent = Config.Bind("Void Cradle", "Curse Percent", 0.1f, "Decimal.");
            destroyVoidCradleAfterTeleporter = Config.Bind("Void Cradle", "Destroy Void Cradles after Teleporter?", true, "This is to prevent cheese");
            choiceCount = Config.Bind("Void Cradle", "Corruptible Max Choice Count", 3, "");
            enableVoidInfestors = Config.Bind("Void Cradle", "Enable Void Infestor spawns?", true, "");
            voidInfestorCount = Config.Bind("Void Cradle", "Void Infestor Count", 2, "");
            alterPotentials = Config.Bind("Void Potential", "Alter Void Potentials?", true, "This is to make acquiring void reds and void yellows easier, since you need the prerequisite to corrupt from the cradle now");
            destroyVoidPotentialAfterTeleporter = Config.Bind("Void Potential", "Destroy Void Potentials after Teleporter?", true, "This is to prevent Eclipse cheese");
            alterVoidSeeds = Config.Bind("Void Seed", "Alter Void Seeds?", true, "Makes Void Stalks less likely to appear in Void Seeds, and as a result, Void Cradles and Void Potentials are more likely to appear inside them");
            voidSeedLimit = Config.Bind("Void Seed", "Void Seed Spawn Limit Per Stage", 1, "");
            voidSeedRadius = Config.Bind("Void Seed", "Radius", 70f, "Vanilla is 60");
            rebalanceSingularityBand = Config.Bind("Items", "Rebalance Singularity Band?", true, "Makes Singularity Band do much more total damage and stack radius instead of damage");
            pluripotentLarvaDownside = Config.Bind("Items", "Add a downside to Pluripotent Larva?", true, "Makes Pluripotent Larva reroll your white and green item stacks on pickup");

            blacklistedItems = Config.Bind("Items", "Blacklisted Items", "", "Takes the non-void counterparts of items and makes them uncorruptable, use DebugToolkits list_item command to see internal item names, separate each one with a comma and a space afterwards, e.g. ChainLightning, Missile");

            voidBarnacleTweaks = Config.Bind("Enemies", "Change Void Barnacle Skills, Stats and AI?", true, "Currently removes Void Barnacle regen");
            voidReaverTweaks = Config.Bind("Enemies", "Change Void Reaver Skills, Stats and AI?", true, "Currently makes Void Reavers follow you closely, and explode faster");
            voidDevastatorTweaks = Config.Bind("Enemies", "Change Void Devastator Skills and AI?", true, "Currently makes Void Devastators explode faster");

            uselessBuff = ScriptableObject.CreateInstance<BuffDef>();
            uselessBuff.isDebuff = false;
            uselessBuff.isHidden = true;
            uselessBuff.canStack = false;
            uselessBuff.isCooldown = false;

            VoidPotential.Init();
            VoidSeed.Init();
            SingularityBand.Init();

            if (enableCradleChanges.Value)
            {
                VoidCradle.Init();
            }

            if (voidBarnacleTweaks.Value)
            {
                VoidBarnacle.Init();
            }

            if (voidReaverTweaks.Value)
            {
                VoidReaver.Init();
            }

            if (voidDevastatorTweaks.Value)
            {
                VoidDevastator.Init();
            }
        }
    }
}
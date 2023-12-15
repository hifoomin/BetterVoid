using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BetterVoid.Items
{
    public static class SingularityBand
    {
        public static void Init()
        {
            if (Main.rebalanceSingularityBand.Value)
            {
                GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
                IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
                LanguageAPI.Add("ITEM_ELEMENTALRINGVOID_DESC", "Hits that deal <style=cIsDamage>more than 400% damage</style> also fire a black hole that <style=cIsUtility>draws enemies within 12m <style=cStack>(+6m per stack)</style> into its center</style>. Lasts <style=cIsUtility>5</style> seconds before collapsing, dealing <style=cIsDamage>175%</style> TOTAL damage. Recharges every <style=cIsUtility>15</style> seconds. <style=cIsVoid>Corrupts all Runald's and Kjaro's Bands</style>.");
            }
        }

        public static void GlobalEventManager_OnHitEnemy(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdsfld(typeof(DLC1Content.Buffs), "ElementalRingVoidReady")))
            {
                c.Remove();
                c.Emit<Main>(OpCodes.Ldsfld, nameof(Main.uselessBuff));
            }
            else
            {
                Main.btsLogger.LogError("Failed to apply Singularity Band Deletion hook");
            }
        }

        public static void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (!damageReport.attacker)
            {
                return;
            }

            var body = damageReport.attackerBody;

            if (!body)
            {
                return;
            }

            var inventory = body.inventory;

            if (!inventory)
            {
                return;
            }

            if (damageReport.damageInfo.procCoefficient > 0)
            {
                if (!damageReport.damageInfo.procChainMask.HasProc(ProcType.Rings) && damageReport.damageInfo.damage / damageReport.attackerBody.damage >= 4f)
                {
                    if (body.HasBuff(DLC1Content.Buffs.ElementalRingVoidReady))
                    {
                        body.RemoveBuff(DLC1Content.Buffs.ElementalRingVoidReady);
                        var buffCount = 1;
                        while (buffCount <= 15f)
                        {
                            body.AddTimedBuff(DLC1Content.Buffs.ElementalRingVoidCooldown, buffCount);
                            buffCount++;
                        }

                        var procMask = damageReport.damageInfo.procChainMask;
                        procMask.AddProc(ProcType.Rings);

                        var stack = inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid);
                        if (stack > 0)
                        {
                            var singularity = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ElementalRingVoidBlackHole");
                            var radialForce = singularity.GetComponent<RadialForce>();
                            var projectileExplosion = singularity.GetComponent<ProjectileExplosion>();

                            var scale = (12f + 6f * (stack - 1)) / 15f;
                            singularity.transform.localScale = new Vector3(scale, scale, scale);
                            radialForce.radius = 12f + 6f * (stack - 1);
                            projectileExplosion.blastRadius = 12f + 6f * (stack - 1);

                            float damage = 1.75f;
                            float totalDamage = Util.OnHitProcDamage(damageReport.damageInfo.damage, body.damage, damage);

                            ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                            {
                                damage = totalDamage,
                                crit = damageReport.damageInfo.crit,
                                damageColorIndex = DamageColorIndex.Void,
                                position = damageReport.damageInfo.position,
                                procChainMask = procMask,
                                force = 6000f,
                                owner = damageReport.damageInfo.attacker,
                                projectilePrefab = singularity,
                                rotation = Quaternion.identity,
                                target = null
                            });
                        }
                    }
                }
            }
        }
    }
}
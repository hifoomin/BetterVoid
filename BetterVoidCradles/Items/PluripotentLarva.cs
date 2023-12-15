using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace BetterVoid.Items
{
    public static class PluripotentLarva
    {
        public static void Init()
        {
            if (Main.pluripotentLarvaDownside.Value)
            {
                On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
                LanguageAPI.Add("ITEM_EXTRALIFEVOID_PICKUP", "Shuffle your inventory, and get a <style=cIsVoid>corrupted</style> extra life. Consumed on use. <style=cIsVoid>Corrupts all Dio's Best Friends.</style>.");
                LanguageAPI.Add("ITEM_EXTRALIFEVOID_DESC", "<style=cIsUtility>Shuffle your inventory</style>. <style=cIsUtility>Upon death</style>, this item will be <style=cIsUtility>consumed</style> and you will <style=cIsHealing>return to life</style> with <style=cIsHealing>3 seconds of invulnerability</style>, and all of your items that can be <style=cIsUtility>corrupted</style> will be. <style=cIsVoid>Corrupts all Dio's Best Friends</style>.");
                LanguageAPI.Add("PLURI_CORRUPTED", "<style=cWorldEvent>{0} has been... corrupted.</color>");
                LanguageAPI.Add("PLURI_CORRUPTED_2P", "<style=cWorldEvent>You have been... corrupted.</color>"); // me
            }
        }

        public static void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (itemIndex == DLC1Content.Items.ExtraLifeVoid.itemIndex)
            {
                List<ItemIndex> tier1Indices = new();
                List<int> stacks = new();

                for (int i = 0; i < self.itemAcquisitionOrder.Count; i++)
                {
                    var index = self.itemAcquisitionOrder[i];
                    var itemDef = ItemCatalog.GetItemDef(index);

                    if (itemDef.tier == ItemTier.Tier1 || itemDef.deprecatedTier == ItemTier.Tier1 || itemDef.tier == ItemTier.Tier2 || itemDef.deprecatedTier == ItemTier.Tier2)
                    {
                        tier1Indices.Add(index);
                        stacks.Add(self.GetItemCount(index));
                    }
                }

                int n = stacks.Count;
                while (n > 1)
                {
                    n--;
                    int k = UnityEngine.Random.Range(0, n + 1);
                    int temp = stacks[k];
                    stacks[k] = stacks[n];
                    stacks[n] = temp;
                }

                for (int i = 0; i < tier1Indices.Count; i++)
                {
                    var index = tier1Indices[i];
                    var stackCount = stacks[i];
                    self.RemoveItem(index, self.GetItemCount(index));
                    self.GiveItem(index, stackCount);
                }

                var body = self.gameObject.GetComponent<CharacterMaster>()?.GetBody();

                if (NetworkServer.active && body && body.isPlayerControlled)
                {
                    Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage
                    {
                        subjectAsCharacterBody = body,
                        baseToken = "PLURI_CORRUPTED"
                    });
                }
            }
        }
    }
}
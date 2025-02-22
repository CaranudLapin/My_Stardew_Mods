using System;
using StardewValley;
using StardewValley.TerrainFeatures;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace CustomTapperFramework;

using SObject = StardewValley.Object;

public class AutomatePatcher {
  public static void ApplyPatches(Harmony harmony) {
    var dataBasedMachineType = AccessTools.TypeByName("Pathoschild.Stardew.Automate.Framework.Machines.DataBasedObjectMachine");
    var tapperMachineType = AccessTools.TypeByName("Pathoschild.Stardew.Automate.Framework.Machines.Objects.TapperMachine");

    harmony.Patch(
        original: AccessTools.Method(dataBasedMachineType, "OnOutputCollected"),
        postfix: new HarmonyMethod(typeof(AutomatePatcher),
          nameof(AutomatePatcher.DataBasedMachine_OnOutputCollected_Postfix)));

    harmony.Patch(
        original: AccessTools.Method(tapperMachineType, "Reset"),
        postfix: new HarmonyMethod(typeof(AutomatePatcher),
          nameof(AutomatePatcher.TapperMachine_Reset_Postfix)));
  }

	static void DataBasedMachine_OnOutputCollected_Postfix(object __instance, Item item) {
    try {
      var machine = ModEntry.Helper.Reflection.GetProperty<SObject>(__instance, "Machine").GetValue();
      if (machine.IsTapper()) {
        Utils.UpdateTapperProduct(machine);
      }
    } catch (Exception e) {
      ModEntry.StaticMonitor.Log(e.Message, LogLevel.Error);
    }
  }

  // Needed for non-vanilla tappers on trees
	static void TapperMachine_Reset_Postfix(object __instance, Item item) {
    try {
      var machine = ModEntry.Helper.Reflection.GetProperty<SObject>(__instance, "Machine").GetValue();
      if (machine.IsTapper()) {
        Utils.UpdateTapperProduct(machine);
      }
    } catch (Exception e) {
      ModEntry.StaticMonitor.Log(e.Message, LogLevel.Error);
    }
  }
}

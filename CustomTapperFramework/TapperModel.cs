using System;
using System.Collections.Generic;
using StardewValley.GameData.WildTrees;

namespace CustomTapperFramework;

// Legacy Tapper API Model object
public class ExtendedTapItemData : WildTreeTapItemData {
  // If specified, only applies this rule if the tap object is of this ID.
  public string SourceId = null;
  public bool RecalculateOnCollect = false;
}

public class TapperModel {
  public bool AlsoUseBaseGameRules = false;
  public List<ExtendedTapItemData> TreeOutputRules { get; set; } = null;
  public List<ExtendedTapItemData> FruitTreeOutputRules { get; set; } = null;
  public List<ExtendedTapItemData> GiantCropOutputRules { get; set; } = null;
}

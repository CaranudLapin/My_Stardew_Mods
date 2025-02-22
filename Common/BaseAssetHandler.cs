using System.Collections;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Machines;
using StardewValley.GameData.BigCraftables;
using Microsoft.Xna.Framework.Graphics;

using SObject = StardewValley.Object;

namespace SelphCommon;

public abstract class DictAssetHandler<AssetType> {
  private string dataPath;
  private IMonitor monitor;

  public Dictionary<string, AssetType> data;

  public DictAssetHandler(string dataPath, IMonitor monitor) {
    this.dataPath = dataPath;
    this.monitor = monitor;
  }

  public void RegisterEvents(IModHelper helper) {
    helper.Events.Content.AssetRequested += this.OnAssetRequested;
    helper.Events.Content.AssetReady += this.OnAssetReady;
    helper.Events.Content.AssetsInvalidated += this.OnAssetsInvalidated;
    helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
  }

  public void OnAssetRequested(object sender, AssetRequestedEventArgs e) {
    if (e.NameWithoutLocale.IsEquivalentTo(this.dataPath)) {
      var dict = new Dictionary<string, AssetType>();
      e.LoadFrom(() => dict, AssetLoadPriority.Low);
    }
  }

  public void OnAssetReady(object sender, AssetReadyEventArgs e) {
    if (e.NameWithoutLocale.IsEquivalentTo(this.dataPath)) {
      this.data = Game1.content.Load<Dictionary<string, AssetType>>(this.dataPath);
      monitor.Log($"Loaded asset {dataPath} with {data.Count} entries.", LogLevel.Info);
    }
  }

  public void OnGameLaunched(object sender, GameLaunchedEventArgs e) {
    this.data = Game1.content.Load<Dictionary<string, AssetType>>(this.dataPath);
  }

  public void OnAssetsInvalidated(object sender, AssetsInvalidatedEventArgs e) {
    foreach (var name in e.NamesWithoutLocale) {
      if (name.IsEquivalentTo(this.dataPath)) {
        monitor.Log($"Asset {dataPath} invalidated, reloading.", LogLevel.Info);
        this.data = Game1.content.Load<Dictionary<string, AssetType>>(this.dataPath);
      }
    }
  }

}

namespace TwoSimpleDevs.Project.Core
{
  public static partial class AppEvent
  {
    // Basic app events
    public const string AppFocusReturned = "AppFocusReturned";
    public const string BackButtonPressed = "BackButtonPressed";
    public const string SceneTransitionedIn = "SceneTransitionedIn";
    public const string SceneTransitionedOut = "SceneTransitionedOut";

    // Store
    public const string StorePurchaseSucceeded = "StorePurchaseSucceeded";
    public const string StorePurchaseFailed = "StorePurchaseFailed";
    public const string StoreRestoreComplete = "StoreRestoreComplete";
    public const string PurchaseCurrencyPack = "PurchaseCurrencyPack";

    // Wallet
    public const string CurrencyIncrease = "CurrencyIncrease";
    public const string CurrencyDecrease = "CurrencyDecrease";

    // User interaction
    public const string ConfirmedChoice = "ConfirmedChoice";
  }
}
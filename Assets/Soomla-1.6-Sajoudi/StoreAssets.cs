using UnityEngine;
using System.Collections;
using Soomla.Store;

public class StoreAssets : IStoreAssets {

	// Product ID as it must be registered, with this exact string, in BOTH the
	// Cafe Bazaar Developer Console and the Myket Developer Console.
	// NOTE: never change this string after publishing - it must stay identical
	// forever, across all future app updates, or existing buyers lose their purchase.
	public const string FULL_VERSION_ITEM_ID = "morphodent_full_version";

	// Reference price only (Toman). The price actually charged is whatever is
	// configured per-store in the Bazaar / Myket console for this product id.
	private static double fullVersionPrice = 200000d;

	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() {
		return 0;
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {FullVersion};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{};
	}

	/// <summary>
	/// Retrieves the array of all non-consumable items served by your store.
	/// </summary>
	/// <returns>All non consumable items served in your game.</returns>

	/*public NonConsumableItem[] GetNonConsumableItems()
	{
		return new NonConsumableItem[]{};
	}*/






	public static VirtualGood FullVersion = new LifetimeVG(
		FULL_VERSION_ITEM_ID,												// name
		"Morphodent Full Version",											// description
		FULL_VERSION_ITEM_ID,												// item id
		new PurchaseWithMarket(FULL_VERSION_ITEM_ID, fullVersionPrice));

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class StoreEventHandler  {

	
	/// <summary>
	/// Constructor.
	/// Subscribes to potential events.
	/// </summary>
	public StoreEventHandler () {
		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnMarketRefund += onMarketRefund;
		StoreEvents.OnItemPurchased += onItemPurchased;
		StoreEvents.OnGoodEquipped += onGoodEquipped;
		StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
		StoreEvents.OnGoodUpgrade += onGoodUpgrade;
		StoreEvents.OnBillingSupported += onBillingSupported;
		StoreEvents.OnBillingNotSupported += onBillingNotSupported;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		//StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
		StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
		StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
		StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;
		StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
		#if UNITY_ANDROID && !UNITY_EDITOR
		StoreEvents.OnIabServiceStarted += onIabServiceStarted;
		StoreEvents.OnIabServiceStopped += onIabServiceStopped;
		#endif
	}
	
	/// <summary>
	/// Handles a market purchase event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	/// <param name="purchaseToken">Purchase token.</param>
	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> dict) {
		   //("MarketPurchase called for : " + pvi.Name + " and payload : " + payload + "and token : " + token );
		/*if (pvi.ItemId == "SL3_Coin1") {
			PlayerPrefs.SetInt ("AllScore", PlayerPrefs.GetInt ("AllScore") + 1000);
			GameHandler.Instance.VersionText.text = (PlayerPrefs.GetInt("AllScore")).ToString();
		}
		if (pvi.ItemId == "SL3_Car1") {

			GameHandler.Instance.VersionText.text = "SL3_Car1";
		}*/


	}
	
	/// <summary>
	/// Handles a market refund event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketRefund(PurchasableVirtualItem pvi) {
		   //("onMarketRefund");
	}
	
	/// <summary>
	/// Handles an item purchase event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {
		   //("ItemPurchased with name :" + pvi.Name + " and payload : " + payload);
		PlayerPrefs.SetInt(pvi.ItemId + "Purchased", 1);
		CameraController.ShowAlertBox("پرداخت با موفقیت انجام شد.");
	}
	
	/// <summary>
	/// Handles a good equipped event.
	/// </summary>
	/// <param name="good">Equippable virtual good.</param>
	public void onGoodEquipped(EquippableVG good) {
		   //("onGoodEquipped called for : " + good.Name);
	}
	
	/// <summary>
	/// Handles a good unequipped event.
	/// </summary>
	/// <param name="good">Equippable virtual good.</param>
	public void onGoodUnequipped(EquippableVG good) {
		   //("onGoodUnequipped called for " + good.Name);
	}
	
	/// <summary>
	/// Handles a good upgraded event.
	/// </summary>
	/// <param name="good">Virtual good that is being upgraded.</param>
	/// <param name="currentUpgrade">The current upgrade that the given virtual
	/// good is being upgraded to.</param>
	public void onGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade) {
		   //("onGoodUpgrade for good : " + good.Name + " with currentUpgrade : " + currentUpgrade.Name);
	}
	
	/// <summary>
	/// Handles a billing supported event.
	/// </summary>
	public void onBillingSupported() {
		   //("BillingSupported");
	}
	
	/// <summary>
	/// Handles a billing NOT supported event.
	/// </summary>
	public void onBillingNotSupported() {
		   //("BillingNotSupported");
	}
	
	/// <summary>
	/// Handles a market purchase started event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi) {
		   //("onMarketPurchaseStarted for item : " + pvi.Name);
	}
	
	/// <summary>
	/// Handles an item purchase started event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchaseStarted(PurchasableVirtualItem pvi) {
		   //("onItemPurchaseStarted for item : " + pvi.Name);
	}
	
	/// <summary>
	/// Handles an item purchase cancelled event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) {
		   //("onMarketPurchaseCancelled for item : " + pvi.Name);
	}
	
	/// <summary>
	/// Handles an unexpected error in store event.
	/// </summary>
	/// <param name="message">Error message.</param>
	public void onUnexpectedErrorInStore(string message) {
		   //("onUnexpectedErrorInStore , " + message);
		//CameraController.ShowAlertBox("عملیات پرداخت با مشکل مواجه شد. لطفا دوباره امتحان کنید.");
	}
	
	/// <summary>
	/// Handles a currency balance changed event.
	/// </summary>
	/// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
	/// <param name="balance">Balance of the given virtual currency.</param>
	/// <param name="amountAdded">Amount added to the balance.</param>
	public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
		   //("onCurrencyBalanceChanged");
	}
	
	/// <summary>
	/// Handles a good balance changed event.
	/// </summary>
	/// <param name="good">Virtual good whose balance has changed.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>
	public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
		   //("onGoodBalanceChanged");
	}
	
	/// <summary>
	/// Handles a restore Transactions process started event.
	/// </summary>
	public void onRestoreTransactionsStarted() {
		   //("onRestoreTransactionsStarted");
	}
	
	/// <summary>
	/// Handles a restore transactions process finished event.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	public void onRestoreTransactionsFinished(bool success) {
		if (success) {
			CameraController.ShowAlertBox("بازیابی خرید با موفقیت انجام شد.");
		} else {
			CameraController.ShowAlertBox("خریدی برای بازیابی یافت نشد یا عملیات با خطا مواجه شد.");
		}
	}
	
	/// <summary>
	/// Handles a store controller initialized event.
	/// </summary>
	public void onSoomlaStoreInitialized() {
		   //("onSoomlaStoreInitialized");
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	public void onIabServiceStarted() {
		
	}
	public void onIabServiceStopped() {
		
	}
	#endif
}

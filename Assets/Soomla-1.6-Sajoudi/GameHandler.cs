using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Soomla.Store;

public class GameHandler : MonoBehaviour {

	public static GameHandler Instance;

	public static StoreEventHandler handler;

	public Text VersionText;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		if(handler==null)
			handler = new StoreEventHandler();
		SoomlaStore.Initialize(new StoreAssets());

	}



	public void BuyGame(string ID)
	{
		StoreInventory.BuyItem(ID);
	}
}

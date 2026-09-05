using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

// One-click switch between Myket and Cafe Bazaar billing before building the Android player.
// Only one provider's jars may sit in Soomla/libs at a time - both define the same
// com.soomla.store.billing.google.GooglePlayIabService class, so having two present
// at once causes a duplicate-class build error.
public static class MarketBuildSwitcher
{
	private const string LibsDir = "/Plugins/Android/Soomla/libs/";
	private const string SourceRoot = "/WebPlayerTemplates/SoomlaConfig/android/android-billing-services/";
	private const string ManifestRelPath = "/Plugins/Android/AndroidManifest.xml";

	private const string MyketPermission = "ir.mservices.market.BILLING";
	private const string BazaarPermission = "com.farsitel.bazaar.permission.PAY_THROUGH_BAZAAR";
	private const string GooglePermission = "com.android.vending.BILLING";

	[MenuItem("Morphodent/Switch Market/Myket")]
	public static void SwitchToMyket()
	{
		Switch("myket", "AndroidStoreMyket.jar", "IInAppBillingServiceMyket.jar", MyketPermission);
	}

	[MenuItem("Morphodent/Switch Market/Cafe Bazaar")]
	public static void SwitchToBazaar()
	{
		Switch("bazaar", "AndroidStoreBazaar.jar", "IInAppBillingServiceBazaar.jar", BazaarPermission);
	}

	private static void Switch(string sourceFolder, string storeJar, string iabJar, string permission)
	{
		string libsDir = Application.dataPath + LibsDir;

		DeleteIfExists(libsDir + "AndroidStoreMyket.jar");
		DeleteIfExists(libsDir + "IInAppBillingServiceMyket.jar");
		DeleteIfExists(libsDir + "AndroidStoreBazaar.jar");
		DeleteIfExists(libsDir + "IInAppBillingServiceBazaar.jar");
		DeleteIfExists(libsDir + "AndroidStoreGooglePlay.jar");
		DeleteIfExists(libsDir + "IInAppBillingService.jar");

		string sourceDir = Application.dataPath + SourceRoot + sourceFolder + "/";
		FileUtil.CopyFileOrDirectory(sourceDir + storeJar, libsDir + storeJar);
		FileUtil.CopyFileOrDirectory(sourceDir + iabJar, libsDir + iabJar);

		UpdateManifestPermission(permission);

		AssetDatabase.Refresh();
		Debug.Log("Morphodent: billing provider switched to " + sourceFolder + " (" + permission + "). You can now build the Android player for this store.");
	}

	private static void DeleteIfExists(string path)
	{
		if (File.Exists(path)) {
			FileUtil.DeleteFileOrDirectory(path);
		}
		if (File.Exists(path + ".meta")) {
			FileUtil.DeleteFileOrDirectory(path + ".meta");
		}
	}

	private static void UpdateManifestPermission(string newPermission)
	{
		string manifestPath = Application.dataPath + ManifestRelPath;
		string text = File.ReadAllText(manifestPath);
		string pattern = "<uses-permission android:name=\"(" +
			Regex.Escape(MyketPermission) + "|" +
			Regex.Escape(BazaarPermission) + "|" +
			Regex.Escape(GooglePermission) + ")\" />";
		text = Regex.Replace(text, pattern, "<uses-permission android:name=\"" + newPermission + "\" />");
		File.WriteAllText(manifestPath, text);
	}
}

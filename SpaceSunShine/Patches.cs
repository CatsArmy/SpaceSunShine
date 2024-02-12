using HarmonyLib;
namespace SpaceSunShine
{
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.StartGame))]
        private static void OnShipStartLand(StartOfRound __instance)
        {
            //Plugin.Instance.Sun.SetActive(false);
            Plugin.TogglePointLight(false);
            //RoundManager.Instance.playersManager.shipDoorsAnimator.SetBool("Closed", value: true);
            //UnityEngine.GameObject.FindObjectOfType<HangarShipDoor>().SetDoorButtonsEnabled(doorButtonsEnabled: false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        private static void OnShipLeave()
        {
            //Plugin.Instance.Sun.SetActive(true);
            Plugin.TogglePointLight(Plugin.Instance.EnablePointLight);
        }
    }
}


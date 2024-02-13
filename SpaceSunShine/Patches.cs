using HarmonyLib;
namespace SpaceSunShine
{
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.StartGame))]
        private static void OnShipStartLand(StartOfRound __instance)
        {
            if (Plugin.ShipLightsPostClone != null)
            {
                Plugin.ShipLightsPostClone.SetActive(false);
            }
            if (Plugin.Sun != null)
            {
                Plugin.Sun.SetActive(false);
            }
            if (Plugin.OutsideShipRoomClone != null)
            {
                Plugin.OutsideShipRoomClone.SetActive(false);
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        private static void OnShipLeave()
        {
            if (Plugin.ShipLightsPostClone != null)
            {
                Plugin.ShipLightsPostClone.SetActive(Plugin.EFL.Value);
            }
            if (Plugin.Sun != null)
            {
                Plugin.Sun.SetActive(Plugin.UOS.Value);
            }
            if (Plugin.OutsideShipRoomClone != null)
            {
                Plugin.OutsideShipRoomClone.SetActive(Plugin.SL.Value);
            }
        }
    }
}


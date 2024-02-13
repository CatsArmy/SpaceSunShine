using HarmonyLib;
namespace SpaceSunShine
{
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.StartGame))]
        private static void OnShipStartLand(StartOfRound __instance)
        {
            SpaceSunShine.ShipLightsPostClone?.SetActive(false);
            SpaceSunShine.Sun?.SetActive(false);
            SpaceSunShine.OutsideShipRoomClone?.SetActive(false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        private static void OnShipLeave()
        {
            SpaceSunShine.ShipLightsPostClone?.SetActive(Configs.EFL.Value);
            SpaceSunShine.Sun?.SetActive(Configs.UOS.Value);
            SpaceSunShine.OutsideShipRoomClone?.SetActive(Configs.SL.Value);
        }
    }
}


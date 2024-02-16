using System.Collections;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BetterCrouch
{
    internal class Patches
    {
        public static bool crouchOnHitGround = false;

        public static bool HitGround = false;
        public static bool IsJumping = false;
        private static ManualLogSource Logger = Plugin.Log;
        private static Coroutine coroutine;
        private const string Crouch = nameof(Crouch);
        private const string Jump = nameof(Jump);
        private const string Update = nameof(Update);
        private const string Jump_performed = nameof(Jump_performed);
        private static IEnumerator PerformDelayedAction(PlayerControllerB __instance, RaycastHit ___hit)
        {
            var Jump = GetJump();
            if (Jump.IsPressed())//performed == on button up
            {
                Logger.LogError("case1");
                IsJumping = true;
                if (__instance.isCrouching && __instance.CanJump(___hit))
                {
                    __instance.Crouch(false);
                    crouchOnHitGround = true;
                    Logger.LogError("type(a)");
                }
            }
            else
            {
                yield return null;
                __instance.Crouch(false);
                Logger.LogError("case2");
            }

            coroutine = null;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Prefix_PreformedJump(PlayerControllerB __instance,// ref InputAction.CallbackContext context,
            RaycastHit ___hit)
        {
            Logger.LogError("Begin routine");
            coroutine = __instance.StartCoroutine(PerformDelayedAction(__instance, ___hit));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Postfix_PreformedJump(PlayerControllerB __instance)
        {
            Logger.LogError("case3");
            //if (crouchOnHitGround)
            //{
            //    crouchOnHitGround = false;
            //    IsJumping = false;
            //    __instance.Crouch(true);
            //}
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerHitGroundEffects))]
        public static void PlayerHitGroundEffects(PlayerControllerB __instance)
        {
            Logger.LogFatal("am i patching");
            InputAction Jump = GetJump();
            IsJumping = false;
            if (crouchOnHitGround)
            {
                if (!Jump.IsPressed() && !__instance.isCrouching)
                {
                    __instance.Crouch(true);
                }
            }
            //__instance.StopCoroutine(coroutine);
            //coroutine = null;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), "Crouch_performed")]
        private static void Crouch_performed(PlayerControllerB __instance)
        {
            var Crouch = GetCrouch();
            if (!Crouch.IsPressed() && !__instance.quickMenuManager.isMenuOpen &&
                ((__instance.IsOwner && __instance.isPlayerControlled
                && (!__instance.IsServer || __instance.isHostPlayerObject)) || __instance.isTestingPlayer)
                && !__instance.inSpecialInteractAnimation && __instance.thisController.isGrounded
                && !__instance.isTypingChat && IsJumping)
            {
                __instance.Crouch(!__instance.isCrouching);
            }
        }



        public static InputAction GetJump()
        {
            return IngamePlayerSettings.Instance.playerInput.actions.FindAction(Jump);
        }
        public static InputAction GetCrouch()
        {
            return IngamePlayerSettings.Instance.playerInput.actions.FindAction(Crouch);
        }
    }
    public static class Extenion
    {
        private const float maxDistance = 0.72f;
        public static bool CanJump(this PlayerControllerB localPlayerController, RaycastHit? ___hit = null)
        {
            bool canJump = !Physics.Raycast(localPlayerController.gameplayCamera.transform.position, Vector3.up, out RaycastHit hit,
                maxDistance, localPlayerController.playersManager.collidersAndRoomMask, QueryTriggerInteraction.Ignore);
            ___hit = hit;
            return canJump;
        }
    }
}

using System;
using System.Collections;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterCrouch
{
    [HarmonyPatch]
    internal class Patches
    {
        public static bool crouchOnHitGround = false;

        public static bool HitGround = false;
        public static bool IsJumping = false;
        private static ManualLogSource Logger = Plugin.Log;
        private const string Crouch = nameof(Crouch);
        private const string Jump = nameof(Jump);
        private const string Update = nameof(Update);
        private const string Jump_performed = nameof(Jump_performed);
        private static float time = (float)new TimeSpan(0, 0, 0, 0, 35).TotalMilliseconds;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Prefix_PreformedJump(PlayerControllerB __instance)
        {
            //__instance.StartCoroutine(PerformDelayedAction());
            Plugin.Instance.ExecuteAfterSeconds(time, delegate
            {
                if (__instance.isCrouching)
                {
                    __instance.Crouch(false);
                    crouchOnHitGround = true;

                    var Jump = GetJump();
                    if (!Jump.IsPressed())//performed == on button up
                    {
                        IsJumping = false;
                    }
                    else
                    {
                        IsJumping = true;
                    }
                }
            });

        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerHitGroundEffects))]
        public static void PlayerHitGroundEffects(PlayerControllerB __instance)
        {
            InputAction Jump = GetJump();
            IsJumping = false;
            if (crouchOnHitGround)
            {
                crouchOnHitGround = false;
                if (!Jump.IsPressed() && !__instance.isCrouching)
                {
                    __instance.Crouch(true);
                }
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Postfix_PreformedJump(PlayerControllerB __instance)
        {
            var jumpCoroutine = Traverse.Create(__instance).Field("jumpCoroutine");
            jumpCoroutine.SetValue(null);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), "PlayerJump")]
        private static bool Prefix_PlayerJump(PlayerControllerB __instance)
        {
            if (IsJumping)
            {
                IsJumping = false;
                return true;
            }
            return false;
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
        public static void ExecuteAfterFrames(this MonoBehaviour mb, int delay, Action action)
        {
            mb.StartCoroutine(ExecuteAfterFramesCoroutine(delay, action));
        }
        public static void ExecuteAfterSeconds(this MonoBehaviour mb, float delay, Action action)
        {
            mb?.StartCoroutine(ExecuteAfterSecondsCoroutine(delay, action));
        }

        private static IEnumerator ExecuteAfterFramesCoroutine(int delay, Action action)
        {
            for (int i = 0; i < delay; i++)
                yield return null;

            action();
        }
        private static IEnumerator ExecuteAfterSecondsCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterCrouch
{
    internal static class Patches
    {
        private static PlayerControllerB __mainPlayer;
        public static bool crouchOnHitGround = false;
        public static bool Initialized = false;
        private const float maxDistance = 0.72f;
        public static bool InstaJump = true;
        private const string Crouch = nameof(Crouch);
        private const string Update = nameof(Update);
        private const string Jump_performed = nameof(Jump_performed);

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), Update)]
        public static void ReadInput(PlayerControllerB __instance, RaycastHit ___hit)
        {
            if (!Initialized)
            {
                return;
            }

            if (__mainPlayer == null)
            {
                __mainPlayer = StartOfRound.Instance.localPlayerController;
            }
            PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;
            // Testing conditions where the player crouch state cannot be changed
            if ((!__instance.IsOwner || !__instance.isPlayerControlled || (__instance.IsServer && !__instance.isHostPlayerObject))
                && !__instance.isTestingPlayer && ValidatePlayerState())
            {
                InputAction CrouchAction = GetCrouch();
                if (CrouchAction == null)
                {
                    return;
                }
                // The player is no longer holding the crouch button OR the player was forced to uncrouch
                // Other players see that this player is not crouching
                // Player is in a space where they cannot jump/must crouch
                if (!localPlayerController.playerBodyAnimator.GetBool("crouching"))
                {
                    return;
                }
                if (!CrouchAction.IsPressed() && localPlayerController.CanJump())
                {
                    localPlayerController.isCrouching = false;
                    localPlayerController.playerBodyAnimator.SetBool("crouching", false);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Prefix_PreformedJump(PlayerControllerB __instance, RaycastHit ___hit)
        {
            if (__instance.isCrouching && __instance.CanJump(___hit))
            {
                __instance.isCrouching = false;
                crouchOnHitGround = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), Jump_performed)]
        public static void Postfix_PreformedJump(PlayerControllerB __instance)
        {
            if (crouchOnHitGround)
            {
                __instance.isCrouching = true;
                crouchOnHitGround = false;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerHitGroundEffects))]
        public static void PlayerHitGroundEffects(PlayerControllerB __instance)
        {
            if (__instance.isCrouching)
            {
                __instance.playerBodyAnimator.SetTrigger("startCrouching");
                __instance.playerBodyAnimator.SetBool("crouching", true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(Crouch_performed))]
        public static void Crouch_performed()
        {
        }
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(PlayerControllerB), Update)]
        public static IEnumerable<CodeInstruction> RemoveJumpDelay(IEnumerable<CodeInstruction> instructions)
        {
            if (!InstaJump)
            {
                return instructions;
            }

            List<CodeInstruction> list = new(instructions);
            for (int i = 0; i < list.Count; i++)
            {
                CodeInstruction val = list[i];
                if (val.opcode != OpCodes.Newobj)
                {
                    continue;
                }

                ConstructorInfo? constructorInfo = val.operand as ConstructorInfo;
                if (constructorInfo?.DeclaringType == typeof(WaitForSeconds))
                {
                    list[i] = new CodeInstruction(OpCodes.Ldnull, null);
                    list.RemoveAt(i - 1);
                    i--;

                    Plugin.Log.LogDebug("Patched Instant-Jump");
                }
            }

            return list;
        }
        public static bool CanJump(this PlayerControllerB localPlayerController, RaycastHit? ___hit = null)
        {
            bool canJump = !Physics.Raycast(localPlayerController.gameplayCamera.transform.position, Vector3.up, out RaycastHit hit,
                maxDistance, localPlayerController.playersManager.collidersAndRoomMask, QueryTriggerInteraction.Ignore);
            ___hit = hit;
            return canJump;
        }

        public static bool ValidatePlayerState()
        {
            return !(__mainPlayer.inTerminalMenu || __mainPlayer.isTypingChat ||
                     __mainPlayer.isPlayerDead || __mainPlayer.quickMenuManager.isMenuOpen);
        }

        public static InputAction GetCrouch()
        {
            return IngamePlayerSettings.Instance.playerInput.actions.FindAction(Crouch);
        }
    }
}

using UnityEngine;

namespace SpaceSunShine
{
    internal class OnEvents
    {
        public class OnFloodlight : MonoBehaviour
        {
            public void OnEnable()
            {
                SpaceSunShine.ShipLightsPostClone?.SetActive(false);
            }
            public void OnDisable()
            {
                SpaceSunShine.ShipLightsPostClone?.SetActive(true);
            }
            public void OnDestroy()
            {
                SpaceSunShine.ShipLightsPostClone = null;
            }
        }
        public class OnLadder : MonoBehaviour
        {
            public void OnEnable()
            {
                SpaceSunShine.OutsideShipRoomClone?.SetActive(false);
            }
            public void OnDisable()
            {
                SpaceSunShine.OutsideShipRoomClone?.SetActive(true);
            }
            public void OnDestroy()
            {
                SpaceSunShine.OutsideShipRoomClone = null;
            }
        }
    }
}

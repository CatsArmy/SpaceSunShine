using UnityEngine;
using UnityEngine.Rendering;


namespace SpaceSunShine
{
    internal static class LethalExpansionLightSettingsExtension
    {
        private const float Matrix4x4One = 1.00000f;
        private const float Matrix4x4Zero = 0.00000f;
        private static Matrix4x4 ShadowMatrixOverride()
        {
            Vector4 Collum1 = new Vector4(Matrix4x4One, Matrix4x4Zero, Matrix4x4Zero, Matrix4x4Zero);
            Vector4 Collum2 = new Vector4(Matrix4x4Zero, Matrix4x4One, Matrix4x4Zero, Matrix4x4Zero);
            Vector4 Collum3 = new Vector4(Matrix4x4Zero, Matrix4x4Zero, Matrix4x4One, Matrix4x4Zero);
            Vector4 Collum4 = new Vector4(Matrix4x4Zero, Matrix4x4Zero, Matrix4x4Zero, Matrix4x4One);

            Matrix4x4 shadowMatrixOverride = new Matrix4x4(Collum1, Collum2, Collum3, Collum4);
            return shadowMatrixOverride;
        }

        private static Matrix4x4 WorldToLocalMatrix()
        {
            Vector4 Collum1 = new Vector4(0.86603f, Matrix4x4Zero, 0.50000f, Matrix4x4Zero);
            Vector4 Collum2 = new Vector4(-0.38302f, 0.64279f, 0.66341f, Matrix4x4Zero);
            Vector4 Collum3 = new Vector4(-0.32139f, -0.76604f, 0.55667f, Matrix4x4Zero);
            Vector4 Collum4 = new Vector4(Matrix4x4Zero, Matrix4x4Zero, Matrix4x4Zero, Matrix4x4One);

            Matrix4x4 worldToLocalMatrix = new Matrix4x4(Collum1, Collum2, Collum3, Collum4);
            return worldToLocalMatrix;
        }
        private static Matrix4x4 LocalToWorldMatrix()
        {
            Vector4 Collum1 = new Vector4(0.86603f, -0.382302f, -0.32139f, Matrix4x4Zero);
            Vector4 Collum2 = new Vector4(Matrix4x4Zero, 0.64279f, -0.76604f, Matrix4x4Zero);
            Vector4 Collum3 = new Vector4(0.50000f, 0.66341f, 0.55667f, Matrix4x4Zero);
            Vector4 Collum4 = new Vector4(Matrix4x4Zero, Matrix4x4Zero, Matrix4x4Zero, Matrix4x4One);

            Matrix4x4 worldToLocalMatrix = new Matrix4x4(Collum1, Collum2, Collum3, Collum4);
            return worldToLocalMatrix;
        }

        private static LightBakingOutput LightBakingOutput()
        {
            LightBakingOutput baking = new LightBakingOutput();
            baking.probeOcclusionLightIndex = -1;
            baking.occlusionMaskChannel = -1;
            baking.lightmapBakeType = LightmapBakeType.Realtime;
            baking.mixedLightingMode = MixedLightingMode.Shadowmask;
            baking.isBaked = false;
            return baking;
        }

        private static float[] LayerShadowCullDistances()
        {
            float[] layerShadowCullDistances = new float[32];
            for (int i = layerShadowCullDistances.Length - 2; i >= 0; i--)
            {
                layerShadowCullDistances[i] = 0f;
            }
            return layerShadowCullDistances;
        }
        private static void SetRotation(this Transform transform, Quaternion rotation)
        {
            transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }
        private static void SetRotation(this Quaternion quaternion, Quaternion rotation)
        {
            quaternion.Set(rotation.x, rotation.y, rotation.z, rotation.w);
        }
        private static void SetMatrix(this Matrix4x4 matrix4, Matrix4x4 matrix)
        {
            for (int i = 0; i < 4; i++)
            {
                matrix4.SetColumn(i, matrix.GetColumn(i));
            }
        }

        private const int renderingLayerMask = 1;
        private const int shadowCustomResolution = -1;
        private const float tempertaure = 6570f;
        private const float bounceIntensity = 1f;
        private const float cookieSize = 10f;
        private const float innerSpotAngle = 21.8021f;
        private const float intensity = 10f;
        private const float range = 10f;
        private const float shadowBias = 0.05f;
        private const float shadowNearPlane = 0.2f;
        private const float shadowNormalBias = 0.4f;
        private const LightShadowResolution shadowResolution = LightShadowResolution.FromQualitySettings;
        private const LightShadows shadows = LightShadows.Soft;
        private static Color color = new Color(1f, 0.9569f, 0.8392f, 1f);
        private static Vector3 up = new Vector3(-0.383f, 0.6428f, 0.6634f);
        private static Vector3 right = new Vector3(0.866f, 0f, 0.5f);
        private static Vector3 forward = new Vector3(-0.3214f, -0.766f, 0.5567f);
        private static Vector3 rotation = new Vector3(50f, -30f, 0f);
        private static Quaternion rootRotation = Quaternion.Euler(rotation);
        public static void UseLethalExpansionLightSettings(this Light Light)
        {
            Light.transform.SetParent(null);
            Light.transform.localScale = Vector3.one;
            Light.transform.position = Vector3.zero;
            Light.transform.localPosition = Vector3.zero;

            Animator animator = Light.transform.gameObject.GetComponent<Animator>();
            animator.enabled = false;
            animator.rootRotation.SetRotation(rootRotation);
            //animator.enabled = true;
            Light.transform.SetRotation(rootRotation);
            Light.transform.up = up;
            Light.transform.right = right;
            Light.transform.forward = forward;
            Light.transform.localToWorldMatrix.SetMatrix(LocalToWorldMatrix());
            Light.transform.worldToLocalMatrix.SetMatrix(WorldToLocalMatrix());
            Light.bakingOutput = LightBakingOutput();
            Light.bounceIntensity = bounceIntensity;
            Light.boundingSphereOverride = Vector4.zero;
            Light.color = color;
            Light.colorTemperature = tempertaure;
            Light.cookieSize = cookieSize;
            Light.cookie = null;
            Light.flare = null;
            Light.innerSpotAngle = innerSpotAngle;
            Light.intensity = intensity;
            Light.layerShadowCullDistances = LayerShadowCullDistances();
            Light.lightShadowCasterMode = LightShadowCasterMode.Everything;
            Light.range = range;
            Light.renderingLayerMask = renderingLayerMask;
            Light.renderMode = LightRenderMode.Auto;
            Light.shadowBias = shadowBias;
            Light.shadowCustomResolution = shadowCustomResolution;
            Light.shadowMatrixOverride = ShadowMatrixOverride();
            Light.shadowNearPlane = shadowNearPlane;
            Light.shadowNormalBias = shadowNormalBias;
            Light.shadowResolution = shadowResolution;
            Light.shadows = shadows;
            Light.shape = LightShape.Cone;
            Light.spotAngle = 30f;
            Light.type = LightType.Directional;
            Light.useBoundingSphereOverride = false;
            Light.useColorTemperature = true;
            Light.useShadowMatrixOverride = false;
            Light.useViewFrustumForShadowCasterCull = true;
        }
        public static void Light(Light Light)
        {

        }
        public static void HDAdditionalLightData(Light Light)
        {
            ///true
            ///true
            ///false
            ///false
            ///0.5f
            ///true
            ///null
            ///-1
            ///Camara
            ///Off
            ///120f
            ///Rectangle
            ///1f
            ///90f
            ///0.05f
            ///0.01f
            ///Color(1, 0.9569, 0.8392 1)
            ///true
            ///30f
            ///false
            ///150000000000f
            ///false
            ///null
            ///true
            ///0
            ///15f
            ///0f
            ///0f
            ///10000f
            ///16
            ///16
            ///true
            ///4f
            ///2f
            ///Color(1 1 1 1)
            ///null
            ///null
            ///null
            ///true
            ///0f
            ///0f
            ///10f
            ///true
            ///5
            ///leg light = SpaceLight(Clone) Light
            ///1f
            ///1f
            ///LightLayerDefault
            ///0.5f
            ///Lux
            ///true
            ///1f
            ///0.001f
            ///0.99f
            ///0.1f
            ///false
            ///0.75f
            ///4
            ///true
            ///false
            ///false
            ///10f
            ///false
            ///false
            ///0
            ///float[] 
            ///{
            ///0.2f,
            ///0.2f,
            ///0.2f,
            ///0.2f
            ///}
            ///float[]
            ///{
            ///0.05f,
            ///0.2f,
            ///0.3f
            ///}
            ///1f
            ///10000f
            ///CascadedDirectional
            ///0.1f
            ///0
            ///


            ///ShadowMap on
            ///every frame
            ///512
            ///
        }
    }
}
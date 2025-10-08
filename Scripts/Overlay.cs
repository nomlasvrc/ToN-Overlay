
using System;
using TMPro;
using UnityEngine;
using Valve.VR;

namespace Nomlas.ToN_Overlay
{
    [RequireComponent(typeof(OverlayPreset))]
    public class Overlay : MonoBehaviour
    {
        // オーバーレイのサイズなど
        private OverlayPreset overlayPreset;

        // オーバーレイに必要な定数
        private const string overlayKey = "jp.nomlas.ToN_Overlay";
        private const string overlayName = "ToN_Overlay";

        // 変数たち
        private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;
        private RenderTexture rt;
        [SerializeField] private Camera overlayCamera;
        [SerializeField] private TextMeshProUGUI overlayStatusText;
        private bool hasSetTexture = false;

        private void Start()
        {
            overlayPreset = GetComponent<OverlayPreset>();

            CreateTexture();

            InitOpenVR();
            overlayHandle = CreateOverlay(overlayKey, overlayName);
            FlipOverlayVertical(overlayHandle);
            SetOverlaySize(overlayHandle, overlayPreset.Size);
            ShowOverlay(overlayHandle);

            StartInitializeProcess();
        }

        private void Update()
        {
            if (!hasSetTexture && rt != null && rt.IsCreated() == true)
            {
                SetOverlayRenderTexture(overlayHandle, rt);
            }
        }

        private void CreateTexture()
        {
            if (rt != null) return;
            rt = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);
            rt.Create();
            overlayCamera.targetTexture = rt;
        }

        private void StartInitializeProcess()
        {
            if (OpenVR.System == null)
            {
                Debug.LogWarning("[ToN_Overlay] 初期化プロセスに失敗しました。");
                return;
            }

            var rightControllerIndex = OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
            if (rightControllerIndex != OpenVR.k_unTrackedDeviceIndexInvalid)
            {
                SetOverlayTransformRelative(overlayHandle, rightControllerIndex, overlayPreset.Position, overlayPreset.Rotation);
            }
        }

        #region オーバーレイの処理

        private static bool TryOverlayProcess(string func, EVROverlayError error)
        {
            if (error != EVROverlayError.None)
            {
                Debug.LogError($"[ToN_Overlay]{func}に失敗しました: {error}");
                return false;
            }
            return true;
        }

        private static ulong CreateOverlay(string key, string name)
        {
            var handle = OpenVR.k_ulOverlayHandleInvalid;
            TryOverlayProcess("オーバーレイの作成", OpenVR.Overlay.CreateOverlay(key, name, ref handle));
            return handle;
        }

        private static void FlipOverlayVertical(ulong handle)
        {
            var bounds = new VRTextureBounds_t
            {
                uMin = 0,
                uMax = 1,
                vMin = 1,
                vMax = 0
            };

            TryOverlayProcess("反転", OpenVR.Overlay.SetOverlayTextureBounds(handle, ref bounds));
        }

        private static void SetOverlaySize(ulong handle, float size)
        {
            TryOverlayProcess("オーバーレイのサイズ設定", OpenVR.Overlay.SetOverlayWidthInMeters(handle, size));
        }

        private static void SetOverlayTransformRelative(ulong handle, uint deviceIndex, Vector3 position, Quaternion rotation)
        {
            var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
            var matrix = rigidTransform.ToHmdMatrix34();
            TryOverlayProcess("オーバーレイの位置設定", OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(handle, deviceIndex, ref matrix));
        }

        private static void ShowOverlay(ulong handle)
        {
            TryOverlayProcess("オーバーレイの表示", OpenVR.Overlay.ShowOverlay(handle));
        }

        private void SetOverlayRenderTexture(ulong handle, RenderTexture renderTexture)
        {
            var nativeTexturePtr = renderTexture.GetNativeTexturePtr();
            var texture = new Texture_t
            {
                eColorSpace = EColorSpace.Auto,
                eType = ETextureType.DirectX,
                handle = nativeTexturePtr
            };
            var isSuccess = TryOverlayProcess("テクスチャの描画", OpenVR.Overlay.SetOverlayTexture(handle, ref texture));
            if (!hasSetTexture && isSuccess)
            {
                hasSetTexture = true;
                overlayStatusText.text = "オーバーレイ: <color=green>準備完了</color>";
            }
        }

        private static void DestroyOverlay(ulong handle)
        {
            if (handle != OpenVR.k_ulOverlayHandleInvalid)
            {
                TryOverlayProcess("オーバーレイの破棄", OpenVR.Overlay.DestroyOverlay(handle)); // OpenVR.Shutdownより先に実行する必要がある
            }
        }
        #endregion

        #region OpenVR
        private void InitOpenVR()
        {
            if (OpenVR.System != null) return;

            var error = EVRInitError.None;
            OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);
            if (error != EVRInitError.None)
            {
                throw new Exception("OpenVRの初期化に失敗しました: " + error);
            }
        }

        private void OnApplicationQuit()
        {
            DestroyOverlay(overlayHandle);
        }

        private void OnDestroy()
        {
            rt?.Release();
            ShutdownOpenVR();
        }

        private void ShutdownOpenVR()
        {
            if (OpenVR.System != null)
            {
                OpenVR.Shutdown();
            }
        }
        #endregion
    }
}

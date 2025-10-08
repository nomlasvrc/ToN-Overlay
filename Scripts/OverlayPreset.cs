
using UnityEngine;

namespace Nomlas.ToN_Overlay
{
    public interface IOverlayPreset
    {
        public string PresetName { get; }
        public float Size { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
    }
    
    public class OverlayPreset : MonoBehaviour, IOverlayPreset
    {
        [SerializeField] private string presetName;
        [Space]
        [Range(0, 1)] private float size = 0.25f;
        [Range(-0.5f, 0.5f)][SerializeField] private float positionX;
        [Range(-0.5f, 0.5f)][SerializeField] private float positionY;
        [Range(-0.5f, 0.5f)][SerializeField] private float positionZ;
        [Range(0, 360)][SerializeField] private float rotationX;
        [Range(0, 360)][SerializeField] private float rotationY;
        [Range(0, 360)][SerializeField] private float rotationZ;

        public string PresetName => presetName;
        public float Size => size;
        public Vector3 Position => new Vector3(positionX, positionY, positionZ);
        public Quaternion Rotation => Quaternion.Euler(rotationX, rotationY, rotationZ);
    }
}

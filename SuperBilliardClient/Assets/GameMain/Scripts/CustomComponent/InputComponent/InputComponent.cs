using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class InputComponent : GameFrameworkComponent
    {
        public Vector3 MousePosition
        {
            get
            {
                return Input.mousePosition;
            }
        }
        public bool LeftMouseButton => Input.GetMouseButton(0);
        public bool LeftMouseButtonDown => Input.GetMouseButtonDown(0);
        public bool LeftMouseButtonUp => Input.GetMouseButtonUp(0);
        public bool FireBilliard => Input.GetKeyDown(KeyCode.Space);
        public bool PlaceWhiteBall => Input.GetMouseButtonDown(0);
        public float StorageEnergy => Input.GetAxis("Vertical");
        public void SwitchUI()
        {

        }
        public void SwitchGamePlay()
        {

        }
    }
}
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using LabDiner.Shared.Input;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StaffBox : MonoBehaviour, IInteractable
    {
        [SerializeField] VerticalSwingEffect _arrowSwing;
        private Component _staff;
        private IStaffUnboxer _spawner;
        public void Setup(Component staff, IStaffUnboxer spawner)
        {
            _staff = staff;
            _spawner = spawner;
            _arrowSwing.Show();
        }
        public bool CanInteract()
        {
            return true;
        }

        public void OnInteract()
        {
            _spawner.UnboxStaff(_staff);
            PoolContext.Instance.StaffBoxPool.ReturnToPool(this);
            _arrowSwing.Hide();
        }
    }
}

using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StaffBox : MonoBehaviour, IInteractable
    {
        private Component _staff;
        private IStaffUnboxer _spawner;
        public void Setup(Component staff, IStaffUnboxer spawner)
        {
            _staff = staff;
            _spawner = spawner;
        }
        public bool CanInteract()
        {
            return true;
        }

        public void OnInteract()
        {
            _spawner.UnboxStaff(_staff);
            PoolContext.Instance.StaffBoxPool.ReturnToPool(this);
        }
    }
}
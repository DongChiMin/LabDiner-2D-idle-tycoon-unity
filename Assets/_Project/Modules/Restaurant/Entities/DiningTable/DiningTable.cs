
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class DiningTable : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField] private GuestContext _occupiedGuest;

        public bool IsOccupied => _occupiedGuest != null;

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }
    }
}
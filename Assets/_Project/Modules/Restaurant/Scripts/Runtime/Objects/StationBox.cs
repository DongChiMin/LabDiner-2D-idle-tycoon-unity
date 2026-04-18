
using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StationBox : MonoBehaviour, IInteractable
    {
        private Station _station;
        private IStationUnboxer _spawner;
        public void Setup(Station station, IStationUnboxer spawner)
        {
            _station = station;
            _spawner = spawner;
        }
        public bool CanInteract()
        {
            return true;
        }

        public void OnInteract()
        {
            _spawner.UnboxStation(_station);
            PoolContext.Instance.StationBoxPool.ReturnToPool(this);
        }
    }
}
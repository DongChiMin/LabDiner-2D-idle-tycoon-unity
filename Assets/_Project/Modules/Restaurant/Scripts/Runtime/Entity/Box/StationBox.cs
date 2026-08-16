using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using LabDiner.Shared.Input;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StationBox : MonoBehaviour, IInteractable
    {
        [SerializeField] VerticalSwingEffect _arrowSwing;
        private Station _station;
        private IStationUnboxer _spawner;
        public void Setup(Station station, IStationUnboxer spawner)
        {
            _station = station;
            _spawner = spawner;
            _arrowSwing.Show();
        }
        public bool CanInteract()
        {
            return true;
        }

        public void OnInteract()
        {
            _spawner.UnboxStation(_station);
            PoolContext.Instance.StationBoxPool.ReturnToPool(this);
            _arrowSwing.Hide();
        }
    }
}
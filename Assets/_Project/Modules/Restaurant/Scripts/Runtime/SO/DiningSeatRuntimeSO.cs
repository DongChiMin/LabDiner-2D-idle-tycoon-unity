using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "DiningSeatRuntimeSet", menuName = "SO/Runtime/DiningSeat")]
    public class DiningSeatRuntimeSO : ScriptableObject
    {
        // Danh sách tất cả các ghế đã được spawn, được quản lý bởi DiningTableManager
        public List<DiningSeat> ActiveSeats = new List<DiningSeat>();

        public void SetOccupy(DiningSeat seat, GuestContext guest)
        {
            seat.Occupy(guest);
        }

        public void Add(DiningSeat seat)
        {
            if (!ActiveSeats.Contains(seat)) ActiveSeats.Add(seat);
        }

        public void Remove(DiningSeat seat)
        {
            if (ActiveSeats.Contains(seat)) ActiveSeats.Remove(seat);
        }

        // API lấy danh sách ghế trống
        public List<DiningSeat> GetAvailableSeats()
        {
            return ActiveSeats.FindAll(seat => !seat.IsOccupied);
        }

        // Reset danh sách khi game bắt đầu/kết thúc
        public void Clear() => ActiveSeats.Clear();
    }
}
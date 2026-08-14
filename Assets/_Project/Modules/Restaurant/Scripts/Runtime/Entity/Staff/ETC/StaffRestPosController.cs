using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public partial class StaffRestPosController : MonoBehaviour
    {
        [Header("[DEBUG]")]
        [SerializeField] List<StaffRestPos> _restPositions = new List<StaffRestPos>();

        public void Rebuild()
        {
            //Xóa list cũ, tạo mới list, thêm các bàn ăn vào list
            _restPositions.Clear();
            _restPositions.AddRange(GetComponentsInChildren<StaffRestPos>());
        }

        public StaffRestPos GetAvailableRestPos(StaffType staffType)
        {
            foreach (var restPos in _restPositions)
            {
                if (!restPos.IsOccupied && restPos.StaffType == staffType)
                {
                    return restPos;
                }
            }

            Debug.LogWarning($"No available rest position for staff type {staffType}. Please check the rest positions in the scene.");
            return null; // Không có bàn ăn trống
        }
    }
}
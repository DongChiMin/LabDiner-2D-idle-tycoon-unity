using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Game/Level Config")]
    public class LevelConfigSO : ScriptableObject
    {
        public int levelIndex;
        public string levelName;

        [Header("Guest Settings")]
        public int maxGuestCount;                      // Số lượng khách tối đa xuất hiện trong level này
        [Header("Guest Order Settings")]
        public int maxUniqueStations;           // Số lượng trạm chính khác nhau tối đa có trong đơn hàng của khách (ví dụ: 2 thì khách chỉ gọi món từ 2 trạm chính khác nhau, dù có thể gọi nhiều món từ mỗi trạm)
        public int maxTotalQtyPerOrder;         // Số lượng món tối đa trong đơn hàng của khách (ví dụ: 5 thì khách chỉ gọi tối đa 5 món, dù có thể là 5 món từ cùng 1 trạm chính)
    }
}
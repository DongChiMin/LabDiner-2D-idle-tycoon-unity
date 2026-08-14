using UnityEngine;

namespace LabDiner.LevelEditor
{
        public enum LevelDesignStep
        {
            Step1_CustomerArea = 1,   // Khu vực + Spawn Khách
            Step2_ChefArea = 2,       // Khu vực + Spawn Đầu bếp
            Step3_Stations = 3,       // CoreStation + Stations + WorkPos
            Step4_Dining = 4,         // DiningTable + Seats + WorkPos
            Step5_PassTable = 5,      // Bàn trung chuyển
            Step6_WaitingLine = 6,     // Hàng đợi
            Step7_UIOrientation = 7     // UI Orientation
        }

        public class LevelElementMarker : MonoBehaviour
        {
            [Header("Classification")]
            [SerializeField] private LevelDesignStep _step = LevelDesignStep.Step1_CustomerArea;
            [SerializeField] private int _orderInStep = 1;
            [SerializeField] private string _elementLabel = "Element";

            // Public Getters để Editor Tool đọc dữ liệu
            public LevelDesignStep Step => _step;
            public int OrderInStep => _orderInStep;
            public string ElementLabel => _elementLabel;
        }
    
}

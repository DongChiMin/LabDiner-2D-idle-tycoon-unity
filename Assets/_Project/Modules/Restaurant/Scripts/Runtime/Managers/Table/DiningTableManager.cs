
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class DiningTableManager : MonoBehaviour
    {
        [SerializeField] private List<DiningTable> _diningTables;
        [SerializeField] private TableEvent _onTableFreed;
        [SerializeField] private TableEvent _onTableDirty;

        void OnEnable()
        {
            _onTableDirty.Register(SetTableDirty);
        }

        void OnDisable()
        {
            _onTableDirty.Unregister(SetTableDirty);
        }

        public List<DiningTable> GetAvailableTables()
        {
            List<DiningTable> availableTables = new List<DiningTable>();
            foreach (var table in _diningTables)
            {
                if (!table.IsOccupied)
                    availableTables.Add(table);
            }
            return availableTables;   
        }

        public void OccupyTable(DiningTable table, GuestContext guest)
        {
            table.Occupy(guest);
        }

        private void SetTableDirty(DiningTable table)
        {
            Debug.Log("TODO: có thể set trạng thái bàn bẩn vào đây");
            table.Occupy(null);

            //Sau khi dọn xong
            _onTableFreed.Raise(table);
        }

    }
}
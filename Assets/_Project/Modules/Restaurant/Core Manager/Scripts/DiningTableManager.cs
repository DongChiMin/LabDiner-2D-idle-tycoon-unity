
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class DiningTableManager : MonoBehaviour
    {
        [SerializeField] private List<DiningTable> _diningTables;
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
    }
}
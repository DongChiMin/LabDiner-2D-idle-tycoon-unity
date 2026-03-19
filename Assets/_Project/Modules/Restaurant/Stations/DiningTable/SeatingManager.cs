using System.Collections.Generic;
using UnityEngine;

public class SeatingManager : Singleton<SeatingManager> {
    public List<Transform> tables; // Kéo các điểm Transform của ghế vào đây
    private List<bool> tableStatus;

    protected override void Awake() {
        base.Awake();
        tableStatus = new List<bool>(new bool[tables.Count]);
    }

    public Transform GetEmptyTable(out int index) {
        for (int i = 0; i < tables.Count; i++) {
            if (!tableStatus[i]) {
                tableStatus[i] = true; // Đánh dấu đã có người
                index = i;
                return tables[i];
            }
        }
        index = -1;
        return null;
    }

    public void ReleaseTable(int index) {
        if (index >= 0) tableStatus[index] = false;
    }
}
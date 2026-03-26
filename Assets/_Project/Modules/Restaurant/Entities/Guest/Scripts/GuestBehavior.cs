
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestBehavior : MonoBehaviour 
{
    [SerializeField] Dictionary<CoreStation, int> _orderDict = new Dictionary<CoreStation, int>();
    [SerializeField] private float _eatDuration = 3f;
    [SerializeField] private float _payDuration = 0f;

    public IEnumerator WaitForServe() {
        Debug.Log("Đang chờ phục vụ...");
        // Ở đây có thể bật animation chờ đợi
        yield return null; // Chờ cho đến khi được gọi tiếp tục
    }

    public IEnumerator WaitForFood() {
        Debug.Log("Đang chờ đồ ăn...");
        // Ở đây có thể bật animation chờ đợi
        yield return null; // Chờ cho đến khi được gọi tiếp tục
    }

    public IEnumerator Eat() {
        Debug.Log("Đang ăn...");
        // Ở đây có thể bật animation ăn uống
        yield return new WaitForSeconds(_eatDuration);
        Debug.Log("Ăn xong rồi!");
    }

    public IEnumerator Pay() {
        Debug.Log("Đang trả tiền...");
        yield return new WaitForSeconds(_payDuration);
        Debug.Log("Trả tiền xong rồi!");
    }

    public void SetOrder(Dictionary<CoreStation, int> orderDict) {
        _orderDict = orderDict;
    }
}
}

using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestBehavior : MonoBehaviour 
{
    [SerializeField] private float eatDuration = 3f;
    [SerializeField] private float payDuration = 0f;

    public IEnumerator WaitForServe() {
        Debug.Log("Đang chờ có bàn trống...");
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
        yield return new WaitForSeconds(eatDuration);
        Debug.Log("Ăn xong rồi!");
    }

    public IEnumerator Pay() {
        Debug.Log("Đang trả tiền...");
        yield return new WaitForSeconds(payDuration);
        Debug.Log("Trả tiền xong rồi!");
    }
}
}
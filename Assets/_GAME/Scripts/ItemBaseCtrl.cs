using UnityEngine;

namespace _GAME.Scripts
{
    public class ItemBaseCtrl : MonoBehaviour
    {
        public SpriteRenderer sprt;
        public Vector3 successPos;
        public bool isSuccess;

        public void Init()
        {
            // thực hiện lấy lại reference của component
            sprt = GetComponent<SpriteRenderer>();
            // thực hiện ưu tiên hiển thị các item chưa đúng lên trên
            sprt.sortingOrder = 1;

            successPos = transform.position;
            isSuccess = false;
        }

        public void RandomPos()
        {
            var posX = Random.Range(-5, 5);
            var posY = Random.Range(-5, 5);
            var posZ = successPos.z;
            transform.position = new Vector3(posX, posY, posZ);
        }

        public void SetSelected(bool selected)
        {
            if (selected)
                sprt.sortingOrder = 2;
            else sprt.sortingOrder = 1;
        }

        public bool IsSuccess()
        {
            return isSuccess;
        }

        public void CheckCanSuccess()
        {
            // kiểm tra khoảng cách giữa vị trí hiện tại và vị trí đúng
            // ở vị trí gần đúng thì set về vị trí đúng
            if (Vector3.Distance(transform.position, successPos) < 0.35f)
            {
                transform.position = successPos;
                isSuccess = true;

                sprt.sortingOrder = 0;

                Debug.Log("Item is success!");
            }
        }
    }
}
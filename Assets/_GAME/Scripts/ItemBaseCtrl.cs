using UnityEngine;

namespace _GAME.Scripts
{
    public class ItemBaseCtrl : MonoBehaviour
    {
        public Vector3 successPos;
        public bool isSuccess;

        public void Init()
        {
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
                
                Debug.Log("Item is success!");
            }
        }
    }
}
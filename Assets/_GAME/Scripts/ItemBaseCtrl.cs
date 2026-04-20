using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Scripts
{
    public class ItemBaseCtrl : MonoBehaviour
    {
        public int index;
        public SpriteRenderer sprt;
        public Vector3 successPos;
        public bool isSuccess;
        public ItemBaseCtrl parent;
        public List<ItemBaseCtrl> sameItems;

        public void Init(int newIndex)
        {
            index = newIndex;
            // thực hiện lấy lại reference của component
            sprt ??= GetComponent<SpriteRenderer>();
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
            // kiểm tra item_cha đã ở vị trí đúng chưa mới cho phép iten đc đặt vị trí đúng
            if (parent != null && !parent.IsSuccess())
            {
                Debug.Log("có item cha và item cha chưa được đặt đúng vị trí");
                return;
            }

            // kiểm tra khoảng cách giữa vị trí hiện tại và vị trí đúng
            // ở vị trí gần đúng thì set về vị trí đúng
            if (Vector3.Distance(transform.position, successPos) < 0.35f)
            {
                transform.position = successPos;
                isSuccess = true;

                sprt.sortingOrder = 0;

                Debug.Log("Item is success!");
            }
            else CheckCanSuccessInSameItemPos();
        }

        private void CheckCanSuccessInSameItemPos()
        {
            // ko có item giống thì bỏ qua
            if (sameItems == null || sameItems.Count == 0) return;

            // kiểm tra với từng same item
            foreach (var sameItem in sameItems)
            {
                if (Vector3.Distance(transform.position, sameItem.successPos) < 0.35f)
                {
                    // nếu ở gần vị trí đúng của same item thì move nó về vị trí đúng
                    transform.position = sameItem.successPos;
                    isSuccess = true;
                    sprt.sortingOrder = 0;
                    // và tráo đổi lại vị trí đúng của 2 item với nhau
                    sameItem.successPos = successPos;
                    // và xóa liên kết của 2 item để tránh same item có thể đặt lại vị trí vừa mất
                    sameItem.RemoveSameItem(this.index);

                    Debug.Log("Item is success!");
                    // và dừng kiểm kiểm tra vị trí ngay
                    return;
                }
            }
        }

        private void RemoveSameItem(int sameItemIndex)
        {
            for (var i = 0; i < sameItems.Count; i++)
            {
                if (sameItems[i].index == sameItemIndex)
                {
                    sameItems.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
using System.Collections.Generic;
using DG.Tweening;
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

        public void Init()
        {
            isSuccess = false;
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                sprt.sortingOrder = 2;
                // khi đang được người chơi cầm lên để di chuyển thì sẽ để item nằm im và di chuyển theo tay thôi
                ClearAnimation();
            }
            else
            {
                sprt.sortingOrder = 1;
                // khi item được thả ra thì đặt lại trạng thái lơ lửng
                PlayAnimationActive();
            }
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
            if (Vector3.Distance(transform.position, successPos) < 0.5f)
            {
                transform.DOMove(successPos, 0.25f);
                isSuccess = true;
                sprt.sortingOrder = 0;
                ClearAnimation();

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
                if (Vector3.Distance(transform.position, sameItem.successPos) < 0.5f)
                {
                    // nếu ở gần vị trí đúng của same item thì move nó về vị trí đúng
                    transform.DOMove(sameItem.successPos, 0.25f);
                    isSuccess = true;
                    sprt.sortingOrder = 0;
                    ClearAnimation();

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

        private List<Tweener> _animActive = new();
        private void ClearAnimation()
        {
            foreach (var tweener in _animActive)
            {
                tweener.Complete();
                tweener.Kill();
            }
            // chỉnh góc xoay của item về ban đầu
            transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
        }

        public void PlayAnimationActive()
        {
            ClearAnimation();

            var moveHeight = 0.3f; // độ cao lên xuống
            var moveDuration = 1f; // thời gian 1 nhịp sóng
            var tiltAngle = 10f; // góc nghiêng
            var tiltDuration = 3.5f;

            // di chuyển lên xuống
            var anim1 = transform.DOMoveY(transform.position.y + moveHeight, moveDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // nghiêng qua lại
            var anim2 = transform.DORotate(new Vector3(0, 0, tiltAngle), tiltDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            _animActive.Add(anim1);
            _animActive.Add(anim2);
        }
    }
}
using System.Collections.Generic;
using _GAME.Scripts;
using UnityEngine;

public class LevelDataController : MonoBehaviour
{
    public List<ItemBaseCtrl> listItem;

    public void Init()
    {
        // giá trị đã được init bằng AutoReference
        // // thực hiện khởi tạo các giá trị ban đầu cho item
        // for (var i = 0; i < listItem.Count; i++)
        //     listItem[i].Init(i);

        // thực hiện set vị trí ngẫu nhiên cho item khi vào đầu level
        foreach (var item in listItem)
        {
            item.transform.SetParent(transform);
            item.RandomPos();
        }
    }

    [ContextMenu("AutoReference")]
    public void AutoReference()
    {
        AutoReferenceParent();
        AutoReferenceSameItem();
        AutoReferenceOther();
    }

    private void AutoReferenceParent()
    {
        // làm mới lại danh sách item
        listItem = new List<ItemBaseCtrl>();

        // thực hiện quét qua tất cả các item con của level
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<ItemBaseCtrl>();
            if (child == null) continue;
            // nếu child là item có thể thao tác thì mới thêm vào danh sách
            child.index = listItem.Count;
            listItem.Add(child);
            // lớp con đầu tiên sẽ ko bị phụ thuộc vào item khác
            child.parent = null;
            // tiếp tục tự tìm reference cho các item con
            AutoReferenceChild(child);
            // tự động cập nhật trục z theo layer
            var oldPos = child.transform.localPosition;
            child.transform.localPosition = new Vector3(oldPos.x, oldPos.y, child.index * -0.001f);
        }
    }

    private void AutoReferenceChild(ItemBaseCtrl item)
    {
        // thực hiện quét qua tất cả các item con của level
        for (var i = 0; i < item.transform.childCount; i++)
        {
            var child = item.transform.GetChild(i).GetComponent<ItemBaseCtrl>();
            if (child == null) continue;
            // nếu child là item có thể thao tác thì mới thêm vào danh sách
            // thêm vào danh sách item chung
            child.index = listItem.Count;
            listItem.Add(child);
            // set item là parent của child
            child.parent = item;
            // tiếp tục tự tìm reference cho các item con
            AutoReferenceChild(child);
            // tự động cập nhật trục z theo index
            var oldPos = child.transform.localPosition;
            child.transform.localPosition = new Vector3(oldPos.x, oldPos.y, child.index * -0.001f);
        }
    }

    private void AutoReferenceSameItem()
    {
        // tự động tìm các item có tên giống nhau
        var dictSameItems = new Dictionary<string, List<ItemBaseCtrl>>();
        foreach (var item in listItem)
        {
            dictSameItems.TryAdd(item.gameObject.name, new List<ItemBaseCtrl>());
            dictSameItems[item.gameObject.name].Add(item);
        }
        // tự gán link cho các item giống nhau
        foreach (var sameItems in dictSameItems.Values)
        {
            foreach (var sameItem in sameItems)
            {
                sameItem.sameItems = new List<ItemBaseCtrl>();
                foreach (var sameItemOther in sameItems)
                {
                    // nếu item là chính nó thì bỏ qua
                    if (sameItem.index == sameItemOther.index) continue;
                    sameItem.sameItems.Add(sameItemOther);
                }
            }
        }
    }
    private void AutoReferenceOther()
    {
        foreach (var item in listItem)
        {
            // tự gán các reference này thì khi load level sẽ ko phải thực hiện init nữa
            // => sẽ cải thiện hiệu năng
            item.sprt = item.GetComponent<SpriteRenderer>();
            item.sprt.sortingOrder = 1;
            item.successPos = item.transform.position;
            item.isSuccess = false;
        }
    }
}
using System.Collections.Generic;
using _GAME.Scripts;
using UnityEngine;

public class LevelDataController : MonoBehaviour
{
    public List<ItemBaseCtrl> listItem;

    public void Init()
    {
        // thực hiện khởi tạo các giá trị ban đầu cho item
        for (var i = 0; i < listItem.Count; i++)
            listItem[i].Init(i);

        // thực hiện set vị trí ngẫu nhiên cho item khi vào đầu level
        foreach (var item in listItem)
            item.RandomPos();
    }
}
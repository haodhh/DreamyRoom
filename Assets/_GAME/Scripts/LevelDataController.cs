using System.Collections.Generic;
using _GAME.Scripts;
using UnityEngine;

public class LevelDataController : MonoBehaviour
{
    public List<ItemBaseCtrl> listItem;

    private void Start()
    {
        // thực hiện khởi tạo các giá trị ban đầu cho item
        foreach (var item in listItem)
            item.Init();

        // thực hiện set vị trí ngẫu nhiên cho item khi vào đầu level
        foreach (var item in listItem)
            item.RandomPos();
    }
}
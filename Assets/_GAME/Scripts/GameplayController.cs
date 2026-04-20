using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public List<LevelDataController> listLevel;
    private LevelDataController curLevel;

    // Start is called before the first frame update
    void Start()
    {
        PlayLevel(0);
    }

    private void PlayLevel(int level)
    {
        // thực hiện xóa level cũ
        if (curLevel != null) Destroy(curLevel.gameObject);

        // load level mới lên
        curLevel = Instantiate(listLevel[level], transform);
        curLevel.Init();
    }
}
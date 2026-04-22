using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;
    private void Awake() { Instance = this; }

    public List<LevelDataController> listLevel;
    public LevelDataController curLevel;

    public SkeletonAnimation boxAnim;

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

        boxAnim.gameObject.SetActive(true);
    }
}
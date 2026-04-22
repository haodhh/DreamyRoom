using _GAME.Scripts;
using UnityEngine;

public class UserActionController : MonoBehaviour
{
    private ItemBaseCtrl _curItemSelected;
    private Vector3 _deltaPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HandlePutItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandlePickItem();

            // thực hiện kiểm tra spawn item khi ko pick trúng item nào
            if (_curItemSelected == null)
            {
                CheckSpawnItem();
            }
        }

        if (_curItemSelected != null)
        {
            HandleMoveItem();
        }
    }

    private void HandlePickItem()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rayCastPos = new Vector3(mousePos.x, mousePos.y, 0);
        var rayCastHits = Physics2D.RaycastAll(rayCastPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Gameplay"));
        foreach (var hit in rayCastHits)
        {
            if (hit.collider != null && hit.collider.tag.Equals("Item"))
            {
                var itemSelected = hit.collider.gameObject.GetComponent<ItemBaseCtrl>();
                if (itemSelected.IsSuccess()) continue;
                _curItemSelected = itemSelected;
                _deltaPos = _curItemSelected.transform.position - rayCastPos;
                _curItemSelected.SetSelected(true);
                break;
            }
        }
    }

    private void CheckSpawnItem()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rayCastPos = new Vector3(mousePos.x, mousePos.y, 0);
        var rayCastHits = Physics2D.RaycastAll(rayCastPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Gameplay"));
        foreach (var hit in rayCastHits)
        {
            // thực hiện spawn item khi bấm vào box
            if (hit.collider != null && hit.collider.tag.Equals("Box"))
            {
                if (!GameplayController.Instance.curLevel.SpawnItem())
                {
                    // ẩn box đi khi đã hết item
                    GameplayController.Instance.boxAnim.gameObject.SetActive(false);
                }
                break;
            }
        }
    }

    private void HandleMoveItem()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var posZ = _curItemSelected.successPos.z;
        var newObjPos = new Vector3(mousePos.x, mousePos.y, posZ);
        _curItemSelected.transform.position = newObjPos + _deltaPos;
    }

    private void HandlePutItem()
    {
        if (_curItemSelected == null) return;
        var curPos = _curItemSelected.transform.position;
        var posZ = _curItemSelected.successPos.z;
        _curItemSelected.transform.position = new Vector3(curPos.x, curPos.y, posZ);
        _curItemSelected.SetSelected(false);
        _curItemSelected.CheckCanSuccess();
        _curItemSelected = null;
    }
}
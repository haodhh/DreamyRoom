using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionController : MonoBehaviour
{
    private GameObject _curObjSelected;

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
        }

        if (_curObjSelected != null)
        {
            HandleMoveItem();
        }
    }

    private void HandlePickItem()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rayCastPos = new Vector3(mousePos.x, mousePos.y, 0);
        var rayCastHits = Physics2D.RaycastAll(rayCastPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Default"));
        foreach (var hit in rayCastHits)
        {
            if (hit.collider != null)
            {
                _curObjSelected = hit.collider.gameObject;
                break;
            }
        }
    }

    private void HandlePutItem()
    {
        _curObjSelected = null;
    }

    private void HandleMoveItem()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _curObjSelected.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
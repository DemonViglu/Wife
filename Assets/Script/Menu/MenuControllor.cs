using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllor : MonoBehaviour
{
    public GameObject menu;
    public Text text;

    void Update()
    {
        OpenAndClose();
    }

    public void OpenAndClose()
    {
        Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.blue, duration: 1.0f);
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.zero);
        //Debug.Log(hitInfo.collider);
        if (hitInfo.collider == null) return;
        if(Input .GetMouseButtonDown(1)&&hitInfo.collider.transform.CompareTag("Player")&&GameManager.instance.currentGame==PlayMode.Chat)
        {
            if (DialogManager.instance.isOnPlay) return;
            menu.SetActive(!menu.activeSelf);
        }
        if (Input.GetMouseButtonDown(0) && hitInfo.collider.transform.CompareTag("Ball"))
        {
            hitInfo.transform.gameObject.GetComponent<Animator>().SetBool("boom", true);
            hitInfo.transform.gameObject.GetComponent<Balloon>().Idle();
            text.text = hitInfo.transform.parent.gameObject.GetComponent<Ball>().score.ToString();
        }

    }
}

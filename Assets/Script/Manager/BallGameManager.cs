using System.Collections;
using System.Collections.Generic;
//using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.PackageManager;
using DemonViglu.MouseInput;

public class BallGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject shot1;
    [SerializeField]private GameObject shot2;
    [SerializeField]private GameObject shot3;
    [SerializeField]private GameObject balloon;
    public int balloonsNum;
    public float balloonsV;
    public float shootingSpeed;
    private bool hasPlay;

    public Text text;
    public int score;
    public bool finished;
    public List<GameObject> ballList=new List<GameObject>();
    public event Action OnGameFinished;

    private RaycastHit2D hitInfo;
    private void Start()
    {
        hasPlay = false;
        for(int i = 0; i < balloonsNum; i++)
        {
            ballList.Add(Instantiate(balloon, shot1.transform.position, balloon.transform.rotation,this.transform));
            ballList[i].SetActive(false);
        }
        hitInfo=new RaycastHit2D();
    }
    private void Update()
    {
        GameLogic();
    }

    private void GameLogic() {
        hitInfo = MouseInputManager.instance.mouseHitInfo;
        if (!hitInfo) return;
        if (Input.GetMouseButtonDown(0) && hitInfo.collider.transform.CompareTag("Ball")) {
            hitInfo.transform.gameObject.GetComponent<Animator>().SetBool("boom", true);
            hitInfo.transform.gameObject.GetComponent<Balloon>().Idle();
            text.text = hitInfo.transform.parent.gameObject.GetComponent<BallGameManager>().score.ToString();
        }
        for (int i = 0; i < ballList.Count; i++) {
            if (ballList[i].GetComponent<Rigidbody2D>().velocity.y < 0) {
                if (!ballList[i].GetComponent<Animator>().GetBool("boom")) ballList[i].SetActive(false);
            }
        }
    }
    public void BeginToShoot()
    {
        finished = false;
        hasPlay = false;
        text.gameObject.SetActive(true);
        text.text = "0";
        score = 0;
        StartCoroutine("ShootBalloon");
    }
    public IEnumerator ShootBalloon()
    {
        GameObject tmp;

        for (int i = 0; i < balloonsNum; i++)
        {
            tmp = ballList[i];
            tmp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    tmp.transform.position=shot1.transform.position;
                    break;
                case 1:
                    tmp.transform.position = shot2.transform.position;
                    break;
                case 2:
                    tmp.transform.position = shot3.transform.position;
                    break;
            }
            tmp.SetActive(true);
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, balloonsV);
            yield return new WaitForSeconds(shootingSpeed);
        }
        EndGame();
    }
    public void EndGame()
    {
        finished=true;
        hasPlay = true;
        OnGameFinished?.Invoke();
    }

    public void CloseScoreTap()
    {
        text.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        finished = false;
        score = 0;
    }

    public bool HasPlayed()
    {
        return hasPlay;
    }
}

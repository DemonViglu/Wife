using System.Collections;
using System.Collections.Generic;
//using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shot1;
    public GameObject shot2;
    public GameObject shot3;
    public GameObject balloon;
    public int balloonsNum;
    public float balloonsV;
    public float shootingSpeed;
    private bool hasPlay;

    public Text text;
    public int score;
    public bool finished;
    public List<GameObject> ballList=new List<GameObject>();

    private void Start()
    {
        hasPlay = false;
        for(int i = 0; i < balloonsNum; i++)
        {
            ballList.Add(Instantiate(balloon, shot1.transform.position, balloon.transform.rotation,this.transform));
            ballList[i].SetActive(false);
        }
    }
    private void Update()
    {
        for(int i=0;i<ballList.Count;i++)
        {
            if (ballList[i].GetComponent<Rigidbody2D>().velocity.y<0)
            {
                if (!ballList[i].GetComponent<Animator>().GetBool("boom"))ballList[i].SetActive(false);
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
        StartCoroutine(ShootBalloon());
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

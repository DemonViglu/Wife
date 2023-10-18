using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    static public EnvironmentManager instance;
    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    #endregion

    public Animator flowerAnim;

    public void FlowerBegin()
    {
        flowerAnim.SetBool("flow", true);
    }

    public void FlowerEnd()
    {
        flowerAnim.SetBool("flow", false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indispensaveis : MonoBehaviour
{
    public static List<string> escolhasFeitas = new List<string>();
    public static Indispensaveis singleton;

    //Preenchendo o singleton
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(this.gameObject);
        }
    }
}

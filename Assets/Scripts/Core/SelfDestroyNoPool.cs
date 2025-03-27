using System.Collections;
using UnityEngine;

public class SelfDestroyNoPool : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayedDestroy());
    }


    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    
}

using System.Collections;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    // Call this method before destroying your GameObject to make that Coroutine persistent and to be able to keep running even after that object is destroyed.
    public Coroutine StartPersistingCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}

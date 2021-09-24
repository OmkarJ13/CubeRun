using System.Collections;
using UnityEngine;

/// <summary>
/// <para> This class handles coroutine calls. </para>
/// </summary>
public class CoroutineHandler : MonoBehaviour
{
    /// <summary>
    /// <para> Call this method to start a coroutine that lasts even after the instance of the class that it had been called from is destroyed. </para>
    /// </summary>
    /// <param name = "routine"> </param>
    /// <returns> </returns>
    public Coroutine StartPersistingCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}

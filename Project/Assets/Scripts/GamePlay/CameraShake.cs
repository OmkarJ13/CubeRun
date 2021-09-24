using System.Collections;
using UnityEngine;

/// <summary>
/// <para> Instantiate an object of this class to use the CameraShake functionality. </para>
/// </summary>
public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// <para> Shakes the parent GameObject with the given intensity and duration. </para>
    /// </summary>
    /// <param name = "intensity"> </param>
    /// <param name = "time"> </param>
    /// <returns> </returns>
    public IEnumerator Shake(float intensity, float time)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            float xOffset = Random.Range(-1.0f, 1.0f) * intensity;
            float yOffset = Random.Range(-1.0f, 1.0f) * intensity;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

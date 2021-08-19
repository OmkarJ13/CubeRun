using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float intensity, float time)
    {
        if (intensity == 0.0f || time == 0.0f) yield return null;
        
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

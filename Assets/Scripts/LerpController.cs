using UnityEngine;

public class LerpController : MonoBehaviour
{
    [Header("Lerp")]
    public AnimationCurve lerpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    private float progress;
    private float currentLerpTime;

    public bool LerpObject(Vector3 startPosition, Vector3 endPosition, float objectCooldown)
    {
        currentLerpTime += Time.fixedDeltaTime;
        if (currentLerpTime > objectCooldown)
        {
            currentLerpTime = objectCooldown;
        }
        progress = currentLerpTime / objectCooldown;
        transform.position = Vector3.Lerp(startPosition, endPosition, lerpCurve.Evaluate(progress));
        if (transform.position == new Vector3(endPosition.x, endPosition.y, transform.position.z))
        {
            currentLerpTime = 0;
            return false;
        }
        return true;
    }
}

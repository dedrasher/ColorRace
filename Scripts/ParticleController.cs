using UnityEngine;
public class ParticleController : MonoBehaviour
{
    private Vector3 direction;
    private Transform currentTransform;
    public void Init()
    {
        enabled = true;
        currentTransform = transform;
        direction = Vector3.forward * ValueManager.singleton.GetCubeSpeed(false) * 0.16f;
        Destroy(gameObject, 1f);
    }
    private void FixedUpdate()
    {
        currentTransform.Translate(direction);
    }
}

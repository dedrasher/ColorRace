using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
public class SkinViewController : MonoBehaviour
{
    public static GameObject SharedGameObject { get; private set; }
    [SerializeField]
    private UIBehaviour acceptPanel;
    private Transform holderTransform, currentTransform;
    private Vector3 one;
    private void Awake()
    {
       one = Vector3.one;
        currentTransform = transform;
        holderTransform = currentTransform.parent;
        SharedGameObject = holderTransform.gameObject;
    }
    
    private void OnMouseDrag()
    {
        if (Input.touchCount > 0 && holderTransform.localScale == one && !acceptPanel.IsActive())
            currentTransform.Rotate(Input.touches.Last().deltaPosition.y * 0.3f,0f,0f);   
    }
    
}

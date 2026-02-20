using UnityEngine;

public class DEstroy : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.1f;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}

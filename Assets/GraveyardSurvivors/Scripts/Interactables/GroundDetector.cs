using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private int _numberOfColliders = 50;
    [SerializeField] private LayerMask _mask;
    
    private const int MaxDistance = 1000000;
    private RaycastHit[] _results;

    private void Awake()
    {
        _results = new RaycastHit[_numberOfColliders];
    }

    public Vector3 GetGroundPosition(Vector3 endPoint)
    {
        Vector3 finalPos = Vector3.zero;
        
        var hits = Physics.RaycastNonAlloc(endPoint, Vector3.down, _results, MaxDistance, _mask);

        if (hits == 0)
        {
            return endPoint;
        }
        else
        {
            for (int i = 0; i < hits; i++)
            {
                finalPos  = _results[i].point;
            }
        }

        return finalPos;
    }
}
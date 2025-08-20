using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "name_EC", menuName = "EC/无载荷")]
public class VoidEC: DescSO
{
    public event UnityAction p0evt;
    public void Invoke()
    {
        p0evt.Invoke();
    }
}

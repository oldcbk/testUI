using UnityEngine.Events;

//[CreateAssetMenu(fileName = "name_EC", menuName = "EC/1 payload/T")]
public abstract class Payload1EC<T>: DescSO
{
	public event UnityAction<T> p1evt;
	public void Invoke(T t)
	{
		p1evt?.Invoke(t);
	}
}

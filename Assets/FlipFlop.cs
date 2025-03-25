using UnityEngine;
using UnityEngine.Events;

public class FlipFlop : MonoBehaviour
{
    bool state = true;
    public UnityEvent A;
    public UnityEvent B;

    public void toggle()
    {
        if (state == true)
        {
            state = false;
            A.Invoke();
        }
        else
        {
            state = true;
            B.Invoke();
        }
    }
}

using UnityEngine;

public static class RandomHelper
{
    public static bool RandomBoolean(float probabilityOfTrue = 0.5f, bool squaredResponse = false, bool sqrtResponse = false)
    {
        float max = 100;
        probabilityOfTrue *= 100f;

        if(squaredResponse)
        {
            probabilityOfTrue = Mathf.Pow(probabilityOfTrue, 2f);
            max = Mathf.Pow(max, 2);
        }
        else if(sqrtResponse)
        {
            probabilityOfTrue = Mathf.Sqrt(probabilityOfTrue);
            max = Mathf.Sqrt(max);
        }

        Debug.Log("probOfTrue: " + probabilityOfTrue);
        Debug.Log("max: " + max);

        return Random.Range(0, max) < probabilityOfTrue;
    }
}
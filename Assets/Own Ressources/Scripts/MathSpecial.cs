using UnityEngine;
using System.Collections;

public class MathSpecial : MonoBehaviour {

    //This is only for floats near at an integervalue
	public static int floatToInt(float f)
    {
        int i = (int)f;

        //Correct the rounderror
        if (delta(i, f) > .0005)
            if (i < f)
                i++;
            else
                i--;

        return i;
    }

    public static float delta(float a, float b)
    {
        return System.Math.Abs(a - b);
    }
}

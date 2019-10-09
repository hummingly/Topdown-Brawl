[System.Serializable]

// Credit: https://forum.unity.com/threads/pid-controller.68390/?_ga=2.228063454.215661671.1570456541-343444133.1548967889
// Doku: https://en.wikipedia.org/wiki/PID_controller, https://pidexplained.com/pid-controller-explained/

public class PIDController
{
    public float pFactor, iFactor, dFactor; 
    //(p)roportional (get to desired value fast at start)
    //(i)ntegral (change it just a bit each step, so the longer the process takes the more influence)
    //(d)erivative (prevent overshooting by looking at how far away from desired value and wslowing down)

    float integral;
    float lastError;


    public PIDController(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }


    public float Update(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        integral += present * timeFrame;
        float deriv = (present - lastError) / timeFrame;
        lastError = present;
        return present * pFactor + integral * iFactor + deriv * dFactor;
    }
}

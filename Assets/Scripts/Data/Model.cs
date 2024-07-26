using System;
using UnityEngine;

[Serializable]
public class Model
{
    public int id ;
    public string name ;
    public string description ;
    public string image ;
    public string link ;
    public int courseId ;
    public int userId ;
    
    public double posX ;
    public double posY ;
    public double posZ ;
    public double rotX ;
    public double rotY ;
    public double rotZ ;
    public double rotW ;
    public double scaleX ;
    public double scaleY ;
    public double scaleZ ;
    public bool positioningCreated;

    public Vector3 GetPosition()
    {
        return new Vector3((float)posX, (float)posY, (float)posZ);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion((float)rotX,  (float)rotY,  (float)rotZ,  (float)rotW);
    }

    public Vector3 GetScale()
    {
        return new Vector3((float)scaleX, (float)scaleY, (float)scaleZ);
    }
}

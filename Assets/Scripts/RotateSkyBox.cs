using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS FROM UNITY: https://gamedevbeginner.com/how-to-change-the-skybox-in-unity/ ON [How to rotate a skybox in Unity]
// THE SKYBOX MATERIAL IS FROM: https://assetstore.unity.com/packages/2d/textures-materials/sky/customizable-skybox-174576
// SHOUT OUT TO THE CREATORS AS THIS IS NOT ORIGIONAL CREATION FROM LUNA

public class RotateSkyBox : MonoBehaviour
{
    public Skybox skybox;
    public float rotationSpeed = 3;

    int rotationID;

    private void Start()
    {
        rotationID = Shader.PropertyToID("_Rotation");
    }

    void Update()
    {
        float newRot = skybox.material.GetFloat(rotationID);
        newRot += rotationSpeed * Time.deltaTime;
        if (newRot > 360)
        {
            newRot -= 360;
        }
        skybox.material.SetFloat(rotationID, newRot);
    }
}

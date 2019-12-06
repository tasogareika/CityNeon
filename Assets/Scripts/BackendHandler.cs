using UnityEngine;

public class BackendHandler : MonoBehaviour
{
    public static BackendHandler singleton;
    [HideInInspector] public float appWidth, appHeight;

    private void Awake()
    {
        singleton = this;
        appWidth = 1080;
        appHeight = 1920;
    }
}

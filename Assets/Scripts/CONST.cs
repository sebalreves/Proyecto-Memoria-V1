using UnityEngine;
public class CONST {
    public const float playerGrabCD = 0.3f;
    public const float playerGrabCollisionIgnoreCD = 0.2f;
    public const string PLAYER_READY = "player_ready";
    public const float FRENADO_VIENTO = 0.9f;
    public const float PLATFORM_REPULSION = 10f;
    public const float PLATFORM_ATTRACTION = 10f;
    public const float PLATFORM_INNER_ATRACTION = 8f;
    public const float waitTimeEnterCamZone = 1f;

    //PLAYER PROPERTIES
    public const float playerMaxSpeed = 10f;
    public const float playerAcc = 155f;

    //raise events
    public const byte GrabBallEventCode = 1;

    //Ball properties
    public const float cubeMass = 6f;
    public const float ballMass = 0.8f;
    public const string cubeTag = "Cube";
    public const string ballTag = "Ball";
    public const string platformTag = "Platform";
    public const float ballLinearDrag = 3f;
    public const float cubeLinearDrag = 5f;

    //ball spring joint
    public const float frequency = 1.8f;
    public const float breakForce = 240f;

    //layer indexing
    public const int playerLayer = (1 << 8);
    public const int ballLayer = (1 << 7);
    public const int cubeLayer = (1 << 6);
    public const int platformInteractionLayer = (1 << 10);


    //figure index
    public const int A = 0;
    public const int B = 1;
    public const int C = 2;
    public const int D = 3;

    //Portal variables
    public const string Any = "Any";
    public const string Cube = "Cube";
    public const string Ball = "Ball";
    public const string Red = "Red";
    public const string Blue = "Blue";

    public const float PointerBorderSize = 100f;

    public const string LocalName = "LocalPlayer";
    public const string NoLocalName = "NoLocalName";
}

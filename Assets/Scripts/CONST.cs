using UnityEngine;
public class CONST {
    public const float playerGrabCD = 0.3f;
    public const string PLAYER_READY = "player_ready";
    public const float FRENADO_VIENTO = 0.2f;
    public const float PLATFORM_REPULSION = 10f;
    public const float PLATFORM_ATTRACTION = 10f;
    public const float PLATFORM_INNER_ATRACTION = 8f;
    public const float waitTimeEnterCamZone = 0.4f;

    //raise events
    public const byte GrabBallEventCode = 1;

    //Ball properties
    public const float cubeMass = 11f;
    public const float ballMass = 0.5f;
    public const string cubeTag = "Cube";
    public const string ballTag = "Ball";
    public const float ballLinearDrag = 3f;
    public const float cubeLinearDrag = 5f;

    //ball spring joint
    public const float frequency = 1.4f;
    public const float breakForce = 100f;

    //layer indexing
    public const int playerLayer = (1 << 8);
    public const int ballLayer = (1 << 7);
    public const int cubeLayer = (1 << 6);


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
}

using UnityEngine;
public class CONST {
    public const float playerGrabCD = 0.3f;
    public const float playerGrabCollisionIgnoreCD = 0.2f;
    public const string PLAYER_READY = "player_ready";
    public const float FRENADO_VIENTO = 0.9f;
    public const float PLATFORM_REPULSION = 10f;
    public const float PLATFORM_ATTRACTION = 10f;
    public const float PLATFORM_INNER_ATRACTION = 8f;
    public const float waitTimeEnterCamZone = 0.4f;

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

    //rotate camera
    public const float cameraInclination = 1.6f;

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
    public const int E = 4;
    public const int F = 5;
    public const int G = 6;
    public const int H = 7;
    public const int I = 8;
    public const int J = 9;

    //Portal variables
    public const string Any = "Any";
    public const string Cube = "Cube";
    public const string Ball = "Ball";
    public const string Red = "Red";
    public const string Blue = "Blue";

    public const float PointerBorderSize = 100f;

    public const string LocalName = "LocalPlayer";
    public const string NoLocalName = "NoLocalName";

    //windareas
    public const float simulationSpeedPlay = 3.3f;
    public const float simulationSpeedPause = 0.3f;
    public const float spriteVelocity = 2f;

    //code execution
    public const float codeVelocity = 1.1f; //[seconds]
    public const float codeLoopVelocity = 1f; //[seconds]
    public const float slowTapTime = 0.8f; //[seconds]
    public const float fillLoopOffsetTime = 0.5f; //[seconds]
    public const float codeTabSize = 0.7f;

    //networking variables
    public const string levelProp = "lvl";
    public const string level_1 = "Level 1";
    public const string level_2 = "Level 2";



}

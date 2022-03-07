using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GenericBall : MonoBehaviourPun, IPunInstantiateMagicCallback {
    public Color RedColor, BlueColor;
    public Sprite BallSprite, CubeSrite;
    public GameObject WindInteractionGameObject;
    public SpriteRenderer innerSpriteRenderer;
    public SpriteRenderer outlineSpriteRenderer;
    public CircleCollider2D myCircleCollider;
    public BallGrabScript ballGrabScript;
    // public float mass;
    public CodeDescription codeDescription;

    public Rigidbody2D myRb, anchorRb;
    public BoxCollider2D UICollider;
    public CircleCollider2D UIEffectorCollider;

    string[] colorList = new string[] { CONST.Red, CONST.Blue };
    string[] shapeList = new string[] { CONST.Ball, CONST.Cube };

    // [Dropdown("colorList")]
    public string color;

    // [Dropdown("shapeList")]
    public string shape;
    private void Awake() {
        Physics2D.IgnoreCollision(UICollider, UIEffectorCollider);
    }

    // private void OnDestroy() {
    //     if (PhotonNetwork.IsConnectedAndReady) {

    //         if (photonView.IsMine) {
    //             photonView.RPC("ReleaseBallBlackHole", RpcTarget.AllBuffered, ballGrabScript.beingCarried, ballGrabScript.ActualPlayerWhoGrabId);
    //         }
    //     } else {
    //         ReleaseBallBlackHole(ballGrabScript.beingCarried, ballGrabScript.ActualPlayerWhoGrabId);
    //     }
    // }

    // [PunRPC]
    // private void ReleaseBallBlackHole(bool beingCarried, int playerId = 0) {
    //     if (beingCarried) {
    //         PlayerFactory._instance.findPlayer(playerId).GetComponent<PlayerGrab>().TryRelease();
    //     }
    // }

    [PunRPC]
    public void ReleaseBallBlackHole(bool beingCarried, int playerId = 0) {
        if (beingCarried) {
            PlayerFactory._instance.findPlayer(playerId).GetComponent<PlayerGrab>().TryRelease();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] data = info.photonView.InstantiationData;
        string syncColor = (string)data[0];
        color = syncColor;
        initializeBall();
    }

    private void initializeBall() {
        if (color == CONST.Red)
            innerSpriteRenderer.color = RedColor;
        else if (color == CONST.Blue)
            innerSpriteRenderer.color = BlueColor;
        else
            innerSpriteRenderer.color = Color.grey;

        if (shape == CONST.Cube) {
            codeDescription.titulo = "Cubo";
            convertToCube();

        } else if (shape == CONST.Ball) {
            codeDescription.titulo = "Pelota";
            convertToBall();
        } else
            innerSpriteRenderer.color = Color.grey;
    }

    // private void OnValidate() {
    //     initializeBall();
    // }


    private void Start() {
        if (!PhotonNetwork.IsConnectedAndReady)
            initializeBall();
    }

    private void convertToBall() {
        WindInteractionGameObject.SetActive(true);
        // mass = CONST.ballMass;
        gameObject.tag = CONST.ballTag;
        outlineSpriteRenderer.sprite = BallSprite;
        myRb.drag = CONST.ballLinearDrag;

        if (!ballGrabScript.beingCarried) {
            gameObject.layer = LayerMask.NameToLayer("Balls");
            myRb.mass = CONST.ballMass;
        }

    }

    private void convertToCube() {
        WindInteractionGameObject.SetActive(false);
        gameObject.tag = CONST.cubeTag;
        outlineSpriteRenderer.sprite = CubeSrite;
        myRb.drag = CONST.cubeLinearDrag;
        // mass = CONST.cubeMass;


        if (!ballGrabScript.beingCarried) {
            gameObject.layer = LayerMask.NameToLayer("Cubes");
            myRb.mass = CONST.cubeMass;
        }
    }

    [PunRPC]
    public void onPortalTransform(string _newColor, string _newShape) {
        if (_newColor != color) {
            if (_newColor == CONST.Red)
                innerSpriteRenderer.color = RedColor;
            else
                innerSpriteRenderer.color = BlueColor;

            color = _newColor;
        }

        if (_newShape == shape) return;
        shape = _newShape;

        //CHANGE MATERIAL
        if (_newShape == CONST.Cube) {
            BallFactory._instance.CubeCount++;
            BallFactory._instance.BallCount--;
            convertToCube();

        } else if (_newShape == CONST.Ball) {
            BallFactory._instance.CubeCount--;
            BallFactory._instance.BallCount++;
            convertToBall();
        }
    }

    // private void FixedUpdate() {
    //     anchorRb.position = myRb.position;
    // }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log(
    //     other.collider.gameObject.layer
    //     );
    // }




}

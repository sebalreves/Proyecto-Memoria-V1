using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GenericBall : MonoBehaviour {
    public Color RedColor, BlueColor;
    public Sprite BallSprite, CubeSrite;
    public GameObject WindInteractionGameObject;
    public SpriteRenderer innerSpriteRenderer;
    public SpriteRenderer outlineSpriteRenderer;
    public CircleCollider2D myCircleCollider;
    public BallGrabScript ballGrabScript;
    public float mass;

    public Rigidbody2D myRb;

    string[] colorList = new string[] { CONST.Red, CONST.Blue };
    string[] shapeList = new string[] { CONST.Ball, CONST.Cube };

    // [Dropdown("colorList")]
    public string color;

    // [Dropdown("shapeList")]
    public string shape;

    private void Start() {
        if (color == CONST.Red)
            innerSpriteRenderer.color = RedColor;
        else
            innerSpriteRenderer.color = BlueColor;

        if (shape == CONST.Cube) {
            convertToCube();

        } else if (shape == CONST.Ball) {
            convertToBall();
        }
    }

    private void convertToBall() {
        WindInteractionGameObject.SetActive(true);
        mass = CONST.ballMass;
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
            convertToCube();

        } else if (_newShape == CONST.Ball) {
            convertToBall();
        }
    }
}

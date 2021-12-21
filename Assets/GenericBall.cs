using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBall : MonoBehaviour {
    public Color RedColor, BlueColor;
    public Sprite BallSprite, CubeSrite;
    public GameObject WindInteractionGameObject;
    public SpriteRenderer innerSpriteRenderer;
    public SpriteRenderer outlineSpriteRenderer;
    public PhysicsMaterial2D ballMaterial;
    public CircleCollider2D myCircleCollider;
    public float mass;

    public Rigidbody2D myRb;

    string[] colorList = new string[] { CONST.Red, CONST.Blue };
    string[] shapeList = new string[] { CONST.Ball, CONST.Cube };

    [Dropdown("colorList")]
    public string color;

    [Dropdown("shapeList")]
    public string shape;

    public void onPortalTransform(string _newColor, string _newShape) {
        // Debug.Log(_newShape);
        // Debug.Log();
        // Debug.Log(_newColor);
        // Debug.Log(color);
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
            WindInteractionGameObject.SetActive(false);
            // mass = CONST.cubeMass;
            gameObject.tag = CONST.cubeTag;
            gameObject.layer = LayerMask.NameToLayer("Cubes");
            outlineSpriteRenderer.sprite = CubeSrite;
            myRb.mass = CONST.cubeMass;
            myRb.sharedMaterial = null;
            myRb.drag = CONST.cubeLinearDrag;
            myCircleCollider.sharedMaterial = null;


        } else if (_newShape == CONST.Ball) {
            WindInteractionGameObject.SetActive(true);
            mass = CONST.ballMass;
            gameObject.tag = CONST.ballTag;
            gameObject.layer = LayerMask.NameToLayer("Balls");
            outlineSpriteRenderer.sprite = BallSprite;
            myRb.mass = CONST.ballMass;
            myRb.sharedMaterial = ballMaterial;
            myRb.drag = CONST.ballLinearDrag;
            myCircleCollider.sharedMaterial = ballMaterial;

        }
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}

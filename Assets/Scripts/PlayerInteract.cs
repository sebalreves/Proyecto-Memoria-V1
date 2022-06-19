using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class PlayerInteract : MonoBehaviourPun {
    // Start is called before the first frame update
    PlayerGrab playerGrabScrip;
    TargetingScript targetingScriptReference;
    PlayerMovement playerMovementScript;
    Keyboard kb;
    GameObject signalPointerPrefab;
    // public Transform SignalPosition;

    GameObject ActualButtonPressed = null;
    TextMeshProUGUI ActualButtonText = null;
    Slider ActualButtonSlider = null;
    float sliderValue = 0f;
    bool buttonReady = false;
    bool filling = false;
    public AnimationCurve ButtonSliderAnimation;
    Coroutine fadeOutAnimation = null;
    public Color originalColorCode;
    bool signalSent = false;

    public Color red, green;


    private void Awake() {
        playerGrabScrip = GetComponent<PlayerGrab>();
        targetingScriptReference = GetComponent<TargetingScript>();
        playerMovementScript = GetComponent<PlayerMovement>();
        kb = InputSystem.GetDevice<Keyboard>();
    }

    private void Start() {
        signalPointerPrefab = Resources.Load("SignalPointer") as GameObject;
    }

    private void Update() {
        if (ActualButtonSlider != null)
            if (!ActualButtonSlider.isActiveAndEnabled) {
                RestartSliderValue(Color.red);
                ActualButtonSlider = null;
                buttonReady = false;
            }

        if (ActualButtonSlider != null && !buttonReady && filling) {
            ActualButtonSlider.value = ButtonSliderAnimation.Evaluate(sliderValue / (CONST.slowTapTime));
            sliderValue += Time.deltaTime;
            if (ActualButtonSlider.value == 1) {
                buttonReady = true;
                ActualButtonText.text = "¡Soltar!";
                filling = false;
            }
        }
    }

    [PunRPC]
    public void ReleaseBallBlackHole(bool beingCarried, int playerId = 0) {
        if (beingCarried) {
            PlayerFactory._instance.findPlayer(playerId).GetComponent<PlayerGrab>().TryRelease();
        }


    }

    [PunRPC]
    public void SyncCount(bool isCube) {
        if (isCube) BallFactory._instance.CubeCount--;
        else BallFactory._instance.BallCount--;
    }

    private void SendSignal(Vector3 _position) {
        #region SIGNALS
        signalSent = true;
        //Signals que deja el jugador cada vez que hace una interaccion
        // if (SignalPointer.signalCount < 4) {
        if (PhotonNetwork.IsConnectedAndReady) {
            // object[] customData = new object[] { _color };
            PhotonNetwork.Instantiate(signalPointerPrefab.name, _position, Quaternion.identity);
        } else {
            Instantiate(signalPointerPrefab, _position, Quaternion.identity);
        }
        // SignalPointer.signalCount++;
        // Instantiate(signalPointer, gameObject.transform.position, Quaternion.identity);

        #endregion
    }

    private void RestartSliderValue(Color color) {
        //restaura los valores y hace tilin con un color el slider
        if (ActualButtonSlider != null) {
            ActualButtonSlider.value = 0f;
            ActualButtonSlider.transform.Find("Background").GetComponent<Image>().color = originalColorCode;
            // LeanTween.color(ActualButtonSlider.transform.Find("Background").gameObject, color, 1f);
            if (fadeOutAnimation != null)
                StopCoroutine(fadeOutAnimation);
            fadeOutAnimation = StartCoroutine(RestartSliderValueRoutine(color, ActualButtonSlider.transform.Find("Background").GetComponent<Image>()));
        }
        sliderValue = 0f;
        filling = false;
        buttonReady = false;
        ActualButtonText.text = "Mantener <sprite=0>";

    }

    private IEnumerator RestartSliderValueRoutine(Color _color, Image _BGImage) {
        Color originalColor = _BGImage.color;
        _BGImage.color = _color;
        // Debug.Log(_BGImage);
        while (_BGImage.color != originalColor) {
            _BGImage.color = Color.Lerp(_BGImage.color, originalColor, 5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }


    public void OnInteract(InputAction.CallbackContext _context) {
        signalSent = false;
        // if (_context.phase == InputActionPhase.Canceled && _context.interaction is UnityEngine.InputSystem.Interactions.MultiTapInteraction) {
        //     Debug.Log("Tap");
        // }
        if (PhotonNetwork.IsConnectedAndReady && !photonView.IsMine) return;
        if (!playerMovementScript.controllEnabled) return;

        // Debug.Log(_context.phase);

        #region BUTTON BEGIN PRESS
        if (_context.phase == InputActionPhase.Started && _context.interaction is UnityEngine.InputSystem.Interactions.SlowTapInteraction) {
            var focusedElement = targetingScriptReference.getTargetedButton();
            if (focusedElement != null && focusedElement.GetComponent<GenericButton>().activable) {
                filling = true;
                ActualButtonPressed = focusedElement;
                ActualButtonSlider = focusedElement.transform.parent.Find("InGameUI").Find("2ndSpring").Find("Canvas").Find("Slider").GetComponent<Slider>();
                ActualButtonText = focusedElement.transform.parent.Find("InGameUI").Find("2ndSpring").Find("Canvas").Find("Slider").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            }
        }
        #endregion

        #region BUTTON CANCELL PRESS
        if (_context.phase == InputActionPhase.Canceled && _context.interaction is UnityEngine.InputSystem.Interactions.SlowTapInteraction) {
            RestartSliderValue(red);
        }
        #endregion


        if (!(_context.phase == InputActionPhase.Performed)) return;
        if (_context.interaction is UnityEngine.InputSystem.Interactions.SlowTapInteraction) {
            //Presionar botones
            #region BUTTONS ACTIVATION
            // if (kb.spaceKey.wasPressedThisFrame) {
            var focusedElement = targetingScriptReference.getTargetedButton();

            if (focusedElement != null && focusedElement == ActualButtonPressed) {
                if (buttonReady) {
                    focusedElement.GetComponent<GenericButton>().Presionar(gameObject);
                    RestartSliderValue(green);
                    SendSignal(focusedElement.transform.position);

                } else {
                    RestartSliderValue(red);
                }
            }

            #endregion

        } else if (_context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction) {
            // Debug.Log("Hola");



            #region GRAB INTERACTION ONRELEASE 
            if (playerGrabScrip.grabCdTimer <= 0f) {
                if (playerGrabScrip.grabingBall) {
                    playerGrabScrip.TryReleaseAndThrow();
                    SendSignal(transform.position);

                } else {
                    var focusedElement = targetingScriptReference.getTargetedBall();
                    if (focusedElement != null) {
                        playerGrabScrip.grabCdTimer = CONST.playerGrabCD;
                        playerGrabScrip.TryGrab(focusedElement);
                        SendSignal(focusedElement.transform.position);

                    }
                }
            }
            #endregion

            if (!signalSent)
                SendSignal(transform.position);
        }
    }






}

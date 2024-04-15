using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Wxy.Tool
{
    [RequireComponent(typeof(Rect))]
    [RequireComponent(typeof(CanvasGroup))]
    /// <summary>
    /// Add this component to a GUI Image to have it act as a button.
    /// Bind pressed down, pressed continually and released actions to it from the inspector
    /// Handles mouse and multi touch
    /// </summary>
    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, ISubmitHandler
    {
     
        public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp, Disabled }
        [Header("Binding")]
        /// The method(s) to call when the button gets pressed down
        public UnityEvent ButtonDown_UnityEvent;
        /// The method(s) to call when the button gets released
        public UnityEvent ButtonUp_UnityEvent;
        /// The method(s) to call while the button is being pressed
        public UnityEvent ButtonPressed_UnityEvent;

        [Header("Sprite Swap")]
        public Sprite Disabled_Sprite;
        public Sprite Pressed_Sprite;
        public Sprite Highlighted_Sprite;

        [Header("Color Changes")]
        public bool PressedChangeColor_bool = false;
        public Color Pressed_Color = Color.white;
        public bool LerpColor_bool = true;
        public float LerpColorDuration_float = 0.2f;
        public AnimationCurve LerpColor_AnimationCurve;

        [Header("Opacity")]
        /// the new opacity to apply to the canvas group when the button is pressed
        public float PressedOpacity_float = 1f;
        public float IdleOpacity_float = 1f;
        public float DisabledOpacity_float = 1f;

        [Header("Delays")]
        public float PressedFirstTimeDelay_float = 0f;
        public float ReleasedDelay_float = 0f;

        [Header("Buffer")]
        public float BufferDuration_float = 0f;

        [Header("Animation")]
        public Animator Animator;
        public string IdleAnimationParameterName_string = "Idle";
        public string DisabledAnimationParameterName_string = "Disabled";
        public string PressedAnimationParameterName_string = "Pressed";

        [Header("Mouse Mode")]
        /// If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).
        public bool MouseMode_bool = false;

        public bool ReturnToInitialSpriteAutomatically_bool { get; set; }

        /// the current state of the button (off, down, pressed or up)
        public ButtonStates CurrentState_ButtonStates { get; protected set; }

        protected bool _zonePressed = false;
        protected CanvasGroup _canvasGroup;
        protected float _initialOpacity;
        protected Animator _animator;
        protected Image _image;
        protected Sprite _initialSprite;
        protected Color _initialColor;
        protected float _lastClickTimestamp = 0f;
        protected Selectable _selectable;
        protected float _lastStateChangeAt = -50f;

        protected Color _imageColor;
        protected Color _fromColor;
        protected Color _toColor;

        /// <summary>
        /// On Start, we get our canvasgroup and set our initial alpha
        /// </summary>
        protected virtual void Awake()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                MouseMode_bool = false;
            }
            else
            {
                MouseMode_bool = true;
            }
            ReturnToInitialSpriteAutomatically_bool = true;

            _selectable = GetComponent<Selectable>();

            _image = GetComponentInChildren<Image>();
            if (_image != null)
            {
                _initialColor = _image.color;
                _initialSprite = _image.sprite;
            }

            _animator = GetComponent<Animator>();
            if (Animator != null)
            {
                _animator = Animator;
            }

            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup != null)
            {
                _initialOpacity = IdleOpacity_float;
                _canvasGroup.alpha = _initialOpacity;
                _initialOpacity = _canvasGroup.alpha;
            }
            ResetButton();
        }

        /// <summary>
        /// Every frame, if the touch zone is pressed, we trigger the OnPointerPressed method, to detect continuous press
        /// </summary>
        protected virtual void Update()
        {
            switch (CurrentState_ButtonStates)
            {
                case ButtonStates.Off:
                    SetOpacity(IdleOpacity_float);
                    if ((_image != null) && (ReturnToInitialSpriteAutomatically_bool))
                    {
                        _image.sprite = _initialSprite;
                    }
                    if (_selectable != null)
                    {
                        _selectable.interactable = true;
                        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
                        {
                            if (Highlighted_Sprite != null)
                            {
                                _image.sprite = Highlighted_Sprite;
                            }
                        }
                    }
                    break;

                case ButtonStates.Disabled:
                    SetOpacity(DisabledOpacity_float);
                    if (_image != null)
                    {
                        if (Disabled_Sprite != null)
                        {
                            _image.sprite = Disabled_Sprite;
                        }
                    }
                    if (_selectable != null)
                    {
                        _selectable.interactable = false;
                    }
                    break;

                case ButtonStates.ButtonDown:

                    break;

                case ButtonStates.ButtonPressed:
                    SetOpacity(PressedOpacity_float);
                    OnPointerPressed();
                    if (_image != null)
                    {
                        if (Pressed_Sprite != null)
                        {
                            _image.sprite = Pressed_Sprite;
                        }
                        if (PressedChangeColor_bool)
                        {
                            _image.color = Pressed_Color;
                        }
                    }
                    break;

                case ButtonStates.ButtonUp:

                    break;
            }

            if ((_image != null) && (PressedChangeColor_bool))
            {
                if (Time.time - _lastStateChangeAt < LerpColorDuration_float)
                {
                    float t = LerpColor_AnimationCurve.Evaluate(Remap(Time.time - _lastStateChangeAt, 0f, LerpColorDuration_float, 0f, 1f));
                    _image.color = Color.Lerp(_fromColor, _toColor, t);
                }
            }

            UpdateAnimatorStates();
        }

        /// <summary>
        /// At the end of every frame, we change our button's state if needed
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (CurrentState_ButtonStates == ButtonStates.ButtonUp)
            {
                _lastStateChangeAt = Time.time;
                _fromColor = Pressed_Color;
                _toColor = _initialColor;
                CurrentState_ButtonStates = ButtonStates.Off;
            }
            if (CurrentState_ButtonStates == ButtonStates.ButtonDown)
            {
                _lastStateChangeAt = Time.time;
                _fromColor = _initialColor;
                _toColor = Pressed_Color;
                CurrentState_ButtonStates = ButtonStates.ButtonPressed;
            }
        }

        /// <summary>
        /// Triggers the bound pointer down action
        /// </summary>
        public virtual void OnPointerDown(PointerEventData data)
        {
            if (Time.time - _lastClickTimestamp < BufferDuration_float)
            {
                return;
            }

            if (CurrentState_ButtonStates != ButtonStates.Off)
            {
                return;
            }
            CurrentState_ButtonStates = ButtonStates.ButtonDown;
            _lastClickTimestamp = Time.time;
            if ((Time.timeScale != 0) && (PressedFirstTimeDelay_float > 0))
            {
                Invoke("InvokePressedFirstTime", PressedFirstTimeDelay_float);
            }
            else
            {
                ButtonDown_UnityEvent.Invoke();
            }
        }

        protected virtual void InvokePressedFirstTime()
        {
            if (ButtonDown_UnityEvent != null)
            {
                ButtonDown_UnityEvent.Invoke();
            }
        }

        /// <summary>
        /// Triggers the bound pointer up action
        /// </summary>
        public virtual void OnPointerUp(PointerEventData data)
        {
            if (CurrentState_ButtonStates != ButtonStates.ButtonPressed && CurrentState_ButtonStates != ButtonStates.ButtonDown)
            {
                return;
            }

            CurrentState_ButtonStates = ButtonStates.ButtonUp;
            if ((Time.timeScale != 0) && (ReleasedDelay_float > 0))
            {
                Invoke("InvokeReleased", ReleasedDelay_float);
            }
            else
            {
                ButtonUp_UnityEvent.Invoke();
            }
        }

        protected virtual void InvokeReleased()
        {
            if (ButtonUp_UnityEvent != null)
            {
                ButtonUp_UnityEvent.Invoke();
            }
        }

        /// <summary>
        /// Triggers the bound pointer pressed action
        /// </summary>
        public virtual void OnPointerPressed()
        {
            CurrentState_ButtonStates = ButtonStates.ButtonPressed;
            if (ButtonPressed_UnityEvent != null)
            {
                ButtonPressed_UnityEvent.Invoke();
            }
        }

        /// <summary>
        /// Resets the button's state and opacity
        /// </summary>
        protected virtual void ResetButton()
        {
            SetOpacity(_initialOpacity);
            CurrentState_ButtonStates = ButtonStates.Off;
        }

        /// <summary>
        /// Triggers the bound pointer enter action when touch enters zone
        /// </summary>
        public virtual void OnPointerEnter(PointerEventData data)
        {
            if (!MouseMode_bool)
            {
                OnPointerDown(data);
            }
        }

        /// <summary>
        /// Triggers the bound pointer exit action when touch is out of zone
        /// </summary>
        public virtual void OnPointerExit(PointerEventData data)
        {
            if (!MouseMode_bool)
            {
                OnPointerUp(data);
            }
        }
        /// <summary>
        /// OnEnable, we reset our button state
        /// </summary>
        protected virtual void OnEnable()
        {
            ResetButton();
        }

        public virtual void DisableButton()
        {
            CurrentState_ButtonStates = ButtonStates.Disabled;
        }

        public virtual void EnableButton()
        {
            if (CurrentState_ButtonStates == ButtonStates.Disabled)
            {
                CurrentState_ButtonStates = ButtonStates.Off;
            }
        }

        protected virtual void SetOpacity(float newOpacity)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = newOpacity;
            }
        }

        protected virtual void UpdateAnimatorStates()
        {
            if (_animator == null)
            {
                return;
            }
            if (DisabledAnimationParameterName_string != null)
            {
                _animator.SetBool(DisabledAnimationParameterName_string, (CurrentState_ButtonStates == ButtonStates.Disabled));
            }
            if (PressedAnimationParameterName_string != null)
            {
                _animator.SetBool(PressedAnimationParameterName_string, (CurrentState_ButtonStates == ButtonStates.ButtonPressed));
            }
            if (IdleAnimationParameterName_string != null)
            {
                _animator.SetBool(IdleAnimationParameterName_string, (CurrentState_ButtonStates == ButtonStates.Off));
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            if (ButtonDown_UnityEvent != null)
            {
                ButtonDown_UnityEvent.Invoke();
            }
            if (ButtonUp_UnityEvent != null)
            {
                ButtonUp_UnityEvent.Invoke();
            }
        }

        protected virtual float Remap(float x, float A, float B, float C, float D)
        {
            float remappedValue = C + (x - A) / (B - A) * (D - C);
            return remappedValue;
        }
    }
}
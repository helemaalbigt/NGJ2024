using System;
using UnityEngine;

namespace Rowhouse
{
    public class PhysicsButton : MonoBehaviour
    {
        public event Action OnClickDown;
        public event Action OnClickUp;

        public Transform _button;
        public Rigidbody _buttonTopRigidBody;
        public InteractionListener _listener;

        public Axis _axis = Axis.Z;
        public float _lowerLimit = -0.05f;

        [SerializeField]
        private float _upperLimit;
        public float upperLimit {
            get => _upperLimit;
            set {
                if (value != _upperLimit) {
                    _framesOnNewUpperLimit = 0;
                    _upperLimit = value;
                }
            }
        }
        [Range(0f,1f)]
        public float _clickDownTreshold = 0.7f;
        [Range(0f,1f)]
        public float _clickUpTreshold = 0.8f;
        public float _upForce = 10f;
        public bool _useDepricatedPositioning = true;

        [Header("Debug")]
        [SerializeField]
        private bool _logPresses;
        [SerializeField]
        private bool _isPressed;

        private float _upperlowerDiff => _upperLimit - _lowerLimit;
        private bool _prevPressState;
        private bool _applyForce;
        
        private int _framesOnNewUpperLimit = 0;
        private float _timeEnabled = 0f;
        private const int MinFramesOnUpperLimit = 1; //these checks prevent buttons from autopressing when they're in different menu screens on the same spot
        private const float MinTimeEnabled = 0.3f;
        
        private void Update() {
            if (_useDepricatedPositioning) {
                PositionButton_Depricated();
            } else {
                PositionButton();
            }

            _timeEnabled += Time.unscaledDeltaTime;
        }

        private void PositionButton() {
            _button.localRotation = Quaternion.Euler(0, 0, 0);
            var pos = 0f;
            
            switch (_axis) {
                case Axis.X:
                    _button.localPosition = new Vector3(_button.localPosition.x, 0, 0);
                    pos = _button.localPosition.x;
                    break;
                
                case Axis.Y:
                    _button.localPosition = new Vector3(0, _button.localPosition.y, 0);
                    pos = _button.localPosition.y;
                    break;
                
                case Axis.Z:
                    _button.localPosition = new Vector3(0, 0,  _button.localPosition.z);
                    pos = _button.localPosition.z;
                    break;
            }

            var newPos = pos;
            var f = (pos - _lowerLimit) / (_upperLimit - _lowerLimit); //using factor makes this pos/neg direction independent
            if (f >= 1) {
                newPos = _upperLimit;
                _framesOnNewUpperLimit++;
                _applyForce = false;
            } else {
                _applyForce = true;
            }
            
            if (f <= 0) {
                newPos = _lowerLimit;
            }
            
            switch (_axis) {
                case Axis.X:
                    _button.localPosition = new Vector3(newPos, 0, 0);
                    break;
                
                case Axis.Y:
                    _button.localPosition = new Vector3(0, newPos, 0);
                    break;
                
                case Axis.Z:
                    _button.localPosition = new Vector3(0, 0, newPos);
                    break;
            }

            var fDown = _clickDownTreshold;// (_clickDownTreshold - _lowerLimit) / (_upperLimit - _lowerLimit);
            var fUp = _clickUpTreshold;//(_clickUpTreshold - _lowerLimit) / (_upperLimit - _lowerLimit);
            var belowClickDown = f <= fDown;
            var aboveClickUp = f >= fUp;

            var canInteract = _framesOnNewUpperLimit > MinFramesOnUpperLimit &&
                              _timeEnabled > MinTimeEnabled;
            
            if (canInteract && fDown < 1f) {
                if (!_isPressed && belowClickDown && _listener.IsHovered()){
                    OnClickDown?.Invoke();
                    _isPressed = true;
                    if(_logPresses)
                        Debug.Log($"[PhysicsButton] <{gameObject.name}> click down");
                } else if (_isPressed && aboveClickUp) {
                    OnClickUp?.Invoke();
                    _isPressed = false;
                    if(_logPresses)
                        Debug.Log($"[PhysicsButton] <{gameObject.name}> click up");
                }
            }
        }

        private void PositionButton_Depricated() {
            _button.localPosition = new Vector3(0, _button.localPosition.y, 0);
            _button.localRotation = Quaternion.Euler(-90f, 0, 0);

            if (_button.localPosition.y >= _upperLimit) {
                _button.localPosition = new Vector3(0, _upperLimit, 0);
                _framesOnNewUpperLimit++;
                _applyForce = false;
            } else {
                _applyForce = true;
            }

            if (_button.localPosition.y <= _lowerLimit) {
                _button.localPosition = new Vector3(0, _lowerLimit, 0);
            }
            
            var belowClickDown = _button.localPosition.y - _lowerLimit <= _upperlowerDiff * _clickDownTreshold;
            var aboveClickUp = _button.localPosition.y - _lowerLimit >= _upperlowerDiff * _clickUpTreshold;

            if (_framesOnNewUpperLimit > MinFramesOnUpperLimit) {
                if (!_isPressed && belowClickDown && _listener.IsHovered()){
                    OnClickDown?.Invoke();
                    _isPressed = true;
                    if(_logPresses)
                        Debug.Log($"[PhysicsButton] <{gameObject.name}> click down");
                } else if (_isPressed && aboveClickUp) {
                    OnClickUp?.Invoke();
                    _isPressed = false;
                    if(_logPresses)
                        Debug.Log($"[PhysicsButton] <{gameObject.name}> click up");
                }
            }
        }

        private void OnDisable() {
            _button.localPosition = new Vector3(0, _upperLimit, 0);
            _isPressed = false;
        }

        private void OnEnable() {
            _framesOnNewUpperLimit = 0;
            _timeEnabled = 0f;
        }

        private void FixedUpdate() {
            if (_applyForce) {
                switch (_axis) {
                    case Axis.X:
                        var dirX = _upperLimit - _lowerLimit >= 0 ? _button.right : -_button.right;
                        _buttonTopRigidBody.AddForce(dirX * _upForce * Time.fixedDeltaTime);
                        break;
                    case Axis.Y:
                        var dirY = _upperLimit - _lowerLimit >= 0 ? _button.up : -_button.up;
                        _buttonTopRigidBody.AddForce(dirY * _upForce * Time.fixedDeltaTime);
                        break;
                    case Axis.Z:
                        var dirZ = _upperLimit - _lowerLimit >= 0 ? _button.forward : -_button.forward;
                        _buttonTopRigidBody.AddForce(dirZ * _upForce * Time.fixedDeltaTime);
                        break;
                }
            }
        }

        public enum Axis
        {
            X,
            Y,
            Z
        }
    }
}
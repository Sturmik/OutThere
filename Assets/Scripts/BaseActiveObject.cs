using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract object for all interactable objects in game
public abstract class BaseActiveObject : MonoBehaviour
{
    #region Variables

    // Active object rigidbody
    protected Rigidbody2D _objectRigidbody;
    // Screen borders
    protected float _leftConstraint;
    protected float _rightConstraint;
    protected float _bottomConstraint;
    protected float _topConstraint;
    protected float _screenEdgeBuffer = 1.0f;
    protected Camera _mainCamera;

    #endregion

    #region Unity

    protected virtual void Awake()
    {
        // Getting object rigidbody
        _objectRigidbody = GetComponent<Rigidbody2D>();
        // Getting our camera
        _mainCamera = Camera.main;
        // Calculating camera constraints
        _leftConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        _rightConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x;
        _bottomConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y;
        _topConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f)).y;
    }

    protected virtual void Update()
    {
        ScreenWrapping();
    }

    // OnCollisionCheck
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checking, if object went of the screen
    /// </summary>
    /// <returns>Position, where object should appear after reaching screen constraints, else zero vector3</returns>
    protected Vector3 CheckConstraints()
    {
        Vector3 constraintCheck = Vector3.zero;
        if (transform.position.x < _leftConstraint - _screenEdgeBuffer)
        {
            constraintCheck=new Vector3(_rightConstraint + _screenEdgeBuffer, transform.position.y, transform.position.z);
        }
        if (transform.position.x > _rightConstraint + _screenEdgeBuffer)
        {
            constraintCheck =new Vector3(_leftConstraint - _screenEdgeBuffer, transform.position.y, transform.position.z);
        }
        if (transform.position.y < _bottomConstraint - _screenEdgeBuffer)
        {
            constraintCheck = new Vector3(transform.position.x, _topConstraint + _screenEdgeBuffer, transform.position.z);
        }
        if (transform.position.y > _topConstraint + _screenEdgeBuffer)
        {
            constraintCheck =new  Vector3(transform.position.x, _bottomConstraint - _screenEdgeBuffer, transform.position.z);
        }
        return constraintCheck;
    }

    /// <summary>
    /// Changes object position, whent it reaches edges of the map
    /// </summary>
    protected void ScreenWrapping()
    {
        Vector3 newPos = CheckConstraints();
        if (newPos != Vector3.zero)
        {
            transform.position = newPos;
        }
    }

    // Behaviour of an object, after it was hit by some collider
    // Class, which inherit this class must implement and describe
    // how object should react to being hit
    protected abstract void Hit(Collision2D collision);

    #endregion
}
using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static event Action<Vector3> MovePerformed;
    public static event Action<Vector3> PressPerformed;
    public static event Action<Vector3> PressCancelled;

    private Vector3 _inputPos;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inputPos = Input.mousePosition;
            PressPerformed?.Invoke(_inputPos);
        }
        if (Input.GetMouseButton(0) && (_inputPos != Input.mousePosition))
        {
            _inputPos = Input.mousePosition;
            MovePerformed?.Invoke(_inputPos);
        }
        if (Input.GetMouseButtonUp(0))
            PressCancelled?.Invoke(Input.mousePosition);
    }

    #region Singleton
    private static InputSystem _instance;

    public static InputSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputSystem();
            }

            return _instance;
        }
    }

	#endregion


}

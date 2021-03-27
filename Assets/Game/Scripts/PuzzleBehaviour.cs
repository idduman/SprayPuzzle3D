using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBehaviour : MonoBehaviour
{
    [SerializeField] private float _snapTreshold = 0.5f;

    private bool[] _completion;

    private Camera _camera;
    private LayerMask _pieceMask, _planeMask;
    private Transform _selectedPiece, _movingPiece;
    private Vector3 _hitDiff;
    private bool _moving, _finished;
    private Animator _animator;
	// Start is called before the first frame update
	private void Awake()
	{
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        _completion = new bool[transform.childCount - 1];
    }

	void Start()
    {
        _camera = Camera.main;

        _pieceMask = LayerMask.GetMask("Piece");
        _planeMask = LayerMask.GetMask("Plane");

        InputSystem.PressPerformed += OnPressPerformed;
        InputSystem.PressCancelled += OnPressCancelled;
        InputSystem.MovePerformed += OnMovePerformed;
    }

	private void OnDisable()
	{
        InputSystem.PressPerformed -= OnPressPerformed;
        InputSystem.PressCancelled -= OnPressCancelled;
        InputSystem.MovePerformed -= OnMovePerformed;
    }

	private void OnDestroy()
	{
        InputSystem.PressPerformed -= OnPressPerformed;
        InputSystem.PressCancelled -= OnPressCancelled;
        InputSystem.MovePerformed -= OnMovePerformed;
    }

	// Update is called once per frame
	void Update()
    {
        if (_finished)
            return;

        if (_completion.All(x => x))
        {
            Debug.Log("Completed");
            _animator.enabled = true;
            _animator.SetTrigger("Completed");
            _finished = true;
        }
        if (_moving)
        {
            _movingPiece.localPosition = Vector3.Lerp(
            _movingPiece.localPosition,Vector3.zero,4*Time.deltaTime);

            if (Vector3.Distance(_movingPiece.localPosition, Vector3.zero) < 0.08f)
            {
                _movingPiece.localPosition = Vector3.zero;
                _completion[_movingPiece.GetSiblingIndex() - 1] = true;
                _movingPiece = null;
                _moving = false;
            }
        }
    }

    void OnPressPerformed(Vector3 mousePosition)
    {
        if (_moving)
            return;

        Ray ray = _camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var hit, 100f, _pieceMask))
        {
            _selectedPiece = hit.transform;
            _hitDiff = hit.point - _selectedPiece.position;
            _selectedPiece.position = GetPlanePoint(mousePosition) - _hitDiff;
        }
    }

    void OnPressCancelled(Vector3 mousePosition)
    {
        if (!_selectedPiece)
            return;

        var localOffset = _selectedPiece.localPosition;
        localOffset.y = 0;
        if (Vector3.Distance(localOffset, Vector3.zero) < _snapTreshold)
        {
            _selectedPiece.gameObject.layer = LayerMask.NameToLayer("Default");
            _movingPiece = _selectedPiece;
            _selectedPiece = null;
            _moving = true;
        }
    }

    void OnMovePerformed(Vector3 mousePosition)
    {
        if (!_selectedPiece)
            return;

        _selectedPiece.position = GetPlanePoint(mousePosition) - _hitDiff;
    }

    Vector3 GetPlanePoint(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out var hit, 100f, _planeMask);
        return hit.point;
    }

}

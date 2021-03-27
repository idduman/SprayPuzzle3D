using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	[SerializeField] private RectTransform _puzzleTutorial;
    void Start()
    {
		PuzzleBehaviour.Pressed += OnPuzzlePressed;
    }

	private void OnDisable()
	{
		PuzzleBehaviour.Pressed -= OnPuzzlePressed;
	}

	private void OnDestroy()
	{
		PuzzleBehaviour.Pressed -= OnPuzzlePressed;
	}
	void OnPuzzlePressed()
    {
		_puzzleTutorial.gameObject.SetActive(false);
    }
}

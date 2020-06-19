#if UNITY_EDITOR
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolfGenerator : MonoBehaviour
{
    [SerializeField, Range(2, 20)]
    private int courseLength = 10;

    [SerializeField, Range(0, 1)]
    private float straightPieceChance = .5f;

    [SerializeField]
    private GameObject startPiecePrefab = null;

    [SerializeField]
    private GameObject[] straightPieces = null;

    [SerializeField]
    private GameObject[] curvedPieces = null;

    [SerializeField]
    private GameObject[] endPieces = null;

    [SerializeField, ReadOnly(true)]
    private float pieceSize = 0;

    [SerializeField, HideInInspector]
    private List<GameObject> course = null;

    private void OnValidate()
    {
        if (null != startPiecePrefab)
            pieceSize = startPiecePrefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.x;
    }

    /// <summary>
    /// Destroy previous children of this and clears the course list to be reused again.
    /// </summary>
    private void ClearCourse()
    {
        if (null != course)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            course.Clear();
        }
    }

    /// <summary>
    /// This method will procedurally generate a golf course using the serialized properties and the prefab pieces.
    /// </summary>
    public void GenerateCourse()
    {
        ClearCourse();

        course = new List<GameObject>(courseLength);

        // Create start piece
        var startPiece = Instantiate(startPiecePrefab, transform);
        startPiece.name = "Start";
        course.Add(startPiece);

        RecursivePlacePiece(courseLength - 1, Vector3.zero, Vector3.forward);
    }

    private Vector3 RecursivePlacePiece(
        int piecesLeft,
        Vector3 lastLocalPosition,
        Vector3 currentDirection,
        float turned = 0)
    {
        var isPathPiece = piecesLeft > 1;
        var randomDirection = currentDirection;
        float angleToTurn = 0;

        if (piecesLeft > 2)
        {
            randomDirection = GetRandomDirection(currentDirection, turned, out angleToTurn);
        }

        var piecePrefab = isPathPiece
            ? GetPrefabFromDirection(angleToTurn)
            : endPieces[Random.Range(0, endPieces.Length)];

        var newPiece = Instantiate(piecePrefab, transform);

        newPiece.name = isPathPiece ? $"Piece #{course.Count}" : "End";

        // Add to the previous location the size of a piece in the current direction
        var newPosition = lastLocalPosition + currentDirection * pieceSize;

        // Set the new piece's localPosition and save the location for next piece
        newPiece.transform.localPosition = newPosition;

        if (angleToTurn < 0)
            newPiece.transform.Rotate(Vector3.up, 90);

        newPiece.transform.Rotate(Vector3.up, turned);

        course.Add(newPiece);

        turned += angleToTurn;

        if (isPathPiece)
            return RecursivePlacePiece(--piecesLeft, newPosition, randomDirection, turned);
        
        return newPosition;
    }

    private Vector3 GetRandomDirection(Vector3 currentDirection, float turned, out float angleTurned)
    {
        Vector3 randomDirection = currentDirection;
        angleTurned = 0;

        if (Random.value >= straightPieceChance)
        {
            if (Mathf.Abs(turned) < 180)
            {
                angleTurned = (Random.value > .5f ? 1 : -1) * 90;
            }
            else
            {
                angleTurned = -Mathf.Sign(turned) * 90;
            }
        }

        randomDirection = Quaternion.Euler(Vector3.up * angleTurned) * randomDirection;
        return randomDirection;
    }

    private GameObject GetPrefabFromDirection(float angle)
    {
        if (angle == 0)
            return straightPieces[Random.Range(0, straightPieces.Length)];
        return curvedPieces[Random.Range(0, curvedPieces.Length)];
    }
}
#endif
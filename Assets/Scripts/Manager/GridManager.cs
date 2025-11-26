using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 4;
    public RectTransform gridParent;
    public Tile tilePrefab;
    public float cellSize = 200f;
    public float spacing = 25f;

    [Header("Animation")]
    public float moveDuration = 0.15f;
    public Ease moveEase = Ease.InOutQuad;
    
    public AudioSource audioSource;
    public AudioClip mergeSFX;


    [HideInInspector] public Tile[,] Tiles;
    [HideInInspector] public int[,] Values;

    public void Initialize()
    {
        Tiles = new Tile[gridSize, gridSize];
        Values = new int[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Tile tile = Instantiate(tilePrefab, gridParent);
                tile.transform.localScale = Vector3.one;
                tile.transform.localPosition = GetTilePosition(x, y);
                tile.SetValue(0);
                Tiles[x, y] = tile;
                Values[x, y] = 0;
            }
        }
    }

    public Vector3 GetTilePosition(int x, int y)
    {
        float totalSize = gridSize * cellSize + (gridSize - 1) * spacing;
        float startX = -totalSize / 2 + cellSize / 2;
        float startY = totalSize / 2 - cellSize / 2;

        float posX = startX + x * (cellSize + spacing);
        float posY = startY - y * (cellSize + spacing);
        return new Vector3(posX, posY, 0);
    }

    public void AnimateTiles()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Tile tile = Tiles[x, y];
                tile.transform.DOKill();
                tile.transform.DOLocalMove(GetTilePosition(x, y), moveDuration).SetEase(moveEase);
                tile.SetValue(Values[x, y]);
            }
        }
        foreach (var evt in MergeEventBuffer.Events)
        {
            Tile tile = Tiles[evt.x, evt.y];
            Vector3 pos = tile.transform.position;

            // Snow burst
            ParticlePool.Instance.PlayAt(pos);

            // Pop scale
            tile.transform.DOKill();
            tile.transform.localScale = Vector3.one * 1.12f;
            tile.transform.DOScale(Vector3.one, 0.22f).SetEase(Ease.OutBack);

            // 🔊 Merge sound
            if (audioSource && mergeSFX)
                audioSource.PlayOneShot(mergeSFX);
        }

        MergeEventBuffer.Clear();

    }

    public void SpawnTileAt(int x, int y, int value)
    {
        Values[x, y] = value;
        Tile tile = Tiles[x, y];
        tile.SetValue(value);

        tile.transform.DOKill();
        tile.transform.localScale = Vector3.one * 0.7f;
        tile.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    public Vector2Int GetRandomEmptyCell()
    {
        List<Vector2Int> empty = new List<Vector2Int>();
        for (int y = 0; y < gridSize; y++)
            for (int x = 0; x < gridSize; x++)
                if (Values[x, y] == 0)
                    empty.Add(new Vector2Int(x, y));

        return empty.Count > 0 ? empty[Random.Range(0, empty.Count)] : new Vector2Int(-1, -1);
    }
}

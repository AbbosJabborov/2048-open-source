using System.Collections;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public GridManager gridManager;
        public MoveHandler moveHandler;
        public UndoManager undoManager;
        public ScoreUpdater scoreUpdater;
        public GameOverManager gameOverManager;


        private bool isMoving;

        void Start()
        {
            gridManager.Initialize();
            SpawnRandomTile();
            SpawnRandomTile();
        }

        void Update()
        {
            if (isMoving) return;
            if (Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2Int.down);
            if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2Int.up);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2Int.left);
            if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2Int.right);
        }

        void SpawnRandomTile()
        {
            Vector2Int pos = gridManager.GetRandomEmptyCell();
            if (pos.x == -1) return;
            int newVal = Random.value < 0.9f ? 2 : 4;
            gridManager.SpawnTileAt(pos.x, pos.y, newVal);
        }

        public void Move(Vector2Int dir)
        {
            StartCoroutine(MoveRoutine(dir));
        }

        IEnumerator MoveRoutine(Vector2Int dir)
        {
            isMoving = true;

            // Save state before move
            undoManager.SaveState(gridManager.Values, scoreUpdater.CurrentScore);

            // Perform movement
            int gainedScore = moveHandler.Move(gridManager.Values, dir, gridManager.gridSize);

            if (gainedScore < 0)  // no movement happened
            {
                isMoving = false;
                undoManager.PopLastState(); // undo accidental save
                yield break;
            }

            // Update score
            scoreUpdater.AddScore(gainedScore);

            gridManager.AnimateTiles();
            yield return new WaitForSeconds(gridManager.moveDuration);

            SpawnRandomTile();

            if (CheckClassicGameOver())
                gameOverManager.TriggerGameOver();


            isMoving = false;
        }
    
        bool CheckClassicGameOver()
        {
            int[,] values = gridManager.Values;
            int size = gridManager.gridSize;

            // Check empty tiles
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                if (values[x, y] == 0)
                    return false;

            // Check possible merges
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int v = values[x, y];
                    if (x < size - 1 && values[x + 1, y] == v) return false;
                    if (y < size - 1 && values[x, y + 1] == v) return false;
                }
            }

            return true;
        }


        public void UndoMove()
        {
            const int undoCost = 50;

            if (!undoManager.CanUndo()) return;

            if (scoreUpdater.CurrentScore < undoCost)
            {
                scoreUpdater.PlayNoUndoAnimation();
                return;
            }

            var (lastGrid, lastScore) = undoManager.Undo();
            if (lastGrid == null) return;

            // Restore grid
            int size = lastGrid.GetLength(0);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                gridManager.Values[x, y] = lastGrid[x, y];

            // Apply penalty
            int penalizedScore = Mathf.Max(lastScore - undoCost, 0);
            scoreUpdater.SetScore(penalizedScore);

            // Update score history too — so next undo will restore penalized score
            undoManager.SaveAfterPenaltyOnly(penalizedScore);

            gridManager.AnimateTiles();
        }

    }
}

using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    public int Move(int[,] grid, Vector2Int dir, int gridSize)
    {
        bool[][] merged = new bool[gridSize][];
        for (int i = 0; i < gridSize; i++)
            merged[i] = new bool[gridSize];

        bool moved = false;
        int gainedScore = 0;

        int startX = dir.x > 0 ? gridSize - 1 : 0;
        int startY = dir.y > 0 ? gridSize - 1 : 0;
        int stepX = dir.x > 0 ? -1 : 1;
        int stepY = dir.y > 0 ? -1 : 1;

        for (int y = startY; y >= 0 && y < gridSize; y += stepY)
        {
            for (int x = startX; x >= 0 && x < gridSize; x += stepX)
            {
                if (grid[x, y] == 0) continue;

                int cx = x;
                int cy = y;

                while (true)
                {
                    int nx = cx + dir.x;
                    int ny = cy + dir.y;

                    if (nx < 0 || nx >= gridSize || ny < 0 || ny >= gridSize)
                        break;

                    // Empty cell → slide
                    if (grid[nx, ny] == 0)
                    {
                        grid[nx, ny] = grid[cx, cy];
                        grid[cx, cy] = 0;
                        cx = nx;
                        cy = ny;
                        moved = true;
                    }
                    else if (grid[nx, ny] == grid[cx, cy] && !merged[nx][ny])
                    {
                        grid[nx, ny] *= 2;
                        grid[cx, cy] = 0;
                        merged[nx][ny] = true;
                        moved = true;
                        
                        MergeEventBuffer.Register(nx, ny, grid[nx, ny]);

                        gainedScore += grid[nx, ny];
                        break;
                    }
                    else break;
                }
            }
        }

        return moved ? gainedScore : -1;
    }
}
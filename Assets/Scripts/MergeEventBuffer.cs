using System.Collections.Generic;

public static class MergeEventBuffer
{
    public struct MergeEvent
    {
        public int x, y, value;
        public MergeEvent(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
    }

    private static List<MergeEvent> events = new List<MergeEvent>();
    public static IReadOnlyList<MergeEvent> Events => events;

    public static void Register(int x, int y, int value)
    {
        events.Add(new MergeEvent(x, y, value));
    }

    public static void Clear()
    {
        events.Clear();
    }
}
using System.Collections.Concurrent;

namespace Core;

public class Room
{
    public ConcurrentDictionary<string, string> Users { get; } = new ConcurrentDictionary<string, string>();
}

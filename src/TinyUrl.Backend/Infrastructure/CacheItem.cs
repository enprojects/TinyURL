using System.Security.AccessControl;

namespace TinyUrl.Backend.Infrastructure
{
    public class CacheItem
    {
        public DateTime LastAccessed { get; set; }
        public byte[] Value { get; set; }
        
        public CacheItem(DateTime lastAccessed, byte[] value)
        {
            LastAccessed = lastAccessed;
            Value = value;
        }
    }
}
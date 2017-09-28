using System;

namespace ClassLibrary1
{
    [Serializable]
    public class FeedMessage
    {
        public virtual int Id { get; set; }

        public virtual string FeedType { get; set; }

        public virtual int Version { get; set; }
    }
}

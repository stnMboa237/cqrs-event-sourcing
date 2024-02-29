using CQRS.Core.Messages;

namespace CQRS.Core.Events
{
    public abstract class BaseEvent: Message
    {
        /// <summary>
        /// Version will be used when we run a replay the latest state of aggregate
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// Type will be used when we do polymorphic data binding when serialize our event objects
        /// </summary>
        public string Type { get; set; }

        protected BaseEvent(string type)
        {
            this.Type = type;
        }
    }
}
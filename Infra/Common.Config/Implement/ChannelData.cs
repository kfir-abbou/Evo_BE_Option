using Common.Config.Interface;

namespace Common.Config.Implement
{
    [Serializable]
    public class ChannelData : IChannelData
    {
        public string Id { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RoutingKey { get; set; }


        public ChannelData()
        {
	        
        }
        public ChannelData(string id)
        {
	        Id = id;
        }

        public override string ToString()
        {
	        return $"{Id}, {Exchange}, {Queue}, {RoutingKey}";
        }
    }
}

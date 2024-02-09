using System.Collections.Generic;

namespace Samples.Remote.DTO
{
    public interface IConvertibleConfig
    {
        float FloatValue();
        bool BoolValue();
        int IntValue();
        string StringValue();
    }
    
    public class FogConfig : IConvertibleConfig
    {
        public string key { get; set; }
        public object value { get; set; }
        
        public float FloatValue()
        {
            return float.Parse(value.ToString());
        }

        public bool BoolValue()
        {
            return bool.Parse(value.ToString());
        }

        public int IntValue()
        {
            return int.Parse(value.ToString());
        }

        public string StringValue()
        {
            return (string) value;
        }
    }

    public class MovementConfig
    {
        public string type { get; set; }
        public int speed { get; set; }
    }

    public class MadboxSheet
    {
        public List<MovementConfig> MovementConfig { get; set; }
        public List<FogConfig> FogConfig { get; set; }
    }
}
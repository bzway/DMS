namespace Bzway.Common.Utility
{
    interface ISerializePolicy
    {
        T Deserialize<T>(string payload) where T : new();
        string Serialize(object payload);
    }
}
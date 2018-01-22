using System;

class MongoClient
{
    private string connectionString;

    public MongoClient(string connectionString)
    {
        this.connectionString = connectionString;
    }

    internal object GetDatabase(string datebaseName)
    {
        throw new NotImplementedException();
    }
}
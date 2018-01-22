using Bzway.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System;

namespace Bzway.Tools.ETL
{
    public class DataRowConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataRow row = value as DataRow;

            // *** HACK: need to use root serializer to write the column value
            //     should be fixed in next ver of JSON.NET with writer.Serialize(object)
            JsonSerializer ser = new JsonSerializer();

            writer.WriteStartObject();
            foreach (DataColumn column in row.Table.Columns)
            {
                writer.WritePropertyName(column.ColumnName);
                ser.Serialize(writer, row[column]);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataRow).IsAssignableFrom(objectType);
        }
    }
    public class DataTableConverter : JsonConverter
    {
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataTable).IsAssignableFrom(valueType);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataTable table = value as DataTable;
            DataRowConverter converter = new DataRowConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Rows");
            writer.WriteStartArray();

            foreach (DataRow row in table.Rows)
            {
                converter.WriteJson(writer, row, serializer);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class DataSetConverter : JsonConverter
    {
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataSet).IsAssignableFrom(valueType);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataSet dataSet = value as DataSet;
            DataTableConverter converter = new DataTableConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Tables");
            writer.WriteStartArray();
            foreach (DataTable table in dataSet.Tables)
            {
                converter.WriteJson(writer, table, serializer);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
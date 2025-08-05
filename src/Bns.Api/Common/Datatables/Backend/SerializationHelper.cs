using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bns.Api.Common.Datatables.Backend;

public class SerializationHelper
{
    public static string Serialize<T>(T configuration)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (TextWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, configuration);

            return writer.ToString();
        }
    }

    public static T Deserialize<T>(string configuration)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StringReader reader = new StringReader(configuration))
        {
            return (T)serializer.Deserialize(reader);
        }
    }

    public static T DeserialiazeXElement<T>(XElement element)
    {
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(element.CreateReader());
    }

    public static XElement ToXElement<T>(T obj)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(streamWriter, obj);
                return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
            }
        }
    }
}
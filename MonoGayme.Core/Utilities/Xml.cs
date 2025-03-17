using System.Xml;
using System.Xml.Serialization;

namespace MonoGayme.Core.Utilities;

public static class Xml
{
	public static T Deserialise<T>(string path)
	{
		XmlSerializer serialiser = new XmlSerializer(typeof(T));

		using Stream file = new FileStream(path, FileMode.Open);
		return (T?)serialiser.Deserialize(file) ?? throw new XmlException("Could not deserialise xml file.");
	}

	public static void Serialise<T>(T data, string path)
	{
		XmlSerializer serialiser = new XmlSerializer(typeof(T));

		using StreamWriter file = new StreamWriter(path);
		serialiser.Serialize(file, data);
	}
}
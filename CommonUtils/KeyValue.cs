using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace CommonUtils
{
	//static XMLSer
	[Serializable()]
	public class KeyValue<T>
	{
		public string key;
		public T value;

		public KeyValue()
		{

		}

		public KeyValue(SerializationInfo info, StreamingContext ctxt)
		{
			key = (String)info.GetValue("key", typeof(string));
			value = (T)info.GetValue("value", typeof(T));
		}
		
		public KeyValue(string xml)
		{
			if (_xmlSerializer == null)
				_xmlSerializer = new XmlSerializer(GetType());
			StringReader textReader = new StringReader(xml);
			KeyValue<T> t = (KeyValue<T>)_xmlSerializer.Deserialize(textReader);
			key = t.key;
			value = t.value;
		}
		//Serialization function.
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("key", key);
			info.AddValue("value", value);
		}

		public string GetXml()
		{
			if (_xmlSerializer == null)
				_xmlSerializer = new XmlSerializer(GetType());

			StringWriter textWriter = new StringWriter();
			_xmlSerializer.Serialize(textWriter, this);
			return textWriter.ToString();
		}

		private
			XmlSerializer _xmlSerializer; 
		
	}
}

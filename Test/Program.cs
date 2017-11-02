using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils;
using System.Xml.Serialization;
using System.IO;

namespace Test
{
	class Program
	{
		static void TestRandomUtil()
		{
			List<string> names = new List<string>() { "Anjali", "Anna", "Bindiya", "Chayanika", "Diana", "Ellen", "Farah", "Gina" };
			CommonUtils.Utilities.Shuffle(ref names);
			names.ForEach(Console.WriteLine);
			
		}

		void SerializationTest()
		{
			KeyValue<int> x = new KeyValue<int> { key = "age", value = 100 };
			string st = x.GetXml();
			Type tt = x.GetType();
			KeyValue<int> xy = new KeyValue<int>(st);
			KeyValue<string> d = new KeyValue<string> { key = "age", value = "100" };
			tt = x.GetType();

			XmlSerializer xmlSerializer = new XmlSerializer(x.GetType());
			StringWriter textWriter = new StringWriter();


			xmlSerializer.Serialize(textWriter, x);
			string str = textWriter.ToString();

			StringReader textReader = new StringReader(str);
			XmlSerializer xml = new XmlSerializer(x.GetType());
			KeyValue<int> t = (KeyValue<int>)xmlSerializer.Deserialize(textReader);
		}
		static void Main(string[] args)
		{
			do
			{
				TestRandomUtil();
				Console.WriteLine("Press q to stop");
			}
			while (Console.Read() != 'q');
		}
	}
}

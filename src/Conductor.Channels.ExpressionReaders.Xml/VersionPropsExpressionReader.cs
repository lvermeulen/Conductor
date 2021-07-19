using System.IO;
using System.Xml.Linq;

namespace Conductor.Channels.ExpressionReaders.Xml
{
    public class VersionPropsExpressionReader : XmlExpressionReader
    {
        public static VersionPropsExpressionReader LoadFrom(string fileName)
        {
            using var stm = new FileStream(fileName, FileMode.Open);
            return new VersionPropsExpressionReader(XDocument.Load(stm, LoadOptions.None));
        }

        public static VersionPropsExpressionReader Parse(string s) => new VersionPropsExpressionReader(XDocument.Parse(s));

        public VersionPropsExpressionReader(XDocument document)
            : base(document)
        { }
    }
}

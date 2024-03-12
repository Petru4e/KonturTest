using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace KonturTest
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument? doc = new XmlDocument();
            doc.Load("List.xml");
            XmlReader? xmlReadB = new XmlTextReader(new StringReader(doc.DocumentElement.OuterXml));

            XslCompiledTransform? xslt = new XslCompiledTransform();
            xslt.Load("XSLT.xslt");

            XmlWriterSettings? settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter? writer = XmlWriter.Create("Groups.xml", settings);
            xslt.Transform(xmlReadB, null, writer, null);
            writer.Close();


            XmlDocument? doc2 = new XmlDocument();
            doc2.Load("Groups.xml");
            XmlElement? root = doc2.DocumentElement;
            foreach (XmlElement group in root.ChildNodes)
            {
                group.SetAttribute("count", group.ChildNodes.Count.ToString());
            }
            doc2.Save("Groups.xml");

            XmlDocument? doc3 = new XmlDocument();
            doc3.Load("List.xml");
            XmlElement? root1 = doc3.DocumentElement;
            root1?.SetAttribute("count", root1.ChildNodes.Count.ToString());
            doc3.Save("List.xml");


            XmlNodeList? xmlNodeList = root.SelectNodes("group");
            if (xmlNodeList is not null)
            {
                var result = "";
                foreach (XmlNode node in xmlNodeList)
                    result += $"group name {node.SelectSingleNode("@name")?.Value} count {node.SelectSingleNode("@count")?.Value}\n";
                textBox1.Text = result;
            }

            XmlNode? xmlNode = root1 as XmlNode;
            if (xmlNode is not null)
            {
                textBox2.Text = $"list count {xmlNode.SelectSingleNode("@count")?.Value}";
            }
        }
    }
}
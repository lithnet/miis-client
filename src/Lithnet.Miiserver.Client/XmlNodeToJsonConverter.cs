using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /*
     Copyright (c) 2007 James Newton-King

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    */

    /// <summary>
    /// Converts an <see cref="XmlNode"/> to a JSON string using framework-native types only.
    /// </summary>
    /// <remarks>
    /// This is a faithful, dependency-free reimplementation of the write path of
    /// Newtonsoft.Json's <c>JsonConvert.SerializeXmlNode(XmlNode)</c> (v11.0.1) with the default
    /// settings used by that overload (OmitRootObject = false, WriteArrayAttribute = false,
    /// Formatting.None, StringEscapeHandling.Default). It exists solely so that
    /// <see cref="XmlObjectBase.ToJson"/> can produce byte-for-byte identical output without taking
    /// a dependency on Newtonsoft.Json. The algorithm (node grouping, property naming, array
    /// detection, value/object/null selection) and the string escaping rules are ported directly
    /// from the Json.NET source so existing consumers of the JSON output are unaffected.
    /// </remarks>
    internal sealed class XmlNodeToJsonConverter
    {
        private const string TextName = "#text";
        private const string CommentName = "#comment";
        private const string CDataName = "#cdata-section";
        private const string WhitespaceName = "#whitespace";
        private const string SignificantWhitespaceName = "#significant-whitespace";
        private const string DeclarationName = "?xml";
        private const string DocumentTypeLocalName = "DOCTYPE";
        private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";
        private const string XmlnsNamespaceUri = "http://www.w3.org/2000/xmlns/";

        private readonly StringBuilder builder = new StringBuilder();

        // JSON writer state machine, mirroring Newtonsoft.Json's JsonWriter so that value
        // delimiters (commas) are emitted in exactly the same positions.
        private enum WriterState
        {
            Start,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            Property
        }

        private WriterState state = WriterState.Start;

        // true = object, false = array
        private readonly List<bool> containerStack = new List<bool>();

        /// <summary>
        /// Converts the specified node to a JSON string, matching the output of
        /// <c>JsonConvert.SerializeXmlNode(node)</c>.
        /// </summary>
        /// <param name="node">The node to convert. May be null.</param>
        /// <returns>The JSON representation of the node.</returns>
        public static string Convert(XmlNode node)
        {
            if (node == null)
            {
                // Matches JsonConvert.SerializeXmlNode(null), which serializes a null value.
                return "null";
            }

            XmlNodeToJsonConverter converter = new XmlNodeToJsonConverter();
            converter.WriteDocument(node);
            return converter.builder.ToString();
        }

        private void WriteDocument(XmlNode node)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
            this.PushParentNamespaces(node, manager);

            // OmitRootObject is false for the SerializeXmlNode(node) overload.
            this.WriteStartObject();
            this.SerializeNode(node, manager, true);
            this.WriteEndObject();
        }

        #region XML traversal (ported from Newtonsoft.Json XmlNodeConverter)

        private void PushParentNamespaces(XmlNode node, XmlNamespaceManager manager)
        {
            List<XmlNode> parentElements = null;

            XmlNode parent = node;
            while ((parent = GetParent(parent)) != null)
            {
                if (parent.NodeType == XmlNodeType.Element)
                {
                    if (parentElements == null)
                    {
                        parentElements = new List<XmlNode>();
                    }

                    parentElements.Add(parent);
                }
            }

            if (parentElements != null)
            {
                parentElements.Reverse();

                foreach (XmlNode parentElement in parentElements)
                {
                    manager.PushScope();
                    foreach (XmlNode attribute in GetAttributes(parentElement))
                    {
                        if (attribute.NamespaceURI == XmlnsNamespaceUri && attribute.LocalName != "xmlns")
                        {
                            manager.AddNamespace(attribute.LocalName, attribute.Value);
                        }
                    }
                }
            }
        }

        private static XmlNode GetParent(XmlNode node)
        {
            XmlAttribute attribute = node as XmlAttribute;
            if (attribute != null)
            {
                return attribute.OwnerElement;
            }

            return node.ParentNode;
        }

        private string ResolveFullName(XmlNode node, XmlNamespaceManager manager)
        {
            string localName = GetLocalName(node);

            string prefix;
            if (node.NamespaceURI == null || (localName == "xmlns" && node.NamespaceURI == XmlnsNamespaceUri))
            {
                prefix = null;
            }
            else
            {
                prefix = manager.LookupPrefix(node.NamespaceURI);
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                return prefix + ":" + XmlConvert.DecodeName(localName);
            }

            return XmlConvert.DecodeName(localName);
        }

        private string GetPropertyName(XmlNode node, XmlNamespaceManager manager)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Attribute:
                    if (node.NamespaceURI == JsonNamespaceUri)
                    {
                        return "$" + node.LocalName;
                    }

                    return "@" + this.ResolveFullName(node, manager);

                case XmlNodeType.CDATA:
                    return CDataName;

                case XmlNodeType.Comment:
                    return CommentName;

                case XmlNodeType.Element:
                    if (node.NamespaceURI == JsonNamespaceUri)
                    {
                        return "$" + node.LocalName;
                    }

                    return this.ResolveFullName(node, manager);

                case XmlNodeType.ProcessingInstruction:
                    return "?" + this.ResolveFullName(node, manager);

                case XmlNodeType.DocumentType:
                    return "!" + this.ResolveFullName(node, manager);

                case XmlNodeType.XmlDeclaration:
                    return DeclarationName;

                case XmlNodeType.SignificantWhitespace:
                    return SignificantWhitespaceName;

                case XmlNodeType.Text:
                    return TextName;

                case XmlNodeType.Whitespace:
                    return WhitespaceName;

                default:
                    throw new InvalidOperationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
            }
        }

        private static bool IsArray(XmlNode node)
        {
            foreach (XmlNode attribute in GetAttributes(node))
            {
                if (attribute.LocalName == "Array" && attribute.NamespaceURI == JsonNamespaceUri)
                {
                    return XmlConvert.ToBoolean(attribute.Value);
                }
            }

            return false;
        }

        private void SerializeGroupedNodes(XmlNode node, XmlNamespaceManager manager, bool writePropertyName)
        {
            List<XmlNode> childNodes = GetChildNodes(node);

            switch (childNodes.Count)
            {
                case 0:
                    {
                        // nothing to serialize
                        break;
                    }

                case 1:
                    {
                        // avoid grouping when there is only one node
                        string nodeName = this.GetPropertyName(childNodes[0], manager);
                        this.WriteGroupedNodes(manager, writePropertyName, childNodes, nodeName);
                        break;
                    }

                default:
                    {
                        // check whether nodes have the same name
                        // if they don't then group into dictionary together by name

                        // value of dictionary will be a single XmlNode when there is one for a name,
                        // or a List<XmlNode> when there are multiple
                        Dictionary<string, object> nodesGroupedByName = null;

                        string nodeName = null;

                        for (int i = 0; i < childNodes.Count; i++)
                        {
                            XmlNode childNode = childNodes[i];
                            string currentNodeName = this.GetPropertyName(childNode, manager);

                            if (nodesGroupedByName == null)
                            {
                                if (nodeName == null)
                                {
                                    nodeName = currentNodeName;
                                }
                                else if (currentNodeName == nodeName)
                                {
                                    // current node name matches others
                                }
                                else
                                {
                                    nodesGroupedByName = new Dictionary<string, object>();
                                    if (i > 1)
                                    {
                                        List<XmlNode> nodes = new List<XmlNode>(i);
                                        for (int j = 0; j < i; j++)
                                        {
                                            nodes.Add(childNodes[j]);
                                        }

                                        nodesGroupedByName.Add(nodeName, nodes);
                                    }
                                    else
                                    {
                                        nodesGroupedByName.Add(nodeName, childNodes[0]);
                                    }

                                    nodesGroupedByName.Add(currentNodeName, childNode);
                                }
                            }
                            else
                            {
                                object value;
                                if (!nodesGroupedByName.TryGetValue(currentNodeName, out value))
                                {
                                    nodesGroupedByName.Add(currentNodeName, childNode);
                                }
                                else
                                {
                                    List<XmlNode> nodes = value as List<XmlNode>;
                                    if (nodes == null)
                                    {
                                        nodes = new List<XmlNode> { (XmlNode)value };
                                        nodesGroupedByName[currentNodeName] = nodes;
                                    }

                                    nodes.Add(childNode);
                                }
                            }
                        }

                        if (nodesGroupedByName == null)
                        {
                            this.WriteGroupedNodes(manager, writePropertyName, childNodes, nodeName);
                        }
                        else
                        {
                            // loop through grouped nodes. write single name instances as normal,
                            // write multiple names together in an array
                            foreach (KeyValuePair<string, object> nodeNameGroup in nodesGroupedByName)
                            {
                                List<XmlNode> nodes = nodeNameGroup.Value as List<XmlNode>;
                                if (nodes != null)
                                {
                                    this.WriteGroupedNodes(manager, writePropertyName, nodes, nodeNameGroup.Key);
                                }
                                else
                                {
                                    this.WriteGroupedNodes(manager, writePropertyName, (XmlNode)nodeNameGroup.Value, nodeNameGroup.Key);
                                }
                            }
                        }

                        break;
                    }
            }
        }

        private void WriteGroupedNodes(XmlNamespaceManager manager, bool writePropertyName, List<XmlNode> groupedNodes, string elementNames)
        {
            bool writeArray = groupedNodes.Count != 1 || IsArray(groupedNodes[0]);

            if (!writeArray)
            {
                this.SerializeNode(groupedNodes[0], manager, writePropertyName);
            }
            else
            {
                if (writePropertyName)
                {
                    this.WritePropertyName(elementNames);
                }

                this.WriteStartArray();

                for (int i = 0; i < groupedNodes.Count; i++)
                {
                    this.SerializeNode(groupedNodes[i], manager, false);
                }

                this.WriteEndArray();
            }
        }

        private void WriteGroupedNodes(XmlNamespaceManager manager, bool writePropertyName, XmlNode node, string elementNames)
        {
            bool writeArray = IsArray(node);

            if (!writeArray)
            {
                this.SerializeNode(node, manager, writePropertyName);
            }
            else
            {
                if (writePropertyName)
                {
                    this.WritePropertyName(elementNames);
                }

                this.WriteStartArray();

                this.SerializeNode(node, manager, false);

                this.WriteEndArray();
            }
        }

        private void SerializeNode(XmlNode node, XmlNamespaceManager manager, bool writePropertyName)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Document:
                case XmlNodeType.DocumentFragment:
                    this.SerializeGroupedNodes(node, manager, writePropertyName);
                    break;

                case XmlNodeType.Element:
                    if (IsArray(node) && AllSameName(node) && GetChildNodes(node).Count > 0)
                    {
                        this.SerializeGroupedNodes(node, manager, false);
                    }
                    else
                    {
                        manager.PushScope();

                        List<XmlNode> attributes = GetAttributes(node);

                        foreach (XmlNode attribute in attributes)
                        {
                            if (attribute.NamespaceURI == XmlnsNamespaceUri)
                            {
                                string namespacePrefix = (attribute.LocalName != "xmlns")
                                    ? XmlConvert.DecodeName(attribute.LocalName)
                                    : string.Empty;
                                string namespaceUri = attribute.Value;
                                if (namespaceUri == null)
                                {
                                    throw new InvalidOperationException("Namespace attribute must have a value.");
                                }

                                manager.AddNamespace(namespacePrefix, namespaceUri);
                            }
                        }

                        if (writePropertyName)
                        {
                            this.WritePropertyName(this.GetPropertyName(node, manager));
                        }

                        List<XmlNode> childNodes = GetChildNodes(node);

                        if (!ValueAttributes(attributes) && childNodes.Count == 1
                            && childNodes[0].NodeType == XmlNodeType.Text)
                        {
                            // write elements with a single text child as a name value pair
                            this.WriteValue(childNodes[0].Value);
                        }
                        else if (childNodes.Count == 0 && attributes.Count == 0)
                        {
                            // empty element
                            XmlElement element = (XmlElement)node;
                            if (element.IsEmpty)
                            {
                                this.WriteNull();
                            }
                            else
                            {
                                this.WriteValue(string.Empty);
                            }
                        }
                        else
                        {
                            this.WriteStartObject();

                            for (int i = 0; i < attributes.Count; i++)
                            {
                                this.SerializeNode(attributes[i], manager, true);
                            }

                            this.SerializeGroupedNodes(node, manager, true);

                            this.WriteEndObject();
                        }

                        manager.PopScope();
                    }

                    break;

                case XmlNodeType.Comment:
                    if (writePropertyName)
                    {
                        this.WriteComment(node.Value);
                    }

                    break;

                case XmlNodeType.Attribute:
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                    if (node.NamespaceURI == XmlnsNamespaceUri && node.Value == JsonNamespaceUri)
                    {
                        return;
                    }

                    if (node.NamespaceURI == JsonNamespaceUri)
                    {
                        if (node.LocalName == "Array")
                        {
                            return;
                        }
                    }

                    if (writePropertyName)
                    {
                        this.WritePropertyName(this.GetPropertyName(node, manager));
                    }

                    this.WriteValue(node.Value);
                    break;

                case XmlNodeType.XmlDeclaration:
                    XmlDeclaration declaration = (XmlDeclaration)node;
                    this.WritePropertyName(this.GetPropertyName(node, manager));
                    this.WriteStartObject();

                    if (!string.IsNullOrEmpty(declaration.Version))
                    {
                        this.WritePropertyName("@version");
                        this.WriteValue(declaration.Version);
                    }

                    if (!string.IsNullOrEmpty(declaration.Encoding))
                    {
                        this.WritePropertyName("@encoding");
                        this.WriteValue(declaration.Encoding);
                    }

                    if (!string.IsNullOrEmpty(declaration.Standalone))
                    {
                        this.WritePropertyName("@standalone");
                        this.WriteValue(declaration.Standalone);
                    }

                    this.WriteEndObject();
                    break;

                case XmlNodeType.DocumentType:
                    XmlDocumentType documentType = (XmlDocumentType)node;
                    this.WritePropertyName(this.GetPropertyName(node, manager));
                    this.WriteStartObject();

                    if (!string.IsNullOrEmpty(documentType.Name))
                    {
                        this.WritePropertyName("@name");
                        this.WriteValue(documentType.Name);
                    }

                    if (!string.IsNullOrEmpty(documentType.PublicId))
                    {
                        this.WritePropertyName("@public");
                        this.WriteValue(documentType.PublicId);
                    }

                    if (!string.IsNullOrEmpty(documentType.SystemId))
                    {
                        this.WritePropertyName("@system");
                        this.WriteValue(documentType.SystemId);
                    }

                    if (!string.IsNullOrEmpty(documentType.InternalSubset))
                    {
                        this.WritePropertyName("@internalSubset");
                        this.WriteValue(documentType.InternalSubset);
                    }

                    this.WriteEndObject();
                    break;

                default:
                    throw new InvalidOperationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
            }
        }

        private static bool AllSameName(XmlNode node)
        {
            foreach (XmlNode childNode in GetChildNodes(node))
            {
                if (childNode.LocalName != node.LocalName)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValueAttributes(List<XmlNode> attributes)
        {
            foreach (XmlNode attribute in attributes)
            {
                if (attribute.NamespaceURI == JsonNamespaceUri)
                {
                    continue;
                }

                if (attribute.NamespaceURI == XmlnsNamespaceUri && attribute.Value == JsonNamespaceUri)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        private static List<XmlNode> GetChildNodes(XmlNode node)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    nodes.Add(child);
                }
            }

            return nodes;
        }

        private static List<XmlNode> GetAttributes(XmlNode node)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    nodes.Add(attribute);
                }
            }

            return nodes;
        }

        private static string GetLocalName(XmlNode node)
        {
            // Newtonsoft.Json's XmlDocumentTypeWrapper overrides LocalName to "DOCTYPE".
            if (node.NodeType == XmlNodeType.DocumentType)
            {
                return DocumentTypeLocalName;
            }

            return node.LocalName;
        }

        #endregion

        #region JSON writer (ported from Newtonsoft.Json JsonTextWriter / JsonWriter, Formatting.None)

        private void WriteStartObject()
        {
            this.WriteValueDelimiterIfRequired(false);
            this.builder.Append('{');
            this.containerStack.Add(true);
            this.state = WriterState.ObjectStart;
        }

        private void WriteEndObject()
        {
            this.containerStack.RemoveAt(this.containerStack.Count - 1);
            this.builder.Append('}');
            this.UpdateStateAfterContainerClose();
        }

        private void WriteStartArray()
        {
            this.WriteValueDelimiterIfRequired(false);
            this.builder.Append('[');
            this.containerStack.Add(false);
            this.state = WriterState.ArrayStart;
        }

        private void WriteEndArray()
        {
            this.containerStack.RemoveAt(this.containerStack.Count - 1);
            this.builder.Append(']');
            this.UpdateStateAfterContainerClose();
        }

        private void WritePropertyName(string name)
        {
            this.WriteValueDelimiterIfRequired(false);
            this.WriteEscapedString(name);
            this.builder.Append(':');
            this.state = WriterState.Property;
        }

        private void WriteValue(string value)
        {
            this.WriteValueDelimiterIfRequired(false);
            if (value == null)
            {
                this.builder.Append("null");
            }
            else
            {
                this.WriteEscapedString(value);
            }

            this.state = this.StateAfterValue();
        }

        private void WriteNull()
        {
            this.WriteValueDelimiterIfRequired(false);
            this.builder.Append("null");
            this.state = this.StateAfterValue();
        }

        private void WriteComment(string text)
        {
            // Comments never get a value delimiter and do not advance the writer state,
            // matching Newtonsoft.Json's state table for the Comment token.
            this.WriteValueDelimiterIfRequired(true);
            this.builder.Append("/*");
            this.builder.Append(text);
            this.builder.Append("*/");
        }

        private void WriteValueDelimiterIfRequired(bool isComment)
        {
            if (!isComment && (this.state == WriterState.Object || this.state == WriterState.Array))
            {
                this.builder.Append(',');
            }
        }

        private WriterState StateAfterValue()
        {
            if (this.containerStack.Count == 0)
            {
                return WriterState.Start;
            }

            return this.containerStack[this.containerStack.Count - 1] ? WriterState.Object : WriterState.Array;
        }

        private void UpdateStateAfterContainerClose()
        {
            if (this.containerStack.Count == 0)
            {
                this.state = WriterState.Start;
            }
            else
            {
                this.state = this.containerStack[this.containerStack.Count - 1] ? WriterState.Object : WriterState.Array;
            }
        }

        private void WriteEscapedString(string value)
        {
            // Mirrors Newtonsoft.Json's JavaScriptUtils.WriteEscapedJavaScriptString with the
            // double-quote delimiter and StringEscapeHandling.Default. The escaped set is:
            // the named escapes below, all control characters below 0x20, and the three
            // line-terminator characters U+0085, U+2028 and U+2029.
            this.builder.Append('"');

            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                {
                    switch (c)
                    {
                        case '\t':
                            this.builder.Append("\\t");
                            break;
                        case '\n':
                            this.builder.Append("\\n");
                            break;
                        case '\r':
                            this.builder.Append("\\r");
                            break;
                        case '\f':
                            this.builder.Append("\\f");
                            break;
                        case '\b':
                            this.builder.Append("\\b");
                            break;
                        case '\\':
                            this.builder.Append("\\\\");
                            break;
                        case '"':
                            this.builder.Append("\\\"");
                            break;
                        default:
                            if (c < ' ' || c == (char)0x85 || c == (char)0x2028 || c == (char)0x2029)
                            {
                                this.AppendUnicodeEscape(c);
                            }
                            else
                            {
                                this.builder.Append(c);
                            }

                            break;
                    }
                }
            }

            this.builder.Append('"');
        }

        private void AppendUnicodeEscape(char c)
        {
            this.builder.Append("\\u");
            this.builder.Append(IntToHex((c >> 12) & 0x0f));
            this.builder.Append(IntToHex((c >> 8) & 0x0f));
            this.builder.Append(IntToHex((c >> 4) & 0x0f));
            this.builder.Append(IntToHex(c & 0x0f));
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }

            return (char)((n - 10) + 97);
        }

        #endregion
    }
}

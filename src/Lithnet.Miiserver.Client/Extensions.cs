using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    internal static class Extensions
    {
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static string ToMmsDateString(this DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static T ReadInnerText<T>(this XmlNode node)
        {
            if (node == null)
            {
                return default(T);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)node.InnerText;
            }

            if (typeof(T) == typeof(DateTime?))
            {
                return (T)(object)node.ReadInnerTextAsDate();
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)node.ReadInnerTextAsInt();
            }

            if (typeof(T) == typeof(DateTime))
            {
                return (T)(object)(node.ReadInnerTextAsDate() ?? new DateTime(0));
            }

            if (typeof(T) == typeof(bool?))
            {
                return (T)(object)node.ReadInnerTextAsNullableBool();
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)node.ReadInnerTextAsBool();
            }

            if (typeof(T).IsEnum)
            {
                return node.ReadInnerTextAsEnum<T>();
            }

            if (typeof(T) == typeof(Guid?))
            {
                return (T)(object)node.ReadInnerTextAsGuid();
            }

            if (typeof(T) == typeof(Guid))
            {
                return (T)(object)(node.ReadInnerTextAsGuid() ?? Guid.Empty);
            }

            throw new InvalidCastException("Unsupported type");
        }

        public static DateTime? ReadInnerTextAsDate(this XmlNode node)
        {
            if (node == null)
            {
                return null;
            }

            string date = node.InnerText;

            if (string.IsNullOrWhiteSpace(date))
            {
                return null;
            }
            else
            {

                if (DateTime.TryParse(date, out DateTime parsedDateTime))
                {
                    return parsedDateTime.ToLocalTime();
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ReadInnerTextAsString(this XmlNode node)
        {
            return node?.InnerText;
        }

        public static int ReadInnerTextAsInt(this XmlNode node)
        {
            if (node == null)
            {
                return 0;
            }

            int.TryParse(node.InnerText, out int result);

            return result;
        }

        public static bool? ReadInnerTextAsNullableBool(this XmlNode node)
        {
            if (node == null)
            {
                return null;
            }

            if (bool.TryParse(node.InnerText, out bool result))
            {
                return result;
            }
            else
            {
                if (node.InnerText == "1")
                {
                    return true;
                }
                else if (node.InnerText == "0")
                {
                    return false;
                }
            }

            return null;
        }

        public static T ReadInnerTextAsEnum<T>(this XmlNode node)
        {
            if (node == null)
            {
                return default(T);
            }

            return GetEnumValueFromXmlEnumAttribute<T>(node.InnerText);
        }

        public static T GetEnumValueFromXmlEnumAttribute<T>(string description)
        {
            Type type = typeof(T);

            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (FieldInfo field in type.GetFields())
            {
                if (System.Attribute.GetCustomAttribute(field, typeof(XmlEnumAttribute)) is XmlEnumAttribute attribute)
                {
                    if (attribute.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException(string.Format("The enum value {0} was not known", description));
        }

        public static bool ReadInnerTextAsBool(this XmlNode node)
        {
            return node.ReadInnerTextAsNullableBool() ?? false;
        }

        public static Guid? ReadInnerTextAsGuid(this XmlNode node)
        {
            if (node == null)
            {
                return null;
            }


            if (Guid.TryParse(node.InnerText, out Guid result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Converts an enumeration of strings into a comma separated list
        /// </summary>
        /// <param name="strings">The enumeration of string objects</param>
        /// <returns>The comma separated list of strings</returns>
        public static string ToCommaSeparatedString(this IEnumerable<string> strings)
        {
            string newString = string.Empty;

            if (strings != null)
            {
                foreach (string text in strings)
                {
                    newString = newString.AppendWithCommaSeparator(text);
                }
            }

            return newString;
        }

        public static string EscapeWqlString(this string text)
        {
            //return text.Replace("\"", "\\\"").Replace("\'", "\\\'").Replace(@"\", @"\\");
            return text.Replace(@"\", @"\\").Replace("\'", "\\\'");
        }

        /// <summary>
        /// Converts an enumeration of strings into a comma separated list
        /// </summary>
        /// <param name="strings">The enumeration of string objects</param>
        /// <param name="separator">The character or string to use to separate the strings</param>
        /// <returns>The comma separated list of strings</returns>
        public static string ToSeparatedString(this IEnumerable<string> strings, string separator)
        {
            string newString = string.Empty;

            foreach (string text in strings)
            {
                newString = newString.AppendWithSeparator(separator, text);
            }

            return newString;
        }

        /// <summary>
        /// Converts an enumeration of strings into a comma separated list
        /// </summary>
        /// <param name="strings">The enumeration of string objects</param>
        /// <returns>The comma separated list of strings</returns>
        public static string ToNewLineSeparatedString(this IEnumerable<string> strings)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string text in strings)
            {
                builder.AppendLine(text);
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// Appends two string together with a comma and a space
        /// </summary>
        /// <param name="text">The original string</param>
        /// <param name="textToAppend">The string to append</param>
        /// <returns>The concatenated string</returns>
        public static string AppendWithCommaSeparator(this string text, string textToAppend)
        {
            return text.AppendWithSeparator(", ", textToAppend);
        }

        /// <summary>
        /// Appends two string together with a comma and a space
        /// </summary>
        /// <param name="text">The original string</param>
        /// <param name="separator">The character or string to use to separate the strings</param>
        /// <param name="textToAppend">The string to append</param>
        /// <returns>The concatenated string</returns>
        public static string AppendWithSeparator(this string text, string separator, string textToAppend)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                text += separator;
            }
            else
            {
                text = string.Empty;
            }

            return text + textToAppend;
        }

        public static string ToMmsGuid(this Guid guid)
        {
            return guid.ToString("B").ToUpper();
        }

        public static string GetMaData(this MMSWebService m, Guid maGuid, MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            return m.GetMaData(maGuid.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata);
        }

        public static string GetMVData(this MMSWebService m, MVData mvdata)
        {
            return m.GetMVData((uint)mvdata);
        }

        public static string EscapeXmlElementText(this string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        public static IEnumerable<CSObjectRef> GetCSObjectReferences(this CounterDetail detail)
        {
            return SyncServer.GetStepDetailCSObjectRefs(detail.StepId, detail.XmlNode.Name);
        }
    }
}

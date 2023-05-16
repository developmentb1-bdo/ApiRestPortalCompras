using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace S7TechIntegracao.API
{
    public static class S7Tech
    {
        public static string GetConsultas(string nomeConsulta)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var targetName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith($".{nomeConsulta}.sql"));

                using (var stream = assembly.GetManifestResourceStream(targetName))
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static class Extensions
    {
        /// <summary> Método Serializar arquivo. </summary>
        /// <param name="Object">  Objeto</param>
        /// <returns></returns>
        public static String GetAsXml(this Object Object)
        {
            MemoryStream MemoriaStream = new MemoryStream();
            XmlSerializer Xml = new XmlSerializer(Object.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoriaStream, Encoding.UTF8);
            Xml.Serialize(xmlTextWriter, Object);
            MemoriaStream = (MemoryStream)xmlTextWriter.BaseStream;
            MemoriaStream.Position = 0;

            XDocument xDoc = XDocument.Load(MemoriaStream);

            return xDoc.ToString();
        }

        /// <summary> faz a transformação para XML a partir de um arquivo XML de dados </summary>
        /// <param name="sourceFile">arquivo de origem contendo os dados para transformação</param>
        /// <param name="styleSheet">arquivo com o template para transformação ".xsl"</param>
        /// <param name="parametres">
        /// parameter[0]: nome da variável
        /// parameter[1]: namespace
        /// parameter[2]: valor
        /// </param>
        /// <param name="xmlTransformedData">arquivo de retorno transformado</param>
        public static String Transform(this String strXml, String styleSheet, List<String[]> parameters)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDocument xmlSourceFile = new XmlDocument();
            xmlSourceFile.LoadXml(strXml);

            String xmlTransformedData = string.Empty;

            try
            {
                String resultAux = String.Empty;

                XslTransform xslt = new XslTransform();
                xslt.Load(styleSheet);

                XsltArgumentList xslArg = null;

                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        xslArg = new XsltArgumentList();
                        foreach (String[] item in parameters)
                        {
                            xslArg.AddParam(item[0], item[1], item[2]);
                        }
                    }
                }

                MemoryStream stm = new MemoryStream();
                xslt.Transform(xmlSourceFile, xslArg, stm);

                stm.Position = 3;
                StreamReader sr = new StreamReader(stm, Encoding.UTF8);
                resultAux = sr.ReadToEnd();

                xmlDoc.LoadXml(resultAux);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                xmlTransformedData = xmlDoc.OuterXml;
            }

            return xmlTransformedData;
        }
    }
}
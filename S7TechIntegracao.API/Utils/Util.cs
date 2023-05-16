using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace S7TechIntegracao.API.Utils {
    public static class Util {
        public static T GetSLayerVal<T>(string json) {
            return JObject.Parse(json)["value"].ToObject<T>();
        }

        public static string QryStrCombine(string baseres, string param) {
            var sb = new StringBuilder(baseres);
            if (param.Length > 0)
                sb.Append("&");
            sb.Append(param);

            return sb.ToString();
        }
    }
}
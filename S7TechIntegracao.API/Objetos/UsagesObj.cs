using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class UsagesObj
    {
        

        private static readonly UsagesObj _instancia = new UsagesObj();

        public static UsagesObj GetInstance()
        {
            return _instancia;
        }

        public List<Usages> ConsultarTodos()
        {
            try
            {
                var ret = new List<Usages>();
                var query = S7Tech.GetConsultas("ConsultarUtilizacoes");
                using (var hanaService = new HanaService())
                {
                    using (var dt = hanaService.ExecuteDataTable(query))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var usage = new Usages()
                            {
                                Id = Convert.ToInt32(row["ID"]),
                                Usage = Convert.ToString(row["Usage"])
                            };

                            ret.Add(usage);
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[UsagesObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }
    }
}
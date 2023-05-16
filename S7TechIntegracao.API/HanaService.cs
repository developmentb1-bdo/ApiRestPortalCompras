using S7TechIntegracao.API.Models;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API
{
    public class HanaService : IDisposable
    {
        private static readonly HanaService _instancia = new HanaService();

        public static HanaService GetInstance()
        {
            return _instancia;
        }

        private HanaConnection _conn;

        public HanaService()
        {
            Connection();
        }

        public object ExecuteScalar(string querySql)
        {
            try
            {
                var cmd = new HanaCommand(querySql, _conn);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal DataTable ExecuteDataTable(string querySql)
        {
            try
            {
                var dt = new DataTable("Table1");
                var da = new HanaDataAdapter(querySql, _conn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Connection()
        {
            try
            {
                _conn = new HanaConnection(ParamsModel.GetInstance().StringConnection);
                _conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal HanaConnection GetHanaConnection()
        {
            try
            {
                return _conn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_conn == null)
                    return;

                if (_conn.State == ConnectionState.Open)
                    _conn.Close();

                _conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
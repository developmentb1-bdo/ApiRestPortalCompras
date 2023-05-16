using System;
using System.Text;

namespace S7TechIntegracao.API.Models {
    public class GetParam {
        /// <summary>
        /// Quantidade de registros a serem retornados. Valor padrão e máximo: 20 registros
        /// </summary>
        public int? top { get; set; }

        /// <summary>
        /// Quantidade de registros a serem pulados (para fazer paginação)
        /// </summary>
        public int? skip { get; set; }

        /// <summary>
        /// Filtro. Ex.: campo1 eq 123 and contains(campo2, 'abc')
        /// 
        /// · startswith
        /// · endswith
        /// · contains
        /// · substringof
        /// 
        /// · and
        /// · or
        /// · le (less than or equal to)
        /// · lt (less than)
        /// · ge (greater than or equal to)
        /// · gt (greater than)
        /// · eq (equal to)
        /// · ne (not equal to)
        /// · not
        /// </summary>
        public string filter { get; set; }

        /// <summary>
        /// Ordena por determinado campo
        /// </summary>
        public string orderby { get; set; }

        public override string ToString() {
            var query = new StringBuilder();
            if (filter != null) {
                if (query.Length > 0)
                    query.Append("&");
                query.Append("$filter=").Append(Uri.EscapeDataString(filter));
            }

            if (orderby != null) {
                if (query.Length > 0)
                    query.Append("&");
                query.Append("$orderby=").Append(Uri.EscapeDataString(orderby));
            }

            if (skip != null) {
                if (query.Length > 0)
                    query.Append("&");
                query.Append("$skip=").Append(skip.ToString());
            }

            if (top != null) {
                if (query.Length > 0)
                    query.Append("&");
                query.Append("$top=").Append(top.ToString());
            }

            return query.ToString();
        }

        public static implicit operator string(GetParam value) => value.ToString();
    }
}
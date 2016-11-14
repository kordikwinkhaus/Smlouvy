using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmlouvaWord
{
    internal class SqlValueProvider : IValueProvider
    {
        private XmlValueProvider _xmlProvider;
        private Dictionary<string, object> _values;

        internal SqlValueProvider(Parameters parameters, XmlValueProvider xmlValueProvider)
        {
            _xmlProvider = xmlValueProvider;
            _values = LoadValues(Okna.Data.Utils.ModifyConnString(parameters.ConnectionString));
        }

        private Dictionary<string, object> LoadValues(string connectionString)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            string sid;
            if (_xmlProvider.GetValue("ID_DOKUMENTU", out sid))
            {
                int id = int.Parse(sid);

                string sql = @"SELECT o.opis AS akce, o.referencja AS ref_cislo, o.oferta_t as cislo_nabidky, ISNULL(ro.oferta_dat, o.modyf) as datum_zmeny, o.ofr_utworz as datum_nabidky, 
RTRIM(LTRIM(RTRIM(LTRIM(k.pt+' '+k.imie))+' '+k.nazwisko)) AS obj_jmeno, k.firma AS obj_firma, k.telefon, k.email, k.bank AS banka, k.konto AS ucet,
RTRIM(k.ulica+' '+k.numer) AS obj_adresa1, LTRIM(RTRIM(k.kod+' '+k.miasto+' ')) AS obj_adresa2,
RTRIM(a.ulica+' '+a.numer)+', '+LTRIM(RTRIM(a.kod+' '+a.miasto)) AS adresa_montaze
FROM dbo.oferty o 
LEFT JOIN dbo.rep_oferta ro ON o.indeks=ro.srcdoc
LEFT JOIN dbo.klienci k on o.odbiorca=k.indeks
LEFT JOIN dbo.klienci a ON o.adr_dostaw=a.indeks
WHERE o.indeks=@id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    string name = dr.GetName(i);
                                    object value = dr[i];

                                    if (value == DBNull.Value)
                                    {
                                        value = null;
                                    }

                                    result.Add(name.ToUpperInvariant(), value);
                                }
                            }
                        }
                    }
                }
            }
            
            return result;
        }

        public bool GetValue(string name, out string result)
        {
            if (name == "ADRESA_MONTAZE")
            {
                return GetAdresaMontaze(out result);
            }
            else
            {
                object obj;
                if (_values.TryGetValue(name, out obj))
                {
                    result = (obj != null) ? obj.ToString() : string.Empty;
                    return true;
                }
                else
                {
                    return _xmlProvider.GetValue(name, out result);
                }
            }
        }

        private bool GetAdresaMontaze(out string result)
        {
            string adresa;
            _xmlProvider.GetValue("ADRESA_MONTAZE", out adresa);

            if (string.IsNullOrWhiteSpace(adresa))
            {
                var obj = _values["ADRESA_MONTAZE"];
                adresa = (obj != null) ? obj.ToString() : string.Empty;

                if (string.IsNullOrWhiteSpace(adresa))
                {
                    obj = _values["AKCE"];
                    adresa = (obj != null) ? obj.ToString() : string.Empty;
                }
            }

            result = adresa;
            return true;
        }
    }
}

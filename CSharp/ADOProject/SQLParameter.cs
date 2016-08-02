/// <summary>
/// Hello everybody. I'm sorry the documentation is in Italian but I used it for an Italian project
/// and I didn't have time to translate. Sorry :(
/// Anyway, it works with a Postgres DB, but can easily be adapted to other RDBMSs, changing ADO objects to the right ones
/// Such as using other DbTypes instead of NpgsqlDbType.
///</summary>

using System;
using NpgsqlTypes;

namespace ADONamespace
{
    /// <summary>
    /// Rappresenta un tipo parametrico per una query SQL da eseguire su un DB PostgreSQL.
    /// Si istanzia un oggetto tramite l'apposito metodo Factory
    /// </summary>
    /// <seealso cref="SQLParameter.Create(String, NpgsqlDbType, Object)"/>
    public sealed class SQLParameter : Tuple<String, NpgsqlDbType, Object>
    {
        /// <summary>
        /// Rappresenta il nome del parametro
        /// </summary>
        /// <example>
        ///     Il valore "@nomeParametro"
        /// </example>
        public String Name
        {
            get
            {
                return base.Item1;
            }
        }

        /// <summary>
        /// Rappresenta il tipo del parametro
        /// </summary>
        /// <example>
        ///     Il valore NpgsqlDbType.Integer
        /// </example>
        public NpgsqlDbType DbType
        {
            get
            {
                return base.Item2;
            }
        }

        /// <summary>
        /// Rappresenta il valore da parametrizzare nelle query SQL
        /// </summary>
        /// <example>
        ///     Compatibilmente con gli esempi precedenti :
        ///     Un qualunque valore numerico
        ///     Si noti che deve essere compatibile con il tipo di parametro dichiarato
        /// </example>
        /// <seealso cref="ParameterType"/>
        public Object Value
        {
            get
            {
                return base.Item3;
            }
        }

        [Obsolete("Non usare piú, usare \"Name\"", true)]
        public new String Item1;

        [Obsolete("Non usare piú, usare \"DbType\"", true)]
        public new String Item2;

        [Obsolete("Non usare piú, usare \"Value\"", true)]
        public new String Item3;

        /// <summary>
        /// Crea una nuova istanza di SQLParameter con i parametri specificati
        /// </summary>
        /// <param name="ParameterName">Il Nome del Parametro</param>
        /// <param name="ParameterType">Il Tipo del Parametro, secondo gli standard PostgreSQL</param>
        /// <param name="ParameterValue">Il valore da parametrizzare</param>
        private SQLParameter(String ParameterName, NpgsqlDbType ParameterType, Object ParameterValue) : base(ParameterName, ParameterType, ParameterValue) { }

        /// <summary>
        /// Crea una nuova istanza di SQLParameter con i parametri specificati (metodo Factory)
        /// </summary>
        /// <param name="ParameterName">Il Nome del Parametro</param>
        /// <param name="ParameterType">Il Tipo del Parametro, secondo gli standard PostgreSQL</param>
        /// <param name="ParameterValue">Il valore da parametrizzare</param>
        public static SQLParameter Create(String ParameterName, NpgsqlDbType ParameterType, Object ParameterValue)
        {
            return new SQLParameter(ParameterName, ParameterType, ParameterValue ?? DBNull.Value);
        }
    }
}

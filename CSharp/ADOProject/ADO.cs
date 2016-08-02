/// <summary>
/// Hello everybody. I'm sorry the documentation is in Italian but I used it for an Italian project
/// and I didn't have time to translate. Sorry :(
/// Anyway, it works with a Postgres DB, but can easily be adapted to other RDBMSs, changing ADO objects to the right ones
/// Such as NpgsqlConnectionStringBuilder, NpgsqlConnection and NpgsqlCommand.
///</summary>

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ADONamespace
{
    /// <summary>
    /// Classe per l'interfacciamento con un database Postgresql.
    /// Permette l'esecuzione di query parameriche e non (anche se ne é sconsigliato l'uso)
    /// </summary>
    public sealed class ADO : IDisposable
    {
        /// <summary>
        /// Nome del Database a cui connettersi.
        /// </summary>
        private String DB;

        /// <summary>
        /// Flag che indica se si é attualmente connessi al DB.
        /// </summary>
        public Boolean Connected;

        /// <summary>
        /// Permette di costruire la ConnectionString.
        /// </summary>
        private NpgsqlConnectionStringBuilder ConnectionString;

        /// <summary>
        /// Rappresenta la connessione al Database.
        /// </summary>
        private NpgsqlConnection Connection;

        /// <summary>
        /// Rappresenta un oggetto per eseguire comandi in Script SQL sulla connessione attiva.
        /// </summary>
        public NpgsqlCommand Command;

        /// <summary>
        /// The port number for the PostgreSql Server
        /// </summary>
        private const UInt16 POSTGRES_PORT = 5432;


        ///<summary>
        ///Crea una nuova Istanza della Classe ADO per la Connessione a un Database MySQL
        ///</summary>
        ///<param name="Path"> Percorso Assoluto del File di DB</param>
        public ADO(String DB)
        {
            this.DB = DB;

            ConnectionString = new NpgsqlConnectionStringBuilder();
            ConnectionString.Host = "localhost"; //SERVER_IP HERE;
            ConnectionString.Port = POSTGRES_PORT;
            ConnectionString.Database = DB;
            //ConnectionString.SearchPath = DEFAULT_SCHEMA;
            ConnectionString.SslMode = SslMode.Disable;
            ConnectionString.Username = "USER";
            ConnectionString.Password = "PASSWORD";

            Connection = new NpgsqlConnection(ConnectionString.ConnectionString);
        }

        ///<summary>
        ///Crea a apre una Connessione al DB Specificato
        ///</summary>
        public void ConnectDB()
        {
            if (!Connected)
            {
                try
                {
                    Connection.Open();
                    Command = Connection.CreateCommand();
                    Command.CommandType = CommandType.Text;
                    this.Connected = true;
                }
                catch (NpgsqlException ex) { this.Connected = false; }
            }
        }

        ///<summary>
        ///Chiude una Connessione al Database Precedentemente Aperta.
        ///</summary>
        public void CloseConnection()
        {
            if (Connected)
                Connection.Close();
            Connected = false;
        }

        ///<summary>
        ///Esegue una Query avente Codice Specificato, aggiungendo i parametri per una maggior sicurezza.
        ///</summary>
        ///<param name="SQL"> Frammento di Codice SQL della Query da eseguire</param>
        ///<param name="Parameters">
        ///    Un vettore di Parameter composte da : il nome del parametro, il tipo del parametro, il valore del parametro.
        ///    Non vengono fatti controlli di coerenza. Se null o vuoto non vengono aggiunti parametri, col rischio di un eventuale 
        ///    SQL Injection.
        ///</param>
        ///<example>
        ///<code>
        ///    List<Parameter> Params = new List<>();
        ///    //non funzionerá poiche i parametri vengono gestiti.
        ///    Params.Add(SQLParameter.Create("@nomeParametro", NpgsqlDbType.Char, "' OR 1=1; DROP TABLE Utenti; --ANAL!"));
        ///    int result = this.ExecuteQuery("SELECT * FROM Table WHERE column = @nomeParametro", Params); 
        ///    if(result == null)
        ///    {
        ///        //handle error...    
        ///    }
        ///</code>
        ///</example>
        ///<returns>Un Oggetto DataTable contente i valori ritornati dalla Query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Controllare l'eventuale vulnerabilità di sicurezza delle query SQL")]
        public DataTable ExecuteQuery(String SQL, List<SQLParameter> Parameters)
        {
            DataTable ret;
            this.ConnectDB();
            if (Connected)
            {
                Command.CommandText = SQL;
                try
                {
                    if (Parameters != null)
                    {
                        foreach (SQLParameter Parameter in Parameters)
                        {
                            Command.Parameters.Add(new NpgsqlParameter(Parameter.Name, Parameter.DbType));
                            Command.Parameters[Parameter.Name].Value = Parameter.Value;
                        }
                        Command.Prepare();
                    }

                    using (NpgsqlDataReader r = Command.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            ret = new DataTable();
                            ret.Load(r);
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    ret = null;
                }
                finally
                {
                    this.CloseConnection();
                }
            }
            else
            {
                ret = null;
            }
            return ret;
        }

        /// <summary>
        /// Esegue una Query avente Codice Specificato.
        /// </summary>
        /// <param name="SQL">Frammento di Codice SQL della Query da eseguire</param>
        /// <returns>Un Oggetto DataTable contente i valori ritornati dalla Query</returns>
        /// <example>
        /// <code>
        ///     DataTable result = this.ExecuteQuery("SELECT * FROM Table");
        ///     if(result == null) 
        ///     {
        ///         //handle error...
        ///     }
        /// </code>
        /// </example>
        [Obsolete("Non é piú sicuro, usare la versione che accetta parametri.", false)]
        public DataTable ExecuteQuery(String SQL)
        {
            return this.ExecuteQuery(SQL, null);
        }

        ///<summary>
        ///Esegue una NonQuery avente Codice Specificato (Inserimento, Modifica, Cancellazione di Record)
        ///</summary>
        ///<param name="SQL"> Frammento di Codice SQL della NonQuery da eseguire</param>
        ///<param name="Parameters">
        ///    Una collezione di Parameter composte da : il nome del parametro, il tipo del parametro, il valore del parametro.
        ///    Non vengono fatti controlli di coerenza. Se null o vuoto non vengono aggiunti parametri, col rischio di un eventuale 
        ///    SQL Injection.
        ///</param>
        ///<example>
        ///<code>
        ///    List<Parameter> Params = new List<>();
        ///    //non funzionerá poiche i parametri vengono gestiti.
        ///    Params.Add(SQLParameter.Create("@nomeParametro", NpgsqlDbType.Char, "' OR 1=1; DROP TABLE Utenti; --ANAL!"));
        ///    int result = this.ExecuteNonQuery("DELETE FROM Table WHERE column = @nomeParametro", Params);
        ///    if(result == -1)
        ///    {
        ///        //handle error...    
        ///    }
        ///</code>
        ///</example>
        ///<returns> il Numero di Righe Affette dalla NonQuery, -1 se è Avvenuto un Errore</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Controllare l'eventuale vulnerabilità di sicurezza delle query SQL")]
        public Int32 ExecuteNonQuery(String SQL, List<SQLParameter> Parameters)
        {
            Int32 ret = -1;
            this.ConnectDB();
            if (Connected)
            {
                Command.CommandText = SQL;
                try
                {
                    if (Parameters != null)
                    {
                        foreach (SQLParameter Parameter in Parameters)
                        {
                            Command.Parameters.Add(new NpgsqlParameter(Parameter.Name, Parameter.DbType));
                            Command.Parameters[Parameter.Name].Value = Parameter.Value;
                        }
                        Command.Prepare();
                    }
                    ret = Command.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    ret = -1;
                }
                finally
                {
                    this.CloseConnection();
                }
            }
            return ret;
        }
c
        ///<summary>
        ///Esegue una NonQuery avente Codice Specificato (Inserimento, Modifica, Cancellazione di Record)
        ///</summary>
        ///<param name="SQL"> Frammento di Codice SQL della NonQuery da eseguire</param>
        ///<example>
        ///<code>
        ///    int result = this.ExecuteNonQuery("DELETE FROM Table WHERE column = 'Value'");
        ///    if(result == -1)
        ///    {
        ///        //handle error...    
        ///    }
        ///</code>
        ///</example>
        ///<returns> il Numero di Righe Affette dalla NonQuery, -1 se è Avvenuto un Errore</returns>
        [Obsolete("Non é piú sicuro, usare la versione che accetta parametri.", false)]
        public Int32 ExecuteNonQuery(String SQL)
        {
            return this.ExecuteNonQuery(SQL, null);
        }

        ///<summary>
        /// Esegue una Query Scalar avente Codice Specificato e ne Restituisce il Valore.
        ///</summary>
        ///<param name="SQL"> Frammento di Codice SQL della Query Scalar da eseguire</param>
        ///<code>
        ///    List<Parameter> Params = new List<>();
        ///    //non funzionerá poiche i parametri vengono gestiti.
        ///    Params.Add(SQLParameter.Create("@nomeParametro", NpgsqlDbType.Char, "' OR 1=1; DROP TABLE Utenti; --ANAL!"));
        ///    Object result = this.ExecuteScalar("SELECT TOP 1 * FROM Table WHERE column = @nomeParametro", Params);
        ///    if(result == null)
        ///    {
        ///        //handle error...    
        ///    }
        ///</code>
        ///</example>
        ///<returns>Il Valore di Ritorno della Query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Controllare l'eventuale vulnerabilità di sicurezza delle query SQL")]
        public Object ExecuteScalar(String SQL, List<SQLParameter> Parameters)
        {
            Object ret = null;
            this.ConnectDB();
            if (Connected)
            {
                Command.CommandText = SQL;
                try
                {
                    if (Parameters != null)
                    {
                        foreach (SQLParameter Parameter in Parameters)
                        {
                            Command.Parameters.Add(new NpgsqlParameter(Parameter.Name, Parameter.DbType));
                            Command.Parameters[Parameter.Name].Value = Parameter.Value;
                        }
                        Command.Prepare();
                    }
                    ret = Command.ExecuteScalar();
                }
                catch (NpgsqlException ex)
                {
                    ret = null;
                }
                finally
                {
                    this.CloseConnection();
                }
            }
            return ret;
        }

        ///<summary>
        /// Esegue una Query Scalar avente Codice Specificato e ne Restituisce il Valore.
        ///</summary>
        ///<param name="SQL"> Frammento di Codice SQL della Query Scalar da eseguire</param>
        ///<example>
        ///<code>
        ///    Object result = this.ExecuteScalar("SELECT TOP 1 * FROM Table WHERE column = 'Value'");
        ///    if(result == null)
        ///    {
        ///        //handle error...    
        ///    }
        ///</code>
        ///</example>
        ///<returns>Il Valore di Ritorno della Query</returns>
        [Obsolete("Non é piú sicuro, usare la versione che accetta parametri.", false)]
        public Object ExecuteScalar(String SQL)
        {
            return this.ExecuteScalar(SQL, null);
        }

        /// <summary>
        /// Funzione di Controllo : Ritorna se due Istanze di ADO sono Equivalenti
        /// </summary>
        /// <param name="obj">Oggetto da Confrontare con l'Istanza Corrente</param>
        /// <returns>Ritorna se due Istanze di ADO sono Equivalenti</returns>
        public override bool Equals(object obj)
        {
            if (obj is ADO)
            {
                if (obj == this)
                    return true;
                else
                    return ((ADO)obj).ConnectionString.ConnectionString.Equals(this.ConnectionString.ConnectionString);
            }
            else return false;
        }

        /// <summary>
        /// Rilascia tutte le risorse allocate dall'Istanza.
        /// </summary>
        public void Dispose()
        {
            if (Command != null)
                Command.Dispose();
            if (Connection != null)
                Connection.Dispose();
        }

        /// <summary>
        /// Ottiene il valore di Hash per l'Istanza Corrente
        /// </summary>
        /// <returns>il valore di Hash per l'Istanza Corrente</returns>
        public override int GetHashCode()
        { return this.Connection.GetHashCode() + this.Command.GetHashCode() + this.DB.GetHashCode() + this.ConnectionString.GetHashCode() + base.GetHashCode() * 43; }

        /// <summary>
        /// Crea un'istanza String dell'oggetto Corrente
        /// </summary>
        /// <returns>un'istanza String dell'oggetti Corrente</returns>
        public override String ToString()
        { return this.ConnectionString.ConnectionString; }
    }
}

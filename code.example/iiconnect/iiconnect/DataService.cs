using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiconnect
{
	sealed class DataService
	{
		private System.Data.IDbConnection _connection = null;

		private System.Data.IDataReader _reader = null;

		public DataService()
		{
			string connection_string = System.Configuration.ConfigurationManager.AppSettings["CONNECTION_STRING"];
			System.Data.IDbConnection connection = new Ingres.Client.IngresConnection(connection_string);
			connection.Open();
			this._connection = connection;
		}

		private void AssertConnection()
		{
			if (this._connection == null)
				throw new Exception("No connection.");
		}

		public System.Data.IDataReader Select(string sql)
		{
			this.AssertConnection();

			Close(this._reader);
			this._reader = null;

			System.Data.IDbCommand command = this.CreateDefaultCommand();
			command.CommandText = sql;
			this._reader = command.ExecuteReader();
			return this._reader;
		}

		public System.Data.IDataReader Select(string sql, params object[] parameters)
		{
			this.AssertConnection();

			Close(this._reader);
			this._reader = null;

			System.Data.IDbCommand command = this.CreateDefaultCommand();
			command.CommandText = sql;
			PrepareParameters(command, parameters);
			this._reader = command.ExecuteReader();
			return this._reader;
		}

		public int Execute(string sql)
		{
			this.AssertConnection();

			System.Data.IDbCommand command = this.CreateDefaultCommand();
			command.CommandText = sql;
			return command.ExecuteNonQuery();
		}

		public int Execute(string sql, params object[] parameters)
		{
			this.AssertConnection();

			System.Data.IDbCommand command = this.CreateDefaultCommand();
			command.CommandText = sql;
			PrepareParameters(command, parameters);
			return command.ExecuteNonQuery();
		}

		private static void PrepareParameters(System.Data.IDbCommand command, params object[] parameters)
		{
			foreach (object unknown in parameters)
			{
				System.Data.IDbDataParameter p = command.CreateParameter();
				p.Value = unknown;
				command.Parameters.Add(p);
			}
		}

		private System.Data.IDbCommand CreateDefaultCommand()
		{
			System.Data.IDbCommand command = this._connection.CreateCommand();
			command.CommandTimeout = 30;
			command.CommandType = System.Data.CommandType.Text;
			return command;
		}

		public void Close()
		{
			Close(this._reader);
			this._reader = null;

			Close(this._connection);
			this._connection = null;
		}

		private static void Close(System.Data.IDataReader reader)
		{
			if (reader == null)
				return;
			reader.Close();
		}

		private static void Close(System.Data.IDbConnection reader)
		{
			if (reader == null)
				return;
			reader.Close();
		}
	}
}

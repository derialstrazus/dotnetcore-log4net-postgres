using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore_log4net_webapi
{
	public class CustomAppender : AdoNetAppender
	{
		public new string ConnectionString
		{
			get
			{
				return base.ConnectionString;
			}
			set
			{
				base.ConnectionString = "Host=localhost;Port=5432;Database=cocktails;Username=postgres;Password=postgres;";
			}
		}

		protected override void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
		{
            //ConnectionString = "Host=localhost;Port=5432;Database=cocktails;Username=postgres;Password=postgres;";

            if (CommandText != null && CommandText.Trim() != "")
			{
				using (IDbCommand dbCmd = Connection.CreateCommand())
				{
					// Set the command string
					dbCmd.CommandText = CommandText;

					// Set the command type
					dbCmd.CommandType = CommandType;
					// Send buffer using the prepared command object
					if (dbTran != null)
					{
						dbCmd.Transaction = dbTran;
					}

					// clear parameters that have been set
					dbCmd.Parameters.Clear();

					// Set the parameter values
					foreach (AdoNetAppenderParameter param in m_parameters)
						param.Prepare(dbCmd);

					// prepare the command, which is significantly faster
					dbCmd.Prepare();

					// run for all events
					foreach (LoggingEvent e in events)
					{
						// Set the parameter values
						foreach (AdoNetAppenderParameter param in m_parameters)
							param.FormatValue(dbCmd, e);

						// Execute the query
						dbCmd.ExecuteNonQuery();
					}
				}
			}
		}
	}
}
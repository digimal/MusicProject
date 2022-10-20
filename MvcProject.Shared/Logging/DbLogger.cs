using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MvcProject.Shared.Logging
{
    public class DbLogger : AbstractLocalLogger
    {
        static DbLogger _dbLogger;

        private DbLogger(string connectionString, string tableName)
        {
            _log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MSSqlServer(connectionString, tableName,
                    columnOptions: ConfigureColumnOptions()
                    ).CreateLogger();   
        }

        private ColumnOptions ConfigureColumnOptions()
        {

            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Properties);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.Exception);

            colOptions.AdditionalDataColumns = new Collection<DataColumn>
            {
                new DataColumn{DataType = typeof(Int32), ColumnName = "UserId"},
            };
            return colOptions;
        }

        public static DbLogger Create(string connectionString = @"Server=DGIEVSKY\SQLEXPRESS; Initial Catalog=MusicProject; Integrated Security=True", string tableName = "Logs")
        {
            return _dbLogger ?? (_dbLogger = new DbLogger(connectionString, tableName));
        }

    }
}

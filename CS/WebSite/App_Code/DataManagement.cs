using System.Data;
using System.Data.OleDb;
using System.Web;

public static class DataManagement
{
    public static DataSet RetrieveDataCache() {
        if(HttpContext.Current.Session["tasklist"] == null) {
            using(OleDbConnection connection = new OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString)) {
                OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM TaskList", connection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet("dataSet");

                dataAdapter.Fill(dataSet);
                dataSet.Tables[0].Constraints.Add("IDPK", dataSet.Tables[0].Columns["ID"], true);
               
                HttpContext.Current.Session["tasklist"] = dataSet;
                connection.Close();
            }
        }

        return (DataSet)HttpContext.Current.Session["tasklist"];
    }

    public static void UpdateDatabase() {
        if(HttpContext.Current.Session["tasklist"] == null)
            return;
        
        using(OleDbConnection connection = new OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString)) {
            OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM TaskList", connection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(selectCommand);
            OleDbCommandBuilder commandBuilder = PrepareCommandBuilder(dataAdapter);
            DataSet dataSet = (DataSet)HttpContext.Current.Session["tasklist"];

            dataAdapter.Update(dataSet);
            dataSet.AcceptChanges();
        }
    }

    public static void RestoreDatabase() {
        DataSet dataSet = RetrieveDataCache();

        using(OleDbConnection connection = new OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DatabaseCopyConnectionString"].ConnectionString)) {
            OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM TaskList", connection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(selectCommand);
            OleDbCommandBuilder commandBuilder = PrepareCommandBuilder(dataAdapter);

            dataAdapter.AcceptChangesDuringFill = false;
            //dataAdapter.FillLoadOption = LoadOption.OverwriteChanges;
            dataAdapter.Fill(dataSet);
            //DataRowState rowState = dataSet.Tables[0].Rows[1].RowState;

            connection.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            dataAdapter.Update(dataSet);
            dataSet.AcceptChanges(); 
        }
    }

    private static OleDbCommandBuilder PrepareCommandBuilder(OleDbDataAdapter dataAdapter) {
        OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

        commandBuilder.QuotePrefix = "[";
        commandBuilder.QuoteSuffix = "]";
        commandBuilder.RefreshSchema();

        return commandBuilder;
    }

}

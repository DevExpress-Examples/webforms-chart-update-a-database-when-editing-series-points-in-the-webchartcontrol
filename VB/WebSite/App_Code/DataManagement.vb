Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Web

Public NotInheritable Class DataManagement
	Private Sub New()
	End Sub
	Public Shared Function RetrieveDataCache() As DataSet
		If HttpContext.Current.Session("tasklist") Is Nothing Then
			Using connection As New OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("DatabaseConnectionString").ConnectionString)
				Dim selectCommand As New OleDbCommand("SELECT * FROM TaskList", connection)
				Dim dataAdapter As New OleDbDataAdapter(selectCommand)
				Dim dataSet As New DataSet("dataSet")

				dataAdapter.Fill(dataSet)
				dataSet.Tables(0).Constraints.Add("IDPK", dataSet.Tables(0).Columns("ID"), True)

				HttpContext.Current.Session("tasklist") = dataSet
				connection.Close()
			End Using
		End If

		Return CType(HttpContext.Current.Session("tasklist"), DataSet)
	End Function

	Public Shared Sub UpdateDatabase()
		If HttpContext.Current.Session("tasklist") Is Nothing Then
			Return
		End If

		Using connection As New OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("DatabaseConnectionString").ConnectionString)
			Dim selectCommand As New OleDbCommand("SELECT * FROM TaskList", connection)
			Dim dataAdapter As New OleDbDataAdapter(selectCommand)
			Dim commandBuilder As OleDbCommandBuilder = PrepareCommandBuilder(dataAdapter)
			Dim dataSet As DataSet = CType(HttpContext.Current.Session("tasklist"), DataSet)

			dataAdapter.Update(dataSet)
			dataSet.AcceptChanges()
		End Using
	End Sub

	Public Shared Sub RestoreDatabase()
		Dim dataSet As DataSet = RetrieveDataCache()

		Using connection As New OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("DatabaseCopyConnectionString").ConnectionString)
			Dim selectCommand As New OleDbCommand("SELECT * FROM TaskList", connection)
			Dim dataAdapter As New OleDbDataAdapter(selectCommand)
			Dim commandBuilder As OleDbCommandBuilder = PrepareCommandBuilder(dataAdapter)

			dataAdapter.AcceptChangesDuringFill = False
			'dataAdapter.FillLoadOption = LoadOption.OverwriteChanges;
			dataAdapter.Fill(dataSet)
			'DataRowState rowState = dataSet.Tables[0].Rows[1].RowState;

			connection.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("DatabaseConnectionString").ConnectionString

			dataAdapter.Update(dataSet)
			dataSet.AcceptChanges()
		End Using
	End Sub

	Private Shared Function PrepareCommandBuilder(ByVal dataAdapter As OleDbDataAdapter) As OleDbCommandBuilder
		Dim commandBuilder As New OleDbCommandBuilder(dataAdapter)

		commandBuilder.QuotePrefix = "["
		commandBuilder.QuoteSuffix = "]"
		commandBuilder.RefreshSchema()

		Return commandBuilder
	End Function

End Class

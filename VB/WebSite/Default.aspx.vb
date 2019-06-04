Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports DevExpress.XtraCharts

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		DevExpress.Web.ASPxWebControl.RegisterBaseScript(Me)

		If Page.Request.Form("__EVENTTARGET") = btnResoreData.ID Then
			DataManagement.RestoreDatabase()
		End If

		BindChartToData(DataManagement.RetrieveDataCache())

		If (Not IsPostBack) Then
			cbLabels.Checked = WebChartControl1.Series(0).Label.Visible
			cbFixedRange.Checked = Not(CType(WebChartControl1.Diagram, XYDiagram)).AxisY.Range.Auto
		End If

		WebChartControl1.Series(0).Label.Visible = cbLabels.Checked
		PrepareAxisRange()
	End Sub

	Private Sub BindChartToData(ByVal dataSet As DataSet)
		' Create a series
		Dim series As New Series("TaskList", ViewType.Gantt)

		series.ArgumentScaleType = ScaleType.Qualitative
		series.ValueScaleType = ScaleType.DateTime

		' Bind series to data
		series.DataSource = dataSet.Tables(0)
		series.ArgumentDataMember = "Task"
		series.ValueDataMembers.AddRange(New String() { "Start", "End" })
		WebChartControl1.Series.Add(series)

		' Adjust series label options
		CType(series.Label, RangeBarSeriesLabel).Position = RangeBarLabelPosition.Inside
		series.Label.ResolveOverlappingMode = ResolveOverlappingMode.Default
		series.PointOptions.ValueDateTimeOptions.Format = DateTimeFormat.Custom
		series.PointOptions.ValueDateTimeOptions.FormatString = "MM'/'dd'/'yyyy"

		' Adjust axes options
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.DateTimeOptions.Format = DateTimeFormat.Custom
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.DateTimeOptions.FormatString = "MM'/'dd"
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.Label.Staggered = True
		CType(WebChartControl1.Diagram, XYDiagram).AxisX.GridLines.Visible = True

		CType(WebChartControl1.Diagram, XYDiagram).AxisY.GridSpacingAuto = False
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.GridSpacing = 1

		CType(WebChartControl1.Diagram, XYDiagram).AxisY.Range.SideMarginsEnabled = False
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.DateTimeGridAlignment = DateTimeMeasurementUnit.Day

		WebChartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right

		WebChartControl1.DataBind()
	End Sub

	Protected Sub WebChartControl1_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.XtraCharts.Web.CustomCallbackEventArgs)
		Dim parameters() As String = e.Parameter.Split(";"c)
		Dim argument As String = parameters(0)
		Dim valueIndex As Integer = Convert.ToInt32(parameters(1))
		Dim [date] As DateTime = Convert.ToDateTime(parameters(2), CultureInfo.InvariantCulture)
		Dim taskList As DataTable = DataManagement.RetrieveDataCache().Tables(0)

		For i As Integer = 0 To taskList.Rows.Count - 1
			If taskList.Rows(i)("Task").ToString() = argument Then
				If valueIndex = 0 Then
					taskList.Rows(i)("Start") = [date]
				ElseIf valueIndex = 1 Then
					taskList.Rows(i)("End") = [date]
				Else
					taskList.Rows(i)("Start") = [date]
					taskList.Rows(i)("End") = Convert.ToDateTime(parameters(3), CultureInfo.InvariantCulture)
				End If

				Dim start As DateTime = Convert.ToDateTime(taskList.Rows(i)("Start"))
				Dim [end] As DateTime = Convert.ToDateTime(taskList.Rows(i)("End"))

				If start > [end] Then
					taskList.Rows(i)("Start") = [end]
					taskList.Rows(i)("End") = start
				End If

				DataManagement.UpdateDatabase()
				Exit For
			End If
		Next i

	End Sub

	Protected Sub cbFixedRange_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		CType(WebChartControl1.Diagram, XYDiagram).AxisY.Range.Auto = cbFixedRange.Checked

		If cbFixedRange.Checked Then
			Session("rangeMin") = (CType(WebChartControl1.Diagram, XYDiagram)).AxisY.Range.MinValue
			Session("rangeMax") = (CType(WebChartControl1.Diagram, XYDiagram)).AxisY.Range.MaxValue
		End If
	End Sub

	Private Sub PrepareAxisRange()
		If IsPostBack AndAlso cbFixedRange.Checked Then
			If Session("rangeMin") IsNot Nothing AndAlso Session("rangeMax") IsNot Nothing Then
				CType(WebChartControl1.Diagram, XYDiagram).AxisY.Range.MinValue = Convert.ToDateTime(Session("rangeMin"))
				CType(WebChartControl1.Diagram, XYDiagram).AxisY.Range.MaxValue = Convert.ToDateTime(Session("rangeMax"))
			End If
		End If
	End Sub

End Class
using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using DevExpress.XtraCharts;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        DevExpress.Web.ASPxClasses.ASPxWebControl.RegisterBaseScript(this);

        if(Page.Request.Form["__EVENTTARGET"] == btnResoreData.ID)
            DataManagement.RestoreDatabase();

        BindChartToData(DataManagement.RetrieveDataCache());

        if(!IsPostBack) {
            cbLabels.Checked = WebChartControl1.Series[0].Label.Visible;
            cbFixedRange.Checked = !((XYDiagram)WebChartControl1.Diagram).AxisY.Range.Auto;
        }
        
        WebChartControl1.Series[0].Label.Visible = cbLabels.Checked;
        PrepareAxisRange();
    }

    private void BindChartToData(DataSet dataSet) {
        // Create a series
        Series series = new Series("TaskList", ViewType.Gantt);

        series.ArgumentScaleType = ScaleType.Qualitative;
        series.ValueScaleType = ScaleType.DateTime;

        // Bind series to data
        series.DataSource = dataSet.Tables[0];
        series.ArgumentDataMember = "Task";
        series.ValueDataMembers.AddRange(new string[] { "Start", "End" });
        WebChartControl1.Series.Add(series);

        // Adjust series label options
        ((RangeBarSeriesLabel)series.Label).Position = RangeBarLabelPosition.Inside;
        series.Label.ResolveOverlappingMode = ResolveOverlappingMode.Default;
        series.PointOptions.ValueDateTimeOptions.Format = DateTimeFormat.Custom;
        series.PointOptions.ValueDateTimeOptions.FormatString = "MM'/'dd'/'yyyy";

        // Adjust axes options
        ((XYDiagram)WebChartControl1.Diagram).AxisY.DateTimeOptions.Format = DateTimeFormat.Custom;
        ((XYDiagram)WebChartControl1.Diagram).AxisY.DateTimeOptions.FormatString = "MM'/'dd";
        ((XYDiagram)WebChartControl1.Diagram).AxisY.Label.Staggered = true;
        ((XYDiagram)WebChartControl1.Diagram).AxisX.GridLines.Visible = true;

        ((XYDiagram)WebChartControl1.Diagram).AxisY.GridSpacingAuto = false;
        ((XYDiagram)WebChartControl1.Diagram).AxisY.GridSpacing = 1;

        ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.SideMarginsEnabled = false;
        ((XYDiagram)WebChartControl1.Diagram).AxisY.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
        ((XYDiagram)WebChartControl1.Diagram).AxisY.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;

        WebChartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;

        WebChartControl1.DataBind();
    }

    protected void WebChartControl1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)     {
        string[] parameters = e.Parameter.Split(';');
        string argument = parameters[0];
        int valueIndex = Convert.ToInt32(parameters[1]);
        DateTime date = Convert.ToDateTime(parameters[2], CultureInfo.InvariantCulture);
        DataTable taskList = DataManagement.RetrieveDataCache().Tables[0];

        for(int i = 0; i < taskList.Rows.Count; i++) {
            if(taskList.Rows[i]["Task"].ToString() == argument) {
                if(valueIndex == 0)
                    taskList.Rows[i]["Start"] = date;
                else if(valueIndex == 1)
                    taskList.Rows[i]["End"] = date;
                else {
                    taskList.Rows[i]["Start"] = date;
                    taskList.Rows[i]["End"] = Convert.ToDateTime(parameters[3], CultureInfo.InvariantCulture);
                }

                DateTime start = Convert.ToDateTime(taskList.Rows[i]["Start"]);
                DateTime end = Convert.ToDateTime(taskList.Rows[i]["End"]);

                if(start > end) {
                    taskList.Rows[i]["Start"] = end;
                    taskList.Rows[i]["End"] = start;
                }

                DataManagement.UpdateDatabase();
                break;
            }
        }

    }

    protected void cbFixedRange_CheckedChanged(object sender, EventArgs e) {
        ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.Auto = cbFixedRange.Checked;

        if(cbFixedRange.Checked) {
            Session["rangeMin"] = ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.MinValue;
            Session["rangeMax"] = ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.MaxValue;
        }
    }

    private void PrepareAxisRange() {
        if(IsPostBack && cbFixedRange.Checked) {
            if(Session["rangeMin"] != null && Session["rangeMax"] != null) {
                ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.MinValue = Convert.ToDateTime(Session["rangeMin"]);
                ((XYDiagram)WebChartControl1.Diagram).AxisY.Range.MaxValue = Convert.ToDateTime(Session["rangeMax"]);
            }
        }
    }

}
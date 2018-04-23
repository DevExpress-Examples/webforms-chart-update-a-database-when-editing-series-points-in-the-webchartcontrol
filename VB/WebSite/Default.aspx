<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.XtraCharts.v9.3.Web, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v9.3, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v9.3, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.3, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="ToolTipStyles.css" rel="stylesheet" type="text/css" />
	<script src="ClientScripts.js" type="text/javascript"></script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<dx:ASPxCheckBox ID="cbLabels" runat="server" AutoPostBack="True" Text="Labels">
		</dx:ASPxCheckBox>
		<dx:ASPxCheckBox ID="cbFixedRange" runat="server" AutoPostBack="True" Text="Fixed range"
			OnCheckedChanged="cbFixedRange_CheckedChanged">
		</dx:ASPxCheckBox>
		<dx:ASPxButton ID="btnResoreData" runat="server" CommandArgument="ResoreData"
			Text="Restore data from a backup database"
			UseSubmitBehavior="False">
		</dx:ASPxButton>
		<p></p>
		<dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Height="300px" Width="800px"
			ClientInstanceName="chart" EnableClientSideAPI="True" EnableClientSidePointToDiagram="True"
			EnableViewState="False" SaveStateOnCallbacks="False" OnCustomCallback="WebChartControl1_CustomCallback"
			ShowLoadingPanel="False">
			<SeriesTemplate LabelTypeName="SideBySideBarSeriesLabel" PointOptionsTypeName="PointOptions"
				SeriesViewTypeName="SideBySideBarSeriesView">
				<View HiddenSerializableString="to be serialized">
				</View>
				<Label HiddenSerializableString="to be serialized" LineVisible="True">
					<FillStyle FillOptionsTypeName="SolidFillOptions">
						<Options HiddenSerializableString="to be serialized"></Options>
					</FillStyle>
				</Label>
				<PointOptions HiddenSerializableString="to be serialized">
				</PointOptions>
				<LegendPointOptions HiddenSerializableString="to be serialized">
				</LegendPointOptions>
			</SeriesTemplate>
			<FillStyle FillOptionsTypeName="SolidFillOptions">
				<Options HiddenSerializableString="to be serialized"></Options>
			</FillStyle>
		</dxchartsui:WebChartControl>
		<div id="draggingToolTip" class="tooltip">
			&lt; Drag the task &gt;
		</div>
	</div>
	</form>
</body>
</html>
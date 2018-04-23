<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.XtraCharts.v13.1.Web, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
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
            ShowLoadingPanel="False" CrosshairEnabled="False">
            <SeriesTemplate>
                <ViewSerializable>
<cc1:SideBySideBarSeriesView HiddenSerializableString="to be serialized">
                </cc1:SideBySideBarSeriesView>
</ViewSerializable>
                <LabelSerializable>
<cc1:SideBySideBarSeriesLabel HiddenSerializableString="to be serialized" LineVisible="True">
                    <FillStyle >
                        <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
                    </FillStyle>
                </cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
                <PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                </cc1:PointOptions>
</PointOptionsSerializable>
                <LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                </cc1:PointOptions>
</LegendPointOptionsSerializable>
            </SeriesTemplate>
            <FillStyle >
                <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
            </FillStyle>
        </dxchartsui:WebChartControl>
        <div id="draggingToolTip" class="tooltip">
            &lt; Drag the task &gt;
        </div>
    </div>
    </form>
</body>
</html>

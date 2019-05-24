window.onload = function() {
    ASPx.Evt.AttachEventToDocument("mousedown", OnMouseDown);
    ASPx.Evt.AttachEventToDocument("mouseup", OnMouseUp);
    ASPx.Evt.AttachEventToDocument("mousemove", OnMouseMove);
    ASPx.Evt.PreventElementDragAndSelect(chart.GetMainDOMElement(), false);
}

var seriesPoint = null;
var coords = null;
var draggingIndex = -1;
var draggingOffsetTime = null;
var dragging = false;


// Event handlers

function OnMouseDown(evt) {
    if (ASPx.Evt.IsLeftButtonPressed(evt))
        dragging = UpdateDraggingFlag(evt);
    
    if (dragging) {
        ASPxClientUtils.SetAbsoluteX(document.getElementById('draggingToolTip'), ASPxClientUtils.GetEventX(evt) + 5);
        ASPxClientUtils.SetAbsoluteY(document.getElementById('draggingToolTip'), ASPxClientUtils.GetEventY(evt) + 10);
        document.getElementById('draggingToolTip').style.visibility = 'visible'; 

        UpdateDraggingText();
    }

    //if (!__aspxIE) {
        evt.preventDefault();
        return false;
    //}
}

function OnMouseUp(evt) {
    if (dragging) {
        dragging = false;
        var parameter = seriesPoint.argument + ';' + draggingIndex + ';' +
            (draggingIndex != 2 ? GetDateString(seriesPoint.values[draggingIndex]) : 
            GetDateString(seriesPoint.values[0]) + ';' + GetDateString(seriesPoint.values[1]));    
        
        chart.PerformCallback(parameter);
        document.getElementById('draggingToolTip').style.visibility = 'hidden';  
    }
}

function OnMouseMove(evt) {
    if (UpdateDraggingFlag(evt)) {
        if (draggingIndex == 2) {
            chart.SetCursor('pointer');
        }
        else
            chart.SetCursor('e-resize');
        
        if (dragging && draggingIndex >= 0 && !coords.IsEmpty()) {
            ASPxClientUtils.SetAbsoluteX(document.getElementById('draggingToolTip'), ASPxClientUtils.GetEventX(evt) + 5);
            ASPxClientUtils.SetAbsoluteY(document.getElementById('draggingToolTip'), ASPxClientUtils.GetEventY(evt) + 10);

            if (draggingIndex < 2)
                seriesPoint.values[draggingIndex] = coords.dateTimeValue;
            else {
                var timeSpan = seriesPoint.values[1] - seriesPoint.values[0];
                seriesPoint.values[0] = new Date(coords.dateTimeValue.getTime() - draggingOffsetTime);
                seriesPoint.values[1] = new Date(coords.dateTimeValue.getTime() + timeSpan - draggingOffsetTime);
            }

            UpdateDraggingText();
        }
    }
    else
        chart.SetCursor('');
}

// Helper functions

function UpdateDraggingFlag(evt) {
    if (seriesPoint == null)
        draggingIndex = -1;

    var srcElement = ASPx.Evt.GetEventSource(evt);   
    if (chart.GetMainDOMElement() != srcElement.parentElement)
        return false;
   
    var x = ASPxClientUtils.GetEventX(evt) - ASPxClientUtils.GetAbsoluteX(srcElement);
    var y = ASPxClientUtils.GetEventY(evt) - ASPxClientUtils.GetAbsoluteY(srcElement);    
    var diagram = chart.GetChart().diagram;    
    coords = diagram.PointToDiagram(x, y);
    
    if (coords.IsEmpty())
        return dragging;        
        
    return dragging || BeginDrag(x, y);
}

function BeginDrag(x, y) {
    var hitInfo = chart.HitTest(x, y);
    
    seriesPoint = null;
    for (var i = 0; i < hitInfo.length; i++) {
        if (hitInfo[i].additionalObject != null) {            
            seriesPoint = hitInfo[i].additionalObject;
            draggingIndex = GetDraggingIndex();
            draggingOffsetTime = coords.dateTimeValue.getTime() - seriesPoint.values[0].getTime();

            return draggingIndex != -1;            
        }
    }
    return false;
}

function GetDraggingIndex() {
    var hitArgument = _aspxToUtcTime(coords.dateTimeValue).getTime();
    var seriesPointArgument1 = _aspxToUtcTime(seriesPoint.values[0]).getTime();
    var seriesPointArgument2 = _aspxToUtcTime(seriesPoint.values[1]).getTime();

    if (hitArgument == seriesPointArgument1) return 0;
    if (hitArgument == seriesPointArgument2) return 1;
    if (hitArgument > seriesPointArgument1 && hitArgument < seriesPointArgument2) return 2;

    return -1;
}

function UpdateDraggingText() {
    if (draggingIndex < 2)
        document.getElementById('draggingToolTip').innerHTML = '<span style=\'white-space:nowrap\'>' + GetDateString(coords.dateTimeValue) + '</span>';
    else {
        var timeSpan = seriesPoint.values[1] - seriesPoint.values[0];
        document.getElementById('draggingToolTip').innerHTML = '<span style=\'white-space:nowrap\'>' +
            GetDateString(new Date(coords.dateTimeValue.getTime() - draggingOffsetTime)) + ' - ' +
            GetDateString(new Date(coords.dateTimeValue.getTime() + timeSpan - draggingOffsetTime)) + '</span>';
    }
}

function GetDateString(date) {
    return (date.getUTCMonth() + 1) + "/" + date.getUTCDate() + "/" + date.getUTCFullYear();
}


var getTimeZoneOffset = function (date) {
    var utcFullYear = date.getUTCFullYear();
    var utcDate = new Date(utcFullYear, date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(),
        date.getUTCMinutes(), date.getUTCSeconds(), date.getUTCMilliseconds());
    if (utcFullYear < 100)
        utcDate.setFullYear(utcFullYear);
    return utcDate - date;
};
_aspxToUtcTime = function (date) {
    var result = new Date();
    result.setTime(date.valueOf() + getTimeZoneOffset(date));
    return result;
};
_aspxToLocalTime = function (date) {
    var result = new Date();
    result.setTime(date.valueOf() - getTimeZoneOffset(date));
    return result;
};
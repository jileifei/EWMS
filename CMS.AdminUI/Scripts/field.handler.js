function FieldHandler() {
    this.DataType = 1;// 数据类型 1=新闻 2=推荐数据
    this.ControlID = "selField";
    this.FieldType = 1; // 字段作用 1=排序 2=条件
    this.ConditonControlID = "selCondition"; // 如果设置条件字段时，需要同时设置该字段的条件类型
}
// 绑定字段列表
FieldHandler.prototype.Init = function (_DataType, _ControlID, _FieldType, _ConditonControlID) {
    this.DataType = _DataType;
    this.ControlID = _ControlID;
    this.FieldType = _FieldType;
    this.ConditonControlID = _ConditonControlID;
}
// 绑定字段列表
FieldHandler.prototype.BindFieldList = function () {
    var fieldType = this.FieldType;
    var dataType = this.DataType;
    var controlID = this.ControlID;
    $("#" + controlID).empty();
    $("#" + controlID).append('<option value="">请选择</option>');
    var thisObj = this;
    $.ajax({
        type: "POST",
        url: "/DataBlock/GetFieldList/",
        data: { DataType: dataType },
        dataType: 'json',
        async: false, 
        success: function (result) {
            if (result.msg == "") {
                var option = null;
                $.each(result.list, function (i, item) {
                    if (fieldType == 1) {// 排序
                        if (item.IsOrder) {
                            option = $('<option value="' + item.Field + '">' + item.FieldName + '</option>');
                        }
                    }
                    else if (fieldType == 2) {// 条件
                        if (item.IsWhere) {
                            option = $('<option value="' + item.Field + '">' + item.FieldName + '</option>');
                        }
                    }
                    else {
                        option = $('<option value="' + item.Field + '">' + item.FieldName + '</option>');
                    }
                    $("#" + controlID).append(option);
                });
                if (fieldType == 2) {
                    $("#" + controlID).change(function () {
                        thisObj.BindConditionList($(this).val(), result.list);
                    });
                }
            }
            else {
                alert(msg);
            }
        },
        error: function (x, e) {
            alert("server error!");
        }
    });
}

// 绑定字段列表
FieldHandler.prototype.BindConditionList = function (curfield, datalist) {
    var controlID = this.ConditonControlID;
    $("#" + controlID).empty();
    $("#" + controlID).append('<option value="">请选择</option>');
    var fieldType = 0; // 字段类型 1=字符串 2=数字 3=日期
    $.each(datalist, function (i, item) {
        if (item.Field == curfield) {
            fieldType = item.FieldType;
            return false;
        }
    });
    if (fieldType == 1)// 字符串
    {
        $("#" + controlID).append('<option value="=">等于</option>');
        $("#" + controlID).append('<option value="!=">不等于</option>');
        $("#" + controlID).append('<option value="like">包含</option>');
        $("#" + controlID).append('<option value="not like">不包含</option>');
        $("#" + controlID).append('<option value="is null">空</option>');
        $("#" + controlID).append('<option value="is not null">非空</option>');
    }
    else if (fieldType == 2)// 数字
    {
        $("#" + controlID).append('<option value="=">等于</option>');
        $("#" + controlID).append('<option value="!=">不等于</option>');
        $("#" + controlID).append('<option value=">">大于</option>');
        $("#" + controlID).append('<option value="<">小于</option>');
        $("#" + controlID).append('<option value=">=">大于等于</option>');
        $("#" + controlID).append('<option value="<=">小于等于</option>');
    }
    else if (fieldType == 3)// 日期
    {
        $("#" + controlID).append('<option value="=">等于</option>');
        $("#" + controlID).append('<option value="!=">不等于</option>');
        $("#" + controlID).append('<option value=">">大于</option>');
        $("#" + controlID).append('<option value="<">小于</option>');
        $("#" + controlID).append('<option value=">=">大于等于</option>');
        $("#" + controlID).append('<option value="<=">小于等于</option>');
        $("#" + controlID).append('<option value="week">本周</option>');
        $("#" + controlID).append('<option value="month">本月</option>');
        $("#" + controlID).append('<option value="year">本年</option>');
    }
    else if (fieldType == 4)// 布尔
    {
        $("#" + controlID).append('<option value="true">是</option>');
        $("#" + controlID).append('<option value="false">否</option>');
    }
}

// 绑定字段列表
FieldHandler.prototype.TranslateWhere = function (field, operator, value) {
    var whereSql = "";
    switch (operator) {
        case "week":
            whereSql = "DATEDIFF(week," + field + ",GETDATE())=0";
            break;
        case "month":
            whereSql = "DATEDIFF(month," + field + ",GETDATE())=0";
            break;
        case "year":
            whereSql = "DATEDIFF(year," + field + ",GETDATE())=0";
            break;
        case "is null":
            whereSql = field + " " + operator;
            break;
        case "is not null":
            whereSql = field + " " + operator;
            break;
        default:
            whereSql = field + " " + operator + " " + value;
            break;
    }
    return whereSql;
}
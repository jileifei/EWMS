var fieldHandler = null;
$(document).ready(function () {
    if (parseInt(PageID) == 0) {
        alert('请选择要编辑的分页设置信息');
        return false;
    }

    $("#selDataType").val(DataType);

    $("#hiddenWhereValue").val('');

    fieldHandler = new FieldHandler();
    fieldHandler.Init(1, 'selOrderField', 1);
    fieldHandler.BindFieldList();
    fieldHandler.Init(1, 'selFieldCondition', 2, "selCondition");
    fieldHandler.BindFieldList();
    InitOrder();
    InitWhere();
   
    // 添加排序
    $("#btnAddOrder").click(function () {
        var orderlistText = $("#selOrderField").find("option:selected").text() + " " + $("#selOrderType").find("option:selected").text();
        var orderlistVal = $("#selOrderField").val() + " " + $("#selOrderType").val();
        if ($.trim($("#selOrderField").val()) != "") {
            $("#selOrderList").append("<option value=\"" + orderlistVal + "\">" + orderlistText + "</option>");
        }
    })

    // 删除排序
    $("#btnDelOrder").click(function () {
        $.each($("#selOrderList option:selected"), function (i, own) {
            $(own).remove();
        })
    })

    // 添加查询条件
    $("#btnAddCondition").click(function () {
        var whereText = $("#selFieldCondition").find("option:selected").text() + " " + $("#selCondition").find("option:selected").text() + " " + $("#txtWhereValue").val();
        var whereVal = fieldHandler.TranslateWhere($("#selFieldCondition").val(), $("#selCondition").val(), $("#txtWhereValue").val());
        if ($.trim($("#selFieldCondition").val()) != "") {
            $("#selConditionList").append("<option value=\"" + whereVal + "\">" + whereText + "</option>");
            // 多个条件时保存条件联接方式
            if (document.getElementById("selConditionList").length > 0) {
                // 条件联接
                var whereJoin = $("input:radio[name='WhereJoin']:checked").val();
                var whereOldJoin = $("#hiddenWhereValue").val();
                if (whereOldJoin == "") {
                    $("#hiddenWhereValue").val(whereJoin);
                }
                else {
                    $("#hiddenWhereValue").val(whereOldJoin + "|" + whereJoin);
                }
            }
        }
    })

    // 删除查询条件
    $("#btnDelCondition").click(function () {
        $.each($("#selConditionList option"), function (i, own) {
            if ($(this).attr("selected")) {
                $(own).remove();
                var whereOper = $("#hiddenWhereValue").val();
                if ($.trim(whereOper) != "") {
                    var operArray = whereOper.split('|');
                    if (i == 0) {
                        operArray.remove(0);
                    }
                    else {
                        operArray.remove(i - 1);
                    }
                    $("#hiddenWhereValue").val(operArray.join('|'));
                }
            }
        })
    })

    // 保存提交
    $("#btnSave").click(function () {
        UpdatePagerSet();
    });
});

function InitOrder() {
    if (OrderByField != "") {
        var orderlist = OrderByField.split(',');
        for (var i = 0; i < orderlist.length; i++) {
            if ($.trim(orderlist[i]) != "") {
                var fieldinfo = orderlist[i].split(' ');
                if (fieldinfo.length == 2) {
                    var itemtext = GetSelectText('selOrderField', fieldinfo[0]) + " " + GetSelectText('selOrderType', fieldinfo[1]);
                    $("#selOrderList").append("<option value=\"" + orderlist[i] + "\">" + itemtext + "</option>");
                }
            }
        }
    }
}

function InitWhere() {
    if (WhereField != "") {
        MatchSQLOper(WhereField);
        WhereField = WhereField.replace(" AND ", "|").replace(" OR ", "|");
        var wherelist = WhereField.split('|');
        for (var i = 0; i < wherelist.length; i++) {
            if ($.trim(wherelist[i]) != "") {
                var fieldinfo = wherelist[i].split(' ');
                if (fieldinfo.length == 3) {
                    if (fieldinfo[0] == "LocationID") {
                        $("#selRecommendPosition").val(fieldinfo[2]);
                    }
                    else {
                        var itemtext = GetSelectText('selFieldCondition', fieldinfo[0]) + " " + GetSelectText('selCondition', fieldinfo[1]) + " " + GetSelectText('txtWhereValue', fieldinfo[2]);
                        $("#selConditionList").append("<option value=\"" + wherelist[i] + "\">" + itemtext + "</option>");
                    }
                }
            }
        }
    }
}

// 更新
function UpdatePagerSet() {
    var DataType = $("#selDataType").val();
    if (DataType == "") {
        alert('请选择数据类型');
        return false;
    }
    var OrderByField = getlistboxval("selOrderList");
    var Where = getlistboxval("selConditionList");
    Where = AnalyseWhere(Where);
    var PageSize = $("#txtPageSize").val();
    if ($.trim(PageSize) == "") {
        alert('请输入每页条数');
        return false;
    }
    if (isNaN(PageSize)) {
        alert('请输入正确的每页条数');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/PagerSet/Update/',
        data: { PageID: PageID, DataType: DataType, OrderBy: OrderByField, Where: Where, PageSize: PageSize },
        cache: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.result == "ok") {
                window.location.href = "/pagerset/";
            }
            else {
                alert(data.msg);
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function AnalyseWhere(wherelist) {
    var returnWhere = "";
    var whereArray = wherelist.split(',');
    var whereJoin = $("#hiddenWhereValue").val().split('|')
    for (i = 0; i < whereArray.length; i++) {
        if ($.trim(whereArray[i]) != "") {
            if (i > 0) {
                returnWhere = returnWhere + " " + whereJoin[i - 1] + " " + whereArray[i];
            }
            else {
                returnWhere = whereArray[i];
            }
        }
    }
    return returnWhere;
}

function getlistboxval(objname) {
    var intvalue = "";
    o = document.getElementById(objname);
    for (i = 0; i < o.length; i++) {
        intvalue += o.options[i].value + ",";
    }  
    return intvalue.substr(0, intvalue.length - 1);
}

function GetSelectText(selid, val) {
    var rettext = val;
    $("#" + selid).children("option").each(function () {
        if ($(this).val() == val) {
            rettext = $(this).text();
            return;
        }
    });
    return rettext;
}


// 得到
function MatchSQLOper(whereSQL) {
    var oReg = new RegExp("[\s\S]*(AND|OR)[\s\S]*", "img");
    if (whereSQL.match(oReg) == null) {
        $("#hiddenWhereValue").val(whereSQL);
    }
    else {
        $("#hiddenWhereValue").val(whereSQL.match(oReg).join("|"));
    }
}
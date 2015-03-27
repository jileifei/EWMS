var fieldHandler = null;
$(document).ready(function () {
    fieldHandler = new FieldHandler();
    fieldHandler.Init($("#selType").val(), 'selOrderField', 1);
    fieldHandler.BindFieldList();
    fieldHandler.Init($("#selType").val(), 'selFieldCondition', 2, "selCondition");
    fieldHandler.BindFieldList();
    // 绑定推荐位置下拉列表
    BindRecommendPositionList();
    $("#hiddenWhereValue").val('');
    // 选择模板
    $('#selTemplate').live('click', function (e) {
        e.preventDefault();
        var common = new CommonPage();
        common.SelTemplate(5);
    });
    if ($("#selType").val() == "2") {
        $("#trRecommendPositionList").show();
    }
    $("#selType").change(function () {
        fieldHandler.Init($(this).val(), 'selOrderField', 1);
        fieldHandler.BindFieldList();
        fieldHandler.Init($(this).val(), 'selFieldCondition', 2, "selCondition");
        fieldHandler.BindFieldList();
        if ($(this).val() == "2") {
            $("#trRecommendPositionList").show();
            $("#commonplate").hide();
        }
        else {
            $("#trRecommendPositionList").hide();
            $("#commonplate").show();
        }
    });
    
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
        //var whereText = $("#selFieldCondition").find("option:selected").text() + " " + $("#selCondition").find("option:selected").text() + " " + $("#txtWhereValue").val();
        var whereVal = fieldHandler.TranslateWhere($("#selFieldCondition").val(), $("#selCondition").val(), $("#txtWhereValue").val());
        //判断是否是多条件关联
        var whereJoin = "";
        if ($.trim($("#txtWhere").val()) != "") {
            whereJoin = $("input:radio[name='WhereJoin']:checked").val();
        }

        $("#txtWhere").val($.trim($("#txtWhere").val()) + " " + $.trim(whereJoin) + " " + $.trim(whereVal));
        $("#hiddenWhereValue").val($("#txtWhere").val());
    })

     

    // 保存提交
    $("#btnSave").click(function () {
        AddDataBlock();
    });
});

// 添加
function AddDataBlock() {
    var BlockName = $("#txtBlockName").val();
    if($.trim(BlockName) == "")
    {
        alert('请输入数据块名称');
        return false;
    }
    var EnName = $("#txtEnName").val();
    if($.trim(EnName) == "")
    {
        alert('请输入数据块英文名称');
        return false;
    }
    var Type = $("#selType").val();
    var RecommendWhere = "";
    var positionID = 0;
    if (Type == 2) {
        positionID = $("#selRecommendPosition").val();
        if (positionID == "") {
            alert('请选择推荐位置');
            return false;
        }
        RecommendWhere = "LocationID = " + positionID;
    } else {
        var TemplateID = $("#hiddenTemplateID").val();
        if (parseInt(TemplateID) == 0) {
            alert('请选择模板');
            return false;
        } else {
            positionID = TemplateID;
        }
    }
    var OrderByField = getlistboxval("selOrderList");
    var Where = $("#txtWhere").val();
    var RowCount = $("#txtRowCount").val();
    if(isNaN(RowCount))
    {
        alert('请输入正确的记录条数');
        return false;
    }
    
    var Note = $("#txtNote").val();

    $.ajax({
        type: 'POST',
        url: '/DataBlock/Create/',
        data: { BlockName: BlockName, EnName: EnName, Type: Type, OrderByField: OrderByField, Where: Where, RowCount: RowCount, TemplateID: positionID, Note: Note },
        cache: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.result == "ok") {
                alert("新增数据块成功");
                window.location.href = "/datablock/";
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

// 绑定推荐位置列表
function BindRecommendPositionList() {
    $("#selRecommendPosition").empty();
    $("#selRecommendPosition").append('<option value="">请选择推荐位置</option>');
    var thisObj = this;
    $.ajax({
        type: "POST",
        url: "/Position/GetPositionList/",
        data: {},
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result.msg == "") {
                var option = null;
                $.each(result.list, function (i, item) {
                    option = $("<option value='" + item.ID + "'>" + item.Name + "</option>");
                    //option.attr("value", item.ID);
                    //option.attr("text", item.Name);
                    $("#selRecommendPosition").append(option);
                });
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

function getlistboxval(objname) {
    var intvalue = "";
    o = document.getElementById(objname);
    for (i = 0; i < o.length; i++) {
        intvalue += o.options[i].value + ",";
    }
    return intvalue.substr(0, intvalue.length - 1);
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

function SetTemplateValue(tid, tname) {
    $("#hiddenTemplateID").val(tid);
    $("#txtTemplateName").val(tname);
}

function CloseSelTemplate() {
    $('#divSelTemplateVar').dialog('close');
}
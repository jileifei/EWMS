jQuery(document).ready(function () {
    if (SpecialID == 0) {
        alert("请选择专题");
        return false;
    }
    // 选择模板
    $('#selTemplate').click(function () {
        var common = new CommonPage();
        common.SelTemplate(4);
    });
    // init page
    PageInit(0);

    $("#aAddChannel").click(function () {
        $('#divDialog').show();
        $('#divDialog').attr("title", "专题栏目：添加栏目");
        $("#divDialog").dialog({
            height: 220,
            width: 400,
            modal: true,
            buttons: {
                "保存": function () {
                    var SubSpecialName = $("#txtSubSpecialName").val();
                    if ($.trim(SubSpecialName) == "") {
                        alert('请输入栏目名称');
                        return false;
                    }
                    var Url = $("#txtUrl").val();
                    var TemplateID = $("#hiddenTemplateID").val();
                    if (TemplateID == "") {
                        TemplateID = 0;
                    }
                    $.ajax({
                        type: 'POST',
                        url: '/Special/SpecialChannelCreate/',
                        data: { SubSpecialName: SubSpecialName, Url: Url, TemplateID: TemplateID,SpecialID:SpecialID },
                        cache: false,
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.result == "ok") {
                                $('#divDialog').dialog('close');
                                Clear();
                                PageInit(0);
                            }
                            else {
                                alert(data.msg);
                            }
                        },
                        error: function (xhr) {
                            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                        }
                    })
                },
                "取消": function () {
                    Clear();
                    $('#divDialog').dialog('close');
                }
            }
        });
    })
});

function PageInit(page) {
    var template = $("#listTemplate").html();
    /* Compile the template as a named template */
    $.template("TableTemplate", template);

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/Special/SpecialChannelServer", //请求的url
        data: { PageIndex: parseInt(page + 1), PageSize: 100, SpecialID: SpecialID },
        success: function (data) {
            $("#divPagination").empty();
            //分页事件
            $("#divPagination").pagination(data.TotalRecords, {
                prev_text: "上一页",
                next_text: "下一页",
                num_edge_entries: 2,
                num_display_entries: 5,
                items_per_page: 100,
                //回调
                callback: PageCallBack
            });
        }
    });
}

function PageCallBack(page) {
    $("#tbodyContent").empty();
    $.ajax({
        type: "post",
        dataType: "json",
        url: "/Special/SpecialChannelServer", //请求的url
        data: { PageIndex: parseInt(page + 1), PageSize: 100, SpecialID: SpecialID },
        success: function (data) {
            $.tmpl("TableTemplate", data.ItemList).appendTo("#tbodyContent");
        }
    });
}

function GetDate(jsondate) {
    if (jsondate != null && jsondate != "") {
        var date = new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", "")));
        return dateFormat(date, "yyyy-mm-dd HH:MM:ss");
    }
    else {
        return jsondate;
    }
}

function Clear() {
    $("#txtSubSpecialName").val('');
    $('#txtUrl').val('');
    $('#hiddenTemplateID').val('');
    $('#txtTemplateName').val('');
}

function EditInit(SpecialChannelID) {
    alert("edit");
    $.ajax({
        type: 'POST',
        url: '/Special/GetSpecialChannel/',
        data: { SpecialChannelID: SpecialChannelID },
        cache: false,
        success: function (data) {
            $("#txtSubSpecialName").val(data.SubSpecialName);
            $("#txtUrl").val(data.Url);
            $("#txtTemplateName").val(data.TemplateName);
            $("#hiddenTemplateID").val(data.TemplateID);
            EditSpecialChannelPost(SpecialChannelID);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
}

function EditSpecialChannelPost(SpecialChannelID) {
    $('#divDialog').show();
    $('#divDialog').attr("title", "专题栏目：修改栏目");
    $("#divDialog").dialog({
        height: 220,
        width: 400,
        modal: true,
        buttons: {
            "保存": function () {
                var SubSpecialName = $("#txtSubSpecialName").val();
                if ($.trim(SubSpecialName) == "") {
                    alert('请输入栏目名称');
                    return false;
                }
                var Url = $("#txtUrl").val();
                var TemplateID = $("#hiddenTemplateID").val();
                if (TemplateID == "") {
                    TemplateID = 0;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Special/UpdateSpecialChannel/',
                    data: { ID: SpecialChannelID, SubSpecialName: SubSpecialName, Url: Url, TemplateID: TemplateID, SpecialID: SpecialID },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $('#divDialog').dialog('close');
                            Clear();
                            PageInit(0);
                        }
                        else {
                            alert(data.msg);
                        }
                    },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    }
                })
            },
            "取消": function () {
                $('#divDialog').dialog('close');
            }
        }
    });
}

function DeleteSpecialChannel(SpecialChannelID) {
    if (confirm('您确认要删除当前栏目吗？')) {
        $.ajax({
            type: 'POST',
            url: '/Special/DelSpecialChannel/',
            data: { ID: SpecialChannelID },
            cache: false,
            success: function (data) {
                data = eval('(' + data + ')');
                if (data.result == "ok") {
                    PageInit(0);
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
}

function SetTemplateValue(tid, tname, type) {
    $("#hiddenTemplateID").val(tid);
    $("#txtTemplateName").val(tname);
}
// 关闭模板选择页面
function CloseSelTemplate() {
    $('#divSelTemplateVar').dialog('close');
}
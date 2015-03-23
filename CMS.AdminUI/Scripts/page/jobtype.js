function AddType() {
    $('#divDialog').show();
    $('#divDialog').dialog({
        title: '添加类型',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                if ($("#typeName").val() == "") {
                    $("#typeNameMsg").html("请填写类型名称");
                    return false;
                }
                $("#typeNameMsg").html("");
                if ($("#typeDesc").val() == "") {
                    $("#typeDescMsg").html("请填写类型描述");
                    return false;
                }
                $("#typeDescMsg").html("");
                var typeName = $("#typeName").val();
                var typeDesc = $("#typeDesc").val();
                $.ajax({
                    url: "/Job/EditType",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { id: 0, typeName: typeName, typeDesc: typeDesc },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    },
                    success: function (data) {
                        if (data == "success") {
                            //$.messager.alert("系统提示", "创建类型成功");
                            //PageInit();
                            ClearFrom();
                            $("#LastMsg").html("创建类型成功");
                        }
                        else if (data == "false") {
                            $.messager.alert("系统提示", "创建类型失败", "warning");
                        }
                    }
                });
            }
        }, {
            text: '取消',
            handler: function () {
                $('#divDialog').dialog('close');
                PageInit();
            }
        }]
    });
}

function EditInit(id) {
    $.ajax({
        type: 'POST',
        url: '/Job/GetTypeInfo/',
        data: { Id: id },
        cache: false,
        success: function (data) {
            $("#typeName").val(data.Name);
            $("#typeDesc").val(data.Description);
            EditType(id);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function EditType(id) {
    $('#divDialog').show();
    $('#divDialog').dialog({
        title: '修改类型',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                if ($("#typeName").val() == "") {
                    $("#typeNameMsg").html("请填写类型名称");
                    return false;
                }
                $("#typeNameMsg").html("");
                if ($("#typeDesc").val() == "") {
                    $("#typeDescMsg").html("请填写类型描述");
                    return false;
                }
                $("#typeDescMsg").html("");
                var typeName = $("#typeName").val();
                var typeDesc = $("#typeDesc").val();
                $.ajax({
                    url: "/Job/EditType",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { id: id, typeName: typeName, typeDesc: typeDesc },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    },
                    success: function (data) {
                        if (data == "success") {
                            //$.messager.alert("系统提示", "修改类型成功");
                            // var a=setTimeout("PageInit()",60000);
                            ClearFrom();
                            $("#LastMsg").html("修改类型成功");
                        }
                        else if (data == "false") {
                            $.messager.alert("系统提示", "修改类型失败", "warning");
                        }
                    }
                });
            }
        }, {
            text: '取消',
            handler: function () {
                $('#divDialog').dialog('close');
                PageInit();
            }
        }]
    });
}

function DelType(id) {
    $.messager.confirm('删除', '您确定要删当前的职位类型吗?', function (r) {
        if (r) {
            $.ajax({
                type: 'POST',
                url: '/Job/DeleteType/',
                data: { ID: id },
                cache: false,
                success: function (data) {
                    if (data == "success") {
                        PageInit();
                    }
                    else {
                        //alert(data.msg);
                    }
                },
                error: function (xhr) {
                    throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                }
            });
        }
    });
}

// pager list
function PageInit() {
    window.location.reload();
}

function ClearFrom() {
    $("#typeName").val("");
    $("#typeDesc").val("");
}
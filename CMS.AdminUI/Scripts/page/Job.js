$(function () {
    $('#fileList').datagrid({
        height: 600,
        url: '/Job/AjaxGetResume?id=' + pid,
        columns: [[
	            { field: 'JobName', title: '职位名称', width: 100, sortable: true },
                { field: 'Name', title: '应聘者', width: 50, sortable: true },
                { field: 'Mobile', title: '电话', width: 90, sortable: true },
                { field: 'Email', title: '邮箱', width: 100, sortable: true },
                { field: 'CreateTime', title: '应聘时间', width: 150, sortable: true },
               { field: 'Status', title: '状态', width: 80, sortable: true, formatter: function (value, rec) {
                   var ct = ViewStatus(value);
                   return ct;
               }
               },
               { field: 'ID', title: '操作', width: 100, align: 'center', rowspan: 2,
                   formatter: function (value, rec) {
                       var name=rec.Name;
                       var email=rec.Email;
                       return '<a href="/job/detail?id=' + value + '"><img src="/Content/img/icon_list_icon.gif" alt="查看详细" title="查看详细" width="16" height="16" /></a>&nbsp;&nbsp;'
                       + '<a href="javascript:void(0);" onclick="Audit(' + value + ');"><img src="/Content/img/icons/page_white_edit.png" alt="审核" title="审核" width="16" height="16" /></a>&nbsp;&nbsp;'
                       + '<a href="javascript:void(0);" onclick="SendMailInit(1,\''+name+'\',\''+email+'\',' + value + ')"><img src="/Content/img/mail.gif" alt="通知面试" title="通知面试" width="16" height="16" /></a>&nbsp;&nbsp;'
                       + '<a href="javascript:void(0);" onclick="Del(' + value + ');"><img src="/Content/img/cancel.png" alt="删除" title="删除" width="16" height="16" /></a>';
                   }
               }
				]],
        pagination: true,
        rownumbers: true
    });
    var p = $('#fileList').datagrid('getPager');
    if (p) {
        $(p).pagination({
            pageSize: 10,
            pageList: [5, 10, 15],
            beforePageText: '第',
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onBeforeRefresh: function () {
            }
        });
    }
});

function ViewStatus(value) {
    switch (value) {
        case "1":
            return "未处理";
            break;
        case "2":
            return "通过审核";
            break;
        case "3":
            return "未通过审核";
            break;
        case "4":
            return "已发信通知";
            break;
            case "5":
            return "已通知负责人";
            break;
    }
}

function Del(id) {
    $.messager.confirm('删除', '您确定要删这个应聘者的信息吗?', function (r) {
        if (r) {
            $.ajax({
                type: 'POST',
                url: '/Job/DeleteResume/',
                data: { ID: id },
                cache: false,
                success: function (data) {
                    if (data == "success") {
                        $.messager.alert("系统提示", "删除成功");
                        PageInit();
                    }
                    else {
                        $.messager.alert("系统提示", "删除失败");
                    }
                },
                error: function (xhr) {
                    throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                }
            });
        }
    });
}

function Audit(id) {
    $('#divDialog').show();
    $('#divDialog').dialog({
        title: '审核信息',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                var status = $("input[@type=radio][name=audit][checked]").val();
                $.ajax({
                    url: "/Job/UpdateResume",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { id: id, status: status },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    },
                    success: function (data) {
                        if (data == "success") {
                            $.messager.alert("系统提示", "审核成功");
                            PageInit();
                        }
                        else if (data == "failed") {
                            $.messager.alert("系统提示", "审核失败", "warning");
                        }
                    }
                });
            }
        }, {
            text: '取消',
            handler: function () {
                $('#divDialog').dialog('close');
            }
        }]
    });
}

function SendMailInit(ty,name,email,id) {
    $.ajax({
        type: 'POST',
        url: '/User/GetCurrentUser',
        cache: false,
        success: function (data) {
            $("#sendName").val(data.Email);
            SendMail(ty,name,email, id);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function SendMail(ty,name, email, id) {
    $("#receiveEmail").val(email);
    $("#receive").val(name);
    $('#MailDialog').show();
    $('#MailDialog').dialog({
        title: '发信',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                var mailTo = $("#receiveEmail").val();
                var title = $("#title").val();
                var content = $("#MailContent").val();
                $.ajax({
                    url: "/Job/SendMail",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: {ty:ty,id: id, mailTo: mailTo,title: title, content: content },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    },
                    success: function (data) {
                        if (data == "success") {
                            $.messager.alert("系统提示", "发信成功");
                        }
                        else if (data == "error") {
                            $.messager.alert("系统提示", "发信失败,请检查您的密码和收信人地址是否有误,", "warning");
                        }
                        else {
                            $.messager.alert(data);
                        }
                    }
                });
            }
        }, {
            text: '取消',
            handler: function () {
                $('#MailDialog').dialog('close');
            }
        }]
    });
}
function PageInit() {
    window.location.reload();
}




function SendMailInit(ty, id) {
    $.ajax({
        type: 'POST',
        url: '/User/GetCurrentUser',
        cache: false,
        success: function (data) {
            $("#sendName").val(data.Email);
            SendMail(ty, id);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function SendMail(ty, id) {
    $('#MailDialog').show();
    $('#MailDialog').dialog({
        title: '发信',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                var mailTo = $("#receive").val();
                var title = $("#title").val();
                var content = $("#MailContent").val();
                $.ajax({
                    url: "/Job/SendMail",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { ty: ty, id: id,mailTo: mailTo,title: title, content: content },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    },
                    success: function (data) {
                        if (data == "success") {
                            $.messager.alert("系统提示", "发信成功");
                            //PageInit();
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
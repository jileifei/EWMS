        $(function () {
            var checkID = "";
            $("#checkAll").click(function () {
                var flag = $(this).attr("checked");
                $("[name$='checkComment']:checkbox").each(function () {
                    $(this).attr("checked", flag);
                });
            })

            var $subcheck = $("[name$='checkComment']:checkbox");
            var $check = $("#checkAll");
            $subcheck.click(function () {
                var flag = true;
                checkID = $(this).val();
                $subcheck.each(function () {
                    if (!this.checked) {
                        flag = false;
                    }
                });
                $check.attr("checked", flag);
            });

            $("#deletedById").bind("click", function () {
                if (checkID == "") {
                    $.messager.alert("系统提示", "请选择要删除的记录", "warning");
                    return;
                }
                Deleted(checkID);
            });

            $("#AuditById").bind("click", function () {
                if (checkID == "") {
                    $.messager.alert("系统提示", "请选择要审核通过的记录", "warning");
                    return;
                }
                AuditComment(checkID);
            });
        })

        function Deleted(id) {
            $.messager.confirm('删除', '您确定要删除吗?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/FeedBack/Delete/",
                        type: 'POST',
                        data: { "ID": id },
                        success: function (request) {
                            if (request == "success") {
                                $.messager.alert("系统提示", "恭喜！您已经删除成功！", "warning");
                                location.href = "/FeedBack/?messageType=" + messageTypeID;
                            }
                            else (request == "false")
                            {
                                $.messager.alert("系统提示", "抱歉！您删除没有成功！", "warning");
                            }
                        }
                    })
                }
            });
        }

        function AuditComment(id) {
            $.messager.confirm('审核', '您确定要审核通过吗?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/FeedBack/AuditComment",
                        type: 'POST',
                        data: { "ID": id },
                        success: function (request) {
                            if (request == "success") {
                                $.messager.alert("系统提示", "恭喜！您已经审核成功！", "warning");
                                location.href = "/FeedBack/?messageType=" + messageTypeID;
                            }
                            else {
                                $.messager.alert("系统提示", "抱歉！您审核没有成功！", "warning");
                            }
                        }
                    })
                }
            });
        }
        
        //备注
        function RemarksDialogShow(id) {
            $('#RemarksDialog').show();
            $('#RemarksDialog').dialog({
                title: '备注',
                buttons: [{
                    text: '提交',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var content = $("#MailContent").text();
                        $.ajax({
                            url: "/FeedBack/RemarksContent",
                            type: 'POST',
                            cache: false,
                            async: false,
                            data: { id: id, content: content },
                            error: function (xhr) {
                                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                            },
                            success: function (data) {
                                if (data.result == "ok") {
                                    $.messager.alert("系统提示", "备注成功");
                                    PageInit();
                                }
                                else if (data.result == "error") {
                                    $.messager.alert("系统提示", data.msg, "warning");
                                }
                            }
                        });
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#RemarksDialog').dialog('close');
                    }
                }]
            });
        }

        function DeleteReplay(id) {
            $.ajax({
                url: "/FeedBack/DeleteReplay",
                type: 'POST',
                data: { "ID": id },
                success: function (request) {
                    if (request == "success") {
                        location.href = "/FeedBack/?messageType=" + messageTypeID;
                    }
                }
            })
        }

        function ReplayContent(id,email) {
            if (id == -10000) {
                $.messager.alert("系统提示", "请先审核通过该信息！");
                return;
            }
            var _content = $('#replayContent' + id).val().replace('暂时还没有回复，请输入回复内容', '').replace(/"/g, '').replace(/[\\\/“”]/, "");
            if (_content.replace(/[^\x00-\xff]/g, "--").length < 10) {
                $.messager.alert("系统提示", "请回复至少10个字节的内容", "warning");
                return false;
            }
            $.ajax({
                url: "/FeedBack/ReplayContent",
                type: 'POST',
                data: { "ID": id, "content": _content, "email": email },
                success: function (request) {
                    if (request == "success") {
                        $.messager.alert("系统提示", "恭喜！您回复已经成功！", "warning");
                        location.href = "/FeedBack/?messageType=" + messageTypeID;
                    }
                    else {
                        $.messager.alert("系统提示", "抱歉！数据传输出错,您回复没有成功！", "warning");
                    }
                }
            })
        }

        function EditContent(id) {
            $("#edit" + id).css("display", "none");
            $("#replay" + id).css("display", "block");
        }

        function SendMailInit(id,email) {
            $.ajax({
                type: 'POST',
                url: '/User/GetCurrentUser',
                cache: false,
                success: function (data) {
                    $("#sendName").val(data.Email);
                    SendMail(id, email, data.Email);
                },
                error: function (xhr) {
                    throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                }
            })
        }

        function SendMail(id, toEmail,fromMail) {
            $("#receive").val(email)
            $('#MailDialog').show();
            $('#MailDialog').dialog({
                title: '发信',
                buttons: [{
                    text: '提交',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var mailTo = email;
                        var smtp = $("#smtp").val();
                        var mailFrom = $("#sendName").val();
                        var mailPwd = $("#passWord").val();
                        var title = $("#title").val();
                        var content = $("#MailContent").val();
                        $.ajax({
                            url: "/FeedBack/SendMail",
                            type: 'POST',
                            cache: false,
                            async: false,
                            data: { id: id, mailFrom: mailFrom, fromPwd: mailPwd, mailTo: mailTo, smtp: smtp, title: title, content: content },
                            error: function (xhr) {
                                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                            },
                            success: function (data) {
                                if (data == "success") {
                                    $.messager.alert("系统提示", "发信成功");
                                    PageInit();
                                }
                                else if (data == "error") {
                                    $.messager.alert("系统提示", "发信失败,请检查您的密码和收信人地址是否有误,", "warning");
                                }
                                else if(data=="false") {
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



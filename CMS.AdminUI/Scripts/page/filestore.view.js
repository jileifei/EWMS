function ViewInit() {
    if (CurrentExtName == null || CurrentExtName == "") {
        return;
    }
    var _currentPath = $("#smc a:last").text();
    var localpath;
    if (_currentPath == "根目录") {
        localpath = CurrentFile;
    }
    else {
        localpath = _currentPath + "/" + CurrentFile;
    }
    $.ajax({
        url: "/FileStore/GetFileDetail",
        type: 'POST',
        cache: false,
        async: false,
        data: { localpath: localpath },
        success: function (data) {
            var jsonObj = eval('(' + data + ')');
            $("#view_FileName").html(CurrentFile);
            $("#view_FileUrl").html(jsonObj.Url);
            $("#view_CreateUser").html(jsonObj.UserName);
            $("#view_UpTime").html(jsonObj.CreateTime);
            $(".combo-text").val(jsonObj.PublicTime);

            ViewFile(jsonObj.Url, localpath);
        }
    });
}

function ViewFile(url, localpath) {
    $('#ViewDialog').show();
    $('#ViewDialog').dialog({
        buttons: [{
            text: '查看',
            handler: function () {
                location.href = url;
            }
        },
        {
            text: '修改发布时间',
            handler: function () {
                $.ajax({
                    url: "/FileStore/UpdateTime",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { localpath: localpath, publicTime: $(".combo-text").val() },
                    success: function (data) {
                        $.messager.alert("系统提示", "更新发布时间成功");
                    }
                });
            }
        }
        ]
    });
}

function DelFile() {
    var selected = $('#fileList').datagrid('getSelected');
    if (selected) {
        if (selected.ExtName == "" || selected.ExtName == undefined) {
            $.messager.confirm('系统提示', '请确保文件夹中已经删除完文件，否则不能删除！', function (r) {
                if (r) {
                    DelFold("1", selected.Filename);
                }
            });
        }
        else {
            $.messager.confirm('系统提示', '您确定要删除此文件吗！', function (r) {
                if (r) {
                    DelFold('2', selected.Filename);
                }
            });
        }
    }

}

function DelFold(ty, foldname) {
    var _currentPath = $("#smc input:last").val();
    if (_currentPath == "根目录") {
        foldPath = CurrentFile;
    }
    else {
        foldPath = _currentPath + "/" + foldname;
    }
    $.ajax({
        url: "/FileStore/DelFile",
        type: 'POST',
        cache: false,
        async: false,
        data: { ty: ty, localpath: foldPath },
        success: function (data) {
            var jsonObj = eval('(' + data + ')');
            if (jsonObj.result == "ok") {
                $("#messageShow").html("恭喜！已经删除成功");
                $('#fileList').datagrid({
                    url: '/FileStore/AjaxJson?localpath=' + _currentPath
                })
            }
            else if (jsonObj.result == "exist") {
                $("#messageShow").html("抱歉！请确保删除文件夹中的所有文件");
            }
            $('#dialogviewMsg').show();
            $('#dialogviewMsg').dialog({
                buttons: [{
                    text: '取消',
                    handler: function () {
                        $('#dialogviewMsg').dialog('close');
                    }
                }]
            });
        }
    });
}

function RenameInit() {
    var selected = $('#fileList').datagrid('getSelected');
    if (selected) {
        if (selected.ExtName == "" || selected.ExtName == undefined) {
            $.messager.confirm('系统提示', '请确保文件夹中没有任何文件，否则不能重命名！', function (r) {
                if (r) {
                    Rename("1", selected.Filename);
                }
            });
        }
        else {
            Rename("2", selected.Filename);
        }

    }
}

function Rename(ty, name) {
    $("#newFold").val(name);
    $('#dialogview').show();
    $('#dialogview').dialog({
        title: '重命名',
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                $("#errormsg").html("");
                if ($("#newFold").val() == "") {
                    $("#errormsg").html("请填写新文件名称");
                    return false;
                }
                var newFold = $("#newFold").val();
                var newpath = "";
                var oldpath = "";
                var _currentPath = $("#smc input:last").val();
                if (_currentPath == "根目录") {
                    newpath = newFold;
                    oldpath = name
                }
                else {
                    newpath = _currentPath + "/" + newFold;
                    oldpath = _currentPath + "/" + name;
                }
                $.ajax({
                    url: "/FileStore/ReName",
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: { ty: ty, newpath: newpath, oldpath: oldpath },
                    error: function (request) {
                        alert(request);
                    },
                    success: function (data) {
                        var jsonObj = eval('(' + data + ')');
                        $('#dialogviewMsg').show();
                        $('#dialogviewMsg').dialog({
                            buttons: [{
                                text: '取消',
                                handler: function () {
                                    $('#dialogviewMsg').dialog('close');
                                }
                            }]
                        });
                        if (jsonObj.result == "ok") {
                            $("#messageShow").html("恭喜！重命名成功");
                            $('#fileList').datagrid({
                                url: '/FileStore/AjaxJson?localpath=' + _currentPath
                            })
                        }
                        else if (jsonObj.result == "exist") {
                            $("#messageShow").html("抱歉！重命名失败，请确保文件夹中没有其它文件等");
                        }
                        else if (jsonObj.result == "existed") {
                            $("#messageShow").html("抱歉！该目录下已经存在同名文件");
                        }
                    }
                });
            }
        }, {
            text: '取消',
            handler: function () {
                $('#dialogview').dialog('close');
            }
        }]
    });
}
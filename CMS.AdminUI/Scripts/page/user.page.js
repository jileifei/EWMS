$(document).ready(function () {
    AddUser();
    $("input[name='Status'][value=1]").attr("checked", true);

    $('#txtUserName').bind("change", function () {
        CheckUserName();
    });
});
var checkflag = true;
function CheckUserName(UserID) {
    var username = "";
    if (UserID == undefined) {
        username = $("#txtUserName").val();
    }
    else {
        username = $("#txtEditUserName").val();
    }
    var flag = false;
    if ($.trim(username) == "") {
        alert("请输入英文名称");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: '/User/CheckUserName/',
        data: { UserID: UserID, UserName: username },
        cache: false,
        async: false,
        success: function (data) {
            if (data) {
                flag = true;
            }
            else {
                alert("该用户名已经存在，请重新输入");
                if (UserID == undefined) {
                    $("#txtUserName").focus();
                }
                else {
                    $("#txtEditUserName").focus();
                }
                flag = false;
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
    checkflag = flag;
}
function AddUser() {
    checkflag = true;
    $("#aAddUser").click(function () {
        $('#divDialog').show();
        $("#trPassword").show();
        $('#divDialog').attr("title", "用户管理：添加用户");

        $("#divDialog").dialog({
            height: 280,
            width: 470,
            modal: true,
            buttons: {
                "保存": function () {
                    var fromValid = $("#frmAdd").data("validator");
                    if (!fromValid.form()) {
                        return false;
                    }
                    if (!checkflag) {
                        alert("该用户名已经存在，请重新输入");
                        $("#txtUserName").focus();
                        return false;
                    }
                    var username = $("#txtUserName").val();
                    var department = $("#txtDepartment").val();
                    var realname = $("#txtRealName").val();
                    var email = $("#txtEmail").val();
                    var password = $("#txtPassword").val();
                    var status = $("input[name=Status]:checked").val(); 
                    $.ajax({
                        type: 'POST',
                        url: '/user/create/',
                        data: { UserName: username, Department: department, RealName: realname, Email: email, Password: password, Status: status },
                        cache: false,
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.result == "ok") {
                                $('#divDialog').dialog('close');
                                clear();
                                PageInit();
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
    })
}

function EditInit(userid) {
    $('#txtEditUserName').unbind("change");
    $("#txtEditUserName").change(function () {
        CheckUserName(userid);
    });
    $.ajax({
        type: 'POST',
        url: '/user/Details/',
        data: { ID: userid },
        cache: false,
        success: function (data) {
            $("#txtEditUserName").val(data.UserName);
            $("#txtEditRealName").val(data.RealName);
            $("#txtEditDepartment").val(data.Department);
            $("#txtEditEmail").val(data.Email);
            $("input[name='EditStatus'][value=" + data.Status + "]").attr("checked", true);
            EditUser(userid);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function EditUser(UserID) {
    $('#divEditDialog').show();
    $('#divEditDialog').attr("title", "用户管理：修改用户信息");
    checkflag = true;
    $("#divEditDialog").dialog({
        height: 280,
        width: 470,
        modal: true,
        buttons: {
            "保存": function () {
                var fromValid = $("#frmEdit").data("validator");
                if (!fromValid.form()) {
                    return false;
                }
                if (!checkflag) {
                    alert("该用户名已经存在，请重新输入");
                    $("#txtEditUserName").focus();
                    return false;
                }
                var username = $("#txtEditUserName").val();
                var department = $("#txtEditDepartment").val();
                var realname = $("#txtEditRealName").val();
                var email = $("#txtEditEmail").val();
                var status = $("input[name=EditStatus]:checked").val(); 
                $.ajax({
                    type: 'POST',
                    url: '/user/update/',
                    data: { ID: UserID, UserName: username, Department: department, RealName: realname, Email: email, Status: status },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $('#divEditDialog').dialog('close');
                            editclear();
                            PageInit();
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
                $('#divEditDialog').dialog('close');
            }
        }
    });
}

function DelUser(UserID,UserName) {
    if (confirm('您确认要删除[' + UserName + ']的用户信息吗？')) {
        $.ajax({
            type: 'POST',
            url: '/user/delete/',
            data: { ID: UserID },
            cache: false,
            success: function (data) {
                data = eval('(' + data + ')');
                if (data.result == "ok") {
                    PageInit();
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

function clear() {
    $("#txtUserName").val("");
    $("#txtDepartment").val("");
    $("#txtRealName").val("");
    $("#txtEmail").val("");
    $("#txtPassword").val("");
    $("input[name='Status'][value=1]").attr("checked", true);
}

function editclear() {
    $("#txtEditUserName").val("");
    $("#txtEditDepartment").val("");
    $("#txtEditRealName").val("");
    $("#txtEditEmail").val("");
    $("input[name='EditStatus'][value=1]").attr("checked", true);
}


// pager list
function PageInit() {
    window.location.reload();
}
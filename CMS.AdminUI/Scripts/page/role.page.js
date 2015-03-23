var RoleID = 0;

jQuery(document).ready(function () {
    RoleID = parseInt($("#tbRoleID").text());
    var tArtist2 = new TabClickGroup("next", "on", ["liBaseInfo", "liMenuAuth", "liChannelAuth"], ["divBaseInfo", "divMenuAuth", "divChannelAuth"]);
    InitMenu(RoleID);
    $("#chbAllMenu").click(function () {
        SelAllMenu();
    });
    $("#chbAllChannel").click(function () {
        SelAllChannel();
    });
    // 保存角色权限映射关系
    $("#btnSaveMenuAuth").click(function () {
        savemenuauth();
    });
    // 保存栏目权限映射关系
    $("#btnSaveChannelAuth").click(function () {
        SaveChannelAuth();
    });
    // 添加角色用户
    $("#btnAddUserToRole").click(function () {
        var common = new CommonPage();
        common.SelSystemUser();
    });
    // 从当前角色中删除指定的用户
    $("#btnDelUserFromRole").click(function () {
        var userlist = "";
        var selCheckUserList = $("#tbUserList input[name='chbUser']:checked");
        if (selCheckUserList.length > 0) {
            for (var i = 0; i < selCheckUserList.length; i++) {
                $.ajax({
                    type: 'POST',
                    url: '/User/DeleteUserRole/',
                    data: { UserID: selCheckUserList.eq(i).val() },
                    cache: false,
                    async: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "success") {
                        }
                        else {
                            userlist += data.msg;
                        }
                    },
                    error: function (xhr) {
                        throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                    }
                })
            }
            if (userlist == "") {
                alert('删除成功');
            }
            else {
                alert(userlist);
            }
            loaduserbyroleid(RoleID);
        }
        else {
            alert("请选择要删除的用户");
            return false;
        }
    })
    loaduserbyroleid(RoleID);
});

// set cur role info
function SetRole(roleID) {
    RoleID = roleID;
    $("#tbRoleID").text(roleID);
    $("#tbRoleName").text($("#liRole" + roleID).text());
    $("#tbRoleNote").text($("#hiddenRoleNote" + roleID).val());
    loaduserbyroleid(roleID);// load role user
    InitMenu(roleID);// load role menu
    InitializeChannelTree(roleID);// load role channel
}

// load user list by roleid
function loaduserbyroleid(roleID) {
    $("#tbUserList").empty();
    var RoleName = $("#tbRoleName").text();
    $.ajax({
        type: 'POST',
        url: '/User/GetUserList/',
        data: { RoleID: roleID },
        cache: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $("#tbUserList").append('<tr><td align="center"><input type="checkbox" name="chbUser" value="' + data[i].ID + '"/></td><td align="center">' + data[i].ID + '</td><td>' + data[i].UserName + '</td><td>' + data[i].Department + '</td><td>' + RoleName + '</td></tr>');
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

// 添加用户到当前角色
function AddUserToRole(UserID, UserName) {
    $.ajax({
        type: 'POST',
        url: '/User/SetUserRole/',
        data: { UserID: UserID, RoleID: RoleID },
        cache: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.result == "success") {
                alert('添加成功');
                loaduserbyroleid(RoleID);
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

// 关闭用户助选
function CloseSystemUser() {
    $('#divSystemUser').dialog('close');
}

// add role
function addrole() {
    clear();
    $('#divDialog').show();
    $('#divDialog').attr("title", "角色管理：添加角色");
    $("#divDialog").dialog({
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!Valid()) {
                    return false;
                }
                var rolename = $("#txtRoleName").val();
                var note = $("#txtNote").val();
                $.ajax({
                    type: 'POST',
                    url: '/role/add/',
                    data: { RoleName: rolename, Description: note },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $("#ulRoleList").append('<li id="liRole' + data.RoleID + '" onclick="SetRole(' + data.RoleID + ');" style="cursor:pointer;">' + rolename + '</li>');
                            $("#ulRoleList").append('<input type="hidden" id="hiddenRoleNote' + data.RoleID + '" value="' + note + '" />');
                            clear();
                            $('#divDialog').dialog('close');
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
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divDialog').dialog('close');
            }
        }]
    });
    
}
function Valid() {
    var rolename = $("#txtRoleName").val();
    if ($.trim(rolename) == "") {
        alert("请输入角色名称");
        return false;
    }
    else {
        return true;
    }
}
function editrole() {
    $("#txtRoleName").val($("#tbRoleName").text());
    $("#txtNote").val($("#tbRoleNote").text());

    $('#divDialog').show();
    $('#divDialog').attr("title", "角色管理：修改角色");
    $("#divDialog").dialog({
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!Valid()) {
                    return false;
                }
                var roleID = $("#tbRoleID").text();
                var rolename = $("#txtRoleName").val();
                var note = $("#txtNote").val();
                $.ajax({
                    type: 'POST',
                    url: '/role/update/',
                    data: { ID: roleID, RoleName: rolename, Description: note },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $("#liRole" + roleID).text(rolename);
                            $("#hiddenRoleNote" + roleID).val(note);
                            $("#tbRoleName").text(rolename);
                            $("#tbRoleNote").text(note);
                            clear();
                            $('#divDialog').dialog('close');
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
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divDialog').dialog('close');
            }
        }]
    });
}

function delrole() {
    if (confirm("您确认要删除该角色吗？")) {
        var roleID = $("#tbRoleID").text();
        if (roleID != "") {
            $.ajax({
                type: 'POST',
                url: '/role/delete/',
                data: { ID: roleID },
                cache: false,
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.result == "ok") {
                        $("#liRole" + roleID).remove();
                        $("#hiddenRoleNote" + roleID).remove();
                        // foreach ulRoleList set right role baseinfo
                        $("#ulRoleList li").each(function (i) {
                            if (i == 0) {
                                $("#tbRoleID").text($(this).attr("id").replace("liRole", ""));
                                $("#tbRoleName").text($(this).text());
                                $("#tbRoleNote").text($("#hiddenRoleNote" + $(this).attr("id").replace("liRole")).val());
                                return false;
                            }
                        })
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
        else {
            alert('请选择要删除的角色');
        }
    }
}
// reset input
function clear() {
    $("#txtRoleName").val('');
    $("#txtNote").val('');
}

//点击滑动门
function TabClickGroup(normalClass, howverClass, arrLabelIDS, arrDivIDS) {
    this.m_nclass = normalClass;
    this.m_hclass = howverClass;
    this.m_labids = arrLabelIDS;
    this.m_itemids = arrDivIDS;

    //wrap method to function
    this.funcwrap = function (ogrup, current) {
        return function () {
            for (var k = 0; k < ogrup.m_labids.length; k++) {
                var strmidk = "#" + ogrup.m_labids[k];
                if (ogrup.m_labids[k] == ogrup.m_labids[current]) {
                    $(strmidk).removeClass(ogrup.m_nclass).addClass(ogrup.m_hclass);
                    if (strmidk == "#liChannelAuth") {
                        InitializeChannelTree(RoleID);// 载入栏目权限
                    }
                } else {
                    $(strmidk).removeClass(ogrup.m_hclass).addClass(ogrup.m_nclass);
                }
            }


            for (var j = 0; j < ogrup.m_itemids.length; j++) {
                var strmid = "#" + ogrup.m_itemids[j];
                if (ogrup.m_itemids[current] == ogrup.m_itemids[j]) {
                    $(strmid).show();
                } else {
                    $(strmid).hide();
                }
            }
        };
    }


    if (this.m_labids.length != this.m_itemids.length) {
        alert("要切换的标签和内容ID数量不一至:标签IDS=" + this.m_labids);
        return;
    }

    //init 
    for (var i = 0; i < this.m_labids.length; i++) {
        var strid = this.m_labids[i];
        $("#" + strid).click(this.funcwrap(this, i));
    }
}
// 初始化栏目
function InitializeChannelTree(RoleID) {
    $('#divChannelTree').treegrid({
        title: '栏目权限',
        iconCls: 'icon-save',
        width: 722,
        height: 400,
        nowrap: false,
        rownumbers: true,
        animate: true,
        collapsible: true,
        url: '/Channel/AuthAjaxLoading?RoleID=' + RoleID,
        idField: 'key',
        treeField: 'title',
        frozenColumns: [[
	                { title: '栏目', field: 'title', width: 180,
	                    formatter: function (value) {
	                        return '<b><span style="color:red">' + value + '</span></b>';
	                    }
	                }
				]],
        columns: [[
					{ field: 'IsBrowse', title: '浏览权限', width: 85,
					    formatter: function (value, row) {
					        var s = "";
					        if (value) {
					            s = '<input id="chbBrowseAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbBrowseAuth\',\'' + row.key + '\');" name="chbBrowseAuth' + row.key + '" checked/>';
					        }
					        else {
					            s = '<input id="chbBrowseAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbBrowseAuth\',\'' + row.key + '\');" name="chbBrowseAuth' + row.key + '"/>';
					        }
					        return s;
					    }
					},
					{ field: 'AddAuth', title: '添加权限', width: 85,
					    formatter: function (value, row) {
					        var s = "";
					        if (value) {
					            s = '<input id="chbAddAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbAddAuth\',\'' + row.key + '\');" name="chbAddAuth' + row.key + '" checked/>';
					        }
					        else {
					            s = '<input id="chbAddAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbAddAuth\',\'' + row.key + '\');" name="chbAddAuth' + row.key + '"/>';
					        }
					        return s;
					    }
					},
                    { field: 'EditAuth', title: '修改权限', width: 85,
                        formatter: function (value, row) {
                            var s = "";
                            if (value) {
                                s = '<input id="chbEditAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbEditAuth\',\'' + row.key + '\');" name="chbEditAuth' + row.key + '" checked/>';
                            }
                            else {
                                s = '<input id="chbEditAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbEditAuth\',\'' + row.key + '\');" name="chbEditAuth' + row.key + '"/>';
                            }
                            return s;
                        }
                    },
                    { field: 'DelAuth', title: '删除权限', width: 85,
                        formatter: function (value, row) {
                            var s = "";
                            if (value) {
                                s = '<input id="chbDelAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbDelAuth\',\'' + row.key + '\');" name="chbDelAuth' + row.key + '" checked/>';
                            }
                            else {
                                s = '<input id="chbDelAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbDelAuth\',\'' + row.key + '\');" name="chbDelAuth' + row.key + '"/>';
                            }
                            return s;
                        }
                    },
                    { field: 'AuditingAuth', title: '审核权限', width: 85,
                        formatter: function (value, row) {
                            var s = "";
                            if (value) {
                                s = '<input id="chbAuditingAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbAuditingAuth\',\'' + row.key + '\');" name="chbAuditingAuth' + row.key + '" checked/>';
                            }
                            else {
                                s = '<input id="chbAuditingAuth' + row.key + '" type="checkbox" onclick="SetChannelAuth(\'chbAuditingAuth\',\'' + row.key + '\');" name="chbAuditingAuth' + row.key + '"/>';
                            }
                            return s;
                        }
                    }
				]]
    });
}

// 栏目列表中的多选框选择变换时
function SetChannelAuth(AuthType, ChannelID) {
    var nodes = $('#divChannelTree').treegrid('getChildren', ChannelID);
    var flag = false;
    if ($("input[name='" + AuthType + ChannelID + "']").attr('checked') == "checked") {
        flag = true;
    }
    for (var i = 0; i < nodes.length; i++) {
        $("input[name='" + AuthType + nodes[i].key + "']").attr('checked', flag);
    }
}

// 保存栏目角色映射关系
function SaveChannelAuth() {
    var roleID = parseInt($("#tbRoleID").text());
    var nodes = $('#divChannelTree').treegrid('getChildren');
    var map = new Array();
    var isBrowse = false;
    var isAddAuth = false;
    var isEditAuth = false;
    var isDelAuth = false;
    var isAuditAuth = false;
    var channelid = "";
    for (var i = 0; i < nodes.length; i++) {
        channelid = nodes[i].key;
        if ($("#chbBrowseAuth" + channelid).attr('checked') == true) {
            isBrowse = true;
        }
        else {
            isBrowse = false;
        }
        if ($("#chbAddAuth" + channelid).attr('checked') == true) {
            isAddAuth = true;
        }
        else {
            isAddAuth = false;
        }
        if ($("#chbEditAuth" + channelid).attr('checked') == true) {
            isEditAuth = true;
        }
        else {
            isEditAuth = false;
        }
        if ($("#chbDelAuth" + channelid).attr('checked') == true) {
            isDelAuth = true;
        }
        else {
            isDelAuth = false;
        }
        if ($("#chbAuditingAuth" + channelid).attr('checked') == true) {
            isAuditAuth = true;
        }
        else {
            isAuditAuth = false;
        }
        var o = { "RoleID": roleID, "ChannelID": channelid, "IsBrowse": isBrowse, "AddAuth": isAddAuth, "EditAuth": isEditAuth, "DelAuth": isDelAuth, "AuditingAuth": isAuditAuth };
        map.push(o);
    }

    $.ajax({
        type: 'POST',
        url: '/Channel/SaveChannelRoleMap/',
        data: $.toJSON({ ListMap: map }),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var o = eval('(' + data + ')');
            if (o.result == "ok") {
                alert('保存成功');
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

// select all channel
function SelAllChannel() {
    var nodes = $('#divChannelTree').treegrid('getChildren');
    var channelid = "";
    if ($("#chbAllChannel").attr('checked')) {
        for (var i = 0; i < nodes.length; i++) {
            channelid = nodes[i].key;
            $("#chbBrowseAuth" + channelid).attr('checked', true);
            $("#chbAddAuth" + channelid).attr('checked', true);
            $("#chbEditAuth" + channelid).attr('checked', true);
            $("#chbDelAuth" + channelid).attr('checked', true);
            $("#chbAuditingAuth" + channelid).attr('checked', true);
        }
    }
    else {
        for (var i = 0; i < nodes.length; i++) {
            channelid = nodes[i].key;
            $("#chbBrowseAuth" + channelid).attr('checked', false);
            $("#chbAddAuth" + channelid).attr('checked', false);
            $("#chbEditAuth" + channelid).attr('checked', false);
            $("#chbDelAuth" + channelid).attr('checked', false);
            $("#chbAuditingAuth" + channelid).attr('checked', false);
        }
    }
}
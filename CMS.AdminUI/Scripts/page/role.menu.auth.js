// init cur role have auth menulist
function InitMenu(roleID) {
    if (roleID != "") {
        $.ajax({
            type: 'POST',
            url: '/Module/GetModuleListByRole/',
            data: { RoleID: roleID },
            cache: false,
            success: function (data) {
                $('#divMenuTree').dynatree('destroy')
                InitializeMenuTree(data);
            },
            error: function (xhr) {
                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
            }
        })
    }
    else {
        $("#hiddenMenuList").val("");
    }
}

// 初始化菜单
function InitializeMenuTree(data) {
    $("#divMenuTree").dynatree({
        title: "模块列表",
        fx: { height: "toggle", duration: 200 },
        autoFocus: false,
        checkbox: true,
        initAjax: {
            url: "/Module/AjaxLoading"
        },
        onActivate: function (node) {
        },
        onCreate: function (node, nodespan) {
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    if (node.data.key == data[i]) {
                        node.select(true);
                        break;
                    }
                }
            }
        }
    });
}

// 保存角色菜单
function savemenuauth() {
    var roleID = $("#tbRoleID").text();
    if (roleID == "") {
        alert('请选择要设置的角色');
        return false;
    }
    var selectedNodes = $("#divMenuTree").dynatree("getTree").getSelectedNodes();
    var selectedKeys = $.map(selectedNodes, function (node) { return node.data.key; });
    if (selectedKeys.length > 0) {
        $.ajax({
            type: 'POST',
            url: '/Module/SaveModuleListByRole/',
            data: { RoleID: roleID, ModuleList: selectedKeys.join(", ") },
            cache: false,
            success: function (data) {
                data = eval('(' + data + ')');
                if (data.result == "ok") {
                    alert('保存成功');
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
        alert('请选择菜单');
    }
}

// select all menu
function SelAllMenu() {
    if ($("#chbAllMenu").attr('checked')) {
        $("#divMenuTree").dynatree("getRoot").visit(function (node) {
            node.select(true);
        });
    }
    else {
        $("#divMenuTree").dynatree("getRoot").visit(function (node) {
            node.select(false);
        });
    }
}
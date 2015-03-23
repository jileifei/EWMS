$(document).ready(function () {   
    InitializeTree();
});
function InitializeTree() {
    $("#divTree").dynatree({
        title: "模块管理",
        fx: { height: "toggle", duration: 200 },
        autoFocus: false,
        initAjax: {
            url: "/Module/AjaxLoading"
        },
        onActivate: function (node) {
        },
        onCreate: function (node, span) {
            BindContextMenu(span);
        },
        onPostInit: function (isReloading, isError) {
            this.reactivate();
        },
        dnd: {
            preventVoidMoves: true,
            onDragStart: function (node) {
                return true;
            },
            onDragEnter: function (node, sourceNode) {
                if (node.parent !== sourceNode.parent)
                    return false;
                return ["before", "after"];
            },
            onDrop: function (node, sourceNode, hitMode, ui, draggable) {
                sourceNode.move(node, hitMode);
                // sort
                sort(sourceNode.data.key, node.data.key);
            }
        }
    });
}

// 绑定右键事件
function BindContextMenu(span) {
    $(span).contextMenu({ menu: "ulTreeHandle" }, function (action, el, pos) {
        var node = $.ui.dynatree.getNode(el);
        switch (action) {
            case "add":
            case "edit":
            case "del":
                NodeHalder(action, node);
                break;
            default:
                alert("请选择操作");
        }
    });
}

// 节点处理
function NodeHalder(action, node) {
    switch (action) {
        case "add":
            add(node);
            break;
        case "edit":
            if (node.data.key == "0") {
                alert("根节点不能操作");
                return false;
            }
            edit(node);
            break;
        case "del":
            if (node.data.key == "0") {
                alert("根节点不能操作");
                return false;
            }
            var flag = confirm('您确认要删除该模块吗？');
            if (flag) {
                del(node);
            }
            break;
        default:
            alert("请选择操作");
    }
}

// 新增
function add(node) {
    clear();
    var nodeid = node.data.key;

    $('#divDialog').show();
    $('#divDialog').attr("title", "模块管理：添加模块");

    $("#divDialog").dialog({
        height: 280,
        width: 580,
        modal: true,
        buttons: {
            "保存": function () {
                var fromValid = $("form").data("validator");
                if (!fromValid.form()) {
                    return false;
                }

                var ModuleName = $("#txtModuleName").val();
                var ModuleUrl = $("#txtModuleUrl").val();
                var SortID = $("#txtSortID").val();
                var Status = $("input:radio[name='radioStatus']:checked").val();
                var Description = $("#txtDescription").val();
               
                $.ajax({
                    type: 'POST',
                    url: '/module/add/',
                    data: { PID:nodeid,ModuleName: ModuleName, ModuleUrl: ModuleUrl, SortID: SortID, Status: Status, Description: Description },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (parseInt(data.ID) > 0) {
                            $('#divDialog').dialog('close');
                            // node add child node
                            node.addChild({
                                title: ModuleName,
                                key: data.ID,
                                isFolder: false
                            });
                            clear();
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

// 更新
function edit(node) {
    var nodeid = node.data.key;
    $.ajax({
        type: 'POST',
        url: '/module/getmoduleinfo/',
        data: { ID: nodeid },
        cache: false,
        success: function (data) {
            $("#txtModuleName").val(data.ModuleName);
            $("#txtModuleUrl").val(data.ModuleUrl);
            $("#txtSortID").val(data.SortID);
            $("input[name='radioStatus'][value=" + data.Status + "]").attr("checked", true);
            $("#txtDescription").val(data.Description);
            update(node);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}
// update
function update(node) {
    var nodeid = node.data.key;
    $('#divDialog').show();
    $('#divDialog').attr("title", "模块管理：修改模块");
    $("#divDialog").dialog({
        height: 580,
        width: 680,
        modal: true,
        buttons: {
            "保存": function () {
                var fromValid = $("form").data("validator");
                if (!fromValid.form()) {
                    return false;
                }

                var ModuleName = $("#txtModuleName").val();
                var ModuleUrl = $("#txtModuleUrl").val();
                var SortID = $("#txtSortID").val();
                var Status = $("input:radio[name='radioStatus']:checked").val();
                var Description = $("#txtDescription").val();

                $.ajax({
                    type: 'POST',
                    url: '/module/update/',
                    data: { ID: nodeid, ModuleName: ModuleName, ModuleUrl: ModuleUrl, SortID: SortID, Status: Status, Description: Description },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $('#divDialog').dialog('close');
                            // node rename
                            node.data.title = ModuleName;
                            node.render();
                            clear();
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

// 删除
function del(node) {
    var nodeid = node.data.key;
    $.ajax({
        type: 'POST',
        url: '/module/delete/',
        data: { ID: nodeid },
        cache: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.result == "ok") {
                node.remove();
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
// sort
function sort(SourceModuleID, ModuleID) {
    $.ajax({
        type: 'POST',
        url: '/module/sort/',
        data: { SourceID: SourceModuleID, ID: ModuleID },
        cache: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.result == "ok") {
                // nothing
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
// reset input
function clear() {
    $("#txtModuleName").val('');
    $("#txtModuleUrl").val('');
    $("#txtDescription").val('');
    $("#txtSortID").val('');
    $("input[name='radioStatus'][value=1]").attr("checked", true);
}
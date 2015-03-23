$(document).ready(function () {
    // 选择模板
    $('#selTemplate').click(function () {
        var common = new CommonPage();
        common.SelTemplate(1);
    });
    // 选择列表页模板
    $('#selListTemplate').click(function () {
        var common = new CommonPage();
        common.SelTemplate(3);
    });
    // 选择内容模板
    $('#selContentTemplate').click(function () {
        var common = new CommonPage();
        common.SelTemplate(2);
    });
    // 选择分页配置信息
    $("#selPagerInfo").click(function () {
        var common = new CommonPage();
        common.SelPagerSet();
    });
    InitializeTree();
});
function InitializeTree() {
    $("#divTree").dynatree({
        title: "栏目管理",
        fx: { height: "toggle", duration: 200 },
        autoFocus: false,
        initAjax: {
            url: "/Channel/AjaxLoading"
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
            var flag = confirm('您确认要删除该栏目吗？');
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
    $('#divDialog').attr("title", "栏目管理：添加栏目");

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
                var name = $("#txtName").val();
                var enname = $("#txtEnName").val();
                var url = $("#txtChannelUrl").val();
                var sort = $("#txtSort").val();
                var type = $("input[name='radioType'][checked]").val();
                var keyword = $("#txtKeyword").val();
                var description = $("#txtDes").val();
                var Status = $("input:radio[name='radioStatus']:checked").val();
                var templateID = $("#hiddenTemplateID").val();
                if (templateID == "") {
                    templateID = "0";
                }
                var listTemplateID = $("#hiddenListTemplateID").val();
                if (listTemplateID == "") {
                    listTemplateID = "0";
                }
                var listPager = $("#hiddenListPager").val();
                if (listPager == "") {
                    listPager = "0";
                }
                var contentTemplateID = $("#hiddenContentTemplateID").val();
                if (contentTemplateID == "") {
                    contentTemplateID = "0";
                }
                $.ajax({
                    type: 'POST',
                    url: '/channel/add/',
                    data: { Name: name, EnName: enname, ChannelUrlPart: url, ParentChannelID: nodeid, Sort: sort, Type: type, Keyword: keyword, Description: description, SortID: 0, Status: Status, TemplateID: templateID, ListTemplateID: listTemplateID, PagerID: listPager, ContentTemplateID: contentTemplateID },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (parseInt(data.ID) > 0) {
                            $('#divDialog').dialog('close');
                            // node add child node
                            node.addChild({
                                title: name,
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
        url: '/channel/getchannelinfo/',
        data: { ID: nodeid },
        cache: false,
        success: function (data) {
            $("#txtName").val(data.Name);
            $("#txtEnName").val(data.EnName);
            $("#txtChannelUrl").val(data.ChannelUrlPart);
            $("#hiddenTemplateID").val(data.TemplateID);
            $("#txtTemplateName").val(data.TemplateName);
            $("#hiddenListTemplateID").val(data.ListTemplateID);
            $("#txtListTemplateName").val(data.ListTemplateName);
            $("#hiddenContentTemplateID").val(data.ContentTemplateID);
            $("#txtContentTemplateName").val(data.ContentTemplateName);
            $("input[name='radioType'][value=" + data.Type + "]").attr("checked", true);
            $("#txtKeyword").val(data.Keyword);
            $("#txtSort").val(data.Sort);
            $("#txtDes").val(data.Description);
            $("input[name='radioStatus'][value=" + data.Status + "]").attr("checked", true);
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
    $('#divDialog').attr("title", "栏目管理：修改栏目");
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
                var name = $("#txtName").val();
                var enname = $("#txtEnName").val();
                var url = $("#txtChannelUrl").val();
                var type = $("input[name='radioType'][checked]").val();
                var sort = $("#txtSort").val();
                var keyword = $("#txtKeyword").val();
                var description = $("#txtDes").val();
                var Status = $("input:radio[name='radioStatus']:checked").val();
                var templateID = $("#hiddenTemplateID").val();
                if (templateID == "") {
                    templateID = "0";
                }
                var listTemplateID = $("#hiddenListTemplateID").val();
                if (listTemplateID == "") {
                    listTemplateID = "0";
                }
                var listPager = $("#hiddenListPager").val();
                if (listPager == "") {
                    listPager = "0";
                }
                var contentTemplateID = $("#hiddenContentTemplateID").val();
                if (contentTemplateID == "") {
                    contentTemplateID = "0";
                }
                $.ajax({
                    type: 'POST',
                    url: '/channel/update/',
                    data: { ID: nodeid, Name: name, EnName: enname, ChannelUrlPart: url, ParentChannelID: node.data.ParentID, Sort: sort, Type: type, Keyword: keyword, Description: description, Status: Status, TemplateID: templateID, ListTemplateID: listTemplateID, PagerID: listPager, ContentTemplateID: contentTemplateID },
                    cache: false,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.result == "ok") {
                            $('#divDialog').dialog('close');
                            // node rename
                            node.data.title = name;
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
        url: '/channel/delete/',
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
function sort(SourcechannelID, ChannelID) {
    $.ajax({
        type: 'POST',
        url: '/channel/sort/',
        data: { SourceID: SourcechannelID, ID: ChannelID },
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
    $("#txtName").val('');
    $("#txtEnName").val('');
    $("#txtChannelUrl").val('');
    $("input[name='radioType'][value=1]").attr("checked", true);
    $("#txtKeyword").val('');
    $("#txtDes").val('');
    $("#txtSort").val('');
    $("input[name='radioStatus'][value=1]").attr("checked", true);
    $("#hiddenTemplateID").val('');
    $("#txtTemplateName").val('');
    $("#hiddenListTemplateID").val('');
    $("#txtListTemplateName").val('');
    $("#hiddenContentTemplateID").val('');
    $("#txtContentTemplateName").val('');
}

function SetTemplateValue(tid, tname, type) {
    if (type == 2) {// 内容模板
        $("#hiddenContentTemplateID").val(tid);
        $("#txtContentTemplateName").val(tname);
    }
    else if (type == 3) {// 列表页模板
        $("#hiddenListTemplateID").val(tid);
        $("#txtListTemplateName").val(tname);
    }
    else {
        $("#hiddenTemplateID").val(tid);
        $("#txtTemplateName").val(tname);
    }
}
// 关闭模板选择页面
function CloseSelTemplate() {
    $('#divSelTemplateVar').dialog('close');
}

function SetPagerSet(pageid, pagetype) {
    $("#hiddenListPager").val(pageid);
    $("#txtPagerName").val(pagetype);
}

// 关闭分页配置选择页面
function CloseSelPagerSet() {
    $('#divPagerSet').dialog('close');
}
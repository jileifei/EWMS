$(document).ready(function () {
    CKEDITOR.replace('fckContent', {
        startupMode:'source',
        toolbar: [['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink'], ['UIColor', '-', 'Source']]
    });
    AddPosition();
    $("input[name='radioType'][value=1]").attr("checked", true);
    
    $('#txtChannelName').combotree({
        url: '/channel/CombotreeAjaxLoading/',
        onSelect: function (node) {
            $("#txtChannelID").val(node.id);
        }
    });
});

function AddPosition() {
    $("#aAddPosition").click(function () {
        $('#divDialog').show();
        $("#trPassword").show();
        $('#divDialog').attr("title", "推荐位置管理：添加推荐位置");

        $("#divDialog").dialog({
            height: 610,
            width: 600,
            modal: true,
            buttons: {
                "保存": function () {
                    var fromValid = $("#frmPosition").data("validator");
                    if (!fromValid.form()) {
                        return false;
                    }
                    var name = $("#txtPositionName").val();
                    var summary = $("#txtSummary").val();
                    var locationtype = $("input[@name='radioType'][checked]").val();
                    var channelid = $("#txtChannelID").val();
                    $("#fckContent").val(CKEDITOR.instances.fckContent.getData());
                    var isinclude = false;
                    var fileEnCode = "UTF-8";
                    if ($("#chbIsInclude").attr("checked") == true) {
                        isinclude = true;
                    }

                    $.ajax({
                        type: 'POST',
                        url: '/position/add/',
                        data: { Name: name, ChannelID: channelid, LocationType: locationtype, Summary: summary, PlateContent: $("#fckContent").val(), IsInclude: isinclude },
                        cache: false,
                        success: function(data) {
                            data = eval('(' + data + ')');
                            if (data.result == "ok") {
                                $('#divDialog').dialog('close');
                                clear();
                                PageInit();
                            } else {
                                alert(data.msg);
                            }
                        },
                        error: function(xhr) {
                            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                        }
                    });
                },
                "取消": function () {
                    $('#divDialog').dialog('close');
                }
            }
        });
    })
}

function EditInit(PositionID) {
    $.ajax({
        type: 'POST',
        url: '/position/GetPositionInfo/',
        data: { ID: PositionID },
        cache: false,
        success: function (data) {
            $("#txtPositionName").val(data.Name);
            $("#txtSummary").val(data.Summary);
            $("#txtChannelID").val(data.ChannelID);
            $("#fckContent").val(data.PlateContent);
            $('#txtChannelName').combotree('setValue', data.ChannelID);
           
            CKEDITOR.instances.fckContent.setData(data.PlateContent);
            $("input[name='radioType'][value=" + data.LocationType + "]").attr("checked", true);
            if (data.IsInclude) {
                $("#chbIsInclude").attr("checked", true);
            }
            EditPosition(PositionID);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function EditPosition(PositionID) {
    $('#divDialog').show();
    $('#divDialog').attr("title", "推荐位置管理：修改位置信息");
    
    $("#divDialog").dialog({
        height: 610,
        width: 600,
        modal: true,
        buttons: {
            "保存": function () {
                var fromValid = $("#frmPosition").data("validator");
                if (!fromValid.form()) {
                    return false;
                }
                var name = $("#txtPositionName").val();
                var summary = $("#txtSummary").val();
                var locationtype = $("input[@name='radioType'][checked]").val();
                var channelid = $("#txtChannelID").val();
                $("#fckContent").val(CKEDITOR.instances.fckContent.getData());
                var isinclude = false;
                var fileEnCode = "UTF-8";
                if ($("#chbIsInclude").attr("checked") == true) {
                    isinclude = true;
                }
                $.ajax({
                    type: 'POST',
                    url: '/position/update/',
                    data: { ID: PositionID, Name: name, ChannelID: channelid, LocationType: locationtype, Summary: summary, PlateContent: $("#fckContent").val(), IsInclude: isinclude },
                    cache: false,
                    success: function (data) {
                        //data = eval('(' + data + ')');
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
}

function DelPosition(PositionID) {
    if (confirm('您确认要删除当前推荐位置信息吗？该推荐位置下的内容也会同时被删除！')) {
        $.ajax({
            type: 'POST',
            url: '/position/delete/',
            data: { ID: PositionID },
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
    $("#txtPositionName").val('');
    $("#txtSummary").val('');
    $("input[name='radioType'][value=1]").attr("checked", true);
    $("#txtChannelID").val('0');
}

// pager list
function PageInit() {
    window.location.reload();
}
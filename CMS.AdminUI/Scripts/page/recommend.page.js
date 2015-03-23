$(document).ready(function () {
    if (PositionID <= 0) {
        alert('请选择推荐位置');
        window.location.href = "/position/";
    }
    UploadImgDialog('aSelSmallImg', 'txtSmallPicUrl');
    UploadImgDialog('aSelBigImg', 'txtBigPicUrl');
    if (PositionType == 2) {
        $("#trSmallPicUrl").show();
        $("#trBigPicUrl").show();
    }
    else {
        $("#trSmallPicUrl").hide();
        $("#trBigPicUrl").hide();
    }
    AddRecommend();
});

function AddRecommend() {
    $("#aAddContent").click(function () {
        $('#divDialog').show();
        $("#trPassword").show();
        $('#divDialog').attr("title", "推荐内容管理：添加推荐内容");

        $("#divDialog").dialog({
            height: 315,
            width: 470,
            modal: true,
            buttons: {
                "保存": function () {
                    var fromValid = $("#frmRecommend").data("validator");
                    if (!fromValid.form()) {
                        return false;
                    }
                    var title = $("#txtTitle").val();
                    var summary = $("#txtSummary").val();
                    var linkurl = $("#txtLinkurl").val();
                    var smallpicurl = $("#txtSmallPicUrl").val();
                    var bigpickurl = $("#txtBigPicUrl").val();
                    var sortid = $("#txtSortID").val();
                    $.ajax({
                        type: 'POST',
                        url: '/recommend/add/',
                        data: { LocationID: PositionID, Title: title, SmallPicUrl: smallpicurl, BigPicUrl: bigpickurl, Summary: summary, LinkUrl: linkurl, SortID: sortid },
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

function EditInit(ReommendID) {
    $.ajax({
        type: 'POST',
        url: '/recommend/GetRecommendInfo/',
        data: { ID: ReommendID },
        cache: false,
        success: function (data) {
            $("#txtTitle").val(data.Title);
            $("#txtSummary").val(data.Summary);
            $("#txtLinkurl").val(data.LinkUrl);
            $("#txtSmallPicUrl").val(data.SmallPicUrl);
            $("#txtBigPicUrl").val(data.BigPicUrl);
            $("#txtSortID").val(data.SortID);
            EditRecommend(ReommendID);
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    })
}

function EditRecommend(ReommendID) {
    $('#divDialog').show();
    $('#divDialog').attr("title", "推荐内容管理：修改推荐内容");

    $("#divDialog").dialog({
        height: 315,
        width: 470,
        modal: true,
        buttons: {
            "保存": function () {
                var fromValid = $("#frmRecommend").data("validator");
                if (!fromValid.form()) {
                    return false;
                }
                var title = $("#txtTitle").val();
                var summary = $("#txtSummary").val();
                var linkurl = $("#txtLinkurl").val();
                var smallpicurl = $("#txtSmallPicUrl").val();
                var bigpickurl = $("#txtBigPicUrl").val();
                var sortid = $("#txtSortID").val();
                $.ajax({
                    type: 'POST',
                    url: '/recommend/update/',
                    data: { ID: ReommendID, Title: title, SmallPicUrl: smallpicurl, BigPicUrl: bigpickurl, Summary: summary, LinkUrl: linkurl, SortID: sortid },
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
}

function DelRecommend(RecommendID) {
    if (confirm('您确认要删除当前推荐内容吗？')) {
        $.ajax({
            type: 'POST',
            url: '/recommend/delete/',
            data: { ID: RecommendID },
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
    $("#txtTitle").val('');
    $("#txtSummary").val('');
    $("txtLinkurl").val('');
    $("#txtSmallPicUrl").val('');
    $("#txtBigPicUrl").val('');
    $("#txtSortID").val('0');
}

// pager list
function PageInit() {
    window.location.reload();
}
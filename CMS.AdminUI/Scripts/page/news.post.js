jQuery(document).ready(function () {
    CKEDITOR.replace('fckContent');
    UploadImgDialog('aSelImg', 'txtSmallImageUrl');
    // 图文按钮
    $("#btnAddTwTitle").click(function () {
        var title = $('#txtTitle').val();
        $('#txtTitle').val(title + '(图文)');
    });
    // 选择颜色
    $('#txtTitleColor').modcoder_excolor();
    // 设置文章发表时间
    $("#btnSetPublishDate").click(function () {
        $("#txtPublisDate").val($.date().format("yyyy-MM-dd HH:mm:ss"));
    });
    // 所属栏目
    $('#txtChannelName').combotree({
        url: '/channel/CombotreeAjaxLoading/',
        onSelect: function (node) {
            $("#txtChannelID").val(node.id);
        }
    });
    // 选择专题
    $("#selSpecial").click(function () {
        SelSpecial();
    });
    $("#btnPost").click(function () {
        var channelID = $("#txtChannelID").val();
        if (channelID == "") {
            alert('请选择所属栏目');
            return false;
        }
        $("#fckContent").val(CKEDITOR.instances.fckContent.getData())
        var options = {
            beforeSubmit: function () {
            },
            success: function (data) {
                var jsonObj = eval('(' + data + ')');
                if (jsonObj.result == 'ok') {
                    alert('发表成功');
                    // clear form
                    $("#frmPost").clearForm();
                    $("#txtSpecialID").val('0');
                    $("#txtSpecialChannelID").val('0');
                    CKEDITOR.instances.fckContent.setData("");
                }
                else {
                    alert(jsonObj.msg);
                }
            },
            url: "/news/postnews/",
            type: "post",
            resetForm: false,
            dataType: "json"
        };
        $("#frmPost").ajaxSubmit(options);
    })
})

function SelSpecial() {
    var $dialog = null;
    var page = '/Special/SelSpecial/';
    var pagetitle = "选择专题"
    if ($("#divSelSpecial").length == 0) {

        $dialog = $('<div id="divSelSpecial"></div>')

    }
    else {
        $dialog = $('#divSelSpecial')
    }
    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    height: 250,
                    width: 630,
                    title: pagetitle
                });
    $dialog.dialog('open');
}

function SetSpecial(SpecialName, SpecialID, SpecialChannelID) {
    $("#txtSpecialName").val(SpecialName);
    $("#txtSpecialID").val(SpecialID);
    $("#txtSpecialChannelID").val(SpecialChannelID);
}

function CloseSelSpecial() {
    $('#divSelSpecial').dialog('close');
}
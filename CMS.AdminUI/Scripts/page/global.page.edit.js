$(document).ready(function () {
    if (GlobalID == 0) {
        alert('请选择要编辑的全局变量');
        window.location.href = "/global/";
    }
    CKEDITOR.replace('fckContent', { toolbar: [['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink'], ['UIColor', '-', 'Source']] });

    $("#chbIsInclude").click(function () {
        if ($(this).attr("checked") == true) {
            $("#trFileCode").show();
        }
        else {
            $("#trFileCode").hide();
        }
    });

    $('#aSelSystemVar').live('click', function (e) {
        e.preventDefault();
        var common = new CommonPage();
        common.SelSystemVar();
    });

    $("#btnSave").click(function () {
        EditGlobalVar();
    });
});
function CheckEnName() {
    var enname = $("#txtEnName").val();
    var flag = false;
    if ($.trim(enname) == "") {
        alert("请输入英文名称");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: '/Global/CheckEnName/',
        data: { GlobalID: GlobalID, EnName: enname },
        cache: false,
        async: false,
        success: function (data) {
            if (data) {
                flag = true;
            }
            else {
                alert("该英文名称已经存在，请重新输入");
                $("#txtEnName").focus();
                flag = false;
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
    return flag;
}

function EditGlobalVar() {
    $('#txtEnName').unbind("change");
    $("#txtEnName").change(function () {
        CheckEnName(GlobalID);
    });
    var fromValid = $("#frmEdit").data("validator");
    if (!fromValid.form()) {
        return false;
    }
    if (!CheckEnName(GlobalID)) {
        return false;
    }
    var globalname = $("#txtGlobalName").val();
    var enname = $("#txtEnName").val();
    $("#fckContent").val(CKEDITOR.instances.fckContent.getData())
    var isinclude = false;
    var fileEnCode = "";
    if ($("#chbIsInclude").attr("checked") == true) {
        isinclude = true;
        fileEnCode = $("#selFileENCode").val();
    }
    $.ajax({
        type: 'POST',
        url: '/global/update/',
        data: { GlobalID: GlobalID, GlobalName: globalname, EnName: enname, Content: $("#fckContent").val(), IsInclude: isinclude, FileEncode: fileEnCode },
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
}

function clear() {
    $("#txtGlobalName").val("");
    $("#txtEnName").val("");
    $("#fckContent").val("");
    $("#chbIsInclude").attr("checked", false);
}

function PageInit() {
    window.location.href = "/global/";
}

function SetGlobalValue(vartag) {
    CKEDITOR.instances.fckContent.insertHtml(vartag);
}

function CloseSelGlobalVal() {
    $('#divSelGlobalVar').dialog('close');
}
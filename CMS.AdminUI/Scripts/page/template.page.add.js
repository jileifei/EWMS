$(document).ready(function () {
    CKEDITOR.replace('txtTemplateContent',
    {
        customConfig: '/Content/ckeditor/template_config.js'
    });

    // 选择系统变量
    $('#aSelSystemVar').live('click', function (e) {
        e.preventDefault();
        var common = new CommonPage();
        common.SelSystemVar();
    });

    // 选择全局变量
    $('#aSelGlobalVar').live('click', function (e) {
        e.preventDefault();
        var common = new CommonPage();
        common.SelGlobalVar();
    });

    // 选择数据块
    $('#aSelDataBlock').live('click', function (e) {
        e.preventDefault();
        var common = new CommonPage();
        common.SelDataBlock();
    });

    // 如果选择的是内容模板则显示字段下拉框
    $("#selType").change(function () {
        if ($(this).val() == "2") {
            //新闻页模板
            $("#trContentField").show();
            $("#trListField").hide();
            $("#trSpecialField").hide();
        }
        else if ($(this).val() == "3") {
            // 列表页模板
            $("#trListField").show();
            $("#trContentField").hide();
            $("#trSpecialField").hide();
        }
        else if ($(this).val() == "4") {
            // 专题模板
            $("#trSpecialField").show();
            $("#trContentField").hide();
            $("#trListField").hide();
        }
        else {
            $("#trContentField").hide();
            $("#trListField").hide();
            $("#trSpecialField").hide();
        }
    });

    // 内容页字段选择
    $("#selContentField").change(function () {
        var selField = $(this).val();
        if (selField != "") {
            CKEditorInsert("${" + selField + "}");
        }
    });

    // 专题字段选择
    $("#selSpecialField").change(function () {
        var selField = $(this).val();
        if (selField != "") {
            CKEditorInsert("${S-" + selField + "}");
        }
    });

    // 分页数据标签点击
    $("#spanPagerDataList").click(function () {
        CKEditorInsert("${PagerDataList}");
    });

    // 分页页码标签点击
    $("#spanPagerNumInfo").click(function () {
        CKEditorInsert("${PagerNumInfo}");
    });

    // 添加
    $("#btnAddTemplate").click(function () {
        var TemplateName = $("#txtName").val();
        if ($.trim(TemplateName) == "") {
            alert('请输入模板名称');
            return false;
        }
        var Type = $("#selType").val();
        if (Type == "") {
            alert("请选择模板类型");
            return false;
        }
        var templateContent = CKEDITOR.instances.txtTemplateContent.getData();
        if ($.trim(templateContent) == "") {
            alert('请输入模板内容');
            return false;
        }
        $.ajax({
            type: 'POST',
            url: '/Template/Create/',
            data: { Name: TemplateName, Type: Type, TemplateCode: templateContent },
            cache: false,
            success: function (data) {
                if (data.result == "ok") {
                    if (Type == 6) {
                        window.location.href = "/template?type=6";
                    } else {
                        window.location.href = "/template/";
                    }
                }
                else {
                    alert(data.msg);
                }
            },
            error: function (xhr) {
                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
            }
        })
    });
});

// 设置系统变量
function SetSystemVarValue(vartag) {
    CKEditorInsert(vartag);
}

// 关闭系统变量窗口
function CloseSelSystemVar() {
    $('#divSelSystemVar').dialog('close');
}

// 设置全局变量
function SetGlobalVarValue(vartag) {
    CKEditorInsert(vartag);
}

// 关闭全局变量窗口
function CloseSelGlobalVar() {
    $('#divGlobalVar').dialog('close');
}

// 设置数据块
function SetDatBlockValue(vartag) {
    CKEditorInsert(vartag);
}

// 关闭数据块窗口
function CloseSelDataBlock() {
    $('#divDataBlockVar').dialog('close');
}

function CKEditorInsert(insertValue) {
    var mode = CKEDITOR.instances.txtTemplateContent.mode;
    if (mode == "wysiwyg") {// 所见即所得模式
        CKEDITOR.instances.txtTemplateContent.insertHtml(insertValue);
    }
    else {
        $("textarea").each(function () {
            if ($(this).attr("id") == "") {
                $(this).insertAtCaret(insertValue);
            }
        })
    }
}

// 专题栏目标签插入
function SetSpecialChannel(specialChannelTag) {
    CKEditorInsert(specialChannelTag);
}
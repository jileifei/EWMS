$(function () {
    $("#selTemplateType").val(TemplateType);
})
function DelTemplate(TemplateID) {
    if (confirm('您确认要删除该模板吗？')) {
        $.ajax({
            type: 'POST',
            url: '/template/delete/',
            data: { ID: TemplateID },
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

function PageInit() {
    window.location.reload();
}
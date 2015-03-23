function DelGlobal(GlobalID) {
    if (confirm('您确认要删除当前全局变量吗？')) {
        $.ajax({
            type: 'POST',
            url: '/global/delete/',
            data: { GlobalID: GlobalID },
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
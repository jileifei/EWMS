function DelPagerSet(PageID) {
    if (confirm('您确认要删除当前分页设置信息吗？')) {
        $.ajax({
            type: 'POST',
            url: '/pagerset/delete/',
            data: { PageID: PageID },
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
function DelBlock(DataBlockID) {
    if (confirm('您确认要删除当前数据块吗？')) {
        $.ajax({
            type: 'POST',
            url: '/DataBlock/Delete/',
            data: { ID: DataBlockID },
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
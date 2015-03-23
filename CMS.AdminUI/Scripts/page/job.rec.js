function Delete(id) {
    $.messager.confirm('删除', '您确定要删当前的职位吗?', function (r) {
        if (r) {
            $.ajax({
                url: '/Job/Delete',
                type: 'POST',
                data: { "id": id },
                success: function (request) {
                    if (request == "success") {
                        $.messager.alert("系统提示", "删除成功！");
                        PageInit();
                    }
                    else {
                        $.messager.alert("系统提示", "删除失败！", "warning");
                    }
                }
            })
        } 
    });
}

function PageInit() {
    window.location.reload();
}

